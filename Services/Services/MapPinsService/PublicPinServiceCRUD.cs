using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Models.Images;
using DTOs.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AuthJWT.Services
{
    public class PublicPinServiceCRUD
    {
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment environment;
        private readonly PinsContext publicPinContext;
        private readonly ModerateContext moderatePinContext;

        public PublicPinServiceCRUD(IHostingEnvironment environment, IMemoryCache cache, PinsContext publicPinContext, ModerateContext moderatePinContext)
        {
            this.cache = cache;
            this.environment = environment;
            this.publicPinContext = publicPinContext;
            this.moderatePinContext = moderatePinContext;
        }
        public async Task<ResponseDTO> AddPublicPin(ProblemPinDTO problemPinDTO, Guid userId)
        {
            List<ProblemImages> uploadedImages = new List<ProblemImages>();
            if (problemPinDTO.Files.Count > 0)
            {
                // Add to GModeration database table => transfer to moderation team
                ProblemPin problemPin = new ProblemPin
                {
                    Lat = problemPinDTO.Lat,
                    Lng = problemPinDTO.Lng,
                    Name = problemPinDTO.Name,
                    ProblemDescription = problemPinDTO.ProblemDescription,
                    Address = problemPinDTO.Address,
                    UserKeyId = userId
                };

                foreach (var file in problemPinDTO.Files)
                {
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                    if (!Directory.Exists(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{userId}\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{userId}\\");
                    }
                    FileStream filestream = File.Create(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{userId}\\" + fileName);
                    await file.CopyToAsync(filestream);
                    await filestream.FlushAsync();
                    ProblemImages image = new ProblemImages
                    {
                        Alt = fileName,
                        ImagePath = $"\\PinPublicImages\\{userId}\\{fileName}",
                        WebImagePath = "http://localhost:54968/PinPublicImages/" + userId + "/" + fileName,
                        problemPin = problemPin,
                        problemPinId = problemPin.Id
                    };
                    uploadedImages.Add(image);

                }

                problemPin.Images = uploadedImages;

                await moderatePinContext.ModerateProblemPins.AddAsync(problemPin);
                int count = await moderatePinContext.SaveChangesAsync();
                if (count > 0)
                {
                    cache.Set(problemPin.Id, problemPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return new ResponseDTO() { Message = "Пин успешно добавлен и на данный момент проходит модерацию", Status = true }; ;
            }
            else
            {
                return new ResponseDTO() { Message = "Ошибка с загрузкой фотографий", Status = false }; ;
            }
         }

        //Simple CRUD without Get all because it's another service with Singleton
        public async Task<ProblemPin> GetPublicPinById(Guid id)
        {
            ProblemPin foundedPin = null;
            if (!cache.TryGetValue(id, out foundedPin))
            {
                foundedPin = await publicPinContext.ProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == id);
                if (foundedPin != null)
                {
                    cache.Set(foundedPin.Id, foundedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return foundedPin;
        }


        public async Task<ResponseDTO> EditPublicPin(Guid oldDataId, ProblemPin newProblemPin)
        {
                var foundedPin = await publicPinContext.ProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                publicPinContext.Entry(foundedPin).CurrentValues.SetValues(newProblemPin);
                int count = await publicPinContext.SaveChangesAsync();
                if (count > 0)
                {
                    cache.Set(foundedPin.Id, foundedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return new ResponseDTO() { Message = "Пин успешно изменён", Status = true };
        }

        public async Task<ResponseDTO> DeletePublicPin(Guid oldDataId)
        {
             var foundedPin = await publicPinContext.ProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == oldDataId);
             foreach (ImageCustom image in foundedPin.Images)
             {
                File.Delete(environment.WebRootPath + image.ImagePath);
             }
            if (cache.TryGetValue(oldDataId, out foundedPin))
            {
                cache.Remove(foundedPin.Id);
            }
            publicPinContext.ProblemPins.Remove(foundedPin);
            await publicPinContext.SaveChangesAsync();
            return new ResponseDTO() { Message = "Пин успешно удалён", Status = true };
        }
    }
}
