using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyReflectionGeneric;

namespace MyTestsIOC
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ioc = new Container();
            ioc.For<ILogger>().Use<SqlServerLogger>();
            var logger = ioc.Resolve<ILogger>();

            Assert.AreEqual(typeof(SqlServerLogger), logger.GetType());
        }

        [TestMethod]
        public void TestMethod2()
        {
            var ioc = new Container();
            //here we are saying for a ILogger i want to use a SQLServerLogger.
            ioc.For<ILogger>().Use<SqlServerLogger>();
            ioc.For<IRepository<Employee>>().Use<SqlRepository<Employee>>();
            var repos = ioc.Resolve<IRepository<Employee>>(); 

            Assert.AreEqual(typeof(SqlRepository<Employee>), repos.GetType());
        }
        [TestMethod]
        public void TestMethod3()
        {
            var ioc = new Container();
            //here we are saying for a ILogger i want to use a SQLServerLogger.
            ioc.For<ILogger>().Use<SqlServerLogger>();
            ioc.For(typeof(IRepository<>)).Use(typeof(SqlRepository<>));
            var service = ioc.Resolve<InvoiceService>();

            Assert.AreEqual(typeof(InvoiceService), service.GetType());
        }
    }
}
