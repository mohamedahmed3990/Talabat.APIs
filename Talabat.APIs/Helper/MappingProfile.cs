using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entity;
using Talabat.Core.Entity.Order_Aggregate;

using userAddress = Talabat.Core.Entity.Identity.Address;


namespace Talabat.APIs.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(PD => PD.ProductBrand, O => O.MapFrom(P => P.Brand.Name))
                .ForMember(PD => PD.ProductCategory, O => O.MapFrom(P => P.Category.Name))
                .ForMember(PD => PD.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());


            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>(); 
            CreateMap<AddressDto, Address>();

            CreateMap<userAddress, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(d => d.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(d => d.Product.ProductName))               
                .ForMember(d => d.PictureUrl, o => o.MapFrom(d => d.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
