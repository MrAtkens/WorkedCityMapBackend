using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace AspNetShop.Services
{
    public class TwilioSmsService
    {
        public async Task<bool> SendVerificationCode(string phoneNumber, string code)
        {
            try
            {
                const string accountSid = "AC8ae5a37332e855c9f2193b87e052e208";
                const string authToken = "10052bcc88070307ac108f2ad5904336";

                TwilioClient.Init(accountSid, authToken);

                await MessageResource.CreateAsync(
                    body: "Verification code:" + code,
                    from: new Twilio.Types.PhoneNumber("+12532317832"),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}