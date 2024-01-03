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
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ActiveMQ;
using Moq;
using StructureMap;

namespace MapCallApi.Tests.Controllers {
    [TestClass]
    public class GeneralLiabilityClaimControllerTest : MapCallApiControllerTestBase<GeneralLiabilityClaimController, GeneralLiabilityClaim, GeneralLiabilityClaimRepository>
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
            var module = GeneralLiabilityClaimController.ROLE;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/GeneralLiabilityClaim/Index/", module);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: returns json
        }

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
            var search = new SearchGeneralLiabilityClaim {
                IncidentDateTime = new DateRange {
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    Operator = RangeOperator.Between
                }
            };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            helper.AreEqual(1, "Id");
            helper.AreEqual(2, "Id", 1);
            helper.AreEqual("description 0", "Description");
            helper.AreEqual("description 1", "Description", 1);
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsNotSent()
        {
            var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
            var search = new SearchGeneralLiabilityClaim();

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsLongerThanOneMonth()
        {
            var now = DateTime.Now;
            var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
            var search = new SearchGeneralLiabilityClaim {
                IncidentDateTime = new DateRange {
                    Start = now.AddDays(-31),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeOperatorIsNotBetween()
        {
            foreach (var op in new[] {
                RangeOperator.Equal, RangeOperator.GreaterThan, RangeOperator.GreaterThanOrEqualTo,
                RangeOperator.LessThan, RangeOperator.LessThanOrEqualTo
            })
            {
                var now = DateTime.Now;
                var entity0 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 0"});
                var entity1 = GetEntityFactory<GeneralLiabilityClaim>().Create(new {Description = "description 1"});
                var search = new SearchGeneralLiabilityClaim {
                    IncidentDateTime = new DateRange {
                        Start = now.AddDays(-1),
                        End = now,
                        Operator = op
                    }
                };

                MyAssert.Throws<InvalidOperationException>(() =>
                    _target.Index(search));
            }
        }

    }
}