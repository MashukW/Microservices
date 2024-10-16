﻿using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models.Entities;
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

            CreateMap<Cart, CartOutgoing>();
            CreateMap<CartItem, CartItemOutgoing>();
            CreateMap<CartProduct, CartProductOutgoing>();

            CreateMap<CheckoutIncoming, CheckoutMessage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.CartItems.Count));
            CreateMap<CartItemIncoming, CartItemMessage>();
            CreateMap<CartProductIncoming, CartProductMessage>();

            CreateMap<CartItemIncoming, CartItem>()
                .ForMember(dest => dest.PublicId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<CartProductIncoming, CartProduct>();
        }
    }
}
