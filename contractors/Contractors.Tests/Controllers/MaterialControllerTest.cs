using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;
using MaterialRepository = Contractors.Data.Models.Repositories.MaterialRepository;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class MaterialControllerTest : ContractorControllerTestBase<MaterialController, Material, MaterialRepository>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override FakeMvcHttpHandler GetRequestHandler()
        {
            // This override is needed or else all of the tests blow up
            // because an action can't automatically be found for for the tests.
            return Application.CreateRequestHandler("~/Material/MaterialSearchByMaterialUsedId");
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/Material/MaterialSearchByMaterialUsedId");
                a.RequiresLoggedInUserOnly("~/Material/MaterialSearchByWorkOrderId");
            });
        }

        [TestMethod]
        public void TestMaterialSearchesUseHttpGetOnly()
        {
            MyAssert.MethodHasAttribute<HttpGetAttribute>(_target,
                "MaterialSearchByMaterialUsedId",
                new[] { typeof(string), typeof(int) });
            MyAssert.MethodHasAttribute<HttpGetAttribute>(_target,
                "MaterialSearchByWorkOrderId",
                new[] { typeof(string), typeof(int) });
        }

        [TestMethod]
        public void TestMaterialSearchByMaterialUsedIdReturnsMaterialsAsJson()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var workOrder =
                GetFactory<WorkOrderFactory>().Create(new
                {
                    AssignedContractor = _currentUser.Contractor,
                    OperatingCenter = operatingCenter
                });
            var materials = new[] {
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675309", Description = "some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675310", Description = "some other thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675311", Description = "nothing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234567", Description = "also some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234568", Description = "also nothing"})
            };
            var materialUsed =
                GetFactory<MaterialUsedFactory>().Create(
                    new { Material = materials[0], WorkOrder = workOrder });
            materials.Each(m =>
            {
                operatingCenter.StockedMaterials.Add(
                    new OperatingCenterStockedMaterial
                    {
                        OperatingCenter = operatingCenter,
                        Material = m
                    });
                Session.Merge(operatingCenter);
            });
            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            Session.Merge(_currentUser.Contractor);
            Session.Flush();
            var result = _target.MaterialSearchByMaterialUsedId("867", materialUsed.Id) as JsonResult;
            Assert.IsNotNull(result);
            var actual = new JavaScriptSerializer().Serialize(result.Data);

            materials
                .Slice(0, 2)
                .Each(m => Assert.IsTrue(actual
                    .Contains(String
                        .Format("\"Text\":\"{0} - {1}\",\"Value\":\"{2}\"",
                            m.PartNumber, m.Description, m.Id))));
        }

        [TestMethod]
        public void TestMaterialSearchByWorkOrderIdReturnsMaterialsAsJson()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var workOrder =
                GetFactory<WorkOrderFactory>().Create(
                    new { AssignedContractor = _currentUser.Contractor, OperatingCenter = operatingCenter });
            var materials = new[] {
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675309", Description = "some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675310", Description = "some other thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675311", Description = "nothing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234567", Description = "also some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234568", Description = "also nothing"})
            };
            var materialUsed =
                GetFactory<MaterialUsedFactory>().Create(
                    new { Material = materials[0], WorkOrder = workOrder });
            materials.Each(m =>
            {
                operatingCenter.StockedMaterials.Add(
                    new OperatingCenterStockedMaterial
                    {
                        OperatingCenter = operatingCenter,
                        Material = m
                    });
                Session.Merge(operatingCenter);
            });
            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            Session.Merge(_currentUser.Contractor);
            Session.Flush();
            var result = _target.MaterialSearchByWorkOrderId("867", materialUsed.WorkOrder.Id) as JsonResult;
            Assert.IsNotNull(result);
            var actual = new JavaScriptSerializer().Serialize(result.Data);

            materials
                .Slice(0, 2)
                .Each(m => Assert.IsTrue(actual
                    .Contains(String
                        .Format("\"Text\":\"{0} - {1}\",\"Value\":\"{2}\"",
                            m.PartNumber, m.Description, m.Id))));
        }

        #endregion
    }
}
