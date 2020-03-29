using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ModerationPolicy")]
    //[Authorize]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly ModerationPinService moderationPinService;
        public ModerationController(ModerationPinService moderationPinService)
        {
            this.moderationPinService = moderationPinService;
        }

        [HttpGet]
        public async Task<IActionResult> GetModerationPins()
        {
            List<ProblemPin> moderateProblemPins = await moderationPinService.GetModerationPins();
            return Ok(new { moderateProblemPins });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModerationPinById(Guid Id)
        {
            bool status = true;
            ProblemPin moderateProblemPin = await moderationPinService.GetModerationPinById(Id);
            if (moderateProblemPin == null)
            {
                status = false;
            }
            return Ok(new { moderateProblemPin, status });
        }

        [HttpPost]
        public async Task<IActionResult> AcceptPublicPin([FromBody]AcceptDTO acceptDTO)
        {
            bool answer = await moderationPinService.ModerationAcceptPin(acceptDTO);
            return Ok(new { answer });
        }

        [HttpPost]
        public async Task<IActionResult> AcceptSolvedPin([FromBody]SolvedPinDTO solvedPinDTO)
        {
            bool answer = await moderationPinService.SolvedProblemPinAccept(solvedPinDTO);
            return Ok(new { answer });
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> EditModerationPin(Guid Id, ProblemPin moderateProblemPin)
        {
            bool answer = await moderationPinService.EditModerationPin(Id, moderateProblemPin);
            return Ok(new { answer });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModerationPin(Guid Id)
        {
            bool answer = await moderationPinService.DeleteModerationPin(Id);
            return Ok(new { answer });
        }

    }
}