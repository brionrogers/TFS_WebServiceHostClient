using System;
using NUnit.Framework;
using SelfHost2.ServiceInterface;
using SelfHost2.ServiceModel;
using ServiceStack.Testing;
using ServiceStack;

namespace SelfHost2.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            var service = appHost.Container.Resolve<MyServices>();

            var response = (HelloResponse)service.Any(new TFS { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }
    }
}
