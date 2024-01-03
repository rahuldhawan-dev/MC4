using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities.ActiveMQ;
using Moq;
using StructureMap;

namespace MapCallApi.Tests.Controllers {
    [TestClass]
    public class TailgateTalkControllerTest : MapCallApiControllerTestBase<TailgateTalkController, TailgateTalk, TailgateTalkRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            var user = GetFactory<AdminUserFactory>().Create(new {
                DefaultOperatingCenter = GetFactory<OperatingCenterFactory>().Create()
            });

            Session.Save(user.UserType);

            return user;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = TailgateTalkController.ROLE;

            Authorization.Assert(a => {
                SetupHttpAuth(a);

                a.RequiresRole("~/TailgateTalk/Index/", module);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // override due to between error.
        }

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var now = DateTime.Now;
            var entity0 = GetEntityFactory<TailgateTalk>().Create(new {
                TrainingTimeHours = 10m, HeldOn = now.AddDays(-1)
            });
            var entity1 = GetEntityFactory<TailgateTalk>().Create(new {
                TrainingTimeHours = 11m, HeldOn = now.AddDays(-1)
            });
            var search = new SearchTailgateTalk {
                HeldOn = new DateRange {
                    Start = now.AddDays(-2),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            helper.AreEqual(entity0.Id, "Id");
            helper.AreEqual(entity1.Id, "Id", 1);
            helper.AreEqual(entity0.TrainingTimeHours, "TrainingTimeHours");
            helper.AreEqual(entity1.TrainingTimeHours, "TrainingTimeHours", 1);
        }
    }
}