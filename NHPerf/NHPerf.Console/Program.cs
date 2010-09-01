using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHPerf.Console;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.ByteCode.LinFu;
using System.Reflection;
using NHibernate.Linq;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate.Criterion;
using System.Collections;

namespace NFPer
{
    class Program
    {
        private static int numberOfIterations = 1000;

        static void Main(string[] args)
        {
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine("Test " + i.ToString());
                Console.WriteLine("======================");

                SqlAdHocAllProducts();
                SqlAdHocAllProductsWithReflection();
                SProcAllProducts();
                NHibernateAllProductsWithLinq();
                NHibernateAllProductsWithHql();
                NHibernateAllProductsWithCriteria();
                NHibernateAllProductsWithProjections();

                SqlAdHocOneProduct();
                SqlAdHocOneProductWithReflection();
                SProcOneProduct();
                NHibernateOneProduct();

                Console.WriteLine();
            }

            Console.ReadLine();
        }

        #region AllProducts

        private static void SqlAdHocAllProducts()
        {
            List<Product> allProducts = null;

            var connectionString = "Data Source=(local);Integrated Security=SSPI;Database=TDC2010;";
            var select = @"SELECT P.Id, P.Description, P.Name, P.Price FROM dbo.Product P";

            var connection = new SqlConnection(connectionString);
            connection.Open();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                allProducts = new List<Product>();
                var command = new SqlCommand(select, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    allProducts.Add(new Product()
                        {

                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            Description = Convert.ToString(reader["Description"]),
                            Price = Convert.ToDecimal(reader["price"])
                        });
                }

                reader.Close();
            }

            watch.Stop();

            connection.Close();
            connection.Dispose();

