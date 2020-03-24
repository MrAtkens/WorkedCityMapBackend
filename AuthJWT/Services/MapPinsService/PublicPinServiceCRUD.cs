using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthJWT.Services
{
    public class PublicPinServiceCRUD
    {
        private readonly PublicPinContext publicPinContext;
        private readonly ModeratePinContext moderatePinContext;

        public PublicPinServiceCRUD(PublicPinContext publicPinContext, ModeratePinContext moderatePinContext)
        {
            this.publicPinContext = publicPinContext;
            this.moderatePinContext = moderatePinContext;
        }
        public async Task<bool> AddPublicPin(ProblemPinDTO problemPinDTO)
        {
            try
            {
                ProblemPin problemPin = new ProblemPin()
                {
                    UserKeyId = problemPinDTO.UserKeyId,
                    Name = problemPinDTO.Name,
                    Description = problemPinDTO.Description,
                    ProblemDescription = problemPinDTO.ProblemDescription,
                    ImagesPath = problemPinDTO.ImagesPath,
                    Lat = problemPinDTO.Lat,
                    Lng = problemPinDTO.Lng,
                    Region = problemPinDTO.Region,
                    Street = problemPinDTO.Street,
                    BuildingNumber = problemPinDTO.BuildingNumber,
                    CreationDate = problemPinDTO.CreationDate
                };
                // Add to Moderation database table => transfer to moderation team
                moderatePinContext.ProblemPins.Add(problemPin);
                await moderatePinContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
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
            try
            {
                var foundedPin = await publicPinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                foundedPin = newProblemPin;
                await publicPinContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeletePublicPin(Guid oldDataId)
        {
            try
            {
                var foundedPin = await publicPinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                publicPinContext.ProblemPins.Remove(foundedPin);
                await publicPinContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
