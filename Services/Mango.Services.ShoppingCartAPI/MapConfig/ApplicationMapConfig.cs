using AutoMapper;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Api;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Messages;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;

namespace Mango.Services.ShoppingCartAPI.MapConfig
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            // SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            // DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<CartProductIncoming, CartProduct>().ReverseMap();
            CreateMap<CartItemIncoming, CartItem>().ReverseMap();

            CreateMap<CartOutgoing, Cart>().ReverseMap();
            CreateMap<CartItemOutgoing, CartItem>().ReverseMap();
            CreateMap<CartProductOutgoing, CartProduct>().ReverseMap();

            CreateMap<CheckoutApi, CheckoutMessage>();
        }
    }
}
