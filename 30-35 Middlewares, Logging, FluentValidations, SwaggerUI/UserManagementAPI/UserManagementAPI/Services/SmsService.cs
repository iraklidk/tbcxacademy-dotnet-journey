namespace UserManagementAPI.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class SmsService
{
    private readonly IConfiguration _config;

    public SmsService(IConfiguration config)
    {
        _config = config;
    }

    public void SendOtp(string toPhone, string otp)
    {
        TwilioClient.Init(
            _config["Twilio:AccountSid"],
            _config["Twilio:AuthToken"]
        );

        MessageResource.Create(
            body: $"ager inebe sheni ertjeradi kodi {otp}",
            from: new Twilio.Types.PhoneNumber(_config["Twilio:FromPhone"]),
            to: new Twilio.Types.PhoneNumber(toPhone)
        );
    }
}
