using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDC2010.Core.Domain;
using FluentNHibernate.Mapping;

namespace TDC2010.Core.Infrastructure.Mapping
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Id(x => x.Id)
                .Not.Nullable()
                .GeneratedBy.Identity();

            Map(x => x.Description)
                .Not.Nullable()
                .Length(255);

            Map(x => x.Name)
                .Not.Nullable()
                .Length(50);

            Map(x => x.Price)
                .Not.Nullable();

            HasManyToMany<ProductCategory>(x => x.Categories)
                .Cascade.None()
                .ParentKeyColumn("ProductId")
                .ChildKeyColumn("ProductCategoryId");
        }
    }
}
