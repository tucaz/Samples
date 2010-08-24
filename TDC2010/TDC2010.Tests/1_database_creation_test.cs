using NUnit.Framework;
using TDC2010.Core.Infrastructure;

namespace TDC2010.Tests
{
    [TestFixture]
    public class database_creation_test
    {        
        [Test]
        public void table_creation_succeeded()
        {     
            Assert.DoesNotThrow(new TestDelegate(create_tables));
        }

        private void create_tables()
        {
            SessionFactoryBuilder.CreateForSqlServer();
        }
    }
}
