using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.ExternalStreetOpeningPermits;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.StreetOpeningPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing;
using Moq;
using Permits.Data.Client.Entities;
using Permits.Data.Client.Repositories;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ExternalStreetOpeningPermitControllerTest
        : MapCallMvcControllerTestBase<ExternalStreetOpeningPermitController, StreetOpeningPermit>
    {
        #region Fields

        private Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Drawing>> _permitDrawingRepository;
        private Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.State>> _permitStateRepository;
        private Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.County>> _permitCountyRepository;
        private Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Municipality>> _permitMunicipalityRepository;
        private Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Payment>> _permitPaymentRepository;
        private Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Permit>> _permitPermitRepository;

        private WorkOrder _workOrder;
        private OperatingCenter _operatingCenter;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void IntializeTest()
        {
            _operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { PermitsOMUserName = "this@user.com" });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(_operatingCenter);
            _workOrder = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = _operatingCenter });
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            _permitDrawingRepository = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Drawing>>();
            _permitStateRepository = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.State>>();
            _permitCountyRepository = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.County>>();
            _permitMunicipalityRepository = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Municipality>>();
            _permitPaymentRepository = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Payment>>();
            _permitPermitRepository = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Permit>>();
            e.For<IPermitsRepositoryFactory>().Use<PermitsRepositoryFactory>();
            e.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Drawing>>().Use(_permitDrawingRepository.Object);
            e.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.State>>().Use(_permitStateRepository.Object);
            e.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.County>>().Use(_permitCountyRepository.Object);
            e.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Municipality>>().Use(_permitMunicipalityRepository.Object);
            e.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Payment>>().Use(_permitPaymentRepository.Object);
            e.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Permit>>().Use(_permitPermitRepository.Object);
        }

        #endregion

        #region Private Methods
        
        private void AddWorkManagementRoleToCurrentUserForOperatingCenter(OperatingCenter opc)
        {
            var role = GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, opc, _currentUser, RoleActions.UserAdministrator);
        }   

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;
            var urlBase = "~/FieldOperations/ExternalStreetOpeningPermit/";

            Authorization.Assert(a => {
                a.RequiresRole(urlBase + "New", role, RoleActions.UserAdministrator);
                a.RequiresRole(urlBase + "NewForPermitForm", role, RoleActions.UserAdministrator);
                a.RequiresRole(urlBase + "Create", role, RoleActions.UserAdministrator);
                a.RequiresRole(urlBase + "EditDrawings", role, RoleActions.UserAdministrator);
                a.RequiresRole(urlBase + "UploadDrawing", role, RoleActions.UserAdministrator);
                a.RequiresRole(urlBase + "BeginPayment", role, RoleActions.UserAdministrator);
                a.RequiresRole(urlBase + "CompletePayment", role, RoleActions.UserAdministrator);
            });
        }

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // no-op: This test is handled by all the other New tests.
        }

        [TestMethod]
        public void TestNewReturnsNotFoundIfWorkOrderDoesNotExist()
        {
            var result = _target.New(0);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestNewReturnsViewModelWithAllPermitsAPIInformationForStatesCountiesAndMunicipalitiesWithForms()
        {
            // Sorry for this. There's no better way to test the interactions we're supposed
            // to have with the permits endpoint. It requires all of this.

            // STATES
            var stateSearch = new NameValueCollection();
            var stateResponse = new[] {
                new Permits.Data.Client.Entities.State {
                    Id = 1,
                    FormId = 100,
                }
            };

            _permitStateRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                  .Callback((NameValueCollection x) => {
                                       stateSearch = x;
                                   })
                                  .Returns(stateResponse);

            // COUNTIES
            var countySearch = new NameValueCollection();
            var countyResponse = new[] {
                new Permits.Data.Client.Entities.County {
                    Id = 2,
                    FormId = 200,
                }
            };

            _permitCountyRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                   .Callback((NameValueCollection x) => {
                                        countySearch = x;
                                    })
                                   .Returns(countyResponse);

            // MUNICIPALITIES
            var municipalitySearch = new NameValueCollection();
            var municipalityResponse = new[] {
                new Permits.Data.Client.Entities.Municipality {
                    Id = 3,
                    FormId = 300,
                }
            };

            _permitMunicipalityRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                         .Callback((NameValueCollection x) => {
                                              municipalitySearch = x;
                                          })
                                         .Returns(municipalityResponse);

            var result = (ViewResult)_target.New(_workOrder.Id);
            MvcAssert.IsViewNamed(result, "New");
            
            // QUERY ASSERTIONS
            Assert.AreEqual(_workOrder.State.Name, stateSearch["name"]);
            Assert.AreEqual(_workOrder.Town.County.Name, countySearch["name"]);
            Assert.AreEqual("1", countySearch["stateId"]);
            Assert.AreEqual(_workOrder.Town.ShortName, municipalitySearch["name"]);
            Assert.AreEqual("2", municipalitySearch["countyId"]);

            // MODEL ASSERTIONS
            var model = (NewExternalStreetOpeningPermitFormOptions)result.Model;
            Assert.AreSame(_workOrder, model.WorkOrder);
            Assert.AreEqual(100, model.PermitsApiStateFormId);
            Assert.AreEqual(200, model.PermitsApiCountyFormId);
            Assert.AreEqual(300, model.PermitsApiMunicipalityFormId);
            Assert.IsFalse(model.UnableToConnectToApi);
        }

        [TestMethod]
        public void TestNewReturnsViewModelWithoutPermitsApiInfoWhenTheApiDoesNotHaveAnyFormsForAnything()
        {
            // Sorry for this. There's no better way to test the interactions we're supposed
            // to have with the permits endpoint. It requires all of this.

            // STATES
            var stateSearch = new NameValueCollection();
            var stateResponse = new[] {
                new Permits.Data.Client.Entities.State {
                    Id = 1,
                    FormId = 0,
                }
            };

            _permitStateRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                  .Callback((NameValueCollection x) => {
                                       stateSearch = x;
                                   })
                                  .Returns(stateResponse);

            // COUNTIES
            var countySearch = new NameValueCollection();
            var countyResponse = new[] {
                new Permits.Data.Client.Entities.County {
                    Id = 2,
                    FormId = 0,
                }
            };

            _permitCountyRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                   .Callback((NameValueCollection x) => {
                                        countySearch = x;
                                    })
                                   .Returns(countyResponse);

            // MUNICIPALITIES
            var municipalitySearch = new NameValueCollection();
            var municipalityResponse = new[] {
                new Permits.Data.Client.Entities.Municipality {
                    Id = 3,
                    FormId = 0,
                }
            };

            _permitMunicipalityRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                         .Callback((NameValueCollection x) => {
                                              municipalitySearch = x;
                                          })
                                         .Returns(municipalityResponse);

            var result = (ViewResult)_target.New(_workOrder.Id);
            MvcAssert.IsViewNamed(result, "New");
            
            // QUERY ASSERTIONS
            Assert.AreEqual(_workOrder.State.Name, stateSearch["name"]);
            Assert.AreEqual(_workOrder.Town.County.Name, countySearch["name"]);
            Assert.AreEqual("1", countySearch["stateId"]);
            Assert.AreEqual(_workOrder.Town.ShortName, municipalitySearch["name"]);
            Assert.AreEqual("2", municipalitySearch["countyId"]);

            // MODEL ASSERTIONS
            var model = (NewExternalStreetOpeningPermitFormOptions)result.Model;
            Assert.AreSame(_workOrder, model.WorkOrder);
            Assert.IsNull(model.PermitsApiStateFormId);
            Assert.IsNull(model.PermitsApiCountyFormId);
            Assert.IsNull(model.PermitsApiMunicipalityFormId);
            Assert.IsFalse(model.UnableToConnectToApi);
        }
        
        [TestMethod]
        public void TestNewDoesNotSearchForCountiesOrMunicipalitiesIfStateIsNull()
        {
            // Sorry for this. There's no better way to test the interactions we're supposed
            // to have with the permits endpoint. It requires all of this.

            // STATES
            var stateSearch = new NameValueCollection();
            _permitStateRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                  .Callback((NameValueCollection x) => {
                                       stateSearch = x;
                                   })
                                  .Returns((IEnumerable<Permits.Data.Client.Entities.State>)null);

            var result = (ViewResult)_target.New(_workOrder.Id);
            MvcAssert.IsViewNamed(result, "New");
            
            // QUERY ASSERTIONS
            Assert.AreEqual(_workOrder.State.Name, stateSearch["name"]);

            // MODEL ASSERTIONS
            var model = (NewExternalStreetOpeningPermitFormOptions)result.Model;
            Assert.AreSame(_workOrder, model.WorkOrder);
            Assert.IsNull(model.PermitsApiStateFormId);
            Assert.IsNull(model.PermitsApiCountyFormId);
            Assert.IsNull(model.PermitsApiMunicipalityFormId);
            Assert.IsTrue(model.UnableToConnectToApi, "When the states enumerable is null, this indicates the permits api is not reachable.");
        }

        [TestMethod]
        public void TestNewDoesNotSearchForMunicipalitiesIfCountyIsNull()
        {
            // STATES
            var stateSearch = new NameValueCollection();
            var stateResponse = new[] {
                new Permits.Data.Client.Entities.State {
                    Id = 1,
                    FormId = 100,
                }
            };

            _permitStateRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                  .Callback((NameValueCollection x) => {
                                       stateSearch = x;
                                   })
                                  .Returns(stateResponse);

            // COUNTIES
            var countySearch = new NameValueCollection();
            _permitCountyRepository.Setup(x => x.Search(It.IsAny<NameValueCollection>()))
                                   .Callback((NameValueCollection x) => {
                                        countySearch = x;
                                    })
                                   .Returns((IEnumerable<Permits.Data.Client.Entities.County>)null);

            var result = (ViewResult)_target.New(_workOrder.Id);
            MvcAssert.IsViewNamed(result, "New");
            
            // QUERY ASSERTIONS
            Assert.AreEqual(_workOrder.State.Name, stateSearch["name"]);
            Assert.AreEqual(_workOrder.Town.County.Name, countySearch["name"]);
            Assert.AreEqual("1", countySearch["stateId"]);

            // MODEL ASSERTIONS
            var model = (NewExternalStreetOpeningPermitFormOptions)result.Model;
            Assert.AreSame(_workOrder, model.WorkOrder);
            Assert.AreEqual(100, model.PermitsApiStateFormId);
            Assert.IsNull(model.PermitsApiCountyFormId);
            Assert.IsNull(model.PermitsApiMunicipalityFormId);
            Assert.IsFalse(model.UnableToConnectToApi);
        }

        #endregion

        #region NewForPermitForm

        [TestMethod]
        public void TestNewForPermitFormReturnsValidationErrorsViewIfModelHasAnyValidationErrors()
        {
            var model = new NewExternalStreetOpeningPermitForm();
            _target.ModelState.AddModelError("Whoops", "Ow!");
            var result = _target.NewForPermitForm(model);
            MvcAssert.IsViewNamed(result, "ValidationFailures");
        }

        [TestMethod]
        public void TestNewForPermitFormReturnsModelWithPermitFormHtml()
        {
            var model = new NewExternalStreetOpeningPermitForm();
            model.NoApiConflictWorkOrderId = _workOrder.Id;
            model.FormId = 123;
            var permitSearch = new NameValueCollection();
            var permitResponse = "a bunch of horrifying html";
            _permitPermitRepository.Setup(x => x.New(It.IsAny<NameValueCollection>()))
                                   .Callback((NameValueCollection x) => {
                                        permitSearch = x;
                                    })
                                   .Returns(permitResponse);

            var result = _target.NewForPermitForm(model);
            Assert.AreEqual(permitResponse, model.PermitFormHtml);
            Assert.AreSame(_workOrder, model.WorkOrder);
            Assert.AreEqual("123", permitSearch["formId"]);
            MvcAssert.IsViewNamed(result, "NewForPermitForm");
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop - the automated tests will not work with this controller.
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // noop - the automated tests will not work with this controller.
        }

        [TestMethod]
        public void TestCreateReturnsValidationFailuresViewIfThereAreModelStateErrors()
        {
            _target.ModelState.AddModelError("Whoops", "Ow!");
            var result = _target.Create(new NewExternalStreetOpeningPermitForm());
            MvcAssert.IsViewNamed(result, "ValidationFailures");
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            var model = new NewExternalStreetOpeningPermitForm();
            model.NoApiConflictWorkOrderId = _workOrder.Id;
            var apiPermit = new Permits.Data.Client.Entities.Permit();
            apiPermit.Id = 12345;
            apiPermit.HasMetDrawingRequirement = true;
            apiPermit.IsPaidFor = true;
            var expectedDate = DateTime.Today;
            _dateTimeProvider.SetNow(expectedDate);

            _permitPermitRepository.Setup(x => x.Save(Request.RequestForm)).Returns(apiPermit);

            _target.Create(model);

            var result = Session.Query<StreetOpeningPermit>().Single(x => x.PermitId == 12345);
            Assert.AreSame(result.WorkOrder, _workOrder);
            Assert.AreEqual(result.DateRequested, expectedDate);
            Assert.AreEqual(string.Empty, result.StreetOpeningPermitNumber);
            Assert.IsTrue(apiPermit.HasMetDrawingRequirement, "This wouldn't be true in practice, but this asserts that these values are being checked and set during creation.");
            Assert.IsTrue(apiPermit.IsPaidFor, "This wouldn't be true in practice, but this asserts that these values are being checked and set during creation.");
        }

        [TestMethod]
        public void TestCreateRedirectsToEditDrawingsIfPermitHasNotMetDrawingRequirements()
        {
            var model = new NewExternalStreetOpeningPermitForm();
            model.NoApiConflictWorkOrderId = _workOrder.Id;
            var apiPermit = new Permits.Data.Client.Entities.Permit();
            apiPermit.Id = 12345;
            apiPermit.HasMetDrawingRequirement = false;
            _permitPermitRepository.Setup(x => x.Save(Request.RequestForm)).Returns(apiPermit);

            var result = _target.Create(model);
            var mapcallPermit = Session.Query<StreetOpeningPermit>().Single(x => x.PermitId == 12345);

            MvcAssert.RedirectsToRoute(result, new { action = "EditDrawings", id = mapcallPermit.Id });
        }

        [TestMethod]
        public void TestCreateRedirectsToBeginPaymentIfPermitHasMetDrawingRequirements()
        {
            var model = new NewExternalStreetOpeningPermitForm();
            model.NoApiConflictWorkOrderId = _workOrder.Id;
            var apiPermit = new Permits.Data.Client.Entities.Permit();
            apiPermit.Id = 12345;
            apiPermit.HasMetDrawingRequirement = true;
            _permitPermitRepository.Setup(x => x.Save(Request.RequestForm)).Returns(apiPermit);

            var result = _target.Create(model);
            var mapcallPermit = Session.Query<StreetOpeningPermit>().Single(x => x.PermitId == 12345);

            MvcAssert.RedirectsToRoute(result, new { action = "BeginPayment", id = mapcallPermit.Id });
        }

        #endregion

        #region EditDrawings

        [TestMethod]
        public void TestEditDrawingsReturnsNotFoundIfPermitDoesNotExist()
        {
            var result = _target.EditDrawings(0);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestEditDrawingsReturnsViewWithModel()
        {
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new { WorkOrder = _workOrder });
            var result = _target.EditDrawings(sop.Id);
            MvcAssert.IsViewNamed(result, "EditDrawings");
            var model = (ExternalStreetOpeningPermitUploadDrawingViewModel)((ViewResult)result).Model;
            Assert.AreEqual(sop.Id, model.StreetOpeningPermit);
        }

        #endregion

        #region UploadDrawing

        [TestMethod]
        public void TestUploadDrawingReturnsNotFoundIfPermitDoesNotExist()
        {
            var result = _target.UploadDrawing(null, 0);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestUploadDrawingSendsDrawingToPermitsApi()
        {
            var uploadModel = new FileController.UploadModel();
            uploadModel.FileUpload = new MMSINC.Metadata.AjaxFileUpload {
                BinaryData = new byte[] { 1, 2, 3, 4, 5 },
                FileName = "I'm in danger!"
            };
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new { WorkOrder = _workOrder, PermitId = 123 });
            Permits.Data.Client.Entities.Drawing expectedDrawing = null;
            _permitDrawingRepository.Setup(x => x.Save(It.IsAny<Drawing>()))
                                    .Callback((Drawing x) => {
                                         expectedDrawing = x;
                                     });

            _target.UploadDrawing(uploadModel, sop.Id);

            Assert.AreSame(uploadModel.FileUpload.BinaryData, expectedDrawing.FileUpload.BinaryData);
            Assert.AreEqual(uploadModel.FileUpload.FileName, expectedDrawing.FileUpload.FileName);
            Assert.AreEqual(123, expectedDrawing.PermitId);
        }

        [TestMethod]
        public void TestUploadDrawingGetsUpdatedPermitInformationFromPermitsApiWhenDrawingUploadsSuccessfully()
        {
            var uploadModel = new FileController.UploadModel();
            uploadModel.FileUpload = new MMSINC.Metadata.AjaxFileUpload {
                BinaryData = new byte[] { 1, 2, 3, 4, 5 },
                FileName = "I'm in danger!"
            };
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new {
                WorkOrder = _workOrder, 
                PermitId = 123,
                HasMetDrawingRequirement = false,
                IsPaidFor = false
            });
            var expectedApiPermit = new Permit { HasMetDrawingRequirement = true, IsPaidFor = true };
            _permitDrawingRepository.Setup(x => x.Save(It.IsAny<Drawing>()))
                                    .Returns(new Drawing());
            _permitPermitRepository.Setup(x => x.Find(123)).Returns(expectedApiPermit);

            var result = (JsonResult)_target.UploadDrawing(uploadModel, sop.Id);

            Assert.IsTrue(sop.HasMetDrawingRequirement);
            Assert.IsTrue(sop.IsPaidFor);
            var data = result.Data.AsDynamic();
            Assert.IsTrue(data.success);
            Assert.IsTrue(data.hasMetDrawingRequirement);
            Assert.AreEqual("I'm in danger!", data.fileName);
            Assert.IsNotNull(data.key, "This will always be a random guid.");
        }

        [TestMethod]
        public void TestUploadDrawingReturnsSuccessFalseIfPermitsApiReturnsNull()
        {
            var uploadModel = new FileController.UploadModel();
            uploadModel.FileUpload = new MMSINC.Metadata.AjaxFileUpload {
                BinaryData = new byte[] { 1, 2, 3, 4, 5 },
                FileName = "I'm in danger!"
            };
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new { WorkOrder = _workOrder, PermitId = 123 });
            _permitDrawingRepository.Setup(x => x.Save(It.IsAny<Drawing>()))
                                    .Returns((Drawing)null);
            
            var result = (JsonResult)_target.UploadDrawing(uploadModel, sop.Id);
            Assert.IsFalse(result.Data.AsDynamic().success);
        }

        #endregion

        #region BeginPayment

        [TestMethod]
        public void TestBeginPaymentReturnsNotFoundIfPermitDoesNotExist()
        {
            var result = _target.BeginPayment(0);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestBeginPaymentRedirectsToEditDrawingsIfPermitHasNotMetDrawingRequirement()
        {
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new {
                WorkOrder = _workOrder,
                PermitId = 12345,
                HasMetDrawingRequirement = false,
            });

            var result = _target.BeginPayment(sop.Id);

            MvcAssert.RedirectsToRoute(result, new { action = "EditDrawings", id = sop.Id });
        }

        [TestMethod]
        public void TestBeginPaymentCallsPermitApiAndGetsPaymentHtmlAndReturnsView()
        {
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new {
                WorkOrder = _workOrder,
                PermitId = 12345,
                HasMetDrawingRequirement = true,
                IsPaidFor = false,
            });

            var paymentNew = new NameValueCollection();
            var paymentHtmlResponse = "some terrible html";

            _permitPaymentRepository.Setup(x => x.New(It.IsAny<NameValueCollection>()))
                                    .Callback((NameValueCollection x) => {
                                         paymentNew = x;
                                     })
                                    .Returns(paymentHtmlResponse);

            var result = (ViewResult)_target.BeginPayment(sop.Id);
            var model = (ExternalStreetOpeningPermitBeginPayment)result.Model;
            Assert.AreEqual(sop.Id, model.Id);
            Assert.AreEqual(paymentHtmlResponse, model.PaymentFormHtml);
            Assert.AreEqual("12345", paymentNew["permitId"]);
        }

        #endregion

        #region CompletePayment

        [TestMethod]
        public void TestCompletePaymentReturnsNotFoundIfPermitDoesNotExist()
        {
            var result = _target.CompletePayment(0);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestCompletePaymentRedirectsToEditDrawingsIfPermitHasNotMetDrawingRequirement()
        {
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new {
                WorkOrder = _workOrder,
                PermitId = 12345,
                HasMetDrawingRequirement = false,
            });

            var result = _target.CompletePayment(sop.Id);

            MvcAssert.RedirectsToRoute(result, new { action = "EditDrawings", id = sop.Id });
        }

        [TestMethod]
        public void TestCompletePaymentDoesNotPostToPermitsApiForPaymentIfMapCallPermitIsAlreadyPaidFor()
        {
            // Also test that it returns the AlreadyPaid view.
            // verify the permit repo is not called
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new {
                WorkOrder = _workOrder,
                PermitId = 12345,
                IsPaidFor = true,
            });

            var result = _target.CompletePayment(sop.Id);

            _permitPaymentRepository.Verify(x => x.New(It.IsAny<NameValueCollection>()), Times.Never);
            MvcAssert.IsViewNamed(result, "AlreadyPaid");
        }

        [TestMethod]
        public void TestCompletePaymentPostsToPaymentsApiAndUpdatesPermitAndReturnsPaymentCompleteView()
        {
            var sop = GetEntityFactory<StreetOpeningPermit>().Create(new {
                WorkOrder = _workOrder,
                PermitId = 12345,
                HasMetDrawingRequirement = true,
                IsPaidFor = false,
            });

            var paymentSave = new NameValueCollection();
            _permitPaymentRepository.Setup(x => x.Save(It.IsAny<NameValueCollection>()))
                                    .Callback((NameValueCollection x) => {
                                         paymentSave = x;
                                     });
            _permitPermitRepository.Setup(x => x.Find(12345)).Returns(new Permit { IsPaidFor = true });

            var result = (ViewResult)_target.CompletePayment(sop.Id);

            Assert.AreEqual("12345", paymentSave["permitId"]);
            Assert.IsTrue(sop.IsPaidFor);
            MvcAssert.IsViewNamed(result, "PaymentComplete");
        }

        #endregion

        #endregion
    }
}
