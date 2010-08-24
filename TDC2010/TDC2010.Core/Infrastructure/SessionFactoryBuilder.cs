using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.ByteCode.LinFu;
using NHibernate.Tool.hbm2ddl;
using HibernatingRhinos.Profiler.Appender.NHibernate;

namespace TDC2010.Core.Infrastructure
{
    public class SessionFactoryBuilder
    {
        public static ISessionFactory CreateForSqlServer()
        {
            NHibernateProfiler.Initialize();
            
            return Fluently.Configure()
               .Database(
                   MsSqlConfiguration.MsSql2008
                       .ShowSql()
                       .ConnectionString(x => x.FromConnectionStringWithKey("MyDbConnection"))
                       .ProxyFactoryFactory(typeof(ProxyFactoryFactory))
                       )
               .Mappings(
                   m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(cfg =>
                {
                    cfg.Properties.Add("generate_statistics", "true");
                    new SchemaUpdate(cfg).Execute(true, true);
                })
               .BuildSessionFactory();
        }
    }
}
