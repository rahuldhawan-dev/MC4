using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ServiceDSICControllerTest : MapCallMvcControllerTestBase<ServiceDSICController, Service, ServiceRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Mock();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Reports/ServiceDSIC/Search", role);
                a.RequiresRole("~/Reports/ServiceDSIC/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var town = GetEntityFactory<Town>().Create();
            var service = GetFactory<ServiceFactory>().Create(new { Town = town, TaskNumber1 = "R18-18B1.17-P-0019", DateInstalled = DateTime.Now });
            var search = new SearchServiceDSIC();

            var result = _target.Index(search) as ViewResult;

            MvcAssert.IsViewNamed(result, "Index");
        }
    }
}