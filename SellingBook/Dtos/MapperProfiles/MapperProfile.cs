using SellingBook.Dtos.BillDto.BillDetailDto;
using SellingBook.Dtos.BooksDto;
using SellingBook.Dtos.ReviewDto;
using SellingBook.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Dtos.BooksDto;

namespace SellingBook.Dtos.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<Review, ReviewsDto>();
            CreateMap<BillDetail, BillDetailsDto>();
        }
    }
}
