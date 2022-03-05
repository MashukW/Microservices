using AutoMapper;
using Mango.Services.OrderAPI.Database.Entities;
using Mango.Services.OrderAPI.Models.Messages;

namespace Mango.Services.OrderAPI.MapConfig
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            CreateMap<Order, OrderOutgoing>();
            CreateMap<OrderItem, OrderItemOutgoing>();

            CreateMap<OrderMessage, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.OrderDateTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                // .ForMember(dest => dest.OrderItems, opt => opt.MapFrom((src, c, a, s) => s.Mapper.Map<List<OrderItem>>(src.OrderItems)))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => false));

            CreateMap<OrderItemMessage, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPublicId, opt => opt.MapFrom(src => src.Product.PublicId));
        }
    }
}
