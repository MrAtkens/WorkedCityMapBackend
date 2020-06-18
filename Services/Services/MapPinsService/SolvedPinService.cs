using AuthJWT.DataAcces;
using AuthJWT.Models;
using DTOs.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AuthJWT.Services.PublicPins
{
    public class SolvedPinService
    {
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment environment;
        private readonly PinsContext solvedPinContext;

        public SolvedPinService(PinsContext solvedPinContext, IMemoryCache cache, IHostingEnvironment environment)
        {
            this.cache = cache;
            this.environment = environment;
            this.solvedPinContext = solvedPinContext;
        }

        public async Task<List<SolvedPin>> GetSolvedPins()
        {
            var solvedPins = await solvedPinContext.SolvedPins.Include(problemPin => problemPin.Images).ToListAsync();
            return solvedPins;
        }

        public async Task<SolvedPin> GetSolvedPinById(Guid id)
        {
            SolvedPin solvedPin = null;
            if (!cache.TryGetValue(id, out solvedPin))
            {
                solvedPin = await solvedPinContext.SolvedPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == id);
                if (solvedPin != null)
                {
                    cache.Set(solvedPin.Id, solvedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return solvedPin;
        }

        public async Task<ResponseDTO> EditSolvedPin(Guid oldDataId, SolvedPin newSolvedPin)
        {
            try
            {
                var foundedPin = await solvedPinContext.SolvedPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == oldDataId);
                solvedPinContext.Entry(foundedPin).CurrentValues.SetValues(newSolvedPin);
                int count = await solvedPinContext.SaveChangesAsync();
                if (count > 0)
                {
                    cache.Set(foundedPin.Id, foundedPin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return new ResponseDTO {Message="Пин изменён успешно", Status=true};
            }
            catch(Exception ex)
            {
                return new ResponseDTO { Message = "Извените на данный момент на стороне сервера ошибка, попробуйте позже", Status = false }; ;
            }
        }

        public async Task<ResponseDTO> DeleteSolvedPin(Guid oldDataId)
        {
            var foundedPin = await solvedPinContext.SolvedPins.Include(problemPin => problemPin.Images).FirstOrDefaultAsync(pins => pins.Id == oldDataId);
            foreach (ImageCustom image in foundedPin.Images)
            {
                try
                {
                    File.Delete(environment.WebRootPath + image.ImagePath);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            if (cache.TryGetValue(oldDataId, out foundedPin))
            {
                cache.Remove(foundedPin.Id);
            }
            solvedPinContext.SolvedPins.Remove(foundedPin);
            await solvedPinContext.SaveChangesAsync();
            return new ResponseDTO() { Message = "Пин успешно удалён", Status = true};
        }
    }
}
