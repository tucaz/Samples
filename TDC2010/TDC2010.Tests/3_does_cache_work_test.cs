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
    public class does_cache_work_test
    {
        ISession mySession = null;

        [SetUp]
        public void create_session()
        {
            mySession = SessionFactoryBuilder.CreateForSqlServer().OpenSession();
        }

        [Test]
        public void should_load_entities_only_once()
        {
            //Statistics should be on!
            var myDao = new ProductDao(mySession);

            var myProduct = myDao.GetById(1);
            var sameProductAgain = myDao.GetById(1);

            Assert.That(1 == mySession.Statistics.EntityCount);
        }

        [TearDown]
        public void close_db_session()
        {
            mySession.Close();
            mySession.Dispose();
        }
    }
}
