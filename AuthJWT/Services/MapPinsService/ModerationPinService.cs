using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Services.PublicPins
{
    public class ModerationPinService
    {
        private readonly PublicPinContext publicPinContext;
        private readonly ModeratePinContext moderatePinContext;
        private readonly SolvedPinContext solvedPinContext;

        public ModerationPinService(PublicPinContext publicPinContext, ModeratePinContext moderatePinContext, SolvedPinContext solvedPinContext)
        {
            this.publicPinContext = publicPinContext;
            this.moderatePinContext = moderatePinContext;
            this.solvedPinContext = solvedPinContext;
        }

        public async Task<List<ProblemPin>> GetModerationPins()
        {
            var moderationPins = await moderatePinContext.ProblemPins.ToListAsync();
            return moderationPins;
        }

        public async Task<ProblemPin> GetModerationPinById(Guid id)
        {
            var foundedPin = await moderatePinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == id);
            return foundedPin;
        }

        public async Task<bool> EditModerationPin(Guid oldDataId, ProblemPin newProblemPin)
        {
            try
            {
                var foundedPin = await moderatePinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                foundedPin = newProblemPin;
                await moderatePinContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteModerationPin(Guid oldDataId)
        {
            try
            {
                var foundedPin = await moderatePinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                moderatePinContext.ProblemPins.Remove(foundedPin);
                await moderatePinContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> ModerationAcceptPin(Guid oldDataId)
        {
            try
            {
                //Remove from Moderation database table
                var foundedPin = await moderatePinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                moderatePinContext.ProblemPins.Remove(foundedPin);
                await moderatePinContext.SaveChangesAsync();

                // Add to Public database table
                publicPinContext.ProblemPins.Add(foundedPin);
                await publicPinContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SolvedProblemPinAccept(Guid oldDataId, SolvedPinDTO solvedPinDTO)
        {
            try
            {
                //Remove from Moderation database table
                var foundedPin = await publicPinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                publicPinContext.ProblemPins.Remove(foundedPin);
                await publicPinContext.SaveChangesAsync();

                // Add to Solved pins DataBase
                SolvedPin solvedPin = new SolvedPin()
                {
                    UserKeyId = foundedPin.UserKeyId,
                    Name = foundedPin.Name,
                    Description = foundedPin.Description,
                    CreationDate = foundedPin.CreationDate,
                    ImagesPath = foundedPin.ImagesPath,
                    BuildingNumber = foundedPin.BuildingNumber,
                    Lat = foundedPin.Lat,
                    Lng = foundedPin.Lng,
                    Street = foundedPin.Street,
                    Region = foundedPin.Region,
                    SolvedPinImagesPath = solvedPinDTO.SolvedPinImagesPath,
                    Report = solvedPinDTO.Report,
                    Team = solvedPinDTO.Team
                };
                solvedPinContext.SolvedPins.Add(solvedPin);
                await solvedPinContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
