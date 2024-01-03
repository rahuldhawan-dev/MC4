using System;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class MarkoutControllerTest : ContractorControllerTestBase<MarkoutController, Markout, MarkoutRepository>
    {
        #region Setup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var markoutRequirement = GetFactory<MarkoutRequirementRoutineFactory>().Create();
                var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                    AssignedContractor = _currentUser.Contractor, 
                    MarkoutRequirement = markoutRequirement
                });

                // TODO: This isn't even creating a MarkoutType entry. It never inserts into the test db.
                var markoutType = GetFactory<MarkoutTypeFactory>().Create();
                return GetFactory<MarkoutFactory>().Create(new {
                    WorkOrder = workOrder,
                    MarkoutType = markoutType,
                    DateOfRequest = DateTime.Today,
                });
            };

            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateMarkout)vm;
                model.Note = "Some note";
                model.MarkoutType = GetFactory<MarkoutTypeFactory>().Create().Id;
            };

            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditMarkout)vm;
                model.Note = "Some note";
            };

            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ExpectedNewViewName = "_New";
            options.ExpectedShowViewName = "_Show";
            options.NewReturnsPartialView = true;
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
        }

        #endregion

        #region Private Methods

        private void SetupDropDownData()
        {
            GetFactory<MarkoutTypeFactory>().CreateList(1);
        }
  
        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Markout/New");
                a.RequiresLoggedInUserOnly("~/Markout/Create");
                a.RequiresLoggedInUserOnly("~/Markout/Edit");
                a.RequiresLoggedInUserOnly("~/Markout/Update");
                a.RequiresLoggedInUserOnly("~/Markout/Show");
              //  a.RequiresLoggedInUserOnly("~/Markout/Index");
                a.RequiresLoggedInUserOnly("~/Markout/Destroy");
            });
        }

        #endregion

        #region Create/New

        [TestMethod]
        public void TestCreateCreatesAMarkoutWithNecessaryValues()
        {
            // Leaving this test due to the extra checks with the work day engine.
            var markoutRequirement = GetFactory<MarkoutRequirementRoutineFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor, MarkoutRequirement = markoutRequirement });
            var markoutType = GetFactory<MarkoutTypeFactory>().Create();
            var markout = GetFactory<MarkoutFactory>().Build(new {
                WorkOrder = workOrder,
                MarkoutType = markoutType,
                DateOfRequest = DateTime.Today
            });
            var model = _viewModelFactory.Build<CreateMarkout, Markout>(markout);

            var result = (PartialViewResult)_target.Create(model);
            var resultModel = (Markout)result.Model;

            Assert.AreEqual(
                WorkOrdersWorkDayEngine.GetReadyDate(markout.DateOfRequest.Value,
                    markoutRequirement.MarkoutRequirementEnum),
                resultModel.ReadyDate);
            Assert.AreEqual(
                WorkOrdersWorkDayEngine.GetExpirationDate(markout.DateOfRequest.Value,
                    markoutRequirement.MarkoutRequirementEnum),
                resultModel.ExpirationDate);
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action
            SetupDropDownData();
            var workOrderID = 919;

            var result = (PartialViewResult)_target.New(workOrderID);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(CreateMarkout));
            Assert.AreEqual(((CreateMarkout)result.Model).DateOfRequest, DateTime.Today);
            Assert.AreEqual(((CreateMarkout)result.Model).WorkOrder, workOrderID);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            SetupDropDownData();
            var markoutRequirement = GetFactory<MarkoutRequirementRoutineFactory>().Create();
            var newMarkoutType = GetFactory<MarkoutTypeFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>()
                .Create(new {
                    AssignedContractor = _currentUser.Contractor,
                    MarkoutRequirement = markoutRequirement
                });
            var markout = GetFactory<MarkoutFactory>()
                .Create(new {
                    WorkOrder = workOrder, DateOfRequest = DateTime.Today
                });
            var model = _viewModelFactory.BuildWithOverrides<EditMarkout, Markout>(markout, new {
                MarkoutType = newMarkoutType.Id,
                DateOfRequest = DateTime.Today.AddDays(-2),
                MarkoutNumber = "321456789"
            });

            var result = (PartialViewResult)_target.Update( model);
            var resultMarkout = (Markout)result.Model;

            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(model.DateOfRequest, resultMarkout.DateOfRequest);
            Assert.AreEqual(model.MarkoutNumber, resultMarkout.MarkoutNumber);
            Assert.AreEqual(newMarkoutType.Id,
                resultMarkout.MarkoutType.Id);
            Assert.AreEqual(
                WorkOrdersWorkDayEngine.GetReadyDate(model.DateOfRequest.Value,
                    markoutRequirement.MarkoutRequirementEnum),
                resultMarkout.ReadyDate);
            Assert.AreEqual(
                WorkOrdersWorkDayEngine.GetExpirationDate(model.DateOfRequest.Value,
                    markoutRequirement.MarkoutRequirementEnum),
                resultMarkout.ExpirationDate);
        }

        #endregion

        #endregion
    }
}
