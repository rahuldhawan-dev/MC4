using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingModuleControllerTest : MapCallMvcControllerTestBase<TrainingModuleController,TrainingModule>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<TrainingModuleController>();
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.OperationsTrainingModules;
                a.RequiresRole("~/TrainingModule/GetByOperatingCenter/", module);
                a.RequiresRole("~/TrainingModule/Search/", module);
                a.RequiresRole("~/TrainingModule/Show/", module);
                a.RequiresRole("~/TrainingModule/Index/", module);
                a.RequiresRole("~/TrainingModule/New/", module, RoleActions.Add);
                a.RequiresRole("~/TrainingModule/Create/", module, RoleActions.Add);
                a.RequiresRole("~/TrainingModule/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/TrainingModule/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/TrainingModule/Destroy/", module, RoleActions.Delete);
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowSetsTrainingRequirementDropDownData()
        {
            var module = GetFactory<TrainingModuleFactory>().Create();
            var expected = module.TrainingRequirement;
            _target.Show(module.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["TrainingRequirement"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<TrainingModuleFactory>().Create(new {Description = "description 0"});
            var entity1 = GetFactory<TrainingModuleFactory>().Create(new {Description = "description 1"});
            var search = new SearchTrainingModule();
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
            var eq = GetFactory<TrainingModuleFactory>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrainingModule, TrainingModule>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<TrainingModule>(eq.Id).Description);
        }

        #endregion
    }
}
