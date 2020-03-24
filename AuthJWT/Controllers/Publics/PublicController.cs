using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IActionResult> GetPublicMapPinById(Guid Id)
        {
            ProblemPin problemPin = await publicPinServiceCRUD.GetPublicPinById(Id);
            return Ok(new { problemPin });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProblemPin(ProblemPinDTO problemPinDTO)
        {
            bool answer = await publicPinServiceCRUD.AddPublicPin(problemPinDTO);
            return Ok(new { answer });
        }


    }
}