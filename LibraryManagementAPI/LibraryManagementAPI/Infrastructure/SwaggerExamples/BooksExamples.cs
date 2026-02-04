using LibraryManagementAPI.DTOs.Book;
using Swashbuckle.AspNetCore.Filters;

namespace LibraryManagementAPI.Infrastructure.SwaggerExamples
{
    public class CreateBookRequestExample : IExamplesProvider<CreateBookRequest>
    {
        public CreateBookRequest GetExamples()
        {
            return new CreateBookRequest
            {
                Title = "New Book Title",
                ISBN = "9781234567890",
                PublicationYear = 2024,
                Description = "An example description",
                Quantity = 5,
                Author = "Firstname Lastname",
            };
        }
    }

    public class UpdateBookRequestExample : IExamplesProvider<UpdateBookRequest>
    {
        public UpdateBookRequest GetExamples()
        {
            return new UpdateBookRequest
            {
                Title = "Updated Book Title",
                ISBN = "9780987654321",
                PublicationYear = 2025,
                Description = "Updated description",
                Quantity = 10,
                Author = "Firstname Lastname",
            };
        }
    }
}
