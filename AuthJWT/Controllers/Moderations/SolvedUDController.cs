using System;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthJWT.Controllers.Moderations
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ModerationPolicy")]
    //[Authorize]
    [ApiController]
    public class SolvedUDController : ControllerBase
    {
        private readonly ILogger<PublicUDController> logger;
        private readonly SolvedPinService solvedPinService;
        public SolvedUDController(ILogger<PublicUDController> logger, SolvedPinService solvedPinService)
        {
            this.logger = logger;
            this.solvedPinService = solvedPinService;

            logger.LogDebug(1, "NLog injected into SolvedUDController");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditSolvedPin(Guid Id, SolvedPin solvedPin)
        {
            try
            {
                bool answer = await solvedPinService.EditSolvedPin(Id, solvedPin);
                return Ok(new { answer });
            }
            catch (Exception ex) {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolvedPin(Guid Id)
        {
            try
            {
                bool answer = await solvedPinService.DeleteSolvedPin(Id);
                return Ok(new { answer });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }
    }
}