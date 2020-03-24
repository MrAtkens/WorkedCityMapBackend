using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers.Moderations
{
    [Route("api/[controller]")]
    [EnableCors("ModerationPolicy")]
    [Authorize]
    [ApiController]
    public class SolvedUDController : ControllerBase
    {
        private readonly SolvedPinService solvedPinService;
        public SolvedUDController(SolvedPinService solvedPinService)
        {
            this.solvedPinService = solvedPinService;
        }

        [HttpPatch]
        public async Task<IActionResult> EditSolvedPin(Guid Id, SolvedPin solvedPin)
        {
            bool answer = await solvedPinService.EditSolvedPin(Id, solvedPin);
            return Ok(new { answer });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePublicPin(Guid Id)
        {
            bool answer = await solvedPinService.DeleteSolvedPin(Id);
            return Ok(new { answer });
        }
    }
}