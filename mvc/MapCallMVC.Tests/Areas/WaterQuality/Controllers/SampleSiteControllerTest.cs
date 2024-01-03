using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class SampleSiteControllerTest : MapCallMvcControllerTestBase<SampleSiteController, SampleSite>
    {
        #region Private Members

        private Mock<INotificationService> _notificationService;

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            const RoleModules role = SampleSiteController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/SampleSite/Search/", role);
                a.RequiresRole("~/WaterQuality/SampleSite/Show/", role);
                a.RequiresRole("~/WaterQuality/SampleSite/Index/", role);
                a.RequiresRole("~/WaterQuality/SampleSite/New/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/SampleSite/Create/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/SampleSite/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleSite/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleSite/AddSamplePlan", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleSite/RemoveSamplePlan", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleSite/AddBracketSite", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleSite/RemoveBracketSite", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleSite/Destroy/", role, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/ByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/ByOperatingCenterIds/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/ActiveBactiSitesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/ByOperatingCenterIdWithMatrices/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/BySampleMatrixId/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/GetSampleSitesByPremiseNumber/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/GetPrimarySampleSitesByStateId/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSite/GetPrimaryEligibleSampleSitesByStateId/");
            });
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject((_notificationService = new Mock<INotificationService>()).Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexRedirectsToShowForSingleResult = true;
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateSampleSite)vm;
                model.Availability = GetEntityFactory<SampleSiteAvailability>().Create().Id;
                model.Status = GetEntityFactory<SampleSiteStatus>().Create().Id;
                model.Coordinate = GetEntityFactory<Coordinate>().Create().Id;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.Facility = GetEntityFactory<Facility>().Create().Id;
                model.SampleSiteAddressLocationType = GetEntityFactory<SampleSiteAddressLocationType>().Create().Id;
                model.LocationNameDescription = "some place";
                model.LeadCopperSite = false;
                model.AgencyId = "Some agency";
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditSampleSite)vm;
                model.Availability = GetEntityFactory<SampleSiteAvailability>().Create().Id;
                model.Status = GetEntityFactory<SampleSiteStatus>().Create().Id;
                model.Coordinate = GetEntityFactory<Coordinate>().Create().Id;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.Facility = GetEntityFactory<Facility>().Create().Id;
                model.SampleSiteAddressLocationType = GetEntityFactory<SampleSiteAddressLocationType>().Create().Id;
                model.LocationNameDescription = "some place";
                model.LeadCopperSite = false;
                model.AgencyId = "Some agency";
            };
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<SampleSite>().Create(new { LocationNameDescription = "description 0" });
            var entity1 = GetEntityFactory<SampleSite>().Create(new { LocationNameDescription = "description 1" });
            var search = new SearchSampleSite();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.LocationNameDescription, "LocationNameDescription");
                helper.AreEqual(entity1.LocationNameDescription, "LocationNameDescription", 1);
            }
        }

        [TestMethod]
        public void TestIndexXLSExportsExcelAndIncludesNotesAndServiceMaterialUsedAndCustomerSideMaterialUsed()
        {
            var premise0 = GetEntityFactory<Premise>().Create();
            var entity0 = GetEntityFactory<SampleSite>().Create(new { LocationNameDescription = "description 0"});
            var entity1 = GetEntityFactory<SampleSite>().Create(new { LocationNameDescription = "description 1" });
            
            var premiseService = GetEntityFactory<Service>().Create(new {
                Premise = premise0,
                ServiceMaterial = GetFactory<ServiceMaterialFactory>().Create(new {
                    Description = "My service material"
                }),
                CustomerSideMaterial = GetFactory<ServiceMaterialFactory>().Create(new {
                    Description = "The customer side material"
                })
            });

            var premiseMostRecentInstalledService = new MostRecentlyInstalledService {
                Premise = premise0,
                Service = premiseService
            };

            var note1 = new Note<SampleSite> {
                Id = 1,
                Entity = entity0,
                LinkedId = entity0.Id,
                Note = GetEntityFactory<Note>().Create(new {
                    Text = "Entity zero first note"
                }),
                DataType = GetEntityFactory<DataType>().Create()
            };

            var note2 = new Note<SampleSite> {
                Id = 2,
                Entity = entity1,
                LinkedId = entity1.Id,
                Note = GetEntityFactory<Note>().Create(new {
                    Text = "Entity one first note"
                }),
                DataType = GetEntityFactory<DataType>().Create()
            };

            premise0.MostRecentService = premiseMostRecentInstalledService;
            
            entity0.SampleSiteNotes.Add(note1);
            entity0.Premise = premise0;

            entity1.SampleSiteNotes.Add(note2);

            var search = new SearchSampleSite();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.LocationNameDescription, "LocationNameDescription");
                helper.AreEqual(entity1.LocationNameDescription, "LocationNameDescription", 1);
                helper.AreEqual(entity0.Notes, "Notes");
                helper.AreEqual(entity1.Notes, "Notes", 1);
                helper.AreEqual(entity0.ServiceMaterialUsed, "ServiceMaterialUsed");
                helper.AreEqual(entity0.CustomerSideMaterialUsed, "CustomerSideMaterialUsed");
                helper.IsNull("ServiceMaterialUsed", 1);
                helper.IsNull("CustomerSideMaterialUsed", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotificationForNewChemicalSampleSites()
        {
            var entity = BuildEntity<SampleSite>(new {
                IsLimsLocation = true,
                LimsFacilityId = "lims-facility-id",
                LimsSiteId = "lims-site-id",
                State = GetFactory<StateFactory>().Create(new { Abbreviation = "NJ" }),
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                SampleSiteProfile = GetFactory<SampleSiteProfileFactory>().Create(new {
                    SampleSiteProfileAnalysisType = GetFactory<ChemicalSampleSiteProfileAnalysisTypeFactory>().Create()
                })
            });

            var expectedNotifierArgs = new NotifierArgs {
                OperatingCenterId = entity.OperatingCenter.Id,
                Module = SampleSiteController.ROLE,
                Purpose = SampleSiteController.CHEMICAL_SAMPLE_SITE_CREATED_PURPOSE,
                Address = _target.AuthenticationService.CurrentUser.Email,
                Data = entity
            };

            Func<NotifierArgs, bool> notifierArgumentsAreSame = actualNotifierArgs => 
                actualNotifierArgs.OperatingCenterId == entity.OperatingCenter.Id &&
                actualNotifierArgs.Module == expectedNotifierArgs.Module &&
                actualNotifierArgs.Purpose == expectedNotifierArgs.Purpose &&
                actualNotifierArgs.Address == expectedNotifierArgs.Address &&
                ((SampleSite)actualNotifierArgs.Data).Id > 0 &&
                ((SampleSite)actualNotifierArgs.Data).ToString() == entity.ToString();

            _target.Create(_viewModelFactory.Build<CreateSampleSite, SampleSite>(entity));

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(y => notifierArgumentsAreSame(y))));
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SampleSite>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSampleSite, SampleSite>(eq, new {
                LocationNameDescription = expected
            }));

            Assert.AreEqual(expected, Session.Get<SampleSite>(eq.Id).LocationNameDescription);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationForChemicalSampleSitesWhenStatusChanges()
        {
            var entity = CreateEntity<SampleSite>(new {
                IsLimsLocation = true,
                LimsFacilityId = "lims-facility-id",
                LimsSiteId = "lims-site-id",
                State = GetFactory<StateFactory>().Create(new { Abbreviation = "NJ" }),
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                Status = GetFactory<InactiveSampleSiteStatusFactory>().Create(),
                SampleSiteProfile = GetFactory<SampleSiteProfileFactory>().Create(new {
                    SampleSiteProfileAnalysisType = GetFactory<ChemicalSampleSiteProfileAnalysisTypeFactory>().Create()
                })
            });

            var expectedNotifierArgs = new NotifierArgs {
                OperatingCenterId = entity.OperatingCenter.Id,
                Module = SampleSiteController.ROLE,
                Purpose = SampleSiteController.CHEMICAL_SAMPLE_SITE_STATUS_CHANGED_PURPOSE,
                Address = _target.AuthenticationService.CurrentUser.Email,
                Data = entity
            };

            Func<NotifierArgs, bool> notifierArgumentsAreSame = actualNotifierArgs => actualNotifierArgs.OperatingCenterId == entity.OperatingCenter.Id &&
                                                                                      actualNotifierArgs.Module == expectedNotifierArgs.Module &&
                                                                                      actualNotifierArgs.Purpose == expectedNotifierArgs.Purpose &&
                                                                                      actualNotifierArgs.Address == expectedNotifierArgs.Address &&
                                                                                      ((SampleSite)actualNotifierArgs.Data).Id == entity.Id && 
                                                                                      ((SampleSite)actualNotifierArgs.Data).ToString() == entity.ToString();

            _target.Update(_viewModelFactory.BuildWithOverrides<EditSampleSite, SampleSite>(entity, new {
                Status = GetFactory<ActiveSampleSiteStatusFactory>().Create().Id
            }));

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(y => notifierArgumentsAreSame(y))));
        }

        [TestMethod]
        public void TestAddSamplePlanRedirectsToSampleSiteShowPageWhenSuccessful()
        {
            var sampleSite = GetEntityFactory<SampleSite>().Create();
            var samplePlan = GetEntityFactory<SamplePlan>().Create();

            var model = new AddSampleSiteSamplePlan(_container) { Id = sampleSite.Id, SamplePlan = samplePlan.Id };

            var result = _target.AddSamplePlan(model);

            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "SampleSite", id = sampleSite.Id });
        }

        [TestMethod]
        public void TestRemoveSamplePlanRedirectsToSamplePlanShowPageWhenSuccessful()
        {
            var sampleSite = GetEntityFactory<SampleSite>().Create();
            var samplePlan = GetEntityFactory<SamplePlan>().Create();
            sampleSite.SamplePlans.Add(samplePlan);

            var model = new RemoveSampleSiteSamplePlan(_container) { Id = sampleSite.Id, SamplePlan = samplePlan.Id};

            var result = _target.RemoveSamplePlan(model);

            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "SampleSite", id = sampleSite.Id });
        }

        #endregion

        #region Ajaxie Methods

        [TestMethod]
        public void TestByOperatingCenterIdReturnsSampleSitesByOperatingCenterId()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();

            /* matches operating center */
            var sampleSite = GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterA
            });

            /* does not match operating center */
            GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterB
            });

            var actionResult = (CascadingActionResult)_target.ByOperatingCenterId(operatingCenterA.Id);
            var sampleSites = ((IEnumerable<SampleSiteDisplayItem>)actionResult.Data).ToList();
            Assert.AreEqual(1, sampleSites.Count);
            Assert.AreEqual(sampleSite.Id, sampleSites[0].Id);
        }

        [TestMethod]
        public void TestByOperatingCenterIdsReturnsSampleSitesByOperatingCenterIds()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterC = GetFactory<UniqueOperatingCenterFactory>().Create();

            /* matches operating center a */
            var sampleSiteA = GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterA
            });

            /* matches operating center b */
            var sampleSiteB = GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterB
            });

            /* does not match operating center */
            GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterC
            });

            var actionResult = (CascadingActionResult)_target.ByOperatingCenterIds(new int[] { operatingCenterA.Id, operatingCenterB.Id });
            var sampleSites = ((IEnumerable<SampleSiteDisplayItem>)actionResult.Data).ToList();
            Assert.AreEqual(2, sampleSites.Count);
            Assert.AreEqual(sampleSiteA.Id, sampleSites[0].Id);
            Assert.AreEqual(sampleSiteB.Id, sampleSites[1].Id);
        }

        [TestMethod]
        public void TestActiveBactiSitesByOperatingCenterIdReturnsActiveBactiSitesByOperatingCenterId()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();

            /* matches operating center and is bacti site and is active */
            var sampleSite = GetEntityFactory<SampleSite>().Create(new {
                BactiSite = true,
                OperatingCenter = operatingCenterA,
                Status = GetFactory<ActiveSampleSiteStatusFactory>().Create(),
            });

            /* matches operating center and is bacti site but is inactive */
            GetEntityFactory<SampleSite>().Create(new {
                BactiSite = true,
                OperatingCenter = operatingCenterA,
                Status = GetFactory<InactiveSampleSiteStatusFactory>().Create(),
            });

            /* matches operating center and is active but is not bacti site */
            GetEntityFactory<SampleSite>().Create(new {
                BactiSite = false,
                OperatingCenter = operatingCenterA,
                Status = GetFactory<ActiveSampleSiteStatusFactory>().Create(),
            });

            /* is bacti site and is active but does not match operating center */
            GetEntityFactory<SampleSite>().Create(new {
                BactiSite = true,
                OperatingCenter = operatingCenterB,
                Status = GetFactory<ActiveSampleSiteStatusFactory>().Create(),
            });

            var actionResult = (CascadingActionResult)_target.ActiveBactiSitesByOperatingCenterId(operatingCenterA.Id);
            var sampleSites = ((IEnumerable<SampleSiteDisplayItem>)actionResult.Data).ToList();
            Assert.AreEqual(1, sampleSites.Count);
            Assert.AreEqual(sampleSite.Id, sampleSites[0].Id);
        }

        [TestMethod]
        public void TestByOperatingCenterIdWithMatricesReturnsByOperatingCenterSamplesSitesWithMatrices()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();

            /* matches operating center and has matrices */
            var sampleSiteA = GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterA
            });

            sampleSiteA.SampleIdMatrices = GetFactory<SampleIdMatrixFactory>().CreateList(10, new {
                SampleSite = sampleSiteA
            });

            /* has matrices but does not match operating center */
            var sampleSiteB = GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterB
            });

            sampleSiteB.SampleIdMatrices = GetFactory<SampleIdMatrixFactory>().CreateList(10, new {
                SampleSite = sampleSiteB
            });

            /* matches operating center and does not have matrices */
            GetEntityFactory<SampleSite>().Create(new {
                OperatingCenter = operatingCenterA
            });

            var actionResult = (CascadingActionResult)_target.ByOperatingCenterIdWithMatrices(operatingCenterA.Id);
            var sampleSites = ((IEnumerable<SampleSiteDisplayItem>)actionResult.Data).ToList();
            Assert.AreEqual(1, sampleSites.Count);
            Assert.AreEqual(sampleSiteA.Id, sampleSites[0].Id);
        }

        [TestMethod]
        public void TestBySampleMatrixIdReturnsBySampleMatrixId()
        {
            /* has matrices with a matching id */
            var sampleSiteA = GetEntityFactory<SampleSite>().Create();

            /* these matrix sample sites will be reassigned... doing this since i don't want a bunch of sample sites created */
            var matrices = GetFactory<SampleIdMatrixFactory>().CreateList(4, new {
                SampleSite = sampleSiteA
            });
            
            matrices[0].SampleSite = sampleSiteA;
            matrices[1].SampleSite = sampleSiteA;

            sampleSiteA.SampleIdMatrices = new List<SampleIdMatrix>() {
                matrices[0],
                matrices[1]
            };

            /* has matrices without a matching id */
            var sampleSiteB = GetEntityFactory<SampleSite>().Create();
            matrices[2].SampleSite = sampleSiteB;
            matrices[3].SampleSite = sampleSiteB;

            sampleSiteB.SampleIdMatrices = new List<SampleIdMatrix>() {
                matrices[2],
                matrices[3]
            };

            /* does not have matrices */
            GetEntityFactory<SampleSite>().Create();

            var actionResult = (CascadingActionResult)_target.BySampleMatrixId(matrices[1].Id);
            var sampleSites = ((IEnumerable<SampleSiteDisplayItem>)actionResult.Data).ToList();
            Assert.AreEqual(1, sampleSites.Count);
            Assert.AreEqual(sampleSiteA.Id, sampleSites[0].Id);
        }

        [TestMethod]
        public void TestGetPrimaryEligibleSitesByStateIdReturnsPrimaryEligibleSitesByStateId()
        {
            var newJerseyState = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var tennesseeState = GetEntityFactory<State>().Create(new { Abbreviation = "TN" });

            /* state matches and is eligible as primary */
            var primaryEligibleSampleSiteA = GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<DownstreamSampleSiteLocationTypeFactory>().Create(),
                State = newJerseyState
            });

            /* state matches and is not eligible as primary */
            GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<PrimarySampleSiteLocationTypeFactory>().Create(),
                State = newJerseyState
            });

            /* state does not matches and is eligible as primary */
            GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<GroundwaterSampleSiteLocationTypeFactory>().Create(),
                State = tennesseeState
            });

            /* state does not match and is not eligible as primary */
            GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<PrimarySampleSiteLocationTypeFactory>().Create(),
                State = tennesseeState
            });

            var actionResult = (CascadingActionResult)_target.GetPrimaryEligibleSampleSitesByStateId(newJerseyState.Id);
            var sampleSites = ((IEnumerable<SampleSiteDisplayItem>)actionResult.Data).ToList();
            Assert.AreEqual(1, sampleSites.Count);
            Assert.AreEqual(primaryEligibleSampleSiteA.Id, sampleSites[0].Id);
        }

        [TestMethod]
        public void TestGetPrimarySitesByStateIdReturnsPrimarySitesByStateId()
        {
            var newJerseyState = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var tennesseeState = GetEntityFactory<State>().Create(new { Abbreviation = "TN" });

            /* state matches and is primary */
            var primarySampleSite = GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<PrimarySampleSiteLocationTypeFactory>().Create(),
                State = newJerseyState
            });

            /* state matches and is not primary */
            GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<DownstreamSampleSiteLocationTypeFactory>().Create(),
                State = newJerseyState
            });

            /* state does not matches and is primary */
            GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<PrimarySampleSiteLocationTypeFactory>().Create(),
                State = tennesseeState
            });

            /* state does not match and is not primary */
            GetEntityFactory<SampleSite>().Create(new {
                LocationType = GetFactory<GroundwaterSampleSiteLocationTypeFactory>().Create(),
                State = tennesseeState
            });

            var actionResult = (CascadingActionResult)_target.GetPrimarySampleSitesByStateId(newJerseyState.Id);
            var sampleSites = ((IEnumerable<SampleSiteDisplayItem>)actionResult.Data).ToList();
            Assert.AreEqual(1, sampleSites.Count);
            Assert.AreEqual(primarySampleSite.Id, sampleSites[0].Id);
        }

        [TestMethod]
        public void TestGetSampleSitesByPremiseNumberReturnsSampleSitesAssociatedWithGivenPremiseNumber()
        {
            var premiseA = GetEntityFactory<Premise>().Create(new { PremiseNumber = "a" });
            var premiseB = GetEntityFactory<Premise>().Create(new { PremiseNumber = "b" });

            var sampleSiteAForPremiseA = GetEntityFactory<SampleSite>().Create(new {
                Premise = premiseA
            });

            var sampleSiteBForPremiseA = GetEntityFactory<SampleSite>().Create(new {
                Premise = premiseA
            });

            GetEntityFactory<SampleSite>().Create(new {
                Premise = premiseB
            });

            GetEntityFactory<SampleSite>().Create();

            var actionResult = (JsonResult)_target.GetSampleSitesByPremiseNumber("a");
            var jsonResultTester = new JsonResultTester(actionResult);

            Assert.AreEqual(2, jsonResultTester.Count);
            jsonResultTester.AreEqual(sampleSiteAForPremiseA.Id, "Id");
            jsonResultTester.AreEqual(sampleSiteBForPremiseA.Id, "Id", 1);
        }

        #endregion
    }
}
