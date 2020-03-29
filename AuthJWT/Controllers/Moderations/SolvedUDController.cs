using System;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers.Moderations
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ModerationPolicy")]
    //[Authorize]
    [ApiController]
    public class SolvedUDController : ControllerBase
    {
        private readonly SolvedPinService solvedPinService;
        public SolvedUDController(SolvedPinService solvedPinService)
        {
            this.solvedPinService = solvedPinService;
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditSolvedPin(Guid Id, SolvedPin solvedPin)
        {
            bool answer = await solvedPinService.EditSolvedPin(Id, solvedPin);
            return Ok(new { answer });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolvedPin(Guid Id)
        {
            bool answer = await solvedPinService.DeleteSolvedPin(Id);
            return Ok(new { answer });
        }
    }
}