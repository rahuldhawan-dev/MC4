using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class ServiceControllerTest : ContractorControllerTestBase<ServiceController, Service, ServiceRepository>
    {
        #region Fields

        private ContractorUser _user;
        private OperatingCenter _operatingCenter;
        private Contractor _contractor;

        #endregion

        #region Init/Cleanup

        protected override ContractorUser CreateUser()
        {
            _operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            _contractor = GetEntityFactory<Contractor>().Create();
            _contractor.OperatingCenters.Add(_operatingCenter);
            _user = GetFactory<ContractorUserFactory>().Create(new {
                Contractor = _contractor
            });
            return _user;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<INotificationService>().Mock();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<MapCall.Common.Model.Repositories.IServiceRepository>()
             .Use<MapCall.Common.Model.Repositories.ServiceRepository>();
            e.For<IAuthenticationService<User>>().Mock();
            e.For<MapCall.Common.Model.Repositories.ITapImageRepository>()
             .Use<MapCall.Common.Model.Repositories.TapImageRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) =>
            {
                // For reasons unknown to me, the default factories are not creating usable values in 
                // the contractors tests. So some of them need to be created with proper values manually.
                tester.TestPropertyValues[nameof(SearchService.OperatingCenter)] = GetFactory<UniqueOperatingCenterFactory>().Create().Id;
                tester.TestPropertyValues[nameof(SearchService.Street)] = GetFactory<StreetFactory>().Create(new { Name = "Street Name" }).Id;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Service/Index");
                a.RequiresLoggedInUserOnly("~/Service/Search");
                a.RequiresLoggedInUserOnly("~/Service/Show");
                a.RequiresLoggedInUserOnly("~/Service/ByStreetId");
                a.RequiresLoggedInUserOnly("~/Service/Edit");
                a.RequiresLoggedInUserOnly("~/Service/Update");
            });
        }

        #region Show

        [TestMethod]
        public void TestShowShowsNotificationMessage()
        {
            var now = DateTime.Now;
            var entity = GetEntityFactory<Service>().Create(new {
                OperatingCenter = _operatingCenter,
                ContactDate = now
            });

            var result = _target.Show(entity.Id) as ViewResult;

            Assert.AreEqual(((List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]).Single(), entity.StatusMessage);
            Assert.AreNotEqual(string.Empty, entity.StatusMessage);
            Assert.AreEqual(string.Format(Service.StatusMessages.APPLICATION_NOT_SENT, now), entity.StatusMessage);
        }

        [TestMethod]
        public void TestShowJsonReturnsJson()
        {
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = "Copperpot"});
            var serviceSize = GetEntityFactory<ServiceSize>().Create(new { ServiceSizeDescription = "1/2", Size = 0.5m });
            var entity = GetEntityFactory<Service>().Create(new {
                OperatingCenter = _operatingCenter,
                DateInstalled = new DateTime(1980, 12, 8),
                ServiceMaterial = serviceMaterial, ServiceSize = serviceSize
            });
            InitializeControllerAndRequest("~/FieldOperations/Service/Show" + entity.Id + ".json");

            var result = _target.Show(entity.Id) as JsonResult;
            var resultData = (dynamic)result.Data;

            Assert.IsNotNull(result);
            Assert.AreEqual("12/8/1980", resultData.DateInstalled);
            Assert.AreEqual(serviceMaterial.Id, resultData.ServiceMaterial);
            Assert.AreEqual(serviceSize.Id, resultData.ServiceSize);
        }

        [TestMethod]
        public void TestShowShowsWarningMessageWhenServiceIsLinkedToASampleSite()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "xyz",
            });

            var sampleSites = GetEntityFactory<SampleSite>().CreateList(2, new {
                Premise = premise
            });

            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise,
                Installation = "9001",
                ServiceNumber = (long?)123
            });

            Session.Flush();
            Session.Clear();

            var result = _target.Show(service.Id) as ViewResult;

            Assert.AreEqual(
                ServiceController.SAMPLE_SITE_WARNING,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestShowDoesNotShowsWarningMessageWhenServiceIsNotLinkedToASampleSite()
        {
            var regularService = GetEntityFactory<Service>().Create(new {
                OperatingCenter = _operatingCenter,
                ServiceNumber = (long?)124, 
                PremiseNumber = "123123124"
            });

            var result = _target.Show(regularService.Id) as ViewResult;
            
            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Service>().Create(new {
                OperatingCenter = _operatingCenter,
                PremiseNumber = "description 0"
            });
            var entity1 = GetEntityFactory<Service>().Create(new {
                OperatingCenter = _operatingCenter,
                PremiseNumber = "description 1"
            });
            var search = new SearchService();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = _container.With(result).With(true).GetInstance<ExcelResultTester>())
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.PremiseNumber, "PremiseNumber");
                helper.AreEqual(entity1.PremiseNumber, "PremiseNumber", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditRendersViewIfRecordExists()
        {
            var service = GetEntityFactory<Service>().Create();

            var result = (ViewResult)_target.Edit(service.Id);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(service.Id, ((EditService)result.Model).Id);
        }

        [TestMethod]
        public void TestEdit404sIfServiceNotFound()
        {
            Assert.IsNotNull(_target.Edit(817) as HttpNotFoundResult);
        }

        [TestMethod]
        public void TestUpdateUpdatesRecordAndRedirectBackToShow()
        {
            var service = GetEntityFactory<Service>().Create(new { CustomerSideSLReplacementCost = 345m });
            
            var model = _viewModelFactory.BuildWithOverrides<EditService, Service>(service, new { LengthOfCustomerSideSLReplaced = 817 });

            var result = (RedirectToRouteResult)_target.Update(model);

            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(817, Session.Get<Service>(service.Id).LengthOfCustomerSideSLReplaced);
        }

        [TestMethod]
        public void TestUpdate404sIfServiceNotFound()
        {
            Assert.IsNotNull(_target.Update(_viewModelFactory.BuildWithOverrides<EditService>(new { Id = 817 })) as HttpNotFoundResult);
        }

        #endregion

        #endregion
    }
}
