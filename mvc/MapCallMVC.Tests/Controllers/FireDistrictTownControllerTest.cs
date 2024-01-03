using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FireDistrictTownControllerTest : MapCallMvcControllerTestBase<FireDistrictTownController, FireDistrictTown>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IFireDistrictRepository>().Use<FireDistrictRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // NOTE: This is a real hack to make this work with the automatic tests and
            // you should probably not do this. This is needed because the model's TownId
            // is set somewhere else(the view?) and the automatic tests have no way of
            // knowing that.
            FireDistrictTown lastValidFDT = null;
            options.CreateValidEntity = () => {
                lastValidFDT = GetEntityFactory<FireDistrictTown>().Create();
                return lastValidFDT;
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateFireDistrictTown)vm;
                model.TownId = lastValidFDT.Town.Id;
            };
            options.CreateRedirectsToReferrerOnSuccess = true;
            options.CreateRedirectSuccessUrlFragment = "#FireDistrictsTab";
            options.DestroyRedirectsToReferrerOnError = true;
            options.DestroyRedirectErrorUrlFragment = "#FireDistrictsTab";
            options.DestroyRedirectsToReferrerOnSuccess = true;
            options.DestroyRedirectSuccessUrlFragment = "#FireDistrictsTab";
            options.UpdateRedirectsToReferrerOnSuccess = true;
            options.UpdateRedirectSuccessUrlFragment = "#FireDistrictsTab";
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateRemovesOldDefaultWhenNewRecordShouldBeDefault()
        {
            var town = GetFactory<TownFactory>().Create();
            var existing = new[] {
                GetFactory<FireDistrictTownFactory>().Create(new {Town = town}),
                GetFactory<FireDistrictTownFactory>().Create(new {Town = town, IsDefault = true})
            };
            var model = new CreateFireDistrictTown(_container) {
                FireDistrict = GetFactory<FireDistrictFactory>().Create().Id,
                TownId = town.Id,
                IsDefault = true
            };

            Session.Flush();
            Session.Clear();

            _target.Create(model);

            foreach (var record in existing)
            {
                Assert.IsFalse(Session.Load<FireDistrictTown>(record.Id).IsDefault);
            }
        }

        [TestMethod]
        public void TestCreateThrowsModelValidationExceptionIfModelIsInvalid()
        {
            var district = GetFactory<FireDistrictFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            _target.ModelState.AddModelError("oops!", "oh noes!!");

            MyAssert.Throws<ModelValidationException>(
                () => _target.Create(new CreateFireDistrictTown(_container) {
                    FireDistrict = district.Id,
                    TownId = town.Id
                }));
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Doesn't return validation result to client.");
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            // noop: This throws an error instead and is tested below.
        }

        [TestMethod]
        public void TestUpdateThrowsModelValidationExceptionIfModelStateIsNotValid()
        {
            var model = _viewModelFactory.Build<MakeDefaultFireDistrictTown, FireDistrictTown>(GetFactory<FireDistrictTownFactory>().Create());
            _target.ModelState.AddModelError("oops", ":(");
            
            MyAssert.Throws<ModelValidationException>(() => _target.Update(model));
        }

        [TestMethod]
        public void TestUpdateMakesFireDistrictTownTheDefault()
        {
            var model = _viewModelFactory.Build<MakeDefaultFireDistrictTown, FireDistrictTown>(GetFactory<FireDistrictTownFactory>().Create());

            _target.Update(model);

            Assert.IsTrue(Session.Load<FireDistrictTown>(model.Id).IsDefault);
        }

        [TestMethod]
        public void TestUpdateRemovesDefaultStatusFromExistingRecords()
        {
            var town = GetFactory<TownFactory>().Create();
            var existing = new[] {
                GetFactory<FireDistrictTownFactory>().Create(new {Town = town}),
                GetFactory<FireDistrictTownFactory>().Create(new {Town = town, IsDefault = true})
            };
            var model = _viewModelFactory.Build<MakeDefaultFireDistrictTown, FireDistrictTown>(GetFactory<FireDistrictTownFactory>().Create(new {Town = town}));

            Session.Flush();
            Session.Clear();

            _target.Update(model);
            
            foreach (var record in existing)
            {
                Assert.IsFalse(Session.Load<FireDistrictTown>(record.Id).IsDefault);
            }
        }

        #endregion
        
        #region Roles
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/FireDistrictTown/Create/", module, RoleActions.Read);
                a.RequiresRole("~/FireDistrictTown/Update/", module, RoleActions.Read);
                a.RequiresRole("~/FireDistrictTown/Destroy/", module, RoleActions.Read);
            });
        }

        #endregion
    }
}
