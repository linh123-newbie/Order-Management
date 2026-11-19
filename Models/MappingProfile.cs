using System;
using AutoMapper;
using OrderManagement.Models.DTOs;
// using OrderManagement.Models.Entities;
namespace OrderManagement.Models;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>()
        .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ReverseMap();
        CreateMap<Order, RevenueReportDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.DerivedOrders, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.Price * src.Quantity))
            .ReverseMap();
    }
}
