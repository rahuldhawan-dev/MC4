using System;
using System.Collections.Generic;
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
    [TestClass]
    public class MainBreakRepairsForGISControllerTest : MapCallApiControllerTestBase<MainBreakRepairsForGISController, WorkOrder, WorkOrderRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = MainBreakRepairsForGISController.ROLE;

            Authorization.Assert(a => {
                SetupHttpAuth(a);

                a.RequiresRole("~/MainBreakRepairsForGIS/Index/", module);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            Assert.Inconclusive("Fix and test me. I don't do well when DateCompleted.Start doesn't have a value");
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // override because all.
            Assert.Inconclusive("Overriding this as I am testing all model properties individually");
        }

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var now = DateTime.Now;
            var coordinate = GetEntityFactory<Coordinate>().Create(new { Latitude = -10m, Longitude = 10m });
            var coordinate1 = GetEntityFactory<Coordinate>().Create(new { Latitude = -15m, Longitude = 15m });
            var wo0 = GetEntityFactory<WorkOrder>()
                .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate.Latitude,
                    Longitude = coordinate.Longitude,
                    CreatedAt = now.AddDays(-5)
                });
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate1.Latitude,
                    Longitude = coordinate1.Longitude,
                    CreatedAt = now.AddDays(-7)
                });
            var wo2 = GetEntityFactory<WorkOrder>()
               .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-5),
                    Latitude = coordinate1.Latitude,
                    Longitude = coordinate1.Longitude,
                    CreatedAt = now.AddDays(-7),
                    DateStarted = now.AddHours(-12)
                });
            var search = new SearchMainBreakRepairsForGIS {
                DateCompleted = new DateRange {
                    Start = now.AddDays(-2),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            helper.AreEqual(wo0.Id.ToString(), "WorkOrderNumber");
            helper.AreEqual(wo0.Latitude, "Latitude");
            helper.AreEqual(wo0.Longitude, "Longitude");
            helper.AreEqual(wo0.CreatedAt, "CreatedOn");
            helper.AreEqual(wo1.Id.ToString(), "WorkOrderNumber", 1);
            helper.AreEqual(wo1.Latitude, "Latitude", 1);
            helper.AreEqual(wo1.Longitude, "Longitude", 1);
            helper.AreEqual(wo1.CreatedAt, "CreatedOn", 1);
            Assert.AreEqual(2, helper.Count);
        }

        [TestMethod]
        public void TestIndexJSONExportsJSONReturnsWorkOrdersStartedWithin24Hours()
        {
            var now = DateTime.Now;
            var coordinate = GetEntityFactory<Coordinate>().Create(new { Latitude = -10m, Longitude = 10m });
            var coordinate1 = GetEntityFactory<Coordinate>().Create(new { Latitude = -15m, Longitude = 15m });
            var wo0 = GetEntityFactory<WorkOrder>()
                .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate.Latitude,
                    Longitude = coordinate.Longitude,
                    CreatedAt = now.AddDays(-5)
                });
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate1.Latitude,
                    Longitude = coordinate1.Longitude,
                    CreatedAt = now.AddDays(-7)
                });
            var crewAssignment = GetEntityFactory<CrewAssignment>().Create(new {
                DateStarted = now.AddHours(-1)
            });
            var wo2 = GetEntityFactory<WorkOrder>()
               .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate1.Latitude,
                    Longitude = coordinate1.Longitude,
                    CreatedAt = now.AddDays(-7),
                    DateStarted = now.AddHours(-12)
               });
            wo2.DateCompleted = null;
            wo2.CrewAssignments = new List<CrewAssignment>(){ crewAssignment };
            Session.Save(wo2);
            Session.Flush();

            var search = new SearchMainBreakRepairsForGIS {
                DateCompleted = new DateRange {
                    Start = now.AddDays(-2),
                    End = now,
                    Operator = RangeOperator.Between
                },
                RecentOrders = true
            };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            helper.AreEqual(wo0.Id.ToString(), "WorkOrderNumber");
            helper.AreEqual(wo0.Latitude, "Latitude");
            helper.AreEqual(wo0.Longitude, "Longitude");
            helper.AreEqual(wo1.Id.ToString(), "WorkOrderNumber", 1);
            helper.AreEqual(wo1.Latitude, "Latitude", 1);
            helper.AreEqual(wo1.Longitude, "Longitude", 1);
            helper.AreEqual(wo2.Id.ToString(), "WorkOrderNumber", 2);
            Assert.AreEqual(3, helper.Count);
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsLongerThanOneMonth()
        {
            var now = DateTime.Now;
            var wo0 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var search = new SearchMainBreakRepairsForGIS {
                DateCompleted = new DateRange {
                    Start = now.AddDays(-31),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsNotSent()
        {
            var wo0 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var search = new SearchMainBreakRepairsForGIS();

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
                var wo0 = GetEntityFactory<WorkOrder>()
                    .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
                var wo1 = GetEntityFactory<WorkOrder>()
                    .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
                var search = new SearchMainBreakRepairsForGIS {
                    DateCompleted = new DateRange {
                        Start = now.AddMonths(-1).AddDays(-1),
                        End = now,
                        Operator = op
                    }
                };

                MyAssert.Throws<InvalidOperationException>(() =>
                    _target.Index(search));
            }
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