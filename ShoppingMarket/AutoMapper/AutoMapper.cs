using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShoppingMarket.Models;
using ShoppingMarket.Models.DTOS;
namespace ShoppingMarket.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<ProductDTO, Product>().
            ForMember(des=> des.ProductImage1,src=> src.MapFrom(src => 
            FormFileToByteArray(src.ProductImageFile1)))
           .ForMember(des => des.ProductImage2, src => src.MapFrom(src =>
            FormFileToByteArray(src.ProductImageFile2))).
            ForMember(des => des.ProductImage3, src => src.MapFrom(src =>
            FormFileToByteArray(src.ProductImageFile3)))
           .ReverseMap();
            CreateMap<OrderDTO, Order>()
             .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetailsDto))
             .AfterMap((src, dest) =>
             {
                 foreach (var detail in dest.OrderDetails)
                 {
                     detail.OrderId = src.Id;
                 }
             })
             .ReverseMap()
             .ForMember(dest => dest.OrderDetailsDto, opt => opt.MapFrom(src => src.OrderDetails))
             .AfterMap((src, dest) =>
             {
                 foreach (var detail in dest.OrderDetailsDto)
                 {
                     detail.OrderId = src.Id;
                 }
             });

            CreateMap<OrderDetailsDTO, OrderDetails>().ReverseMap();
            CreateMap<CustomerDTO, Customer>().ReverseMap();
            CreateMap<Cart, CartDTO>();
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<Favorite, FavoriteDTO>();
            CreateMap<Product, ProductDTO>();





        }

        private static byte[] FormFileToByteArray(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        }
}
