using MapCall.Common.Model.Entities;
using MapCallApi.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class WorkOrderControllerTest : MapCallApiControllerTestBase<WorkOrderController, WorkOrder, IRepository<WorkOrder>>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            GetFactory<NSIWorkOrderRequesterFactory>().Create();
            GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            GetFactory<RoutineWorkOrderPriorityFactory>().Create();
            GetFactory<HydrantInstallationWorkDescriptionFactory>().Create();
            GetFactory<RoutineMarkoutRequirementFactory>().Create();
            GetFactory<CreateSAPWorkOrderStepFactory>().Create();
            GetFactory<HydrantAssetTypeFactory>().Create();
        }
        
        protected override User CreateUser()
        {
            User user = GetFactory<AdminUserFactory>().Create(new {
                UserName = "theUserName",
                FullName = "TheFullName",
                Email = "a@a.com"
            });
            return user;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                SetupHttpAuth(a);
                a.RequiresRole("~/WorkOrder/create", WorkOrderController.ROLE, RoleActions.Add);
            });
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            //no-op since we don't redirect to the show page.
        }

        [TestMethod]
        public void TestCreateOutputsCorrectJsonAfterSuccessfullySaving()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var model = new CreateInstallationWorkOrder(_container) { Hydrant = hydrant.Id, SAPNotificationNumber = 12354, WBSNumber = "0132" };

            var result = _target.Create(model) as JsonResult;
            Assert.IsNotNull(result.Data);
            var id = Convert.ToInt32(result.Data.GetPropertyValueByName("Id"));
            Assert.AreEqual(id, 1);
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            //no-op since we don't redirect to the show page.
        }

        [TestMethod]
        public void TestCreateOutputsCorrectJsonIfModelStateErrorsExist()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var valve = GetEntityFactory<Valve>().Create();
            var model = new CreateInstallationWorkOrder(_container) { Hydrant = hydrant.Id, Valve = valve.Id, SAPNotificationNumber = 12354, WBSNumber = "0132" };
            _target.RunModelValidation(model);
            var result = _target.Create(model) as JsonResult;

            Assert.IsNotNull(result.Data);

            var errors = (Dictionary<string, List<string>>)result.Data.GetPropertyValueByName("Errors");

            Assert.AreEqual(errors["Misc"][0], $"The Hydrant [{hydrant.Id}] and the Valve [{valve.Id}] properties cannot both be set.");
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var model = new CreateInstallationWorkOrder(_container) { Hydrant = hydrant.Id, SAPNotificationNumber = 12354, WBSNumber = "0132" };
            
            _target.Create(model);
            var entity = Repository.Find(model.Id);
            Assert.AreEqual(entity.Id, model.Id);
            Assert.AreEqual(entity.WorkDescription.Id, (int)WorkDescription.Indices.HYDRANT_INSTALLATION);
            Assert.AreEqual(entity.AssetType.Id, AssetType.Indices.HYDRANT);
        }

        #endregion
    }
}
