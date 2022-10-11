using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buytech.Eticaret.Core.Model.Entity
{
    public class User : EntityBase
    {
        public string NameSurname { get; set; }
        public string GlobalNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string TransferNumber { get; set; }
        public string Photo { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }

        public virtual IEnumerable<ProductComment> ProductComments { get; set; }
    }
}
