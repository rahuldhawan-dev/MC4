using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FacilityFacilityAreaControllerTest : MapCallMvcControllerTestBase<FacilityFacilityAreaController, FacilityFacilityArea>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FacilityFacilityArea/FacilityAreaByFacilityId/");
                a.RequiresLoggedInUserOnly("~/FacilityFacilityArea/FacilitySubAreaByFacilityAreaId/");
                a.RequiresLoggedInUserOnly("~/FacilityFacilityArea/ByFacilityId/");
                a.RequiresLoggedInUserOnly("~/FacilityFacilityArea/ByFacilityIds/");
            });
        }
        #endregion

        #region ByFacilityId

        [TestMethod]
        public void TestFacilityAreaByFacilityId()
        {
            var facility1 = GetFactory<FacilityFactory>().Create();
            var facilityArea1 = GetEntityFactory<FacilityArea>().Create(new { Description = "FacilityAreaDescription1" });
            var facilitySubArea1 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription1",
                Area = facilityArea1
            });
            var ffa1 = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility1,
                FacilityArea = facilityArea1,
                FacilitySubArea = facilitySubArea1,
                Id = 1
            });
            var facility2 = GetFactory<FacilityFactory>().Create();
            var facilityArea2 = GetEntityFactory<FacilityArea>().Create(new { Description = "FacilityAreaDescription2" });
            var facilitySubArea2 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription2",
                Area = facilityArea2
            });
            var ffa2 = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility2,
                FacilityArea = facilityArea2,
                FacilitySubArea = facilitySubArea2,
                Id = 2
            });
            //search FacilityArea by facility2
            var result = (CascadingActionResult)_target.FacilityAreaByFacilityId(facility2.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(facility2.Id.ToString(), actual[1].Value);
            Assert.AreEqual("FacilityAreaDescription2", actual[1].Text);
        }

        [TestMethod]
        public void TestFacilitySubAreaByFacilityId()
        {
            var facility1 = GetFactory<FacilityFactory>().Create();
            var facilityArea1 = GetEntityFactory<FacilityArea>().Create(new { Description = "FacilityAreaDescription1" });
            var facilitySubArea1 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription1",
                Area = facilityArea1
            });
            var ffa1 = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility1,
                FacilityArea = facilityArea1,
                FacilitySubArea = facilitySubArea1,
                Id = 1
            });
            var facility2 = GetFactory<FacilityFactory>().Create();
            var facilityArea2 = GetEntityFactory<FacilityArea>().Create(new { Description = "FacilityAreaDescription2" });
            var facilitySubArea2 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription2",
                Area = facilityArea2
            });
            var ffa2 = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility2,
                FacilityArea = facilityArea2,
                FacilitySubArea = facilitySubArea2,
                Id = 2
            });
            //search FacilityArea by facility2 and facilityArea2
            var result = (CascadingActionResult)_target.FacilitySubAreaByFacilityAreaId(facility2.Id, facilityArea2.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(facility2.Id.ToString(), actual[1].Value);
            Assert.AreEqual("SubareaDescription2", actual[1].Text);
        }

        [TestMethod]
        public void TestFacilityFacilityAreaByFacilityId()
        {
            var facility1 = GetFactory<FacilityFactory>().Create();
            var facilityArea1 = GetEntityFactory<FacilityArea>().Create(new { Description = "test1" });
            var facilitySubArea1 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription1",
                Area = facilityArea1
            });
            var ffa1 = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility1,
                FacilityArea = facilityArea1,
                FacilitySubArea = facilitySubArea1,
                Id = 1
            });

            var facility2 = GetFactory<FacilityFactory>().Create();
            var facilityArea2 = GetEntityFactory<FacilityArea>().Create(new { Description = "test2" });
            var facilitySubArea2 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription2",
                Area = facilityArea2
            });
            var ffa2 = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility2,
                FacilityArea = facilityArea2,
                FacilitySubArea = facilitySubArea2,
                Id = 2
            });
            //search by facility2
            var result = (CascadingActionResult)_target.ByFacilityId(facility2.Id);
            var data = (IEnumerable<FacilityFacilityAreaDisplayItem>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual("test2 - SubareaDescription2", data.Single().Display);
        }

        [TestMethod]
        public void TestFacilityFacilityAreaByFacilityIds()
        {
            var facility1 = GetFactory<FacilityFactory>().Create();
            var facilityArea1 = GetEntityFactory<FacilityArea>().Create(new { Description = "test1" });
            var facilitySubArea1 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription1",
                Area = facilityArea1
            });

            _ = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility1,
                FacilityArea = facilityArea1,
                FacilitySubArea = facilitySubArea1,
                Id = 1
            });

            var facility2 = GetFactory<FacilityFactory>().Create();
            var facilityArea2 = GetEntityFactory<FacilityArea>().Create(new { Description = "test2" });
            var facilitySubArea2 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription2",
                Area = facilityArea2
            });

            _ = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility2,
                FacilityArea = facilityArea2,
                FacilitySubArea = facilitySubArea2,
                Id = 2
            });

            var facility3 = GetFactory<FacilityFactory>().Create();
            var facilityArea3 = GetEntityFactory<FacilityArea>().Create(new { Description = "test3" });
            var facilitySubArea3 = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription3",
                Area = facilityArea3
            });

            _ = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility3,
                FacilityArea = facilityArea3,
                FacilitySubArea = facilitySubArea3,
                Id = 3
            });

            Session.Flush();

            var result = (CascadingActionResult)_target.ByFacilityIds(new[] { facility1.Id, facility2.Id });
            var data = ((IEnumerable<FacilityFacilityAreaDisplayItem>)result.Data).ToList();
            
            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(data[0].FacilityAreaDesc, facilityArea1.Description);
            Assert.AreEqual(data[1].FacilityAreaDesc, facilityArea2.Description);
        }

        #endregion
    }
}
