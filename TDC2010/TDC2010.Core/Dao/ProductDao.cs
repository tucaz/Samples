using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDC2010.Core.Domain;
using TDC2010.Core.Infrastructure;
using NHibernate.Linq;
using NHibernate;

namespace TDC2010.Core.Dao
{
    public class ProductDao
    {
        ISession dbSession = null;

        public ProductDao()
        {
            dbSession = SessionFactoryBuilder.CreateForSqlServer().OpenSession();
        }
        
        public ProductDao(ISession session)
        {
            dbSession = session;
        }
        
        public List<Product> GetAllProducts()
        {
            var q = from products in dbSession.Linq<Product>()
                    select products;

            return q.ToList();            
        }

        public Product GetById(int id)
        {
            return dbSession.Get<Product>(id);
        }
    }
}
