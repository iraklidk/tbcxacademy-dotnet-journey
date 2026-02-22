using Application.DTOs.Category;
using Swashbuckle.AspNetCore.Filters;

namespace API.Infrastructure.SwaggerExamples;

public class CreateCategoryDtoExample : IExamplesProvider<CreateCategoryDto>
{
    public CreateCategoryDto GetExamples()
    {
        return new CreateCategoryDto
        {
            Name = "Electronics",
            Description = "Category for electronic products"
        };
    }
}

public class UpdateCategoryDtoExample : IExamplesProvider<UpdateCategoryDto>
{
    public UpdateCategoryDto GetExamples()
    {
        return new UpdateCategoryDto
        {
            Id = 777,
            Name = "Updated Electronics",
            Description = "Updated description for electronics category"
        };
    }
}

public class CategoryDtoExample : IExamplesProvider<CategoryDto>
{
    public CategoryDto GetExamples()
    {
        return new CategoryDto
        {
            Id = 1,
            Name = "Electronics",
            Description = "Category for electronic products"
        };
    }
}
