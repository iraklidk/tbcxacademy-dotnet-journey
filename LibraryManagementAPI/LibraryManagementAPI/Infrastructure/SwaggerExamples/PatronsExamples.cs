using LibraryManagementAPI.DTOs.Patrons;
using Swashbuckle.AspNetCore.Filters;

namespace LibraryManagementAPI.Infrastructure.SwaggerExamples
{
    public class CreatePatronRequestExample : IExamplesProvider<CreatePatronRequest>
    {
        public CreatePatronRequest GetExamples()
        {
            return new CreatePatronRequest
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@email.com",
                PhoneNumber = "+1-555-123-4567"
            };
        }
    }

    public class UpdatePatronRequestExample : IExamplesProvider<UpdatePatronRequest>
    {
        public UpdatePatronRequest GetExamples()
        {
            return new UpdatePatronRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@email.com",
                PhoneNumber = "+1-555-987-6543"
            };
        }
    }
}
