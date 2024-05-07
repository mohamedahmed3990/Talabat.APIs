using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity;

namespace Talabat.Core.Specification.ProductSpec
{
    public class ProductsWithFilterationForCountSpecification : BaseSpecification<Product>
    {
        public ProductsWithFilterationForCountSpecification(ProductSpecParams specParams)
            :base(P =>
                        (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                        (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                        (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
                 )
        {
            
        }
    }
}
