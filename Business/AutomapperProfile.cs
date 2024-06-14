using AutoMapper;
using Business.Models;
using Data.Entities;
using System;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ReverseMap();

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();

            CreateMap<Tuple<Person, Customer>, CustomerModel>()
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Item2.Receipts.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(rd => rd.Id)))
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ReverseMap();

            CreateMap<Person, CustomerModel>()
                .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pm => pm.ProductIds, p => p.MapFrom(x => x.Products.Select(rd => rd.Id)))
                .ReverseMap();
        }
    }
}