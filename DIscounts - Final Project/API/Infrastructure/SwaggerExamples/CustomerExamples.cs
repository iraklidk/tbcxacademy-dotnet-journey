using Application.DTOs.Customer;
using Swashbuckle.AspNetCore.Filters;

namespace API.Infrastructure.SwaggerExamples;

public class CustomerExamples
{
    public class CustomerDtoExample : IExamplesProvider<CustomerDto>
    {
        public CustomerDto GetExamples()
        {
            return new CustomerDto
            {
                Id = 1,
                UserId = 10,
                Balance = 100.50m,
                Firstname = "RaghacSaxeli",
                Lastname = "RaghacGvari"
            };
        }
    }

    public class CreateCustomerDtoExample : IExamplesProvider<CreateCustomerDto>
    {
        public CreateCustomerDto GetExamples()
        {
            return new CreateCustomerDto
            {
                UserId = 10,
                Firstname = "SomeName",
                Lastname = "SomeLastname",
                Balance = 100.50m
            };
        }
    }

    public class UpdateCustomerDtoExample : IExamplesProvider<UpdateCustomerDto>
    {
        public UpdateCustomerDto GetExamples()
        {
            return new UpdateCustomerDto
            {
                Id = 7,
                Firstname = "NEwName",
                Lastname = "NewLastName",
            };
        }
    }
}
