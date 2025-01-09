using AutoMapper;
using E_Book_Recommendation.Common.DTO;
using E_Book_Recommendation.Common.DTO.Response;
using E_Book_Recommendation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Book_Recommendation.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Author, AuthorResponse>().ReverseMap();
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<AuthorDto, AuthorResponse>().ReverseMap();
            CreateMap<Book, BookResponse>().ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author.AuthorName)).ReverseMap(); 
            CreateMap<Book, AuthorResponse>().ReverseMap();
            CreateMap<Recommendation, RecommendationDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));
        }
    }

}
