using System;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ModerationPolicy")]
    //[Authorize]
    [ApiController]
    public class PublicUDController : ControllerBase
    {
        private readonly PublicPinServiceCRUD publicPinServiceCRUD;
        public PublicUDController(PublicPinServiceCRUD publicPinServiceCRUD)
        {
            this.publicPinServiceCRUD = publicPinServiceCRUD;
        }

       [HttpPatch("{id}")]
       public async Task<IActionResult> EditPublicPin(Guid Id, [FromBody] ProblemPin problemPin)
        {
            bool answer = await publicPinServiceCRUD.EditPublicPin(Id, problemPin);
            return Ok(new { answer });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicPin(Guid Id)
        {
            bool answer = await publicPinServiceCRUD.DeletePublicPin(Id);
            return Ok(new { answer });
        }
        

    }
}