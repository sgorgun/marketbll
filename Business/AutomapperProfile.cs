using AutoMapper;
using Business.Models;
using Data.Entities;
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

            //TODO: Create mapping for Product and ProductModel

            //TODO: Create mapping for ReceiptDetail and ReceiptDetailModel

            //TODO: Create mapping that combines Customer and Person into CustomerModel

            //TODO: Create mapping for ProductCategory and ProductCategoryModel

        }
    }
}