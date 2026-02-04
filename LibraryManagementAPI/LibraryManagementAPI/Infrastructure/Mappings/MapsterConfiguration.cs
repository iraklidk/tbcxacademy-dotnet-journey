using LibraryManagement.Application.DTOs;
using LibraryManagementAPI.DTOs.Book;
using LibraryManagementAPI.DTOs.Patrons;
using Mapster;

namespace LibraryManagement.API.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<CreateBookRequest, CreateBookDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<UpdateBookRequest, UpdateBookDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<BookDto, BookResponse>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.ISBN, src => src.ISBN)
                .Map(dest => dest.AuthorId, src => src.AuthorId)
                .TwoWays();

            TypeAdapterConfig<UpdateBookDto, Book>
                .NewConfig()
                .Ignore(dest => dest.Author);

            TypeAdapterConfig<CreateBookDto, Book>
                .NewConfig()
                .Ignore(dest => dest.Author);

            //TypeAdapterConfig<CreateBookDto, Book>
            //    .NewConfig()
            //    .Map(dest => dest.Author, src => MapStringToAuthor(src.Author));

            TypeAdapterConfig<CreatePatronRequest, CreatePatronDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<UpdatePatronRequest, UpdatePatronDto>
                .NewConfig()
                .TwoWays();
        }

        //public static Author MapStringToAuthor(string authorString)
        //{
        //    if (string.IsNullOrWhiteSpace(authorString))
        //        return null;

        //    var names = authorString.Trim().Split(' ', 2);
        //    return new Author
        //    {
        //        FirstName = names[0],
        //        LastName = names.Length > 1 ? names[1] : ""
        //    };
        //}
    }
}
