using AutoMapper;
using Mango.Web.Models.Api.Carts;
using Mango.Web.Models.Api.Checkouts;
using Mango.Web.Models.Api.Products;
using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Checkouts;
using Mango.Web.Models.View.Products;

namespace Mango.Web.MapConfig
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            MapCartMembers();
            MapProductMembers();
            MapCheckoutMembers();
        }

        private void MapProductMembers()
        {
            CreateMap<ProductApi, ProductView>().ReverseMap();
            CreateMap<ProductView, CartProductView>().ReverseMap();
            CreateMap<ProductApi, CartProductView>().ReverseMap();
        }

        private void MapCartMembers()
        {
            CreateMap<CartApi, CartView>().ReverseMap();
            CreateMap<CartItemApi, CartItemView>().ReverseMap();
            CreateMap<CartProductApi, CartProductView>().ReverseMap();
        }

        private void MapCheckoutMembers()
        {
            CreateMap<CheckoutView, CheckoutApi>()
                .ForMember(dest => dest.CartItems, src => src.MapFrom(x => x.CartItems))
                .ForMember(dest => dest.CouponCode, src => src.MapFrom(x => x.CouponCode))
                .ForMember(dest => dest.DiscountAmount, src => src.MapFrom(x => x.DiscountAmount))
                .ForMember(dest => dest.TotalCost, src => src.MapFrom(x => x.TotalCost))
                .ForMember(dest => dest.FirstName, src => src.MapFrom(x => x.UserDetails.FirstName))
                .ForMember(dest => dest.LastName, src => src.MapFrom(x => x.UserDetails.LastName))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.UserDetails.PhoneNumber))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.UserDetails.Email))
                .ForMember(dest => dest.CardNumber, src => src.MapFrom(x => x.PaymentCard.CardNumber))
                .ForMember(dest => dest.Cvv, src => src.MapFrom(x => x.PaymentCard.Cvv))
                .ForMember(dest => dest.ExpityMonthYear, src => src.MapFrom(x => x.PaymentCard.ExpityMonthYear));
        }
    }
}
