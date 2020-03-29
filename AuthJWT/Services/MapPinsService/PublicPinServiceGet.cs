using AuthJWT.DataAcces;
using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthJWT.Services
{
    public class PublicPinServiceGet
    {
        private readonly PinsContext context;

        public PublicPinServiceGet(PinsContext context)
        {
            this.context = context;
        }
        public async Task<List<ProblemPin>> GetPublicPins()
        {
            var publicPins = await context.ProblemPins.ToListAsync();
            return publicPins;      
        }
    }
}
