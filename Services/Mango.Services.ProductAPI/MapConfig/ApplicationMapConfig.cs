using AutoMapper;
using Mango.Services.ProductAPI.Database.Entities;
using Mango.Services.ProductAPI.Models.Dto;

namespace Mango.Services.ProductAPI
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            // SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            // DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}
