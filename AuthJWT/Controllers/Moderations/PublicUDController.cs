using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ModerationPolicy")]
    [Authorize]
    [ApiController]
    public class PublicUDController : ControllerBase
    {
        private readonly PublicPinServiceCRUD publicPinServiceCRUD;
        public PublicUDController(PublicPinServiceCRUD publicPinServiceCRUD)
        {
            this.publicPinServiceCRUD = publicPinServiceCRUD;
        }

       [HttpPatch]
       public async Task<IActionResult> EditPublicPin(Guid Id, ProblemPin problemPin)
        {
            bool answer = await publicPinServiceCRUD.EditPublicPin(Id, problemPin);
            return Ok(new { answer });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePublicPin(Guid Id)
        {
            bool answer = await publicPinServiceCRUD.DeletePublicPin(Id);
            return Ok(new { answer });
        }
        

    }
}