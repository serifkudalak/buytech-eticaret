using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buytech.Eticaret.Core.Model.Entity
{
    public class ProductAllDetails
    {
        public Product Products { get; set; }
        public List<Product> AllProducts { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<ProductComment> ProductComments { get; set; }
        public ProductDetail ProductDetails { get; set; }
    }
}
