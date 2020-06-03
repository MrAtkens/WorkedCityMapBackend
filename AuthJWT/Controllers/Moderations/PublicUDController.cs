using System;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Options;
using AuthJWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [Authorize(Roles = Role.Moderator)]
    [ApiController]
    public class PublicUDController : ControllerBase
    {
        private readonly ILogger<PublicUDController> logger;
        private readonly PublicPinServiceCRUD publicPinServiceCRUD;
        public PublicUDController(ILogger<PublicUDController> logger, PublicPinServiceCRUD publicPinServiceCRUD)
        {
            this.logger = logger;
            this.publicPinServiceCRUD = publicPinServiceCRUD;

            logger.LogDebug(1, "NLog injected into PublicUDController");
        }

        [HttpPatch("{id}")]
       public async Task<IActionResult> EditPublicPin(Guid Id, [FromBody] ProblemPin problemPin)
        {
            try
            {
                bool answer = await publicPinServiceCRUD.EditPublicPin(Id, problemPin);
                return Ok(new { answer });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicPin(Guid Id)
        {
            try
            {
                bool answer = await publicPinServiceCRUD.DeletePublicPin(Id);
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