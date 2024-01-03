using System.Net;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class StreetControllerTest : MapCallApiControllerTestBase<StreetController, Street, StreetRepository>
    {
        #region Init/Cleanup
        
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }
        
        #endregion
        
        #region Tests
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesDataLookups;
            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/Street/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // no op
        }

        [TestMethod]
        public void TestIndexReturnsNoResultsWhenTownIsNotProvided()
        {
            _target.ModelState.AddModelError("Town", "Required");
            var result = (JsonHttpStatusCodeResult)_target.Index(new SearchStreet());
            
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(StreetController.TOWN_REQUIRED_ERROR, result.StatusDescription);
        }
        
        [TestMethod]
        public void TestIndexReturnsResultsForTown()
        {
            var town = GetEntityFactory<Town>().CreateList(2);
            var prefix = GetFactory<StreetPrefixFactory>().Create(new { Description = "N" });
            var suffix1 = GetFactory<StreetSuffixFactory>().Create(new { Description = "ST" });
            var suffix2 = GetFactory<StreetSuffixFactory>().Create(new { Description = "AVE" });
            
            GetEntityFactory<Street>().Create(new {
                Town = town[0],
                Prefix = prefix,
                Suffix = suffix1,
                FullStName = "N FIRST ST",
                Name = "FIRST"
            });
            GetEntityFactory<Street>().Create(new {
                Town = town[0],
                Prefix = prefix,
                Suffix = suffix2,
                FullStName = "N FIRST AVE",
                Name = "FIRST"
            });
            GetEntityFactory<Street>().Create(new {
                Town = town[1],
                Prefix = prefix,
                Suffix = suffix1,
                FullStName = "N SECOND ST",
                Name = "SECOND"
            });
            
            var search = new SearchStreet { Town = town[0].Id };
        
            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);
        
            Assert.AreEqual(2, helper.Count);
            helper.AreEqual(1, "Id", 0);
            helper.AreEqual("N FIRST ST", "FullStName", 0);
            helper.AreEqual(2, "Id", 1);
            helper.AreEqual("N FIRST AVE", "FullStName", 1);
        }
        
        [TestMethod]
        public void TestIndexReturnsResultsForTownAndFullStNamePartial()
        {
            var town = GetEntityFactory<Town>().CreateList(2);
            var prefix = GetFactory<StreetPrefixFactory>().Create(new { Description = "N" });
            var suffix1 = GetFactory<StreetSuffixFactory>().Create(new { Description = "ST" });
            var suffix2 = GetFactory<StreetSuffixFactory>().Create(new { Description = "AVE" });
            
            GetEntityFactory<Street>().Create(new {
                Town = town[0],
                Prefix = prefix,
                Suffix = suffix1,
                FullStName = "N FIRST ST",
                Name = "FIRST"
            });
            GetEntityFactory<Street>().Create(new {
                Town = town[0],
                Prefix = prefix,
                Suffix = suffix2,
                FullStName = "N FIRST AVE",
                Name = "FIRST"
            });
            GetEntityFactory<Street>().Create(new {
                Town = town[1],
                Prefix = prefix,
                Suffix = suffix1,
                FullStName = "N SECOND ST",
                Name = "SECOND"
            });

            var search = new SearchStreet {
                Town = town[0].Id, FullStName = new SearchString {
                    Value = " ave",
                    MatchType = SearchStringMatchType.Wildcard
                }
            };
            
            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);
        
            Assert.AreEqual(1, helper.Count);
            helper.AreEqual(2, "Id", 0);
            helper.AreEqual("N FIRST AVE", "FullStName");
        }

        [TestMethod]
        public void TestIndexReturnsOnlyPageSizeNumberOfResultsWhenUsingPageSize()
        {
            var town = GetEntityFactory<Town>().CreateList(2);
            var prefix = GetFactory<StreetPrefixFactory>().Create(new { Description = "N" });
            var suffix1 = GetFactory<StreetSuffixFactory>().Create(new { Description = "ST" });
            var suffix2 = GetFactory<StreetSuffixFactory>().Create(new { Description = "AVE" });

            GetEntityFactory<Street>().Create(new {
                Town = town[0],
                Prefix = prefix,
                Suffix = suffix1,
                FullStName = "N FIRST ST",
                Name = "FIRST"
            });
            GetEntityFactory<Street>().Create(new {
                Town = town[0],
                Prefix = prefix,
                Suffix = suffix2,
                FullStName = "N FIRST AVE",
                Name = "FIRST"
            });
            GetEntityFactory<Street>().Create(new {
                Town = town[1],
                Prefix = prefix,
                Suffix = suffix1,
                FullStName = "N SECOND ST",
                Name = "SECOND"
            });
            
            var search = new SearchStreet { Town = town[0].Id, PageSize = 1 };
    
            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);
    
            Assert.AreEqual(1, helper.Count);
            helper.AreEqual(1, "Id", 0);
            helper.AreEqual("N FIRST ST", "FullStName", 0);
        }
        
        #endregion
    }
}