using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHPerf.Console
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
    }
}
