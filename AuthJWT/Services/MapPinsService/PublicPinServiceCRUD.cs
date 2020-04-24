using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthJWT.Services
{
    public class PublicPinServiceCRUD
    {
        private readonly IHostingEnvironment environment;
        private readonly PinsContext publicPinContext;
        private readonly ModerateContext moderatePinContext;

        public PublicPinServiceCRUD(IHostingEnvironment environment, PinsContext publicPinContext, ModerateContext moderatePinContext)
        {
            this.environment = environment;
            this.publicPinContext = publicPinContext;
            this.moderatePinContext = moderatePinContext;
        }
        public async Task<bool> AddPublicPin(ProblemPinDTO problemPinDTO, string ip)
        {
            List<ImageCustom> uploadedImages = new List<ImageCustom>();
            if (problemPinDTO.Files.Count > 0)
            {
                try
                {
                    MD5 md5 = MD5.Create();
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(ip));
                    Guid result = new Guid(hash);
                    Console.WriteLine(uploadedImages);
                    // Add to GModeration database table => transfer to moderation team
                    ProblemPin problemPin = new ProblemPin
                    {
                        Lat = problemPinDTO.Lat,
                        Lng = problemPinDTO.Lng,
                        Name = problemPinDTO.Name,
                        ProblemDescription = problemPinDTO.ProblemDescription,
                        LocationDescription = problemPinDTO.LocationDescription,
                        Street = problemPinDTO.Street,
                        Region = problemPinDTO.Region,
                        BuildingNumber = problemPinDTO.BuildingNumber,
                        UserKeyId = result
                    };

                    foreach (var file in problemPinDTO.Files)
                    {
                            if (!Directory.Exists(environment.WebRootPath + "\\PinPublicImages\\"))
                            {
                                Directory.CreateDirectory(environment.WebRootPath + "\\PinPublicImages\\");
                            }
                            FileStream filestream = File.Create(environment.WebRootPath + "\\PinPublicImages\\" + file.FileName);
                            file.CopyTo(filestream);
                            filestream.Flush();
                        ImageCustom image = new ImageCustom
                        {
                            Alt = file.FileName,
                            ImagePath = "http://localhost:54968/uploads/" + file.FileName,
                            problemPin = problemPin,
                            problemPinId = problemPin.Id
                        };
                            uploadedImages.Add(image);
                            
                    }

                    problemPin.Images = uploadedImages;
                        
                    await moderatePinContext.ModerateProblemPins.AddAsync(problemPin);
                    await moderatePinContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }     
        }

        //Simple CRUD without Get all because it's another service with Singleton
        public async Task<ProblemPin> GetPublicPinById(Guid id)
        {
            var foundedPin = await publicPinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == id);
            return foundedPin;
        }


        public async Task<bool> EditPublicPin(Guid oldDataId, ProblemPin newProblemPin)
        {
                var foundedPin = await publicPinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                publicPinContext.Entry(foundedPin).CurrentValues.SetValues(newProblemPin);
                await publicPinContext.SaveChangesAsync();
                return true;
        }

        public async Task<bool> DeletePublicPin(Guid oldDataId)
        {
                var foundedPin = await publicPinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                publicPinContext.ProblemPins.Remove(foundedPin);
                await publicPinContext.SaveChangesAsync();
                return true;
        }
    }
}
