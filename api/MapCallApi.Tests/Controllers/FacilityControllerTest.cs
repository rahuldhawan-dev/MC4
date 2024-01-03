using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
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
    public class FacilityControllerTest : MapCallApiControllerTestBase<FacilityController, Facility, IRepository<Facility>>
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
                a.RequiresRole("~/Facility/Show/1", module);
                a.RequiresRole("~/Facility/Show/string", module);
                a.RequiresRole("~/Facility/Index", module);
            });
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // noop override: returns json. tested below
        }

        [TestMethod]
        public void TestFacilityShowDoesNotBreakBecausePublicWaterSupplyIsNull()
        {
            // Facility/Show does a null check everytime pwsid is referenced
            // except in one place. If it's nullable, make it null check. If it's
            // not nullable, fix the factory.
            Assert.Inconclusive("FIX ME");
        }

        [TestMethod]
        public void TestShowWithIntegerReturnsProperJSON()
        {
            var town = GetEntityFactory<Town>().Create();
            var prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "N" });
            var suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St." });
            var street = GetEntityFactory<Street>().Create(new {
                Town = town,
                Prefix = prefix,
                Name = "Main",
                Suffix = suffix
            });
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var customerData = GetEntityFactory<PublicWaterSupplyCustomerData>().Create(new {
                PWSID = publicWaterSupply,
                PopulationServed = 200,
                NumberCustomers = 100
            });
            var processStage = GetEntityFactory<ProcessStage>().Create(new {Description = "Water Treatment"});
            Session.Evict(publicWaterSupply);
            publicWaterSupply = Session.Load<PublicWaterSupply>(publicWaterSupply.Id);
            var expected = GetEntityFactory<Facility>().Create(new {
                PublicWaterSupply = publicWaterSupply,
                FacilityTotalCapacityMGD = 8.17m,
                FacilityContactInfo = "facility contact info",
                StreetNumber = "123 A",
                Town = town,
                Street = street,
                ZipCode = "07711-0001",
                Process = processStage,
                UpdatedAt = new DateTime(2020, 7, 1, 1, 2, 3)
            });
            AddEquipment(expected);

            var result = (ContentResult)_target.ShowString(expected.FacilityId);
            var actual = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Content);

            ValidateFacilityJson(expected, actual, _now);
        }

        private void AddEquipment(Facility expected)
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
            var equipment = GetEntityFactory<Equipment>().CreateList(2, new {
                Facility = expected,
                Identifier = "NJEW-1-PBKG-53",
                EquipmentModel = equipmentModel,
                EquipmentPurpose = equipmentPurpose,
                EquipmentType = equipmentType,
                UpdatedAt = new DateTime(2020, 7, 9, 1, 2, 3),
                FunctionalLocation = "Somewhere"
            });
            var linkType = GetEntityFactory<LinkType>().Create();
            var equipment1 = equipment.First();
            var equipment2 = equipment.Last();
            equipment1.Links = new HashSet<EquipmentLink> {
                new EquipmentLink { Equipment = equipment1, LinkType = linkType, Url = "http://a.com/" },
                new EquipmentLink { Equipment = equipment1, LinkType = linkType, Url = "http://b.com/" }
            };
            equipment2.Links = new HashSet<EquipmentLink> {
                new EquipmentLink { Equipment = equipment2, LinkType = linkType, Url = "http://c.com/" },
                new EquipmentLink { Equipment = equipment2, LinkType = linkType, Url = "http://d.com/" }
            };
            expected.Equipment.Add(equipment1);
            expected.Equipment.Add(equipment2);
        }

        private static void ValidateFacilityJson(Facility expected, Dictionary<string, object> actual, DateTime now)
        {
            Assert.AreEqual(expected.Id.ToString(), actual["Id"].ToString());
            Assert.AreEqual(expected.FacilityId, actual["FacilityId"]);
            Assert.AreEqual(expected.FacilityName, actual["FacilityName"]);
            Assert.AreEqual(expected.PublicWaterSupply.ToString(), actual["PublicWaterSupply"]);
            Assert.AreEqual(expected.Address, actual["Location"]);
            Assert.AreEqual(expected.FacilityTotalCapacityMGD.ToString(), actual["FacilityTotalCapacityMGD"].ToString());
            Assert.AreEqual(expected.PublicWaterSupply.CustomerData.NumberCustomers.ToString(), actual["NumberCustomers"].ToString());
            Assert.AreEqual(expected.PublicWaterSupply.CustomerData.PopulationServed.ToString(), actual["PopulationServed"].ToString());
            Assert.AreEqual(expected.FacilityContactInfo, actual["FacilityContactInfo"]);
            Assert.AreEqual(expected.Process.Description, actual["Process"]);
            Assert.AreEqual(now, actual["LastUpdated"]);
            var equipment = (Newtonsoft.Json.Linq.JContainer)actual["Equipment"];
            var equipment1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(equipment.First().ToString());
            var equipment2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(equipment.Last().ToString());
            Assert.AreEqual(2, equipment.Count);
            EquipmentControllerTest.ValidateEquipmentJson(expected.Equipment[0], equipment1, now);
            EquipmentControllerTest.ValidateEquipmentJson(expected.Equipment[1], equipment2, now);
        }

        [TestMethod]
        public void TestShowWithStringReturnsProperJSON()
        {
            var town = GetEntityFactory<Town>().Create();
            var prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "N" });
            var suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St." });
            var street = GetEntityFactory<Street>().Create(new {
                Town = town,
                Prefix = prefix,
                Name = "Main",
                Suffix = suffix
            });
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var customerData = GetEntityFactory<PublicWaterSupplyCustomerData>().Create(new {
                PWSID = publicWaterSupply,
                PopulationServed = 200,
                NumberCustomers = 100
            });
            var processStage = GetEntityFactory<ProcessStage>().Create(new { Description = "Water Treatment" });
            Session.Evict(publicWaterSupply);
            publicWaterSupply = Session.Load<PublicWaterSupply>(publicWaterSupply.Id);
            var expected = GetEntityFactory<Facility>().Create(new {
                PublicWaterSupply = publicWaterSupply,
                FacilityTotalCapacityMGD = 8.17m,
                FacilityContactInfo = "facility contact info",
                StreetNumber = "123 A",
                Town = town,
                Street = street,
                ZipCode = "07711-0001",
                Process = processStage,
                UpdatedAt = new DateTime(2020, 7, 1, 1, 2, 3)
            });
            AddEquipment(expected);

            var result = (ContentResult)_target.ShowString(expected.FacilityId);
            var actual = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Content);

            ValidateFacilityJson(expected, actual, _now);
        }

        [TestMethod]
        public void TestShowWithStringReturnsNotFoundForInvalidString()
        {
            var result = (HttpNotFoundResult)_target.ShowString("Invalid-Identifier");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestShowWithFacilityProcessesReturnsProperJson()
        {
            var processStage1 = GetEntityFactory<ProcessStage>().Create(new { Description = "Source of Supply"});
            var processStage2 = GetEntityFactory<ProcessStage>().Create(new { Description = "Water Treatment"});
            var process1 = GetEntityFactory<Process>()
               .Create(new {Sequence = 1.01m, ProcessStage = processStage1, Description = "Surface Water Pumping" });
            var process2 = GetEntityFactory<Process>()
               .Create(new { Sequence = 1.02m, ProcessStage = processStage2, Description = "Water Treatment" });
            var expected = GetEntityFactory<Facility>().Create(new {
                Process = processStage1
            });
            var facilityProcess1 = GetEntityFactory<FacilityProcess>().Create(new {
                Process = process1, Facility = expected
            });
            var facilityProcess2 = GetEntityFactory<FacilityProcess>().Create(new {
                Process = process2, Facility = expected
            });
            expected.FacilityProcesses.Add(facilityProcess1);
            expected.FacilityProcesses.Add(facilityProcess2);

            Session.Flush(); // save changes, so facility has the FacilityProcesses
            
            var result = (ContentResult)_target.Show(expected.Id);
            var actual = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Content);

            Assert.AreEqual(expected.Id.ToString(), actual["Id"].ToString());
            Assert.AreEqual(processStage1.Description, actual["Process"]);
            var processes = (Newtonsoft.Json.Linq.JContainer)actual["FacilityProcesses"];
            Assert.AreEqual(2, processes.Count);
            var first = processes.First();
            var last = processes.Last();
            //id
            Assert.AreEqual(process1.Id, first.Value<int>("Id"));
            Assert.AreEqual(process2.Id, last.Value<int>("Id"));
            //Sequence
            Assert.AreEqual(process1.Sequence, first.Value<decimal>("Sequence"));
            Assert.AreEqual(process2.Sequence, last.Value<decimal>("Sequence"));
            //ProcessStage
            Assert.AreEqual(processStage1.Description, first.Value<string>("ProcessStage"));
            Assert.AreEqual(processStage2.Description, last.Value<string>("ProcessStage"));
            //Process
            Assert.AreEqual(process1.Description, first.Value<string>("Process"));
            Assert.AreEqual(process2.Description, last.Value<string>("Process"));
        }

        [TestMethod]
        public void TestShowWithFacilityAreasReturnsProperJson()
        {
            var facilityArea1 = GetEntityFactory<FacilityArea>().Create(new { Description = "Lab" });
            var facilityArea2 = GetEntityFactory<FacilityArea>().Create(new { Description = "Chemical" });
            var facilitySubArea1 = GetEntityFactory<FacilitySubArea>()
               .Create(new { Area = facilityArea1, Description = "Bacteriological" });
            var facilitySubArea2 = GetEntityFactory<FacilitySubArea>()
               .Create(new { Area = facilityArea2, Description = "Lime" });
            var expected = GetEntityFactory<Facility>().Create();
            var facilityFacilityArea1 = GetEntityFactory<FacilityFacilityArea>()
               .Create(new { FacilityArea = facilityArea1, Facility = expected, FacilitySubArea = facilitySubArea1 });
            var facilityFacilityArea2 = GetEntityFactory<FacilityFacilityArea>()
               .Create(new { FacilityArea = facilityArea2, Facility = expected, FacilitySubArea = facilitySubArea2 });
            expected.FacilityAreas.Add(facilityFacilityArea1);
            expected.FacilityAreas.Add(facilityFacilityArea2);

            Session.Flush(); // save changes, so facility has the FacilityAreas

            var result = (ContentResult)_target.Show(expected.Id);
            var actual = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Content);

            Assert.AreEqual(expected.Id.ToString(), actual["Id"].ToString());
            var areas = (Newtonsoft.Json.Linq.JContainer)actual["FacilityAreas"];
            Assert.AreEqual(2, areas.Count);
            var first = areas.First();
            Assert.AreEqual(facilityFacilityArea1.Id, first.Value<int>("Id"));
            Assert.AreEqual(facilityArea1.Description, first.Value<string>("FacilityArea"));
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop, tested below
        }

        [TestMethod]
        public void Test_Index_ReturnsProperJson()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                State = state
            });
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var facilityStatus = GetFactory<ActiveFacilityStatusFactory>().Create();
            var facilityOwner = GetFactory<AWFacilityOwnerFactory>().Create();
            var facilities = GetEntityFactory<Facility>().CreateList(3,
                new {
                    FacilityStatus = facilityStatus,
                    FacilityOwner = facilityOwner,
                    OperatingCenter = operatingCenter, 
                    PublicWaterSupply = publicWaterSupply
                });

            _ = GetEntityFactory<Facility>().Create();

            var searchFacility = new SearchFacility {
                State = state.Id,
                PublicWaterSupply = publicWaterSupply.Id
            };

            var result = (JsonResult)_target.Index(searchFacility);
            var helper = new JsonResultTester(result);

            Assert.AreEqual(helper.Count, facilities.Count);

            for (var i = 0; i < facilities.Count; i++)
            {
                helper.AreEqual(facilities[i].Id, "Id", i);
                helper.AreEqual(facilities[i].FacilityId, "FacilityId", i);
                helper.AreEqual(facilities[i].FacilityName, "FacilityName", i);
                helper.AreEqual(facilities[i].StreetNumber, "StreetNumber", i);
                helper.AreEqual(facilities[i].Street?.Name, "Street", i);
                helper.AreEqual(facilities[i].Town?.ShortName, "Town", i);
                helper.AreEqual(facilities[i].ZipCode, "ZipCode", i);
            }
        }
    }
}
