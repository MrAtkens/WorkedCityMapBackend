using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("FrontPolicy")]
    [ApiController]
    public class SolvedPublicController : ControllerBase
    {
        private readonly SolvedPinService solvedPinService;
        public SolvedPublicController(SolvedPinService solvedPinService)
        {
            this.solvedPinService = solvedPinService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSolvedMapPins()
        {
            List<SolvedPin> solvedPins = await solvedPinService.GetSolvedPins();
            return Ok(new { solvedPins });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolvedMapPinById(Guid Id)
        {
            bool status = true;
            SolvedPin solvedPin = await solvedPinService.GetSolvedPinById(Id);
            if (solvedPin == null)
            {
                status = false;
            }
            return Ok(new { solvedPin, status });
        }

    }
}