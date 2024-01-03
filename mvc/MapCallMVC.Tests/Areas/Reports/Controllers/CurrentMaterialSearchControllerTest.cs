using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MostRecentlyInstalledServices;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class CurrentMaterialSearchControllerTest
        : MapCallMvcControllerTestBase<CurrentMaterialSearchController, MostRecentlyInstalledService>
    {
        #region Private Members

        private Town _town;
        private Street _street;
        private OperatingCenter _operatingCenter;
        private Premise _premise;
        private ServiceMaterial _material;
        private ServiceMaterial _customerMaterial;
        private ServiceSize _size;
        private ServiceSize _customerSize;
        private ServiceCategory _category;
        private ServiceType _type;
        private Service _service;
        private ServiceInstallationPurpose _purpose;

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IImageToPdfConverter>().Mock();
        }

        protected override IEntity CreateEntityForAutomatedTests(bool saveEntity = true)
        {
            _town = GetEntityFactory<Town>().Create();
            _street = GetEntityFactory<Street>().Create();
            _operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            _premise = GetEntityFactory<Premise>().Create(new {
                Installation = "12345"
            });
            _material = GetEntityFactory<ServiceMaterial>().Create();
            _customerMaterial = GetEntityFactory<ServiceMaterial>().Create();
            _size = GetEntityFactory<ServiceSize>().Create();
            _customerSize = GetEntityFactory<ServiceSize>().Create();
            _category = GetEntityFactory<ServiceCategory>().Create();
            _purpose = GetEntityFactory<ServiceInstallationPurpose>().Create();
            _type = GetEntityFactory<ServiceType>().Create(new {
                OperatingCenter = _operatingCenter,
                ServiceCategory = _category,
                Description = "ServiceType"
            });
            _service = GetEntityFactory<Service>().Create(new {
                Premise = _premise,
                ServiceCategory = _category,
                ServiceInstallationPurpose = _purpose,
                ServiceType = _type,
                ServiceMaterial = _material,
                ServiceSize = _size,
                CustomerSideMaterial = _customerMaterial,
                CustomerSideSize = _customerSize,
                OperatingCenter = _operatingCenter,
                Town = _town,
                Street = _street,
                StreetNumber = "1234",
                DateInstalled = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            return new MostRecentlyInstalledService {
                Premise = _premise,
                Service = _service
            };
        }

        #endregion
        
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.FieldServicesAssets;
                const string path = "~/Reports/CurrentMaterialSearch/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Index", role);
                a.RequiresRole(path + "Show", role);
            });
        }
        
        #endregion
        
        #region Index

        [TestMethod]
        public void Test_IndexXLS_ExportsExcel()
        {
            CreateEntityForAutomatedTests();
            
            var search = new SearchCurrentMaterial();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(_service.Id, nameof(CurrentMaterialReportItem.ServiceId));
                helper.AreEqual(_premise.Id, nameof(CurrentMaterialReportItem.PremiseId));
                helper.AreEqual(
                    _operatingCenter.Description,
                    nameof(CurrentMaterialReportItem.OperatingCenter));
                helper.AreEqual(_town.FullName, nameof(CurrentMaterialReportItem.Town));
                helper.AreEqual(_premise.PremiseNumber, nameof(CurrentMaterialReportItem.PremiseNumber));
                helper.AreEqual(
                    _premise.Installation,
                    nameof(CurrentMaterialReportItem.InstallationNumber));
                helper.AreEqual(_material.Description, nameof(CurrentMaterialReportItem.ServiceMaterial));
                helper.AreEqual(
                    _customerMaterial.Description,
                    nameof(CurrentMaterialReportItem.CustomerSideMaterial));
                helper.AreEqual(_size.Description, nameof(CurrentMaterialReportItem.ServiceSize));
                helper.AreEqual(
                    _customerSize.Description,
                    nameof(CurrentMaterialReportItem.CustomerSideSize));
                helper.AreEqual(_service.DateInstalled, nameof(CurrentMaterialReportItem.InstallDate));
                helper.AreEqual(_service.UpdatedAt, nameof(CurrentMaterialReportItem.UpdatedAt));
                helper.AreEqual(_type.Description, nameof(CurrentMaterialReportItem.ServiceType));
                helper.AreEqual(_category.Description, nameof(CurrentMaterialReportItem.ServiceCategory));
                helper.AreEqual(
                    _purpose.Description,
                    nameof(CurrentMaterialReportItem.PurposeOfInstallation));
                helper.AreEqual(_service.StreetNumber, nameof(CurrentMaterialReportItem.StreetNumber));
                helper.AreEqual(_street.FullStName, nameof(CurrentMaterialReportItem.Street));
            }
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            CreateEntityForAutomatedTests();
            
            var search = new SearchCurrentMaterial();
            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchCurrentMaterial)result.Model).Results.ToList();
            
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(_service.Id, resultModel[0].ServiceId);
            Assert.AreEqual(_premise.Id, resultModel[0].PremiseId);
            Assert.AreEqual(_operatingCenter.Description, resultModel[0].OperatingCenter);
            Assert.AreEqual(_town.FullName, resultModel[0].Town);
            Assert.AreEqual(_premise.PremiseNumber, resultModel[0].PremiseNumber);
            Assert.AreEqual(_premise.Installation, resultModel[0].InstallationNumber);
            Assert.AreEqual(_material.Description, resultModel[0].ServiceMaterial);
            Assert.AreEqual(_customerMaterial.Description, resultModel[0].CustomerSideMaterial);
            Assert.AreEqual(_size.Description, resultModel[0].ServiceSize);
            Assert.AreEqual(_customerSize.Description, resultModel[0].CustomerSideSize);
            Assert.AreEqual(_service.DateInstalled, resultModel[0].InstallDate);
            Assert.AreEqual(_service.UpdatedAt, resultModel[0].UpdatedAt);
            Assert.AreEqual(_type.Description, resultModel[0].ServiceType);
            Assert.AreEqual(_category.Description, resultModel[0].ServiceCategory);
            Assert.AreEqual(_purpose.Description, resultModel[0].PurposeOfInstallation);
            Assert.AreEqual(_service.StreetNumber, resultModel[0].StreetNumber);
            Assert.AreEqual(_street.FullStName, resultModel[0].Street);
        }
        
        #endregion
        
        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            Assert.Inconclusive("Show html is not supported by this controller");
        }

        [TestMethod]
        public void Test_ShowFrag_ReturnsShowPopupView_WhenRecordIsFound()
        {
            var entity = (MostRecentlyInstalledService)CreateEntityForAutomatedTests();
            entity = Repository.Find(entity.Premise.Id);

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;

            var result = (ActionResult)_target.Show(entity.Id);

            MvcAssert.IsViewWithNameAndModel(result, "_ShowPopup", entity);
            MvcAssert.IsPartialView(result);
        }

        [TestMethod]
        public void Test_ShowFrag_ReturnsNotFound_WhenRecordIsNotFound()
        {
            var result = (ActionResult)_target.Show(655321);
            
            MvcAssert.IsNotFound(result);
        }

        #endregion
    }
}
