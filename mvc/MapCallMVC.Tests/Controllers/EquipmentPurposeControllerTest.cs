using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;
using System.Web.Mvc;
using MMSINC;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EquipmentPurposeControllerTest : MapCallMvcControllerTestBase<EquipmentPurposeController, EquipmentPurpose>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEquipmentTypeRepository>().Use<EquipmentTypeRepository>();
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<EquipmentPurposeFactory>().Create(new {Description = "description 0"});
            var entity1 = GetFactory<EquipmentPurposeFactory>().Create(new {Description = "description 1"});
            var search = new SearchEquipmentPurpose();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<EquipmentPurposeFactory>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEquipmentPurpose, EquipmentPurpose>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<EquipmentPurpose>(eq.Id).Description);
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.ProductionEquipment;
                a.RequiresRole("~/EquipmentPurpose/ByEquipmentTypeId", module);
                a.RequiresRole("~/EquipmentPurpose/Search", module);
                a.RequiresRole("~/EquipmentPurpose/Show", module);
                a.RequiresRole("~/EquipmentPurpose/Index", module);
                a.RequiresRole("~/EquipmentPurpose/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/EquipmentPurpose/Update", module, RoleActions.Edit);
                a.RequiresRole("~/EquipmentPurpose/New", module, RoleActions.Add);
                a.RequiresRole("~/EquipmentPurpose/Create", module, RoleActions.Add);
            });
        }

        #endregion

        #region ByEquipmentTypeId

        [TestMethod]
        public void TestByEquipmentTypeIdReturnsExpectedResults()
        {
            var good = GetEntityFactory<EquipmentPurpose>().Create(new { EquipmentType = typeof(EquipmentTypeAeratorFactory) });
            var bad = GetEntityFactory<EquipmentPurpose>().Create(new { EquipmentType = typeof(EquipmentTypeEngineFactory) });

            var result = (CascadingActionResult)_target.ByEquipmentTypeId(new[] {good.EquipmentType.Id});
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(good.Id, data.Single().Id);
        }

        #endregion
    }
}
