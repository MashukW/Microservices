using AutoMapper;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.MapConfig
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            // SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            // DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<CartDto, Cart>().ReverseMap();
            CreateMap<CartItemDto, CartItem>().ReverseMap();
            CreateMap<CartProductDto, CartProduct>().ReverseMap();

        }
    }
}