            Console.WriteLine(
                "Loading " + allProducts.Count + " Products with Sql AdHoc took " + watch.ElapsedMilliseconds + " ms");
        }

        private static void SqlAdHocAllProductsWithReflection()
        {
            List<Product> allProducts = null;

            var connectionString = "Data Source=(local);Integrated Security=SSPI;Database=TDC2010;";
            var select = @"SELECT P.Id, P.Description, P.Name, P.Price FROM dbo.Product P";

            var connection = new SqlConnection(connectionString);
            connection.Open();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                allProducts = new List<Product>();
                var command = new SqlCommand(select, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var newProduct = Activator.CreateInstance<Product>();
                    SetProperty(newProduct, "Id", Convert.ToInt32(reader["Id"]));
                    SetProperty(newProduct, "Description", Convert.ToString(reader["Description"]));
                    SetProperty(newProduct, "Name", Convert.ToString(reader["Name"]));
                    SetProperty(newProduct, "Price", Convert.ToDecimal(reader["Price"]));
                    allProducts.Add(newProduct);
                }

                reader.Close();
            }

            watch.Stop();

            connection.Close();
            connection.Dispose();

            Console.WriteLine(
                "Loading " + allProducts.Count + " Products with Sql AdHoc and Reflection took " + watch.ElapsedMilliseconds + " ms");
        }

        private static void SProcAllProducts()
        {
            List<Product> allProducts = null;

            var connectionString = "Data Source=(local);Integrated Security=SSPI;Database=TDC2010;";
            var select = "SPS_ALL_Products";

            var connection = new SqlConnection(connectionString);
            connection.Open();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                allProducts = new List<Product>();
                var command = new SqlCommand(select, connection);
                command.CommandType = CommandType.StoredProcedure;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    allProducts.Add(new Product()
                    {

                        Id = Convert.ToInt32(reader["Id"]),
                        Name = Convert.ToString(reader["Name"]),
                        Description = Convert.ToString(reader["Description"]),
                        Price = Convert.ToDecimal(reader["price"])
                    });
                }

                reader.Close();
            }

            watch.Stop();

            connection.Close();
            connection.Dispose();

            Console.WriteLine(
                "Loading " + allProducts.Count + " Products with SProc " + watch.ElapsedMilliseconds + " ms");
        }

        private static void NHibernateAllProductsWithLinq()
        {
            List<Product> allProducts = null;

            var session = CreateForSqlServer().OpenSession();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                allProducts = session.Linq<Product>().ToList();
            }

            watch.Stop();

            session.Close();

            Console.WriteLine(
                "Loading " + allProducts.Count + " Products with NHibernate took " + watch.ElapsedMilliseconds + " ms");
        }

        private static void NHibernateAllProductsWithHql()
        {
            IList<Product> allProducts = null;

            var session = CreateForSqlServer().OpenSession();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                allProducts = session.CreateQuery("from Product prod")
                    .List<Product>();
            }

            watch.Stop();

            session.Close();

            Console.WriteLine(
                "Loading " + allProducts.Count + " Products with NHibernate HQL took " + watch.ElapsedMilliseconds + " ms");
        }

        private static void NHibernateAllProductsWithCriteria()
        {
            IList<Product> allProducts = null;

            var session = CreateForSqlServer().OpenSession();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                allProducts = session.CreateCriteria<Product>()
                    .List<Product>();
            }

            watch.Stop();

            session.Close();

            Console.WriteLine(
                "Loading " + allProducts.Count + " Products with NHibernate Criteria took " + watch.ElapsedMilliseconds + " ms");
        }

        private static void NHibernateAllProductsWithProjections()
        {
            IList allProducts = null;

            var session = CreateForSqlServer().OpenSession();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                allProducts = session.CreateCriteria<Product>()
                    .SetProjection(
                        Projections.ProjectionList()
                        .Add(Projections.Property("Id"))
                        .Add(Projections.Property("Name"))
                        .Add(Projections.Property("Description"))
                        .Add(Projections.Property("Price"))
                    )
                    .List();
            }

            watch.Stop();

            session.Close();

            Console.WriteLine(
                "Loading " + allProducts.Count + " Products with NHibernate Projections took " + watch.ElapsedMilliseconds + " ms");
        }

        #endregion

        #region OneProduct

        private static void SqlAdHocOneProduct()
        {
            Product oneProduct = null;

            var connectionString = "Data Source=(local);Integrated Security=SSPI;Database=TDC2010;";
            var select = @"SELECT P.Id, P.Description, P.Name, P.Price FROM dbo.Product P
                WHERE P.Id = @ProductId";

            var connection = new SqlConnection(connectionString);
            connection.Open();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                var command = new SqlCommand(select, connection);
                command.Parameters.Add(new SqlParameter("@ProductId", 1));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    oneProduct = new Product()
                    {

                        Id = Convert.ToInt32(reader["Id"]),
                        Name = Convert.ToString(reader["Name"]),
                        Description = Convert.ToString(reader["Description"]),
                        Price = Convert.ToDecimal(reader["price"])
                    };
                }

                reader.Close();
            }

            watch.Stop();

            connection.Close();
            connection.Dispose();

            Console.WriteLine(
                "Loading 1 Product with Sql AdHoc took " + watch.ElapsedMilliseconds + " ms");
        }

        private static void SqlAdHocOneProductWithReflection()
        {
            Product oneProduct = null;

            var connectionString = "Data Source=(local);Integrated Security=SSPI;Database=TDC2010;";
            var select = @"SELECT P.Id, P.Description, P.Name, P.Price FROM dbo.Product P
                WHERE P.Id = @ProductId";

            var connection = new SqlConnection(connectionString);
            connection.Open();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                var command = new SqlCommand(select, connection);
                command.Parameters.Add(new SqlParameter("@ProductId", 1));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    oneProduct = Activator.CreateInstance<Product>();
                    SetProperty(oneProduct, "Id", Convert.ToInt32(reader["Id"]));
                    SetProperty(oneProduct, "Description", Convert.ToString(reader["Description"]));
                    SetProperty(oneProduct, "Name", Convert.ToString(reader["Name"]));
                    SetProperty(oneProduct, "Price", Convert.ToDecimal(reader["Price"]));                    
                }

                reader.Close();
            }

            watch.Stop();

            connection.Close();
            connection.Dispose();

            Console.WriteLine(
                "Loading 1 Product with Sql AdHoc and Reflection took " + watch.ElapsedMilliseconds + " ms");
        }

        private static void SProcOneProduct()
        {
            Product oneProduct = null;

            var connectionString = "Data Source=(local);Integrated Security=SSPI;Database=TDC2010;";
            var select = "SPS_One_Product";

            var connection = new SqlConnection(connectionString);
            connection.Open();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                var command = new SqlCommand(select, connection);
                command.Parameters.Add(new SqlParameter("@ProductId", 1));
                command.CommandType = CommandType.StoredProcedure;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    oneProduct = new Product()
                    {

                        Id = Convert.ToInt32(reader["Id"]),
                        Name = Convert.ToString(reader["Name"]),
                        Description = Convert.ToString(reader["Description"]),
                        Price = Convert.ToDecimal(reader["price"])
                    };
                }

                reader.Close();
            }

            watch.Stop();

            connection.Close();
            connection.Dispose();

            Console.WriteLine(
                "Loading 1 Product with SProc " + watch.ElapsedMilliseconds + " ms");
        }

        private static void NHibernateOneProduct()
        {
            Product oneProduct = null;

            var session = CreateForSqlServer().OpenSession();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numberOfIterations; i++)
            {
                oneProduct = session.Get<Product>(1);
            }

            watch.Stop();

            session.Close();

            Console.WriteLine(
                "Loading 1 Product with NHibernate took " + watch.ElapsedMilliseconds + " ms");
        }

        #endregion

        private static ISessionFactory CreateForSqlServer()
        {
            return Fluently.Configure()
               .Database(
                   MsSqlConfiguration.MsSql2008
                       .ConnectionString("Data Source=(local);Integrated Security=SSPI;Database=TDC2010;")
                       .ProxyFactoryFactory(typeof(ProxyFactoryFactory))
                       )
               .Mappings(
                   m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(cfg =>
                {
                })
               .BuildSessionFactory();
        }

        private static void SetProperty(object instance, string property, object val)
        {
            Type t = instance.GetType();
            var prop = t.GetProperty(property, BindingFlags.Instance | BindingFlags.Public);
            prop.SetValue(instance, val, null);
        }
    }
}
