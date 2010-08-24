using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;
using TDC2010.Core.Infrastructure;
using TDC2010.Core.Dao;

namespace TDC2010.Tests
{
    [TestFixture]
    public class is_lazy_loaded_test
    {
        ISession mySession = null;
        
        [SetUp]
        public void create_session()
        {
            mySession = SessionFactoryBuilder.CreateForSqlServer().OpenSession();
        }
        
        [Test]
        public void product_category_should_not_be_loaded_before_called()
        {
            var myDao = new ProductDao(mySession);
            var myProduct = myDao.GetById(1);

            var isInitialized = NHibernateUtil.IsInitialized(myProduct.Categories);

            Assert.IsFalse(isInitialized);
        }

        [Test]
        public void product_category_should_be_loaded_after_called()
        {
            var myDao = new ProductDao(mySession);
            var myProduct = myDao.GetById(1);

            var categoriesCount = myProduct.Categories.Count;

            var isInitialized = NHibernateUtil.IsInitialized(myProduct.Categories);

            Assert.IsTrue(isInitialized);
        }

        [TearDown]
        public void close_db_session()
        {
            mySession.Close();
            mySession.Dispose();
        }
    }
}
