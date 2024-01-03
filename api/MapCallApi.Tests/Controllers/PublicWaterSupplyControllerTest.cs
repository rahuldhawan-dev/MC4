using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Newtonsoft.Json;
using NHibernate.Mapping;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public  class PublicWaterSupplyControllerTest : MapCallApiControllerTestBase<PublicWaterSupplyController, PublicWaterSupply, IRepository<PublicWaterSupply>>
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
            var module = PublicWaterSupplyController.ROLE;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/PublicWaterSupply/Index", module);
            });
        }

        [TestMethod]
        // Adding this override as this test is running but is not applicable for this controller
        public override void TestIndexReturnsResults() { }

        [TestMethod]
        public void Test_Index_ReturnsProperJson()
        {
            var state = GetEntityFactory<State>().Create();
            var status = GetEntityFactory<PublicWaterSupplyStatus>().Create();
            var ownership = GetEntityFactory<PublicWaterSupplyOwnership>().Create();
            var publicWaterSupplies = GetEntityFactory<PublicWaterSupply>().CreateList(5, new { Status = status, Ownership = ownership, State = state });

            var search = new SearchPublicWaterSupply { State = state.Id };
            var results = (JsonResult)_target.Index(search);
            var helper = new JsonResultTester(results);

            for (int i = 0; i < publicWaterSupplies.Count; i++)
            {
                var publicWaterSupply = new PublicWaterSupply();

                helper.AreEqual(publicWaterSupplies[i].Id, nameof(publicWaterSupply.Id), i);
                helper.AreEqual(publicWaterSupplies[i].System, nameof(publicWaterSupply.System), i);
                helper.AreEqual(publicWaterSupplies[i].Identifier, nameof(publicWaterSupply.Identifier), i);
                helper.AreEqual(publicWaterSupplies[i].FreeChlorineReported, nameof(publicWaterSupply.FreeChlorineReported), i);
                helper.AreEqual(publicWaterSupplies[i].TotalChlorineReported, nameof(publicWaterSupply.TotalChlorineReported), i);
                helper.AreEqual(publicWaterSupplies[i].Description, nameof(publicWaterSupply.Description), i);
            }
        }
    }
}
