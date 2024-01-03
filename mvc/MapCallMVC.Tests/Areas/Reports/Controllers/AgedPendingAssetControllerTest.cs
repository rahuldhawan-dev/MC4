using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.ChangeTracking;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using NHibernate.Type;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class AgedPendingAssetControllerTest : MapCallMvcControllerTestBase<AgedPendingAssetController, Hydrant, HydrantRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new { IsAdmin = true });
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = AgedPendingAssetController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/AgedPendingAsset/Index", role);
                a.RequiresRole("~/Reports/AgedPendingAsset/Search", role);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var now = _now;
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pending = GetFactory<PendingAssetStatusFactory>().Create();
            var active = GetFactory<ActiveAssetStatusFactory>().Create();
            var pendingValve = GetFactory<PendingAssetStatusFactory>().Create();
            var activeValve = GetFactory<ActiveAssetStatusFactory>().Create();

            var args = new SearchAgedPendingAsset { OperatingCenter = opc1.Id };
            #region Good
            
            _dateTimeProvider.SetNow(now.AddDays(-1));
            var hyd1 = GetEntityFactory<Hydrant>().CreateList(2, new { OperatingCenter = opc1, Status = pending });
            _dateTimeProvider.SetNow(now.AddDays(-90));
            var hyd90 = GetEntityFactory<Hydrant>().CreateList(2, new { OperatingCenter = opc1, Status = pending });

            _dateTimeProvider.SetNow(now.AddDays(-91));
            var hyd91 = GetEntityFactory<Hydrant>().CreateList(3, new { OperatingCenter = opc1, Status = pending });
            _dateTimeProvider.SetNow(now.AddDays(-180));
            var hyd180 = GetEntityFactory<Hydrant>().CreateList(3, new { OperatingCenter = opc1, Status = pending });

            _dateTimeProvider.SetNow(now.AddDays(-181));
            var hyd181 = GetEntityFactory<Hydrant>().CreateList(4, new { OperatingCenter = opc1, Status = pending });
            _dateTimeProvider.SetNow(now.AddDays(-360));
            var hyd360 = GetEntityFactory<Hydrant>().CreateList(4, new { OperatingCenter = opc1, Status = pending });

            _dateTimeProvider.SetNow(now.AddDays(-361));
            var hyd361 = GetEntityFactory<Hydrant>().CreateList(5, new { OperatingCenter = opc1, Status = pending });
            
            #endregion
            _dateTimeProvider.SetNow(now);
            #region Bad
            
            var hydBadStatus = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opc2, Status = pending });
            var hydBadOpCntr = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opc1, Status = active });
            
            #endregion

            var search = new SearchAgedPendingAsset();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = (IEnumerable<AgedPendingAssetReportItem>)result.Model;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count());
            var first = resultModel.First();
            Assert.AreEqual(4, first.ZeroToNinety);
            Assert.AreEqual(6, first.NinetyOneToOneEighty);
            Assert.AreEqual(8, first.OneEightyToThreeSixty);
            Assert.AreEqual(5, first.ThreeSixtyPlus);
            Assert.AreEqual(23, first.Total);
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // noop override: the above test covers this. the repo method used
            // for this blows up when dealing with the automatic test because
            // the automatic test shouldn't inject an IDateTimeProvider value
            // in order to fix it.
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            // noop override: the repo method used
            // for this blows up when dealing with the automatic test because
            // the automatic test shouldn't inject an IDateTimeProvider value
            // in order to fix it. Also, the controller doesn't redirect to Search.
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var now = _now;
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pending = GetFactory<PendingAssetStatusFactory>().Create();
            var active = GetFactory<ActiveAssetStatusFactory>().Create();
            var pendingValve = GetFactory<PendingAssetStatusFactory>().Create();
            var activeValve = GetFactory<ActiveAssetStatusFactory>().Create();

            var args = new SearchAgedPendingAsset { OperatingCenter = opc1.Id };
            #region Good
            
            _dateTimeProvider.SetNow(now.AddDays(-1));
            var hyd1 = GetEntityFactory<Hydrant>().CreateList(2, new { OperatingCenter = opc1, Status = pending });
            _dateTimeProvider.SetNow(now.AddDays(-90));
            var hyd90 = GetEntityFactory<Hydrant>().CreateList(2, new { OperatingCenter = opc1, Status = pending });

            _dateTimeProvider.SetNow(now.AddDays(-91));
            var hyd91 = GetEntityFactory<Hydrant>().CreateList(3, new { OperatingCenter = opc1, Status = pending });
            _dateTimeProvider.SetNow(now.AddDays(-180));
            var hyd180 = GetEntityFactory<Hydrant>().CreateList(3, new { OperatingCenter = opc1, Status = pending });

            _dateTimeProvider.SetNow(now.AddDays(-181));
            var hyd181 = GetEntityFactory<Hydrant>().CreateList(4, new { OperatingCenter = opc1, Status = pending });
            _dateTimeProvider.SetNow(now.AddDays(-360));
            var hyd360 = GetEntityFactory<Hydrant>().CreateList(4, new { OperatingCenter = opc1, Status = pending });

            _dateTimeProvider.SetNow(now.AddDays(-361));
            var hyd361 = GetEntityFactory<Hydrant>().CreateList(5, new { OperatingCenter = opc1, Status = pending });
            
            #endregion
            _dateTimeProvider.SetNow(now);
            #region Bad
            
            var hydBadStatus = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opc2, Status = pending });
            var hydBadOpCntr = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opc1, Status = active });
            
            #endregion

            var search = new SearchAgedPendingAsset();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(4, "0 - 90 Days");
                helper.AreEqual(6, "91 - 180 Days");
            }
        }

        #endregion

        public class MapCallMvcInMemoryDatabaseTestInterceptorWithLessChangeTracking
            : MapCallMVCInMemoryDatabaseTestInterceptorWithChangeTracking
        {
            public MapCallMvcInMemoryDatabaseTestInterceptorWithLessChangeTracking(
                InMemoryDatabaseTestInterceptor testInterceptor,
                ChangeTrackingInterceptor<User> changeTrackingInterceptor)
                : base(testInterceptor,
                    changeTrackingInterceptor) { }

            public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
            {
                if (entity is IEntityWithCreationTimeTracking timeTracking &&
                    timeTracking.CreatedAt > default(DateTime))
                {
                    
                }
                
                return base.OnSave(entity, id, state, propertyNames, types);
            }
        }
    }
}
