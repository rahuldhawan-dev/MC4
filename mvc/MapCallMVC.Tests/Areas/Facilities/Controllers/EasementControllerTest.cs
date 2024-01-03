using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels.Easements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class EasementControllerTest : MapCallMvcControllerTestBase<EasementController, Easement>
    {
        #region Init
        
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateEasement)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.Coordinate = GetEntityFactory<Coordinate>().Create().Id;
                model.Status = GetEntityFactory<EasementStatus>().Create().Id;
                model.Category = GetEntityFactory<EasementCategory>().Create().Id;
                model.Type = GetEntityFactory<EasementType>().Create().Id;
                model.GrantorType = GetEntityFactory<GrantorType>().Create().Id;
                model.DateRecorded = DateTime.Now;
                model.RecordNumber = "Some record number";
                model.EasementDescription = "Some description";
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditEasement)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.Coordinate = GetEntityFactory<Coordinate>().Create().Id;
                model.Status = GetEntityFactory<EasementStatus>().Create().Id;
                model.Category = GetEntityFactory<EasementCategory>().Create().Id;
                model.Type = GetEntityFactory<EasementType>().Create().Id;
                model.GrantorType = GetEntityFactory<GrantorType>().Create().Id;
                model.DateRecorded = DateTime.Now;
                model.RecordNumber = "Some record number";
                model.EasementDescription = "Some description";
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = EasementController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/Easement/Search/", role);
                a.RequiresRole("~/Facilities/Easement/Show/", role);
                a.RequiresRole("~/Facilities/Easement/Index/", role);
                a.RequiresRole("~/Facilities/Easement/New/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/Easement/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/Easement/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/Easement/Update/", role, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Easement>().Create();
            var expected = "Testing Easement Description";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEasement, Easement>(eq, new {
                EasementDescription = expected
            }));

            Assert.AreEqual(expected, Session.Get<Easement>(eq.Id).EasementDescription);
        }

        #endregion

        #endregion
    }
}
