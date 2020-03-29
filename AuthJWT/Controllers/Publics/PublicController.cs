using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("FrontPolicy")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly PublicPinServiceGet publicPinServiceGet;
        private readonly PublicPinServiceCRUD publicPinServiceCRUD;
        public PublicController(PublicPinServiceGet publicPinServiceGet, PublicPinServiceCRUD publicPinServiceCRUD)
        {
            this.publicPinServiceGet = publicPinServiceGet;
            this.publicPinServiceCRUD = publicPinServiceCRUD;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicMapPins()
        {
            List<ProblemPin> publicPins= await publicPinServiceGet.GetPublicPins();
            return Ok(new { publicPins });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublicMapPinById(Guid id)
        {
            bool status = true;
            ProblemPin problemPin = await publicPinServiceCRUD.GetPublicPinById(id);
            if (problemPin == null) {
                status = false;
            }
            return Ok(new { problemPin, status });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProblemPin([FromBody]ProblemPin moderateProblemPin)
        {
            bool answer = await publicPinServiceCRUD.AddPublicPin(moderateProblemPin);
            return Ok(new { answer });
        }


    }
}