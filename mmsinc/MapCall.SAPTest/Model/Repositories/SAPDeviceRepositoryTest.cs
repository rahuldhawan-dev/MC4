using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Entities.Tests;
using MapCall.SAP.Model.Repositories;
using SAP.DataTest.Model.Entities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.SAP.Model.Repositories;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPDeviceRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPDeviceRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());

            _target = _container.GetInstance<SAPDeviceRepository>();

            _sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();
        }
    }
}
