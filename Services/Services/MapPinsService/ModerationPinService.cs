using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Models.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Twilio.Http;
using DTOs.DTOs;
using AuthJWT.Models.AuthModels;

namespace AuthJWT.Services.PublicPins
{
    public class ModerationPinService
    {
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment environment;
        private readonly PinsContext publicPinContext;
        private readonly ModerateContext moderatePinContext;

        public ModerationPinService(IHostingEnvironment environment, IMemoryCache cache, PinsContext publicPinContext, ModerateContext moderatePinContext)
        {
            this.cache = cache;
            this.environment = environment;
            this.publicPinContext = publicPinContext;
            this.moderatePinContext = moderatePinContext;
        }

        public async Task<List<ProblemPin>> GetModerationPins()
        {
            var moderationPins = await moderatePinContext.ModerateProblemPins.Include(problemPin => problemPin.Images).ToListAsync();
            return moderationPins;
        }

        public async Task<ProblemPin> GetModerationPinById(Guid id)
        {
            ProblemPin foundedPin = null;
            if (!cache.TryGetValue(id, out foundedPin))
            {
                foundedPin = await moderatePinContext.ModerateProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == id);
                if (foundedPin != null)
                {
                    cache.Set(foundedPin.Id, foundedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return foundedPin;
        }

        public async Task<ResponseDTO> EditModerationPin(ProblemPin foundedPin, ProblemPinDTO newModerateProblemPin)
        {
                moderatePinContext.Entry(foundedPin).CurrentValues.SetValues(newModerateProblemPin);
                int count = await moderatePinContext.SaveChangesAsync();
                if (count > 0)
                {
                    cache.Set(foundedPin.Id, foundedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return new ResponseDTO() { Message = "Пин изменён успешно", Status = true };
        }

        public async Task<ResponseDTO> DeleteModerationPin(ProblemPin foundedPin)
        {
            if (foundedPin.Images != null)
            {
                foreach (ImageCustom image in foundedPin.Images)
                {
                    File.Delete(environment.WebRootPath + image.ImagePath);
                }
            }
            ProblemPin bufferPin = foundedPin;
            if (cache.TryGetValue(foundedPin.Id, out bufferPin))
            {
                cache.Remove(foundedPin.Id);
            }
            moderatePinContext.ModerateProblemPins.Remove(foundedPin);
            await moderatePinContext.SaveChangesAsync();
            return new ResponseDTO() { Message = "Пин удалён", Status = true }; ;
        }


        public async Task<ResponseDTO> ModerationAcceptPin(AcceptDTO acceptDTO, Moderator moderator)
        {
                //Remove from Moderation database table
                ProblemPin foundedPin = await moderatePinContext.ModerateProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == acceptDTO.Id);
                moderatePinContext.ModerateProblemPins.Remove(foundedPin);
                await moderatePinContext.SaveChangesAsync();
                // Add to Public database table
                foundedPin.ModeratorId = acceptDTO.ModeratorId;
                await publicPinContext.ProblemPins.AddAsync(foundedPin);
                int count = await publicPinContext.SaveChangesAsync();
                if (count > 0)
                {
                    Moderator newModerator = moderator;
                    newModerator.ModeratedPinsCount += 1;
                    moderatePinContext.Entry(moderator).CurrentValues.SetValues(newModerator);
                    int countModerator = await moderatePinContext.SaveChangesAsync();
                    if (countModerator > 0)
                    {
                        cache.Set(newModerator.Id, newModerator, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    }
                    cache.Set(foundedPin.Id, foundedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return new ResponseDTO() { Message = "Пин успешно прошёл модерацию и добавлен в публичную базу", Status = true};
        }

        public async Task<ResponseDTO> SolvedProblemPinAccept(SolvedPinDTO solvedPinDTO)
        {
                //Remove from Moderation database table
                ProblemPin foundedPin = await publicPinContext.ProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == solvedPinDTO.Id);
                publicPinContext.ProblemPins.Remove(foundedPin);
                // Add to Solved pins DataBase
                if (solvedPinDTO.Files.Count > 0)
                {
                    SolvedPin solvedPin = new SolvedPin()
                    {
                        Id = foundedPin.Id,
                        UserKeyId = foundedPin.UserKeyId,
                        Name = foundedPin.Name,
                        ProblemDescription = foundedPin.ProblemDescription,
                        CreationDate = foundedPin.CreationDate,
                        Images = foundedPin.Images,
                        Lat = foundedPin.Lat,
                        Lng = foundedPin.Lng,
                        Report = solvedPinDTO.Report,
                        Team = solvedPinDTO.Team,
                        ModeratorId = foundedPin.ModeratorId,
                        SolvedModerator = solvedPinDTO.ModeratorId
                    };
                List<SolvedImages> uploadedImages = new List<SolvedImages>();

                foreach (var file in solvedPinDTO.Files)
                {
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                    if (!Directory.Exists(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{solvedPin.Id}\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{solvedPin.Id}\\");
                    }
                    FileStream filestream = File.Create(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{solvedPin.Id}\\" + fileName);
                    await file.CopyToAsync(filestream);
                    await filestream.FlushAsync();
                    SolvedImages image = new SolvedImages
                    {
                        Alt = fileName,
                        ImagePath = $"\\PinPublicImages\\{solvedPin.Id}\\{fileName}",
                        WebImagePath = "http://localhost:54968/PinPublicImages/" + solvedPin.Id + "/" + fileName,
                        SolvedPin = solvedPin,
                        solvedPinId = solvedPin.Id
                    };
                    uploadedImages.Add(image);

                }

                solvedPin.SolvedImages = uploadedImages;

                await publicPinContext.SolvedPins.AddAsync(solvedPin);
                int count = await publicPinContext.SaveChangesAsync();
                if (count > 0)
                {
                    cache.Set(solvedPin.Id, solvedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return new ResponseDTO() { Message = "Решение пина успешно подтверждено, пин добавлен в базу решённых пинов", Status = true };
             }
             else {
                return new ResponseDTO() { Message = "Ошибка с загрузкой фотографий", Status = false }; ;
            }
        }

    }
}
