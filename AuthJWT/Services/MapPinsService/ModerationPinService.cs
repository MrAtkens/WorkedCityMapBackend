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
        private readonly PinsContext publicPinContext;
        private readonly ModerateContext moderatePinContext;

        public ModerationPinService(PinsContext publicPinContext, ModerateContext moderatePinContext)
        {
            this.publicPinContext = publicPinContext;
            this.moderatePinContext = moderatePinContext;
        }

        public async Task<List<ProblemPin>> GetModerationPins()
        {
            var moderationPins = await moderatePinContext.ModerateProblemPins.ToListAsync();
            return moderationPins;
        }

        public async Task<ProblemPin> GetModerationPinById(Guid id)
        {
            var foundedPin = await moderatePinContext.ModerateProblemPins.FirstOrDefaultAsync(pins => pins.Id == id);
            return foundedPin;
        }

        public async Task<bool> EditModerationPin(Guid oldDataId, ProblemPin newModerateProblemPin)
        {
            try
            {
                ProblemPin foundedPin = await moderatePinContext.ModerateProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                moderatePinContext.Entry(foundedPin).CurrentValues.SetValues(newModerateProblemPin);
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
                ProblemPin foundedPin = await moderatePinContext.ModerateProblemPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                moderatePinContext.ModerateProblemPins.Remove(foundedPin);
                await moderatePinContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> ModerationAcceptPin(AcceptDTO acceptDTO)
        {
            try
            {
                //Remove from Moderation database table
                ProblemPin foundedPin = await moderatePinContext.ModerateProblemPins.FirstOrDefaultAsync(pins => pins.Id == acceptDTO.Id);
                moderatePinContext.ModerateProblemPins.Remove(foundedPin);
                await moderatePinContext.SaveChangesAsync();
                // Add to Public database table
                foundedPin.ModeratorId = acceptDTO.ModeratorId;
                await publicPinContext.ProblemPins.AddAsync(foundedPin);
                await publicPinContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SolvedProblemPinAccept(SolvedPinDTO solvedPinDTO)
        {
            try
            {
                //Remove from Moderation database table
                ProblemPin foundedPin = await publicPinContext.ProblemPins.FirstOrDefaultAsync(pins => pins.Id == solvedPinDTO.Id);
                publicPinContext.ProblemPins.Remove(foundedPin);
                // Add to Solved pins DataBase
                SolvedPin solvedPin = new SolvedPin()
                {
                    Id = foundedPin.Id,
                    UserKeyId = foundedPin.UserKeyId,
                    Name = foundedPin.Name,
                    Description = foundedPin.Description,
                    ProblemDescription = foundedPin.ProblemDescription,
                    CreationDate = foundedPin.CreationDate,
                    ImagesPath = foundedPin.ImagesPath,
                    BuildingNumber = foundedPin.BuildingNumber,
                    Lat = foundedPin.Lat,
                    Lng = foundedPin.Lng,
                    Street = foundedPin.Street,
                    Region = foundedPin.Region,
                    SolvedPinImagesPath = solvedPinDTO.SolvedPinImagesPath,
                    Report = solvedPinDTO.Report,
                    Team = solvedPinDTO.Team,
                    ModeratorId = foundedPin.ModeratorId,
                    SolvedModerator = solvedPinDTO.ModeratorId
                };

                await publicPinContext.SolvedPins.AddAsync(solvedPin);
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
