using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buytech.Eticaret.Core.Model.Entity
{
    public class UserBasketAllDetail
    {
        public List<Basket> Baskets { get; set; }
        public User Users { get; set; }
        public Product Products { get; set; }
    }
}
