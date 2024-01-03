using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.CommonTest.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EstimatingProjectMaterialTest : InMemoryDatabaseTest<EstimatingProjectMaterial>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(EstimatingProjectFactory).Assembly);
        }

        [TestMethod]
        public void TestTotalCostReturnsCostTimesQuantity()
        {
            var material = GetEntityFactory<Material>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
               .Create(new {Material = material, OperatingCenter = operatingCenter, Cost = 5m});
            var ep = GetEntityFactory<EstimatingProject>().Create(new {OperatingCenter = operatingCenter});
            var epMaterial =
                GetEntityFactory<EstimatingProjectMaterial>()
                   .Create(new {EstimatingProject = ep, Material = material, Quantity = 2});
            Session.Flush();
            Session.Clear();
            epMaterial = Session.Load<EstimatingProjectMaterial>(epMaterial.Id);

            Assert.AreEqual(10, epMaterial.TotalCost);
        }
    }
}
