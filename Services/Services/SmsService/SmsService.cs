using AuthJWT.DataAcces;
using DTOs.DTOs;
using DTOs.DTOs.Auth;
using Models.Models.System;
using Newtonsoft.Json;
using Options.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthJWT.Services
{
    public class SmsService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly UserContext userContext;

        public SmsService(UserContext userContext)
        {
            this.userContext = userContext;
        }
        public async Task<ResponseDTO> SendVerificationCode(string phoneNumber, string code)
        {
            var values = new Dictionary<string, string>
            {
                { "recipient", phoneNumber },
                { "text", $"Код подтверждение: {code}" },
            };
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync($"https://api.mobizon.kz/service/message/sendSmsMessage?output=json&api=v1&apiKey={Mobizon.MobizonApiKey}",
            content);

            string responseString = await response.Content.ReadAsStringAsync();
            MobizonDTO answer = JsonConvert.DeserializeObject<MobizonDTO>(responseString);
            
            if (answer.Code != 0){
                return new ResponseDTO()
                {
                    Message = "Извините на данный момент мы не можем отправить смс из проблем на стороне сервера, пожалуйста попробуйте позже",
                    Status = false
                };
            }
            else
            {
                await userContext.Codes.AddAsync(new VerificationCode() { PhoneNumber = phoneNumber, Code = code });
                await userContext.SaveChangesAsync();
                return new ResponseDTO() { Message = "Смс с кодом верефикация отправлен успешно", Status = true };
            }
        }
    }
}