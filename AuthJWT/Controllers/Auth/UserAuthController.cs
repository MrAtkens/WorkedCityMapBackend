using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Helpers;
using AuthJWT.Models;
using AuthJWT.Services;
using DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twilio.Rest.Trunking.V1;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly ILogger<UserAuthController> logger;
        private readonly SmsService smsService;
        private readonly UserAuthService userAuthService;
        public UserAuthController(UserAuthService userAuthService, SmsService smsService, ILogger<UserAuthController> logger)
        {
            this.logger = logger;
            this.userAuthService = userAuthService;
            this.smsService = smsService;
        }

        [HttpPost]
        public async Task<IActionResult> AcceptPhone([FromBody]AcceptPhoneDTO acceptPhoneDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                ResponseDTO answer = await userAuthService.CheckUser(acceptPhoneDTO.PhoneNumber);
                if (answer.Status == false)
                {
                    return Ok(new { answer });
                }

                string verificationCode = AuthHelpers.RandomString(6);
                answer = await smsService.SendVerificationCode(acceptPhoneDTO.PhoneNumber, verificationCode);

                if(answer.Status == false)
                {
                    return StatusCode(500, answer);
                }

                return Ok(new { answer });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromBody]RegistartionUserDTO registartionDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                ResponseDTO answer = await userAuthService.Registration(registartionDTO);

                if (answer.Status == false)
                {
                    return StatusCode(400, answer);
                }

                AuthUserDTO authDTO = new AuthUserDTO() { Phone = registartionDTO.Phone, Password = registartionDTO.Password };

                User user = await userAuthService.Authenticate(authDTO);

                if (string.IsNullOrEmpty(user.Token))
                {
                    return Unauthorized(new { status = false });
                }

                return Ok(new { user, answer });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task <IActionResult> Authenticate([FromBody]AuthUserDTO authDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                User user = await userAuthService.Authenticate(authDTO);

                if (user == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                return Ok(new { user });
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }
    }
}