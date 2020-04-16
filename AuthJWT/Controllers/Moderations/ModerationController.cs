using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ModerationPolicy")]
    //[Authorize]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly ILogger<ModerationController> logger;
        private readonly ModerationPinService moderationPinService;
        public ModerationController(ILogger<ModerationController> logger, ModerationPinService moderationPinService)
        {
            this.logger = logger;
            this.moderationPinService = moderationPinService;

            logger.LogDebug(1, "NLog injected into ModerationController");
        }

        [HttpGet]
        public async Task<IActionResult> GetModerationPins()
        {
            
            try
            {
                List<ProblemPin> moderateProblemPins = await moderationPinService.GetModerationPins();
                return Ok(new { moderateProblemPins, status = true });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500,new { status = false});
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModerationPinById(Guid id)
        {
            try
            {
                ProblemPin moderateProblemPin = await moderationPinService.GetModerationPinById(id);
                if (moderateProblemPin == null)
                {
                    logger.LogInformation($"GetModerationPinById dont found Id: {id}");
                    return NotFound(new { status = false });
                }
                return Ok(new { moderateProblemPin, status = true });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptPublicPin([FromBody]AcceptDTO acceptDTO)
        {
            try
            {
                bool answer = await moderationPinService.ModerationAcceptPin(acceptDTO);
                return Ok(new { answer });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptSolvedPin([FromBody]SolvedPinDTO solvedPinDTO)
        {
            try
            {
                bool answer = await moderationPinService.SolvedProblemPinAccept(solvedPinDTO);
                return Ok(new { answer });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> EditModerationPin(Guid Id, ProblemPin moderateProblemPin)
        {
            try
            {
                bool answer = await moderationPinService.EditModerationPin(Id, moderateProblemPin);
                return Ok(new { answer });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModerationPin(Guid Id)
        {
            try
            {
                bool answer = await moderationPinService.DeleteModerationPin(Id);
                return Ok(new { answer });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

    }
}