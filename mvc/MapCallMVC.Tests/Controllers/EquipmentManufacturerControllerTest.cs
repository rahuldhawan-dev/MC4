using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EquipmentManufacturerControllerTest : MapCallMvcControllerTestBase<EquipmentManufacturerController, EquipmentManufacturer>
    {
        #region Private Members

        private Mock<ISAPManufacturerRepository> _equipmentManufacturerRepository;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _equipmentManufacturerRepository = new Mock<ISAPManufacturerRepository>();
            _container.Inject(_equipmentManufacturerRepository.Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Search", controller = "EquipmentManufacturer" };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Search", controller = "EquipmentManufacturer" };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = EquipmentManufacturerController.ROLE;

            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/EquipmentManufacturer/ByEquipmentTypeId");

                a.RequiresRole("~/EquipmentManufacturer/Search", role);
                a.RequiresRole("~/EquipmentManufacturer/Index", role);
                a.RequiresRole("~/EquipmentManufacturer/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/EquipmentManufacturer/Update", role, RoleActions.Edit);
                a.RequiresRole("~/EquipmentManufacturer/New", role, RoleActions.Add);
                a.RequiresRole("~/EquipmentManufacturer/Create", role, RoleActions.Add);
            });
        }

        [TestMethod]
        public void TestByEquipmentTypeIdReturnsMatchingManufacturers()
        {
            var equipmentType1 = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentType2 = GetFactory<EquipmentTypeEngineFactory>().Create();
            var equipmentType3 = GetFactory<EquipmentTypeAeratorFactory>().Create();
            var equipmentManufacturer1 = GetFactory<EquipmentManufacturerFactory>().Create(new { EquipmentType = equipmentType1, Description = "Type 1" });
            var equipmentManufacturer2 = GetFactory<EquipmentManufacturerFactory>().Create(new { EquipmentType = equipmentType2, Description = "Type 2" });
            var equipmentManufacturer3 = GetFactory<EquipmentManufacturerFactory>().Create(new { EquipmentType = equipmentType3, Description = "Type 3" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            var manufacturers1 = new SAPManufacturerCollection();
            manufacturers1.Items.Add(new SAPManufacturer { Manufacturer = equipmentManufacturer1.Description, Class = equipmentManufacturer1.EquipmentType.Abbreviation });
            var manufacturers2 = new SAPManufacturerCollection();
            manufacturers2.Items.Add(new SAPManufacturer { Manufacturer = equipmentManufacturer2.Description, Class = equipmentManufacturer2.EquipmentType.Abbreviation });
            var manufacturers3 = new SAPManufacturerCollection();
            manufacturers3.Items.Add(new SAPManufacturer { Manufacturer = equipmentManufacturer3.Description, Class = equipmentManufacturer3.EquipmentType.Abbreviation });
            _equipmentManufacturerRepository.Setup(x => x.Search(It.Is<SAPManufacturer>(y => y.Class == equipmentType1.Abbreviation))).Returns(manufacturers1);
            _equipmentManufacturerRepository.Setup(x => x.Search(It.Is<SAPManufacturer>(y => y.Class == equipmentType2.Abbreviation))).Returns(manufacturers2);
            _equipmentManufacturerRepository.Setup(x => x.Search(It.Is<SAPManufacturer>(y => y.Class == equipmentType3.Abbreviation))).Returns(manufacturers3);

            var result = (CascadingActionResult)_target.ByEquipmentTypeId(new[] { equipmentType1.Id, equipmentType2.Id });
            var actual = ((IEnumerable<EquipmentManufacturer>)result.Data).ToList();

            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.Contains(equipmentManufacturer1));
            Assert.IsTrue(actual.Contains(equipmentManufacturer2));
            Assert.IsFalse(actual.Contains(equipmentManufacturer3));
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<EquipmentManufacturer>().Create(new { Description = "description 0" });
            var entity1 = GetEntityFactory<EquipmentManufacturer>().Create(new { Description = "description 1" });
            var search = new SearchEquipmentManufacturer();
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

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EquipmentManufacturer>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEquipmentManufacturer, EquipmentManufacturer>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<EquipmentManufacturer>(eq.Id).Description);
        }

        #endregion
    }
}
