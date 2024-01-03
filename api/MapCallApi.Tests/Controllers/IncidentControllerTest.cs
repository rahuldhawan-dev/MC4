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
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ActiveMQ;
using Moq;
using StructureMap;

namespace MapCallApi.Tests.Controllers
{
    [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class IncidentControllerTest : MapCallApiControllerTestBase<IncidentController, Incident, IncidentRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.OperationsIncidents;

            Authorization.Assert(a => {
                SetupHttpAuth(a);

                a.RequiresRole("~/Incident/Index/", module);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: returns json result, tested below
        }

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var now = DateTime.Now;
            var entity0 = GetEntityFactory<Incident>().Create(new {
                IncidentDate = now.AddDays(-1)
            });
            var entity1 = GetEntityFactory<Incident>().Create(new {
                IncidentDate = now.AddDays(-1)
            });
            var search = new SearchIncident {
                IncidentDate = new DateRange {
                    Start = now.AddDays(-2),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            helper.AreEqual(entity0.Id, "Id");
            helper.AreEqual(entity1.Id, "Id", 1);
        }
        
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
    }
}
