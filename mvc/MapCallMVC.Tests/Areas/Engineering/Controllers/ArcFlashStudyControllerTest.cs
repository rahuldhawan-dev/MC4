using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Engineering.Controllers;
using MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.Engineering.Controllers
{
    [TestClass]
    public class ArcFlashStudyControllerTest : MapCallMvcControllerTestBase<ArcFlashStudyController, ArcFlashStudy, IRepository<ArcFlashStudy>>
    {
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                return GetEntityFactory<ArcFlashStudy>().Create(new {
                    ArcFlashStatus = typeof(CompletedArcFlashStatusFactory),
                    ArcFlashContractor = "Some arc flash contractor", // This is required when Status is Completed.
                    CostToComplete = 2m, // This is required when Status is Completed.
                    PowerPhase = typeof(EntityLookupTestDataFactory<PowerPhase>),
                    Voltage = typeof(EntityLookupTestDataFactory<Voltage>),
                    TransformerKVARating = typeof(EntityLookupTestDataFactory<UtilityTransformerKVARating>)
                });
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateArcFlashStudy)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ArcFlashStudyController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Engineering/ArcFlashStudy/Search/", role);
                a.RequiresRole("~/Engineering/ArcFlashStudy/Show/", role);
                a.RequiresRole("~/Engineering/ArcFlashStudy/Index/", role);
                a.RequiresRole("~/Engineering/ArcFlashStudy/New/", role, RoleActions.Add);
                a.RequiresRole("~/Engineering/ArcFlashStudy/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Engineering/ArcFlashStudy/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Engineering/ArcFlashStudy/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Engineering/ArcFlashStudy/Destroy/", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ArcFlashStudy>().Create(new { ArcFlashNotes = "ArcFlashNotes 0" });
            var entity1 = GetEntityFactory<ArcFlashStudy>().Create(new { ArcFlashNotes = "ArcFlashNotes 1" });
            var search = new SearchArcFlashStudy();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ArcFlashNotes, "ArcFlashNotes");
                helper.AreEqual(entity1.ArcFlashNotes, "ArcFlashNotes", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ArcFlashStudy>().Create();
            var expected = "ArcFlashNotes field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditArcFlashStudy, ArcFlashStudy>(eq, new {
                ArcFlashNotes = expected
            }));

            Assert.AreEqual(expected, Session.Get<ArcFlashStudy>(eq.Id).ArcFlashNotes);
        }

        #endregion
    }
}
