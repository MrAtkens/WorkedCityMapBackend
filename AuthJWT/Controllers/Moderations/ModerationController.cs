using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ModerationPolicy")]
    [Authorize]
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
            List<ProblemPin> moderationPins = await moderationPinService.GetModerationPins();
            return Ok(new { moderationPins });
        }

        [HttpGet]
        public async Task<IActionResult> GetModerationPinsById(Guid Id)
        {
            ProblemPin problemPin = await moderationPinService.GetModerationPinById(Id);
            return Ok(new { problemPin });
        }

        [HttpPost]
        public async Task<IActionResult> AcceptPublicPin(Guid Id)
        {
            bool answer = await moderationPinService.ModerationAcceptPin(Id);
            return Ok(new { answer });
        }

        [HttpPost]
        public async Task<IActionResult> AcceptSolvedPin(Guid Id, SolvedPinDTO solvedPinDTO)
        {
            bool answer = await moderationPinService.SolvedProblemPinAccept(Id, solvedPinDTO);
            return Ok(new { answer });
        }


        [HttpPatch]
        public async Task<IActionResult> EditModerationPin(Guid Id, ProblemPin problemPin)
        {
            bool answer = await moderationPinService.EditModerationPin(Id, problemPin);
            return Ok(new { answer });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePublicPin(Guid Id)
        {
            bool answer = await moderationPinService.DeleteModerationPin(Id);
            return Ok(new { answer });
        }

    }
}