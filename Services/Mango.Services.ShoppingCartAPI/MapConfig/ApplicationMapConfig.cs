using AutoMapper;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Api;
using Mango.Services.ShoppingCartAPI.Models.Messages;

namespace Mango.Services.ShoppingCartAPI.MapConfig
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            // SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            // DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<CartApi, Cart>().ReverseMap();
            CreateMap<CartItemApi, CartItem>().ReverseMap();
            CreateMap<CartProductApi, CartProduct>().ReverseMap();

            CreateMap<CheckoutApi, CheckoutMessage>();
        }
    }
}
