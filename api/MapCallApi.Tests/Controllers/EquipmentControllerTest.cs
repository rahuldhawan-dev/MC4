using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Newtonsoft.Json;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class EquipmentControllerTest : MapCallApiControllerTestBase<EquipmentController, Equipment, IRepository<Equipment>>
    {
        #region Init/Cleanup
        
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ProductionFacilities;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/Equipment/Show/1", module);
                a.RequiresRole("~/Equipment/Show/string", module);
                a.RequiresRole("~/Equipment/Index", module);
            });
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // noop override: returns json. tested below
        }

        [TestMethod]
        public void TestShowWithIntegerReturnsProperJSON()
        {
            var equipmentModel = GetEntityFactory<EquipmentModel>().Create();
            var equipmentSubCategory = GetEntityFactory<EquipmentSubCategory>().Create(new {
                Description = "Pump"
            });
            var equipmentLifespan = GetEntityFactory<EquipmentLifespan>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                Description = "Raw Water Pump",
                EquipmentSubCategory = equipmentSubCategory,
                EquipmentLifespan = equipmentLifespan
            });
            var equipmentType = GetEntityFactory<EquipmentType>().Create();
            var expected = GetEntityFactory<Equipment>().Create(new {
                EquipmentModel = equipmentModel,
                SerialNumber = "321817",                
                EquipmentPurpose = equipmentPurpose,
                EquipmentType = equipmentType,
                UpdatedAt = new DateTime(2020, 7, 9, 1, 2, 3)
            });
            AddLinks(expected);

            var result = (ContentResult)_target.Show(expected.Id);
            var actual = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Content);

            ValidateEquipmentJson(expected, actual, _now);
        }

        [TestMethod]
        public void TestShowWithStringReturnsProperJSON()
        {
            var equipmentModel = GetEntityFactory<EquipmentModel>().Create();
            var equipmentSubCategory = GetEntityFactory<EquipmentSubCategory>().Create(new {
                Description = "Pump"
            });
            var equipmentLifespan = GetEntityFactory<EquipmentLifespan>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                Description = "Raw Water Pump",
                EquipmentSubCategory = equipmentSubCategory,
                EquipmentLifespan = equipmentLifespan
            });
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var facilityArea = GetEntityFactory<FacilityArea>().Create(new { Description = "test" });
            var facilitySubArea = GetEntityFactory<FacilitySubArea>().Create(new
            {
                Description = "SubareaDescription",
                Area = facilityArea
            });
            var ffa = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility,
                FacilityArea = facilityArea,
                FacilitySubArea = facilitySubArea,
                Id = 1
            });
            var equipmentType = GetEntityFactory<EquipmentType>().Create();
            var expected = GetEntityFactory<Equipment>().Create(new {
                EquipmentModel = equipmentModel,
                EquipmentPurpose = equipmentPurpose,
                EquipmentType = equipmentType,
                UpdatedAt = new DateTime(2020, 7, 9, 1, 2, 3),
                FunctionalLocation = "NJLK-HO-OAKGL",
                Facility = facility,
                FacilityFacilityArea = ffa
            });

            AddLinks(expected);
            var result = (ContentResult)_target.ShowString(expected.Identifier);
            var actual = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Content);
            ValidateEquipmentJson(expected, actual, _now);
        }

        private void AddLinks(Equipment equipment)
        {
            var linkType = GetEntityFactory<LinkType>().Create();
            equipment.Links = new HashSet<EquipmentLink> {
                new EquipmentLink { Equipment = equipment, LinkType = linkType, Url="http://a.com/" },
                new EquipmentLink { Equipment = equipment, LinkType = linkType, Url="http://b.com/" }
            };
            Session.Save(equipment);
            Session.Flush();
            Session.Evict(equipment);
        }

        /// <summary>
        /// The Facility Controller also returns these exact fields for its Equipment
        /// Made static so that the FacilityControllerTest can run its equipment through
        /// the same test. Changes made to equipment returned should be made in both places.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void ValidateEquipmentJson(Equipment expected, Dictionary<string, object> actual, DateTime now)
        {
            Assert.AreEqual(expected.Id.ToString(), actual["Id"].ToString());
            Assert.AreEqual(expected.Description, actual["Description"]);
            Assert.AreEqual(expected.DateInstalled.ToString(), actual["DateInstalled"]);
            Assert.AreEqual(expected.EquipmentManufacturer.ToString(), actual["Manufacturer"]);
            Assert.AreEqual(expected.EquipmentModel.ToString(), actual["EquipmentModel"]);
            Assert.AreEqual(expected.SerialNumber, actual["SerialNumber"]);
            Assert.AreEqual(expected.EquipmentStatus.ToString(), actual["Status"]);
            Assert.AreEqual(expected.EquipmentPurpose.ToString(), actual["EquipmentPurpose"]);
            Assert.AreEqual(expected.FacilityFacilityArea?.FacilityArea?.ToString(), actual["FacilityArea"]);
            Assert.AreEqual(expected.FacilityFacilityArea?.FacilitySubArea?.ToString(), actual["FacilitySubArea"]);
            Assert.AreEqual(expected.EquipmentPurpose.EquipmentSubCategory.ToString(), actual["EquipmentSubCategory"]);
            Assert.AreEqual(expected.EquipmentPurpose.EquipmentLifespan.ToString(), actual["EquipmentLifespan"]);
            Assert.AreEqual(expected.EquipmentType.Description, actual["EquipmentTypeDescription"].ToString());
            var links = (Newtonsoft.Json.Linq.JContainer)actual["Links"];
            Assert.AreEqual(expected.Links.First().Url, links.First().Value<string>("Url"));
            Assert.AreEqual(expected.Links.First().LinkType.Description, links.First().Value<string>("LinkType"));
            Assert.AreEqual(expected.Links.Last().Url, links.Last().Value<string>("Url"));
            Assert.AreEqual(expected.Links.Last().LinkType.Description, links.Last().Value<string>("LinkType"));
            Assert.AreEqual(now, actual["LastUpdated"]);
            Assert.AreEqual(expected.FunctionalLocation, actual["FunctionalLocation"]);
        }

        [TestMethod]
        public void TestShowWithStringReturnsNotFoundForInvalidString()
        {
            var result = (HttpNotFoundResult)_target.ShowString("Invalid-Identifier");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public override void TestIndexReturnsResults() { }

        [TestMethod]
        public void Test_Index_ReturnsProperJson()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var equipmentType = GetEntityFactory<EquipmentType>().Create();
            var scadaTagName = GetEntityFactory<ScadaTagName>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter });
            var equipment = GetEntityFactory<Equipment>().Create(new {OperatingCenter = operatingCenter, Facility = facility, EquipmentType = equipmentType, ScadaTagName = scadaTagName});

            Session.SaveOrUpdate(equipment);
            Session.Flush();

            var search = new SearchEquipment { Facility = facility.Id };
            var result = (JsonResult)_target.Index(search);
            var helper = new JsonResultTester(result);

            helper.AreEqual(equipment.Description, nameof(equipment.Description));
            helper.AreEqual(equipment.EquipmentPurpose.Description, "EquipmentPurpose");
            helper.AreEqual(equipment.Facility.FacilityName, nameof(equipment.Facility));
            helper.AreEqual(equipment.FunctionalLocation, nameof(equipment.FunctionalLocation));
            helper.AreEqual(equipment.HasRegulatoryRequirement, nameof(equipment.HasRegulatoryRequirement));
            helper.AreEqual(equipment.HasSensorAttached, nameof(equipment.HasSensorAttached));
            helper.AreEqual(equipment.Id, nameof(equipment.Id));
            helper.AreEqual(equipment.IsGenerator, nameof(equipment.IsGenerator));
            helper.AreEqual(equipment.Identifier, "MapCallEquipmentId");
            helper.AreEqual(equipment.Number, nameof(equipment.Number));
            helper.AreEqual(equipment.OperatingCenter.Description, nameof(equipment.OperatingCenter));
            helper.AreEqual(equipment.SAPEquipmentId, nameof(equipment.SAPEquipmentId));
            helper.AreEqual(equipment.EquipmentType.Description, nameof(equipment.EquipmentType));
            helper.AreEqual(equipment.ScadaTagName.TagName, nameof(equipment.ScadaTagName));
        }
    }
}
