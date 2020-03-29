using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthJWT.Services
{
    public class PublicPinServiceCRUD
    {
        private readonly PinsContext publicPinContext;
        private readonly ModerateContext moderatePinContext;

        public PublicPinServiceCRUD(PinsContext publicPinContext, ModerateContext moderatePinContext)
        {
            this.publicPinContext = publicPinContext;
            this.moderatePinContext = moderatePinContext;
        }
        public async Task<bool> AddPublicPin(ProblemPin moderateProblemPin)
        {
            try
            {
                // Add to Moderation database table => transfer to moderation team
                await moderatePinContext.ModerateProblemPins.AddAsync(moderateProblemPin);
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
                publicPinContext.Entry(foundedPin).CurrentValues.SetValues(newProblemPin);
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
