using API.Models.Request;
using API.Models.Response;
using API.Models.ViewModels;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identitiy;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

            CreateMap<Address, AddressResponse>().ReverseMap();

            CreateMap<CustomerBasketRequest, CustomerBasket>();
            CreateMap<BasketItemViewModel, BasketItem>();
        }
    }
}