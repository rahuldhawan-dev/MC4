using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class GeneralLiabilityClaimControllerTest : MapCallMvcControllerTestBase<GeneralLiabilityClaimController, GeneralLiabilityClaim>
    {
        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #region Fields

        private OperatingCenter _opCenter;
        private Mock<INotificationService> _notifier;

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
        }

        protected override User CreateUser()
        {
            _opCenter = GetFactory<OperatingCenterFactory>().Create();
            var user = GetEntityFactory<User>().Create(new {DefaultOperatingCenter = _opCenter, FullName = "Full Name"});

            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations}),
                Module = GetFactory<ModuleFactory>().Create(new {Id = ROLE}),
                Action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read}),
                OperatingCenter = _opCenter,
                User = user
            });

            Session.Save(user);

            return user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _opCenter = GetFactory<OperatingCenterFactory>().Create();

            // Needed for Create tests
            ((FakeViewEngine)Application.ViewEngine).Views["Pdf"] = new Mock<IView>().Object;

            _target = Request.CreateAndInitializeController<GeneralLiabilityClaimController>();
        }
        
        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.OperationsHealthAndSafety;
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/Index/", module);
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/Search/", module);
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/Show/", module);
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/New/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/Create/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/GeneralLiabilityClaim/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetEntityFactory<GeneralLiabilityClaim>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/GeneralLiabilityClaim/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var good = GetEntityFactory<GeneralLiabilityClaim>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/GeneralLiabilityClaim/Show" + good.Id + ".frag");

            var result = (PartialViewResult)_target.Show(good.Id);
            
            MvcAssert.IsViewNamed(result, "_ShowPopup");
            Assert.AreSame(good, result.Model);
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var good = GetEntityFactory<GeneralLiabilityClaim>().Create();
            var bad = GetEntityFactory<GeneralLiabilityClaim>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/GeneralLiabilityClaim/Show" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();

            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
            var search = new SearchGeneralLiabilityClaim();
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

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
            var search = new SearchGeneralLiabilityClaim {
                IncidentDateTime = new DateRange {
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    Operator = RangeOperator.Between
                }
            };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result.Data);

            helper.AreEqual(1, "Id");
            helper.AreEqual(2, "Id", 1);
            helper.AreEqual("description 0", "Description");
            helper.AreEqual("description 1", "Description", 1);
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsNotSent()
        {
            var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
            var search = new SearchGeneralLiabilityClaim();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsLongerThanOneMonth()
        {
            var now = DateTime.Now;
            var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
            var search = new SearchGeneralLiabilityClaim {
                IncidentDateTime = new DateRange {
                    Start = now.AddDays(-31),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeOperatorIsNotBetween()
        {
            foreach (var op in new[] {
                RangeOperator.Equal, RangeOperator.GreaterThan, RangeOperator.GreaterThanOrEqualTo,
                RangeOperator.LessThan, RangeOperator.LessThanOrEqualTo
            })
            {
                var now = DateTime.Now;
                var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
                var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
                var search = new SearchGeneralLiabilityClaim {
                    IncidentDateTime = new DateRange {
                        Start = now.AddDays(-1),
                        End = now,
                        Operator = op
                    }
                };
                _target.ControllerContext = new ControllerContext();
                _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                    ResponseFormatter.KnownExtensions.JSON;

                MyAssert.Throws<InvalidOperationException>(() =>
                    _target.Index(search));
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/GeneralLiabilityClaim/Index.map");
            var good = GetEntityFactory<GeneralLiabilityClaim>().Create();
            var bad = GetEntityFactory<GeneralLiabilityClaim>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory)});
            int[] opcInts = {good.OperatingCenter.Id};
            var model = new SearchGeneralLiabilityClaim {OperatingCenter = opcInts};
            
            var result = (MapResult)_target.Index(model);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();

            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotificationEmail()
        {
            var expectedBytes = new byte[] { 1, 2, 3 };
            var pdfRenderer = new Mock<IHtmlToPdfConverter>();
            _container.Inject(pdfRenderer.Object);

            pdfRenderer.Setup(x => x.RenderHtmlToPdfBytes(It.IsAny<string>())).Returns(expectedBytes);

            var ent = GetFactory<GeneralLiabilityClaimFactory>().Create();

            var model = _viewModelFactory.Build<CreateGeneralLiabilityClaim, GeneralLiabilityClaim>(ent);
            model.Id = 0;

            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(GeneralLiabilityClaimController.ROLE, resultArgs.Module);
            Assert.AreEqual(GeneralLiabilityClaimController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersForUserRoleInLookupData()
        {
            var application = GetEntityFactory<MapCall.Common.Model.Entities.Application>().Create(new { Id = RoleApplications.Operations });
            var module = GetEntityFactory<Module>().Create(new { Id = RoleModules.OperationsHealthAndSafety, Application = application });
            var addAction = GetEntityFactory<RoleAction>().Create(new { Id = RoleActions.Add });

            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });
            GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = addAction,
                User = _currentUser,
                OperatingCenter = activeOpc
            });
            GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = addAction,
                User = _currentUser,
                OperatingCenter = inactiveOpc
            });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<GeneralLiabilityClaim>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditGeneralLiabilityClaim, GeneralLiabilityClaim>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<GeneralLiabilityClaim>(eq.Id).Description);
        }

        #endregion
    }
}
