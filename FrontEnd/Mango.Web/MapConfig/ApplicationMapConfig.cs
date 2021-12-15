using AutoMapper;
using Mango.Web.Models.Api.Carts;
using Mango.Web.Models.Api.Products;
using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Products;

namespace Mango.Web.MapConfig
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            MapCartMembers();
            MapProductMembers();
        }

        private void MapCartMembers()
        {
            CreateMap<CartApi, CartView>().ReverseMap();
            CreateMap<CartItemApi, CartItemView>().ReverseMap();
            CreateMap<CartProductApi, CartProductView>().ReverseMap();
        }

        private void MapProductMembers()
        {
            CreateMap<ProductApi, ProductView>().ReverseMap();
            CreateMap<ProductView, CartProductView>().ReverseMap();
            CreateMap<ProductApi, CartProductView>().ReverseMap();
        }
    }
}
