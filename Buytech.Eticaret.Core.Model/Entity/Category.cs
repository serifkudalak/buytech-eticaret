using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buytech.Eticaret.Core.Model.Entity
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
