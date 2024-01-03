using System;
using System.Net;
using System.ServiceModel.Channels;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using Moq;

namespace MapCall.SAP.Model.Repositories.Tests
{
    [TestClass]
    public class SAPHttpClientTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPEquipmentRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            //_container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());
            //_target = _container.GetInstance<SAPEquipmentRepository>();
            //_sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();
            _container = new Container();
        }

        [TestMethod]
        public void TestIsSiteRunningFailsIfSiteIsntRunning()
        {
            var target = new SAPHttpClient {BaseAddress = new Uri("http://invalidlocalserverhost")};

            Assert.IsFalse(target.IsSiteRunning);
        }

        [TestMethod]
        public void TestIsSiteRunningReturnsTrueWhenSiteIsRunning()
        {
            var baseAddress = "http://localhost:2020/";
            var target = new SAPHttpClient {BaseAddress = new Uri(baseAddress)};

            var server = _container.GetInstance<HttpListener>();
            server.Prefixes.Add(baseAddress);
            server.Start();

            var actual = target.IsSiteRunning;
            server.Stop();

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestSecurityProtocol()
        {
            var target = _container.GetInstance<TestSAPHttpClient>();

            var CustomBinding = target.TestBindings();

            Assert.AreEqual("Tls12", ServicePointManager.SecurityProtocol.ToString());
        }
    }

    public class TestSAPHttpClient : SAPHttpClient
    {
        public CustomBinding TestBindings()
        {
            return Bindings();
        }
    }
}
