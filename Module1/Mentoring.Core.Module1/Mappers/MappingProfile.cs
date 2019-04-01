using AutoMapper;
using Mentoring.Core.Data.Models;
using Mentoring.Core.Module1.Models;

namespace Mentoring.Core.Module1.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Products, ProductViewModel>();
            CreateMap<Categories, CategoryViewModel>();
            CreateMap<Suppliers, SupplierViewModel>();

            CreateMap<ProductViewModel, Products>();
            CreateMap<CategoryViewModel, Categories>();
            CreateMap<SupplierViewModel, Suppliers>();
        }
    }
}
