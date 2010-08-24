using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;
using TDC2010.Core.Domain;
using NHibernate.Criterion;
using TDC2010.Core.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace TDC2010.Tests
{
    [TestFixture]
    public class report_test
    {
        ISession mySession = null;

        [SetUp]
        public void create_session()
        {
            mySession = SessionFactoryBuilder.CreateForSqlServer().OpenSession();
        }

        [Test]
        public void can_load_only_some_columns()
        {
            var list = mySession.CreateCriteria(typeof(Product))
                .SetProjection(
                    Projections.ProjectionList()
                    .Add(Projections.Property("Id"))
                    .Add(Projections.Property("Name"))
                    .Add(Projections.Property("cat.Name"))
                )
                .CreateAlias("Categories", "cat")
                .List();

            Assert.That(list != null && list.Count > 0);
        }

        [TearDown]
        public void close_db_session()
        {
            mySession.Close();
            mySession.Dispose();
        }

    }
}
