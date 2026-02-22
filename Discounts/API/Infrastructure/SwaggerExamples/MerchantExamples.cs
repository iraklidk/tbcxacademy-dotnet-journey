using Application.DTOs.Merchant;
using Swashbuckle.AspNetCore.Filters;

namespace API.Infrastructure.SwaggerExamples;

public class MerchantExamples
{
    public class MerchantResponseDtoExample : IExamplesProvider<MerchantResponseDto>
    {
        public MerchantResponseDto GetExamples()
        {
            return new MerchantResponseDto
            {
                Id = 1,
                UserId = 10,
                Name = "SuperShop",
            };
        }
    }

    public class CreateMerchantDtoExample : IExamplesProvider<CreateMerchantDto>
    {
        public CreateMerchantDto GetExamples()
        {
            return new CreateMerchantDto
            {
                UserId = 10,
                Name = "SuperShop",
            };
        }
    }

    public class UpdateMerchantDtoExample : IExamplesProvider<UpdateMerchantDto>
    {
        public UpdateMerchantDto GetExamples()
        {
            return new UpdateMerchantDto
            {
                Id = 7,
                Name = "SuperShop",
            };
        }
    }
}
