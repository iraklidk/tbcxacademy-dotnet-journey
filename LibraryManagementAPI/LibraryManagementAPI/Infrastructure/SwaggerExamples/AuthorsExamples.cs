using LibraryManagementAPI.DTOs.Author;
using Swashbuckle.AspNetCore.Filters;

namespace LibraryManagementAPI.Infrastructure.SwaggerExamples
{
    public class CreateAuthorRequestExample : IExamplesProvider<CreateAuthorRequest>
    {
        public CreateAuthorRequest GetExamples()
        {
            return new CreateAuthorRequest
            {
                FirstName = "Jemal",
                LastName = "Karchkhadze",
                Biography = "Georgian writer",
                DateOfBirth = new DateTime(1936, 10, 10)
            };
        }
    }

    public class UpdateAuthorRequestExample : IExamplesProvider<UpdateAuthorRequest>
    {
        public UpdateAuthorRequest GetExamples()
        {
            return new UpdateAuthorRequest
            {
                FirstName = "George",
                LastName = "Orwell",
                Biography = "Updated biography text.",
                DateOfBirth = new DateTime(1903, 6, 25)
            };
        }
    }
}
