using AuthJWT.DataAcces;
using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthJWT.Services.PublicPins
{
    public class SolvedPinService
    {
        private readonly PinsContext solvedPinContext;

        public SolvedPinService(PinsContext solvedPinContext)
        {
            this.solvedPinContext = solvedPinContext;
        }

        public async Task<List<SolvedPin>> GetSolvedPins()
        {
            var solvedPins = await solvedPinContext.SolvedPins.ToListAsync();
            return solvedPins;
        }

        public async Task<SolvedPin> GetSolvedPinById(Guid id)
        {
            var solvedPin = await solvedPinContext.SolvedPins.FirstOrDefaultAsync(pins => pins.Id == id);
            return solvedPin;
        }

        public async Task<bool> EditSolvedPin(Guid oldDataId, SolvedPin newSolvedPin)
        {
                var foundedPin = await solvedPinContext.SolvedPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                solvedPinContext.Entry(foundedPin).CurrentValues.SetValues(newSolvedPin);
                await solvedPinContext.SaveChangesAsync();
                return true;
        }

        public async Task<bool> DeleteSolvedPin(Guid oldDataId)
        {
                var foundedPin = await solvedPinContext.SolvedPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                solvedPinContext.SolvedPins.Remove(foundedPin);
                await solvedPinContext.SaveChangesAsync();
                return true;
        }
    }
}
