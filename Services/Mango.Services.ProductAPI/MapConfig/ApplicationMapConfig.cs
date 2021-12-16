using AutoMapper;
using Mango.Services.ProductAPI.Database.Entities;
using Mango.Services.ProductAPI.Models.Api;

namespace Mango.Services.ProductAPI
{
    public class ApplicationMapConfig : Profile
    {
        public ApplicationMapConfig()
        {
            // SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            // DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<ProductApi, Product>();
            CreateMap<Product, ProductApi>();
        }
    }
}
