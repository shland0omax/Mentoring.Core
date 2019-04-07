using AutoMapper;
using Mentoring.Core.Services.Models;
using Mentoring.Core.Module1.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Mentoring.Core.Module1.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Supplier, SupplierViewModel>();

            CreateMap<ProductViewModel, Product>();
            CreateMap<SupplierViewModel, Supplier>();
        }
    }
}
