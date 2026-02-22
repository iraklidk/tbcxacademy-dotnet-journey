using Application.DTOs.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace API.Infrastructure.SwaggerExamples;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest
        {
            UserName = "irakli_dolbaia",
            Password = "SecureP@ss123"
        };
    }
}

public class LoginResponseExample : IExamplesProvider<LoginResponse>
{
    public LoginResponse GetExamples()
    {
        return new LoginResponse
        {
            UserId = 5,
            UserName = "irakli_dolbaia",
        };
    }
}

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples()
    {
        return new RegisterRequest
        {
            Email = "irakli@example.com",
            PhoneNumber = "+1234567890",
            UserName = "irakli_d",
            Password = "SecureP@ss123"
        };
    }
}
