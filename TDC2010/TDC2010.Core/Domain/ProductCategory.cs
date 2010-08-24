using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDC2010.Core.Domain
{
    public class ProductCategory
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
