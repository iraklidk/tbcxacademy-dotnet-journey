using Swashbuckle.AspNetCore.Filters;
using Application.DTOs.User;

namespace API.Infrastructure.SwaggerExamples;

public class UserDtoExamples : IExamplesProvider<UserDto>
{
    public UserDto GetExamples()
    {
        return new UserDto
        {
            Id = 5,
            UserName = "irakli_dolbaia",
            Email = "dolbaia@example.com",
        };
    }
}
