using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDC2010.Core.Domain;
using FluentNHibernate.Mapping;

namespace TDC2010.Core.Infrastructure.Mapping
{
    public class ProductCategoryMap : ClassMap<ProductCategory>
    {
        public ProductCategoryMap()
        {
            Id(x => x.Id);

            Map(x => x.Name)
                .Length(100)
                .Not.Nullable();

            Map(x => x.Description, "CategoryDescription")
                .Length(255)
                .Not.Nullable();
        }
    }
}
