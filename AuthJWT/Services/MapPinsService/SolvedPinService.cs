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
    public class SolvedPinService
    {
        private readonly SolvedPinContext solvedPinContext;

        public SolvedPinService(SolvedPinContext solvedPinContext)
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
            try
            {
                var foundedPin = await solvedPinContext.SolvedPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                foundedPin = newSolvedPin;
                await solvedPinContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSolvedPin(Guid oldDataId)
        {
            try
            {
                var foundedPin = await solvedPinContext.SolvedPins.FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                solvedPinContext.SolvedPins.Remove(foundedPin);
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
