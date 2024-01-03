using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ServiceControllerTest : MapCallMvcControllerTestBase<ServiceController, Service, ServiceRepository>
    {
        #region Fields

        private User _user;
        private Mock<INotificationService> _noteServ;
        private Mock<IRoleService> _roleService;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Mock();
            _noteServ = e.For<INotificationService>().Mock();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            _roleService = e.For<IRoleService>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Application.ViewEngine = new MapCallMvcViewEngine();

            // This needs to exist because of MapToEntity in Service view models.
            GetFactory<ServiceMaterialFactory>().Create(new { Description = "Lead" });
        }
        
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateService)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.Street = GetEntityFactory<Street>().Create().Id;
                model.MainSize = GetEntityFactory<ServiceSize>().Create().Id;
                model.ServiceInstallationPurpose = GetEntityFactory<ServiceInstallationPurpose>().Create().Id;
                model.ServicePriority = GetEntityFactory<ServicePriority>().Create().Id;
                model.WorkIssuedTo = GetEntityFactory<ServiceRestorationContractor>().Create().Id;
                model.Block = "Block";
                model.Lot = "Lot";
                model.PremiseNumber = "1234567890";
                model.Installation = "0000000000";
                model.DeviceLocation = "2342342342";
                model.StreetNumber = "1";
                model.TaskNumber1 = "21";
                model.Zip = "10001";
                model.DateIssuedToField = DateTime.Now;
                model.LengthOfService = 1m;
                model.DeveloperServicesDriven = false;
                model.MeterSettingRequirement = false;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditService)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.Street = GetEntityFactory<Street>().Create().Id;
                model.MainSize = GetEntityFactory<ServiceSize>().Create().Id;
                model.ServiceInstallationPurpose = GetEntityFactory<ServiceInstallationPurpose>().Create().Id;
                model.ServicePriority = GetEntityFactory<ServicePriority>().Create().Id;
                model.WorkIssuedTo = GetEntityFactory<ServiceRestorationContractor>().Create().Id;
                model.Block = "Block";
                model.Lot = "Lot";
                model.PremiseNumber = "1234567890";
                model.Installation = "0000000000";
                model.DeviceLocation = "2342342342";
                model.StreetNumber = "1";
                model.TaskNumber1 = "21";
                model.Zip = "10001";
                model.DateIssuedToField = DateTime.Now;
                model.LengthOfService = 1m;
                model.DeveloperServicesDriven = false;
                model.MeterSettingRequirement = false;
            };
        }
		
        #endregion

        #region Tests

        #region Show

        [TestMethod]
        public void TestShowShowsNotificationMessage()
        {
            var now = DateTime.Now;
            var entity = GetEntityFactory<Service>().Create(new {
                DateInstalled = (DateTime?)null,
                ContactDate = now
            });

            var result = _target.Show(entity.Id) as ViewResult;
            _target.AssertTempDataContainsMessage(entity.StatusMessage, MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
            Assert.AreNotEqual(string.Empty, entity.StatusMessage);
            Assert.AreEqual(string.Format(Service.StatusMessages.APPLICATION_NOT_SENT, now), entity.StatusMessage);
        }

        [TestMethod]
        public void TestShowJsonReturnsJson()
        {
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = "Copperpot"});
            var serviceSize = GetEntityFactory<ServiceSize>().Create(new { ServiceSizeDescription = "1/2", Size = 0.5m });
            var entity = GetEntityFactory<Service>().Create(new { DateInstalled = new DateTime(1980, 12, 8), ServiceMaterial = serviceMaterial, ServiceSize = serviceSize });
            InitializeControllerAndRequest("~/FieldOperations/Service/Show" + entity.Id + ".json");

            var result = _target.Show(entity.Id) as JsonResult;
            var resultData = (dynamic)result.Data;

            Assert.IsNotNull(result);
            Assert.AreEqual("12/8/1980", resultData.DateInstalled);
            Assert.AreEqual(serviceMaterial.Id, resultData.ServiceMaterial);
            Assert.AreEqual(serviceSize.Id, resultData.ServiceSize);
        }

        [TestMethod]
        public void TestShowPdfReturnsPdf()
        {
            var dateInstalled = new DateTime(1980, 12, 8);
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = "Copperpot"});
            var serviceSize = GetEntityFactory<ServiceSize>().Create(new { ServiceSizeDescription = "1/2", Size = 0.5m });
            var entity = GetEntityFactory<Service>().Create(new { DateInstalled = dateInstalled, ServiceMaterial = serviceMaterial, ServiceSize = serviceSize });
            InitializeControllerAndRequest("~/FieldOperations/Service/Show" + entity.Id + ".pdf");

            var result = _target.Show(entity.Id) as PdfResult;
            var resultData = (Service)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(dateInstalled, resultData.DateInstalled);
            Assert.AreEqual(serviceMaterial, resultData.ServiceMaterial);
            Assert.AreEqual(serviceSize, resultData.ServiceSize);
            Assert.AreEqual("Pdf", result.ViewName);
        }

        [TestMethod]
        public void TestShowPdfRendersIndianaPdfForServiceLocatedInIndiana()
        {
            var dateInstalled = new DateTime(1980, 12, 8);
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = "Copperpot"});
            var serviceSize = GetEntityFactory<ServiceSize>().Create(new { ServiceSizeDescription = "1/2", Size = 0.5m });
            var indiana = GetEntityFactory<State>().Create(new {Abbreviation = "IN", Name = "Indiana"});
            var entity = GetEntityFactory<Service>().Create(new {
                DateInstalled = dateInstalled,
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize,
                State = indiana
            });
            InitializeControllerAndRequest("~/FieldOperations/Service/Show" + entity.Id + ".pdf");

            var result = _target.Show(entity.Id) as PdfResult;
            var resultData = (Service)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(dateInstalled, resultData.DateInstalled);
            Assert.AreEqual(serviceMaterial, resultData.ServiceMaterial);
            Assert.AreEqual(serviceSize, resultData.ServiceSize);
            Assert.AreEqual("Pdf\\IN", result.ViewName);
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

            var result = (ViewResult)_target.Show(service.Id);
            var warning = ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single();

            Assert.AreEqual(ServiceController.SAMPLE_SITE_WARNING, warning);
        }

        [TestMethod]
        public void TestShowDoesNotShowsWarningMessageWhenServiceIsNotLinkedToASampleSite()
        {
            var regularService = GetEntityFactory<Service>().Create(new { ServiceNumber = (long?)124, PremiseNumber = "123123124" });

            var result = _target.Show(regularService.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var coordinate1 = GetFactory<CoordinateFactory>().Create(new { Latitude = 43m, Longitude = -74m });
            var coordinate2 = GetFactory<CoordinateFactory>().Create(new { Latitude = 1m, Longitude = 2m });
            var entity0 = GetEntityFactory<Service>().Create(new { PremiseNumber = "description 0", Coordinate = coordinate1 });
            var entity1 = GetEntityFactory<Service>().Create(new { PremiseNumber = "description 1", Coordinate = coordinate2 });

            var search = new SearchService { OperatingCenter = entity0.OperatingCenter.Id };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.PremiseNumber, "PremiseNumber");
                helper.AreEqual(entity1.PremiseNumber, "PremiseNumber", 1);
                helper.AreEqual(coordinate1.Latitude, "Latitude");
                helper.AreEqual(coordinate1.Longitude, "Longitude");
                helper.AreEqual(coordinate2.Latitude, "Latitude", 1);
                helper.AreEqual(coordinate2.Longitude, "Longitude", 1);
                helper.DoesNotContainColumn("Coordinate");
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestNewActionsBillionParametersAndWhatTheyDo()
        {
            Assert.Inconclusive("These parameters are not tested anywhere.");
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to multiple New parameters
            var result = (ViewResult)_target.New(null, null, null, null);

            MyAssert.IsInstanceOfType<ActionResult>(result);
            MyAssert.IsInstanceOfType<CreateService>(result.Model);
        }

        [TestMethod]
        public void TestNewFromWorkOrderReturns404IfWorkOrderDoesNotExist()
        {
            var result = _target.NewFromWorkOrder(0);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestNewFromWorkOrderReturnsForbiddenIfUserCannotAccessTheWorkOrder()
        {
            GetFactory<DefaultMapIconFactory>().Create(); // Needs to exist
            var wo = GetFactory<WorkOrderFactory>().Create(new { ApartmentAddtl = "A" });

            var result = (ViewResult)_target.NewFromWorkOrder(wo.Id);
            var model = (ForbiddenRoleAccessModel)result.Model;

            MvcAssert.IsViewNamed(result, "~/Views/Shared/ForbiddenRoleAccess.cshtml");
        }

        [TestMethod]
        public void TestNewFromWorkOrderReturnsNewViewWithWorkOrderSetOnModel()
        {
            GetFactory<DefaultMapIconFactory>().Create(); // Needs to exist
            var wo = GetFactory<WorkOrderFactory>().Create(new{ApartmentAddtl = "A"});
            _roleService.Setup(x => x.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add, wo.OperatingCenter)).Returns(true);

            var result = (ViewResult)_target.NewFromWorkOrder(wo.Id);
            var model = (CreateService)result.Model;
            Assert.AreEqual(wo.Id, model.WorkOrder);
            Assert.AreEqual(wo.ApartmentAddtl, model.ApartmentNumber);
        }

        [TestMethod]
        public void TestNewDoesNotLoadWithSAPNumbersFromExistingService()
        {
            var entity = GetEntityFactory<Service>().Create(new { SAPNotificationNumber = (long)123, SAPWorkOrderNumber = (long)321 });

            var result = (CreateService)((ViewResult)_target.New(entity.Id, true, false, false)).Model;

            Assert.IsNull(result.SAPNotificationNumber);
            Assert.IsNull(result.SAPWorkOrderNumber);
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfRenewalInstalled()
        {
            //Note: We loop through all the services categories to ensure that they are explicitly sent out when they should be.
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(30);

            foreach (var serviceCategory in serviceCategories)
            {
                var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
                var town = GetEntityFactory<Town>().Create();
                var entity = _viewModelFactory.Build<CreateService, Service>( GetEntityFactory<Service>().Build(new { ServiceCategory = serviceCategory, OperatingCenter = opc, Town = town, ServiceNumber = 9000000000, DateInstalled = DateTime.Now }));

                NotifierArgs resultArgs = new NotifierArgs { OperatingCenterId = opc.Id, Module = ServiceController.ROLE, Purpose = ServiceController.UPDATE_RENEWAL_INSTALLED_NOTIFICATION_PURPOSE, Data = entity };
                if (serviceCategory.Id == ServiceCategory.Indices.SEWER_SERVICE_RENEWAL
                         || serviceCategory.Id == ServiceCategory.Indices.WATER_SERVICE_RENEWAL)
                {
                    _noteServ.Setup(x => x.Notify(resultArgs)).Callback<NotifierArgs>(x => resultArgs = x);
                    _target.Create(entity);
                    Assert.AreEqual(ServiceController.UPDATE_RENEWAL_INSTALLED_NOTIFICATION_PURPOSE, resultArgs.Purpose);
                    Assert.AreEqual(ServiceController.ROLE, resultArgs.Module);
                }
                else
                {
                    _target.Create(entity);
                    _noteServ.Verify(x => x.Notify(resultArgs), Times.Never);
                }
                _noteServ.ResetCalls();
            }
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfNewServiceInstalled()
        {
            //Note: We loop through all the services categories to ensure that they are explicitly sent out when they should be.
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(30);
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();

            //serviceCategories = serviceCategories.Where(x => ServiceCategory.GetNewServiceCategories().Any(z => z == x.Id)).ToList();
            foreach (var serviceCategory in serviceCategories)
            {
                var now = DateTime.Now;
                var entity =
                    GetEntityFactory<Service>()
                        .Build(
                            new {
                                ServiceCategory = serviceCategory,
                                OperatingCenter = opc,
                                Town = town,
                                ServiceNumber = 9000000000,
                                DateInstalled = now,
                                OriginalInstallationDate = now,
                                RetiredDate = DateTime.Now,
                                PreviousServiceSize = GetEntityFactory<ServiceSize>().Create(),
                                PreviousServiceMaterial = GetEntityFactory<ServiceMaterial>().Create()
                            });
                var testArgs = new NotifierArgs {
                    OperatingCenterId = opc.Id,
                    Module = ServiceController.ROLE,
                    Purpose = ServiceController.UPDATE_NEW_SERVICE_INSTALLED,
                    Data = entity
                };
                Func<NotifierArgs, bool> argsMatchP = n => n.OperatingCenterId == opc.Id &&
                                                           n.Module == testArgs.Module &&
                                                           n.Purpose == testArgs.Purpose &&
                                                           ((ServiceNotification)n.Data).Service.Id > 0 &&
                                                           ((ServiceNotification)n.Data).Service.ServiceCategory ==
                                                           serviceCategory &&
                                                           ((ServiceNotification)n.Data).Service.OperatingCenter == opc &&
                                                           ((ServiceNotification)n.Data).Service.Town == town &&
                                                           ((ServiceNotification)n.Data).Service.ServiceNumber ==
                                                           entity.ServiceNumber;

                if (ServiceCategory.GetNewServiceCategories().Any(x => x == serviceCategory.Id))
                {
                    _target.Create(_viewModelFactory.Build<CreateService, Service>( entity));
                    _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(n => argsMatchP(n))), $"Not called for ServiceCategoryID: {serviceCategory.Id}");
                }
                else
                {
                    _target.Create(_viewModelFactory.Build<CreateService, Service>( entity));
                    _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(n => argsMatchP(n))), Times.Never,
                        $"Failed on {serviceCategory.Id}");
                }

                _noteServ.ResetCalls();
            }
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfRenewalIsAddedForServiceAssociatedWithASampleSite()
        {
            //Note: We loop through all the services categories to ensure that they are explicitly sent out when they should be.
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(30);
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = CreateEntity<Town>();
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "9001"
            });

            CreateEntity<SampleSite>(new {
                Premise = premise
            });

            var original = CreateEntity<Service>(new {
                Premise = premise
            });

            foreach (var category in serviceCategories)
            {
                var service = BuildEntity<Service>(new {
                    ServiceCategory = category,
                    OperatingCenter = opc,
                    Town = town,
                    ServiceNumber = 9000000000,
                    DateInstalled = DateTime.Now,
                    OriginalInstallationDate = DateTime.Now,
                    RenewalOf = original,
                    CustomerSideReplacementDate = DateTime.Now,
                    RetiredDate = DateTime.Now,
                    PreviousServiceSize = GetEntityFactory<ServiceSize>().Create(),
                    PreviousServiceMaterial = GetEntityFactory<ServiceMaterial>().Create()
                });

                var testArgs = new NotifierArgs {
                    OperatingCenterId = opc.Id, 
                    Module = ServiceController.ROLE, 
                    Purpose = ServiceController.RENEWAL_AT_SERVICE_WITH_SAMPLE_SITE, 
                    Data = service
                };

                Func<NotifierArgs, bool> argsMatchP = n => n.OperatingCenterId == opc.Id &&
                                                           n.Module == testArgs.Module &&
                                                           n.Purpose == testArgs.Purpose &&
                                                           ((ServiceNotification)n.Data).Service.Id > 0 &&
                                                           ((ServiceNotification)n.Data).Service.ServiceCategory ==
                                                           category &&
                                                           ((ServiceNotification)n.Data).Service.OperatingCenter == opc &&
                                                           ((ServiceNotification)n.Data).Service.Town == town &&
                                                           ((ServiceNotification)n.Data).Service.ServiceNumber ==
                                                           service.ServiceNumber;

                if (ServiceCategory.GetRenewalSampleSiteServiceCategories().Any(x => x == category.Id))
                {
                    _target.Create(_viewModelFactory.Build<CreateService, Service>(service));
                    _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(n => argsMatchP(n))));
                }
                else
                {
                    _target.Create(_viewModelFactory.Build<CreateService, Service>(service));
                    _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(n => argsMatchP(n))), Times.Never, $"Failed on {category.Id}");
                }

                _noteServ.Invocations.Clear();
            }
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfServiceAssociatedWithASampleSite()
        {
            //Note: We loop through all the services categories to ensure that they are explicitly sent out when they should be.
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(1);
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = CreateEntity<Town>();
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "9001"
            });

            var sampleSite = CreateEntity<SampleSite>(new {
                Premise = premise
            });

            var service = CreateEntity<Service>(new {
                Premise = premise
            });

            foreach (var category in serviceCategories)
            {
                var entity =
                    BuildEntity<Service>(new {
                        ServiceCategory = serviceCategory,
                        OperatingCenter = opc,
                        Town = town,
                        ServiceNumber = 9000000000,
                        DateInstalled = DateTime.Now,
                        OriginalInstallationDate = DateTime.Now,
                        PremiseNumber = sampleSite.Premise.PremiseNumber,
                        Installation = "900001",
                        RenewalOf = service,
                        CustomerSideReplacementDate = DateTime.Now,
                        RetiredDate = DateTime.Now,
                        PreviousServiceSize = GetEntityFactory<ServiceSize>().Create(),
                        PreviousServiceMaterial = GetEntityFactory<ServiceMaterial>().Create()
                    });

                var testArgs = new NotifierArgs {
                    OperatingCenterId = opc.Id,
                    Module = ServiceController.ROLE,
                    Purpose = ServiceController.SERVICE_WITH_SAMPLE_SITE,
                    Data = entity
                };

                Func<NotifierArgs, bool> argsMatchP = n => n.OperatingCenterId == opc.Id &&
                                                           n.Module == testArgs.Module &&
                                                           n.Purpose == testArgs.Purpose &&
                                                           ((ServiceNotification)n.Data).Service.Id > 0 &&
                                                           ((ServiceNotification)n.Data).Service.ServiceCategory ==
                                                           serviceCategory &&
                                                           ((ServiceNotification)n.Data).Service.OperatingCenter ==
                                                           opc &&
                                                           ((ServiceNotification)n.Data).Service.Town == town &&
                                                           ((ServiceNotification)n.Data).Service.ServiceNumber ==
                                                           entity.ServiceNumber;

                _target.Create(_viewModelFactory.Build<CreateService, Service>(entity));
                _target.Create(_viewModelFactory.Build<CreateService, Service>(entity));
                _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(n => argsMatchP(n))));

                _noteServ.Invocations.Clear();
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Service>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditService, Service>(eq, new {
                PremiseNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<Service>(eq.Id).PremiseNumber);
        }

        // THESE SEND OUT TWO NOTIFICATIONS BECAUSE THE NEW INSTALLATION ONE IS ALSO SENT
        [TestMethod]
        public void TestUpdateSendsNotificationIfInstalledAndFireServiceRenewal()
        {
            //Note: We loop through all the services categories to ensure that they are explicitly sent out when they should be.
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(30);

            foreach (var serviceCategory in serviceCategories)
            {
                var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
                var model = _viewModelFactory.BuildWithOverrides<EditService, Service>(
                    GetEntityFactory<Service>().Create(new {
                        DateInstalled = (DateTime?)null,
                        ServiceCategory = serviceCategory,
                        PremiseNumber = "1234567890",
                        OperatingCenter = opc
                    }),
                    new {
                        DateInstalled = DateTime.Now,
                        CustomerSideReplacementDate = DateTime.Now,
                        RetiredDate = DateTime.Now,
                        OriginalInstallationDate = DateTime.Now,
                        PreviousServiceSize = GetEntityFactory<ServiceSize>().Create().Id,
                        PreviousServiceMaterial = GetEntityFactory<ServiceMaterial>().Create().Id
                    });
                NotifierArgs resultArgs = null;
                if (serviceCategory.Id == ServiceCategory.Indices.FIRE_SERVICE_RENEWAL)
                {
                    _noteServ.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
                    _target.Update(model);
                    _noteServ.Verify(
                        x => x.Notify(It.Is<NotifierArgs>(
                            a => a.Purpose ==
                                 ServiceController
                                    .UPDATE_LARGE_SERVICE_OR_FIRE_NOTIFICATION_PURPOSE )),
                        Times.Once); }
                else if (serviceCategory.Id == ServiceCategory.Indices.SEWER_SERVICE_RENEWAL
                         || serviceCategory.Id == ServiceCategory.Indices.WATER_SERVICE_RENEWAL)
                {
                    _noteServ.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
                    _target.Update(model);
                    _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == ServiceController.UPDATE_RENEWAL_INSTALLED_NOTIFICATION_PURPOSE )), Times.Once);
                }
                else if (!ServiceCategory.GetNewServiceCategories().Any(x => x == serviceCategory.Id))
                {
                    model.Installation = null;
                    _target.Update(model);
                    _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once, $"Failed on {serviceCategory.Id}");
                }
                _noteServ.ResetCalls();
            }
        } // THESE SEND OUT TWO NOTIFICATIONS
        [TestMethod]
        public void TestUpdateSendsNotificationIfWaterServiceNewCommercialOrDomestic3OrLargerBeingInstalled()
        {
            //Note: We loop through all the services categories to ensure that they are explicitly sent out when they should be.
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(30);

            foreach (var serviceCategory in serviceCategories)
            {
                var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
                var size = GetEntityFactory<ServiceSize>().Create(new {Size = 3m});
                var model = _viewModelFactory.BuildWithOverrides<EditService, Service>(
                    GetEntityFactory<Service>().Create(new {
                        DateInstalled = (DateTime?)null,
                        ServiceCategory = serviceCategory,
                        ServiceSize = size,
                        PremiseNumber = "1234567890",
                        OperatingCenter = opc
                    }), new {
                        DateInstalled = DateTime.Now,
                        RetiredDate = DateTime.Now,
                        OriginalInstallationDate = DateTime.Now,
                        PreviousServiceSize = GetEntityFactory<ServiceSize>().Create().Id,
                        PreviousServiceMaterial = GetEntityFactory<ServiceMaterial>().Create().Id
                    });
                NotifierArgs resultArgs = null;

                if (serviceCategory.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_COMMERCIAL
                    || serviceCategory.Id == ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC
                    || serviceCategory.Id == ServiceCategory.Indices.FIRE_SERVICE_RENEWAL)
                {
                    _noteServ.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
                    _target.Update(model);
                    Assert.AreEqual(ServiceController.ROLE, resultArgs.Module);
                }
                else if (serviceCategory.Id == ServiceCategory.Indices.SEWER_SERVICE_RENEWAL
                         || serviceCategory.Id == ServiceCategory.Indices.WATER_SERVICE_RENEWAL)
                {
                    _noteServ.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
                    _target.Update(model);
                    _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == ServiceController.UPDATE_RENEWAL_INSTALLED_NOTIFICATION_PURPOSE )), Times.Once);
                }
                else if (!ServiceCategory.GetNewServiceCategories().Any(x => x == serviceCategory.Id))
                {
                    _target.Update(model);
                    _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once, $"Failed on {serviceCategory.Id}");
                }
                _noteServ.ResetCalls();
            }
        }

        [TestMethod]
        public void TestUpdateSendsNotificationIfServiceInstalled()
        {
            //Note: We loop through all the services categories to ensure that they are explicitly sent out when they should be.
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(30);

            foreach (var serviceCategory in serviceCategories)
            {
                var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
                var size = GetEntityFactory<ServiceSize>().Create(new { Size = 3m });
                var model = _viewModelFactory.BuildWithOverrides<EditService, Service>(
                    GetEntityFactory<Service>().Create(new {
                        ServiceCategory = serviceCategory, ServiceSize = size, PremiseNumber = "1234567890",
                        OperatingCenter = opc
                    }), new {DateInstalled = DateTime.Now});

                NotifierArgs resultArgs = new NotifierArgs { OperatingCenterId = opc.Id, Module = ServiceController.ROLE, Purpose = ServiceController.UPDATE_NEW_SERVICE_INSTALLED, Data = model };
                if (ServiceCategory.GetNewServiceCategories().Any(x => x == serviceCategory.Id))
                {
                    _noteServ.Setup(x => x.Notify(resultArgs)).Callback<NotifierArgs>(x => resultArgs = x);
                    _target.Update(model);
                    Assert.AreEqual(ServiceController.UPDATE_NEW_SERVICE_INSTALLED, resultArgs.Purpose);
                    Assert.AreEqual(ServiceController.ROLE, resultArgs.Module);
                }
                else
                {
                    _target.Update(model);
                    _noteServ.Verify(x => x.Notify(resultArgs), Times.Never, $"Failed on {serviceCategory.Id}");
                }
                _noteServ.ResetCalls();
            }
        }

        [TestMethod]
        public void TestUpdateSendsNotificationIfStatusIsSetToInactiveForServiceAssociatedWithASampleSite()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = CreateEntity<Town>();
            var premise = CreateEntity<Premise>(new {
                SampleSites = new List<SampleSite> {
                    CreateEntity<SampleSite>()
                }
            });

            var category = CreateEntity<ServiceCategory>();

            var service = CreateEntity<Service>(new {
                ServiceCategory = category,
                OperatingCenter = opc,
                Town = town,
                ServiceNumber = 9000000000,
                DateInstalled = DateTime.Now,
                Premise = premise,
                IsActive = true,
                OriginalInstallationDate = DateTime.Now,
                RetiredDate = DateTime.Now,
                PreviousServiceSize = GetEntityFactory<ServiceSize>().Create(),
                PreviousServiceMaterial = GetEntityFactory<ServiceMaterial>().Create()
            });

            var testArgs = new NotifierArgs {
                OperatingCenterId = opc.Id,
                Module = ServiceController.ROLE,
                Purpose = ServiceController.DEACTIVATED_SERVICE_WITH_SAMPLE_SITE,
                Data = service
            };

            Func<NotifierArgs, bool> argsMatchP = n => n.OperatingCenterId == opc.Id &&
                                                       n.Module == testArgs.Module &&
                                                       n.Purpose == testArgs.Purpose &&
                                                       ((ServiceNotification)n.Data).Service.Id > 0 &&
                                                       ((ServiceNotification)n.Data).Service.ServiceCategory ==
                                                       category &&
                                                       ((ServiceNotification)n.Data).Service.OperatingCenter == opc &&
                                                       ((ServiceNotification)n.Data).Service.Town == town &&
                                                       ((ServiceNotification)n.Data).Service.ServiceNumber ==
                                                       service.ServiceNumber;

            _target.Update(_viewModelFactory.BuildWithOverrides<EditService, Service>(service, new {IsActive = false}));
            _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(n => argsMatchP(n))));
        }

        [TestMethod]
        public void TestEditSendsNotificationIfServiceAssociatedWithASampleSite()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = CreateEntity<Town>();
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "9001"
            });

            CreateEntity<SampleSite>(new {
                Premise = premise
            });
            var category = CreateEntity<ServiceCategory>();

            var service = CreateEntity<Service>(new {
                ServiceCategory = category,
                OperatingCenter = opc,
                Town = town,
                ServiceNumber = 9000000000,
                Premise = premise,
                Installation = "900001",
                DateInstalled = DateTime.Now,
                IsActive = true,
                OriginalInstallationDate = DateTime.Now,
                RetiredDate = DateTime.Now,
                PreviousServiceSize = GetEntityFactory<ServiceSize>().Create(),
                PreviousServiceMaterial = GetEntityFactory<ServiceMaterial>().Create()
            });

            var testArgs = new NotifierArgs {
                OperatingCenterId = opc.Id,
                Module = ServiceController.ROLE,
                Purpose = ServiceController.SERVICE_WITH_SAMPLE_SITE,
                Data = service
            };
            Func<NotifierArgs, bool> argsMatchP = n => n.OperatingCenterId == opc.Id &&
                                                       n.Module == testArgs.Module &&
                                                       n.Purpose == testArgs.Purpose &&
                                                       ((ServiceNotification)n.Data).Service.Id > 0 &&
                                                       ((ServiceNotification)n.Data).Service.ServiceCategory ==
                                                       category &&
                                                       ((ServiceNotification)n.Data).Service.OperatingCenter == opc &&
                                                       ((ServiceNotification)n.Data).Service.Town == town &&
                                                       ((ServiceNotification)n.Data).Service.ServiceNumber ==
                                                       service.ServiceNumber;

            _target.Update(_viewModelFactory.BuildWithOverrides<EditService, Service>(service, new {Installation = "9000"}));
            _target.Update(_viewModelFactory.BuildWithOverrides<EditService, Service>(service, new { Installation = "9000" }));
            _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(n => argsMatchP(n))));
        }
        
        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ServiceController.ROLE;
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FieldOperations/Service/AnyWithInstallationNumberAndOperatingCenter/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Service/ByPremiseNumberAndServiceNumber/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Service/ByStreetId/");
                a.RequiresRole("~/FieldOperations/Service/Search/", role);
                a.RequiresRole("~/FieldOperations/Service/Show/", role);
                a.RequiresRole("~/FieldOperations/Service/Index/", role);
                a.RequiresRole("~/FieldOperations/Service/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Service/NewFromWorkOrder/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Service/LinkOrNew/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Service/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Service/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Service/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Service/AddWorkOrder/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Service/RemoveWorkOrder/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Service/AddServicePremiseContact/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Service/RemoveServicePremiseContact/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Service/AddServiceFlush/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Service/RemoveServiceFlush/", role, RoleActions.UserAdministrator);
            });
        }

        #endregion

        #region ByPremiseNumberAndServiceNumber

        [TestMethod]
        public void TestByPremiseNumberAndServiceNumberReturnsJsonSerializedDataForMatchingService()
        {
            var townSection = GetEntityFactory<TownSection>().Create(new {
                Name = "section of town"
            });
            var service = GetFactory<ServiceFactory>().Create(new {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
                StreetNumber = "52",
                Street = typeof(StreetFactory),
                CrossStreet = typeof(StreetFactory),
                TownSection = townSection,
                Lot = "Q",
                Block = "Z",
                ApartmentNumber = "Apartment",
                OperatingCenter = typeof(OperatingCenterFactory),
                ServiceSize = typeof(ServiceSizeFactory),
                ServiceMaterial = typeof(ServiceMaterialFactory),
                DateInstalled = new DateTime(1984, 4, 24)
            });
            var model = new SearchServicePremiseNumberServiceNumber {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = "2"
            };

            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;

            Assert.AreEqual(true, data["success"]);
            Assert.AreEqual("52", data["streetNumber"]);
            Assert.AreEqual(service.Street.Id, data["streetId"]);
            Assert.AreEqual(service.CrossStreet.Id, data["crossStreetId"]);
            Assert.AreEqual("section of town", data["townSection"]);
            Assert.AreEqual(service.Id, data["serviceId"]);
            Assert.AreEqual(service.ServiceNumber, data["serviceNumber"]);
            Assert.AreEqual("1", data["premiseNumber"]);
            Assert.AreEqual("Q", data["lot"]);
            Assert.AreEqual("Apartment", data["apartmentNumber"]);
            Assert.AreEqual("Z", data["block"]);
            Assert.AreEqual(service.OperatingCenter.Id, data["operatingCenterId"]);
            Assert.AreEqual(service.ServiceSize.Id, data["serviceSize"]);
            Assert.AreEqual("4/24/1984", data["dateInstalled"]);
            Assert.AreEqual("WATER", data["serviceType"]);
            Assert.AreEqual(service.ServiceMaterial.Id, data["serviceMaterial"]);
        }

        [TestMethod]
        public void TestByPremiseNumberAndServiceNumberReturnsJsonSerializedDataForMatchingServiceNull()
        {
            var townSection = GetEntityFactory<TownSection>().Create(new
            {
                Name = "section of town"
            });
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)null,
                StreetNumber = "52",
                Street = typeof(StreetFactory),
                CrossStreet = typeof(StreetFactory),
                TownSection = townSection,
                Lot = "Q",
                Block = "Z",
                ApartmentNumber = "Apartment",
                OperatingCenter = typeof(OperatingCenterFactory),
                ServiceSize = typeof(ServiceSizeFactory),
                ServiceMaterial = typeof(ServiceMaterialFactory),
                DateInstalled = new DateTime(1984, 4, 24)
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = null
            };

            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;

            Assert.AreEqual(true, data["success"]);
            Assert.AreEqual("52", data["streetNumber"]);
            Assert.AreEqual(service.Street.Id, data["streetId"]);
            Assert.AreEqual(service.CrossStreet.Id, data["crossStreetId"]);
            Assert.AreEqual("section of town", data["townSection"]);
            Assert.AreEqual(service.Id, data["serviceId"]);
            Assert.AreEqual(service.ServiceNumber, data["serviceNumber"]);
            Assert.AreEqual("1", data["premiseNumber"]);
            Assert.AreEqual("Q", data["lot"]);
            Assert.AreEqual("Apartment", data["apartmentNumber"]);
            Assert.AreEqual("Z", data["block"]);
            Assert.AreEqual(service.OperatingCenter.Id, data["operatingCenterId"]);
            Assert.AreEqual(service.ServiceSize.Id, data["serviceSize"]);
            Assert.AreEqual("4/24/1984", data["dateInstalled"]);
            Assert.AreEqual("WATER", data["serviceType"]);
            Assert.AreEqual(service.ServiceMaterial.Id, data["serviceMaterial"]);
        }

        [TestMethod]
        public void TestByPremiseNumberAndServiceNumberDoesThatHorribleStringMatchingThingWithServiceTypes()
        {
            var fireCat = GetFactory<ServiceCategoryFactory>().Create(new { Description = "Fire in the hole!" });
            var sewerCat = GetFactory<ServiceCategoryFactory>().Create(new { Description = "Sewer in the hole!" });
            var irrCat = GetFactory<ServiceCategoryFactory>().Create(new { Description = "Irrigation in the hole!" });
            var retireCat = GetFactory<ServiceCategoryFactory>().Create(new { Description = "Retirement in the hole!" });
            var waterCat = GetFactory<ServiceCategoryFactory>().Create(new { Description = "Cats don't like water!" });
            
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
                
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = null
            };

            service.ServiceCategory = fireCat;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("FIRE SERVICE", data["serviceType"]);

            service.ServiceCategory = sewerCat;
            result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("SEWER", data["serviceType"]);

            service.ServiceCategory = irrCat;
            result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("IRRIGATION", data["serviceType"]);

            service.ServiceCategory = retireCat;
            result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("RETIREMENT", data["serviceType"]);

            service.ServiceCategory = waterCat;
            result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("WATER", data["serviceType"]);

            // Should be WATER for nulls too.
            service.ServiceCategory = null;
            result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("WATER", data["serviceType"]);
        }

        [TestMethod]
        public void TestTestByPremiseNumberAndServiceNumberReturnsWithoutStreetIfStreetIsNullAndServiceNull()
        {
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)null,
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = null
            };

            service.Street = null;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("streetId"));
        }

        [TestMethod]
        public void TestTestByPremiseNumberAndServiceNumberReturnsWithoutStreetIfStreetIsNull()
        {
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = "2"
            };

            service.Street = null;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("streetId"));
        }

        [TestMethod]
        public void TestTestByPremiseNumberAndServiceNumberReturnsWithoutCrossStreetIfStreetIsNull()
        {
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = "2"
            };

            service.CrossStreet = null;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("crossStreetId"));
        }

        [TestMethod]
        public void TestTestByPremiseNumberAndServiceNumberReturnsWithoutTownIfTownIsNull()
        {
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = "2"
            };

            service.Town = null;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("townId"));
        }

        [TestMethod]
        public void TestTestByPremiseNumberAndServiceNumberReturnsWithoutServiceMaterialDescriptionIfServiceMaterialIsNull()
        {
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = "2"
            };

            service.ServiceMaterial = null;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("serviceMaterial"));
        }

        [TestMethod]
        public void TestTestByPremiseNumberAndServiceNumberReturnsTrueForIsDefaultImageForServiceIfNoneExists()
        {
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = "2"
            };

            service.ServiceMaterial = null;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("True", data["isDefaultImageForService"]);
        }

        [TestMethod]
        public void TestTestByPremiseNumberAndServiceNumberReturnsFalseForIsDefaultImageForServiceIfNoneExists()
        {
            var service = GetFactory<ServiceFactory>().Create(new
            {
                PremiseNumber = "1",
                ServiceNumber = (long?)2,
            });
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "1",
                ServiceNumberSearch = "2"
            };
            var tapImage = GetFactory<TapImageFactory>().Create(new { ServiceNumber = service.ServiceNumber.ToString(), PremiseNumber = service.PremiseNumber});
            Session.Clear();
            service.ServiceMaterial = null;
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual("True", data["isDefaultImageForService"]);
        }

        [TestMethod]
        public void TestByPremiseNumberAndServiceNumberReturnsSuccessFalseWithMessageIfNoMatchesAreFound()
        {
            var model = new SearchServicePremiseNumberServiceNumber
            {
                PremiseNumberSearch = "Oh",
                ServiceNumberSearch = "WUBBA LUBBA DUB DUUBS!"
            };

            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(false, data["success"]);
            Assert.AreEqual("There are no services that match the search parameters.", data["message"]);
        }

        [TestMethod]
        public void TestByPremiseNumberAndServiceNumberReturnsSuccessFalseIfModelStateIsInvalid()
        {
            _target.ModelState.AddModelError("Oops", "Oops");
            var result = (JsonResult)_target.ByPremiseNumberAndServiceNumber(null);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(false, data["success"]);
            Assert.AreEqual("Invalid search parameters.", data["message"]);
        }

        [TestMethod]
        public void TestByStreetIdReturnsCascadeResultOfServicesFilteredByStreetId()
        {
            var invalidValve = GetFactory<ServiceFactory>().Create();
            var street = GetFactory<StreetFactory>().Create();
            var goodValve = GetFactory<ServiceFactory>().Create(new { Street = street, ServiceNumber = (long?)123, PremiseNumber = "456" });

            var result = (CascadingActionResult)_target.ByStreetId(street.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1); // -1 accounts for the select here
            Assert.AreEqual(goodValve.Id.ToString(), actual[1].Value);
            Assert.AreEqual(string.Format("[Id] {0} [Service] 123, [Premise] 456, [Service Type] ", goodValve.Id), actual[1].Text);
        }

        #endregion

        #region AnyWithInstallationNumberAndOperatingCenterAndSampleSites

        [TestMethod]
        public void TestAnyWithReturnsFalseWhenNone()
        {
            var result = _target.AnyWithInstallationNumberAndOperatingCenter("foo", 1) as JsonResult;
            var helper = new JsonResultTester(result.Data);

            Assert.AreEqual(0, helper.Count);
        }

        [TestMethod]
        public void TestAnyWithReturnsTrueWhenExist()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "xyz",
            });

            var sampleSites = GetEntityFactory<SampleSite>().CreateList(2, new {
                Premise = premise
            });

            var services = GetEntityFactory<Service>().CreateList(2, new {
                Installation = "foo", 
                Premise = premise,
                OperatingCenter = operatingCenter
            });

            var result = _target.AnyWithInstallationNumberAndOperatingCenter("foo", operatingCenter.Id) as JsonResult;
            var helper = new JsonResultTester(result?.Data);

            helper.AreEqual(services[0].Id, "Id");
            helper.AreEqual(services[1].Id, "Id", 1);
        }
        
        [TestMethod]
        public void TestAnyWithNullInstallationReturnsFalse()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "xyz",
                SampleSites = GetEntityFactory<SampleSite>().CreateList(2)
            });

            GetEntityFactory<Service>().CreateList(2, new {
                Installation = "", 
                OperatingCenter = operatingCenter,
                Premise = premise
            });

            var result = _target.AnyWithInstallationNumberAndOperatingCenter(string.Empty, operatingCenter.Id) as JsonResult;
            var helper = new JsonResultTester(result?.Data);

            Assert.AreEqual(0, helper.Count);
        }

        #endregion

        #region LinkOrNew

        [TestMethod]
        public void TestLinkOrNewReturnsViewWithServiceNumber()
        {
            var premiseNumber = "000000000";
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = premiseNumber, OperatingCenter = operatingCenter, Town = town
            });
            var service = GetEntityFactory<Service>().Create(new {
                ServiceNumber = 123L, PremiseNumber = premiseNumber, OperatingCenter = operatingCenter, Town = town
            });
            _roleService.Setup(x => x.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add, workOrder.OperatingCenter)).Returns(true);

            var result = (ViewResult)_target.LinkOrNew(workOrder.Id);
            var model = (LinkOrNewService)result.Model;

            Assert.AreEqual(model.RelatedServices.FirstOrDefault().Id, service.Id);
        }

        [TestMethod]
        public void TestLinkOrNewReturnsViewWithoutServiceNumber()
        {
            var premiseNumber = "000000000";
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = premiseNumber, ServiceNumber = "123", OperatingCenter = operatingCenter, Town = town
            });
            var service = GetEntityFactory<Service>().Create(new {
                ServiceNumber = 123L, PremiseNumber = premiseNumber, OperatingCenter = operatingCenter, Town = town
            });
            _roleService.Setup(x => x.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add, workOrder.OperatingCenter)).Returns(true);

            var result = (ViewResult)_target.LinkOrNew(workOrder.Id);
            var model = (LinkOrNewService)result.Model;

            Assert.AreEqual(model.RelatedServices.FirstOrDefault().Id, service.Id);
        }

        [TestMethod]
        public void TestLinkOrNewReturnsViewWithoutService()
        {
            var premiseNumber = "000000000";
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = premiseNumber, ServiceNumber = "123", OperatingCenter = operatingCenter, Town = town
            });
            _roleService.Setup(x => x.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add, workOrder.OperatingCenter)).Returns(true);

            var result = (ViewResult)_target.LinkOrNew(workOrder.Id);
            var model = (LinkOrNewService)result.Model;

            MyAssert.IsEmpty(model.RelatedServices);
        }

        [TestMethod]
        public void TestLinkOrNewReturnsRoleAccessView()
        {
            var premiseNumber = "000000000";
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new
            {
                PremiseNumber = premiseNumber,
                ServiceNumber = "123",
                OperatingCenter = operatingCenter,
                Town = town
            });

            var result = _target.LinkOrNew(workOrder.Id);
            var model = (ForbiddenRoleAccessModel)((ViewResult)result).Model;
            
            MvcAssert.IsViewNamed(result, "~/Views/Shared/ForbiddenRoleAccess.cshtml");
            Assert.AreEqual(1, model.RequiredRoles.Count);
            Assert.AreEqual(RoleModules.FieldServicesAssets, model.RequiredRoles[0].Module);
            Assert.AreEqual(RoleActions.Add, model.RequiredRoles[0].Action);
        }

        #endregion

        #region RemoveWorkOrder

        [TestMethod]
        public void TestRemoveWorkOrderRedirectsToShowPageWhenModelIsInvalid()
        {
            var service = GetEntityFactory<Service>().Create();
            var model = new RemoveServiceWorkOrder(_container) {
                Id = service.Id,
                WorkOrder = 12345
            };
            var result = _target.RemoveWorkOrder(model);
            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "Service", id = service.Id });
        }

        #endregion

        #endregion
    }
}
