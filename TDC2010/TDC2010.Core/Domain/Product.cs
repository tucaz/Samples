using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDC2010.Core.Domain
{
    public class Product
    {
        public Product()
        {
            Categories = new List<ProductCategory>();
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }

        public virtual IList<ProductCategory> Categories { get; set; }
    }
}
