using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TDC2010.Core.Domain;

namespace TDC2010.Core.Infrastructure.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id)
                .Not.Nullable()
                .GeneratedBy.Identity();

            Map(x => x.Name)
                .Not.Nullable()
                .Length(60);

            Map(x => x.Email)
                .Not.Nullable()
                .Length(150)
                .Access.LowerCaseField(Prefix.Underscore);
        }
    }
}
