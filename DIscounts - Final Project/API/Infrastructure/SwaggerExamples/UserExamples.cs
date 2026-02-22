using Application.DTOs.User;
using Swashbuckle.AspNetCore.Filters;

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
