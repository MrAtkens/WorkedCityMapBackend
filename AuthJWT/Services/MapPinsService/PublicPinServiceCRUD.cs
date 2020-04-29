using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Models.Images;
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
            List<ProblemImages> uploadedImages = new List<ProblemImages>();
            if (problemPinDTO.Files.Count > 0)
            {
                MD5 md5 = MD5.Create();
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(ip));
                Guid result = new Guid(hash);

                // Add to GModeration database table => transfer to moderation team
                ProblemPin problemPin = new ProblemPin
                {
                    Lat = problemPinDTO.Lat,
                    Lng = problemPinDTO.Lng,
                    Name = problemPinDTO.Name,
                    ProblemDescription = problemPinDTO.ProblemDescription,
                    Address = problemPinDTO.Address,
                    UserKeyId = result
                };

                foreach (var file in problemPinDTO.Files)
                {
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                    if (!Directory.Exists(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{result}\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{result}\\");
                    }
                    FileStream filestream = File.Create(environment.WebRootPath + "\\PinPublicImages\\" + $"\\{result}\\" + fileName);
                    await file.CopyToAsync(filestream);
                    await filestream.FlushAsync();
                    ProblemImages image = new ProblemImages
                    {
                        Alt = fileName,
                        ImagePath = $"\\PinPublicImages\\{result}\\{fileName}",
                        WebImagePath = "http://localhost:54968/PinPublicImages/" + result + "/" + fileName,
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
            else
            {
                return false;
            }
         }

        //Simple CRUD without Get all because it's another service with Singleton
        public async Task<ProblemPin> GetPublicPinById(Guid id)
        {
            var foundedPin = await publicPinContext.ProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == id);
            return foundedPin;
        }


        public async Task<bool> EditPublicPin(Guid oldDataId, ProblemPin newProblemPin)
        {
                var foundedPin = await publicPinContext.ProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                publicPinContext.Entry(foundedPin).CurrentValues.SetValues(newProblemPin);
                await publicPinContext.SaveChangesAsync();
                return true;
        }

        public async Task<bool> DeletePublicPin(Guid oldDataId)
        {
                var foundedPin = await publicPinContext.ProblemPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                foreach (ImageCustom image in foundedPin.Images){
                   File.Delete(environment.WebRootPath + image.ImagePath);
                }
                publicPinContext.ProblemPins.Remove(foundedPin);
                await publicPinContext.SaveChangesAsync();
                return true;
        }
    }
}
