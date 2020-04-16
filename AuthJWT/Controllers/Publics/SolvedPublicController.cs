using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("FrontPolicy")]
    [ApiController]
    public class SolvedPublicController : ControllerBase
    {
        private readonly ILogger<SolvedPublicController> logger;
        private readonly SolvedPinService solvedPinService;
        public SolvedPublicController(ILogger<SolvedPublicController> logger, SolvedPinService solvedPinService)
        {
            this.logger = logger;
            this.solvedPinService = solvedPinService;

            logger.LogDebug(1, "NLog injected into SolvedPublicController");
        }

        [HttpGet]
        public async Task<IActionResult> GetSolvedMapPins()
        {
            try
            {
                List<SolvedPin> solvedPins = await solvedPinService.GetSolvedPins();
                return Ok(new { solvedPins, status = true});
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolvedMapPinById(Guid id)
        {
            try
            {
                SolvedPin solvedPin = await solvedPinService.GetSolvedPinById(id);
                if (solvedPin == null)
                {
                    logger.LogInformation($"GetSolvedMapPinById dont found Id: {id}");
                    return NotFound(new { status = false });
                }
                return Ok(new { solvedPin, status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

    }
}