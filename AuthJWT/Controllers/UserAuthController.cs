using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly UserAuthService userAuthService;
        public UserAuthController(UserAuthService userAuthService)
        {
            this.userAuthService = userAuthService;
        }

       /* Я на время консервирую проект!!! На данный момент идёт подготовка к ЕНТ 2020 и сейчас я вынужден уйти от проекта на 2 недели
        сейчас 03.06.2020 вернуться к проекту я должен 21.06.2020. Делаю последний комит и ухожу. Когда приду нужно все return в контролерах поменять
        на ResponseDTO это очень важно так я смогу достичь независимости библеотеки Services от контролеров. Так что не забудь, ещё ты должен доделать
        авторизацию и аутентификацию пользователей на сайт на данный момент прикручен Twilio для проверки. Но сама авторизация не сделана*/

        [HttpPost]
        public async Task<IActionResult> Registration(AuthDTO authDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var answer = await userAuthService.Registration(authDTO);

            if (answer == false)
            {
                return StatusCode(500);
            }

            User user = await userAuthService.Authenticate(authDTO);

            if (string.IsNullOrEmpty(user.Token))
            {
                return Unauthorized(new {status = false});
            }

            return Ok(new { user });
        }

        [HttpPost]
        public async Task <IActionResult> Authenticate(AuthDTO authDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

           User user = await userAuthService.Authenticate(authDTO);

            if (string.IsNullOrEmpty(user.Token))
            {
                return Unauthorized(new { status = false });
            }

            return Ok(new { user });
        }
    }
}