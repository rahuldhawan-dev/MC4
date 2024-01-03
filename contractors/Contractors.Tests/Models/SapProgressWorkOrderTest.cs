using System;
using Contractors.Models;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Models
{
    [TestClass]
    public class SapProgressWorkOrderTest
    {
        private WorkOrder workOrder;

        [TestMethod]
        public void TestContructorSetsPropertiesForSapProgressWorkOrder()
        {
            workOrder = new WorkOrder {
                Purpose = new WorkOrderPurpose {
                    SapCode = "I06",
                    CodeGroup = "N-D-PUR1",
                    Description = "Seasonal",
                    Id = 14,
                    IsProduction = true
                },
                PlannedCompletionDate = DateTime.Today,
                MeterLocation = new MeterLocation() {SAPCode = "c1"}
            };
            var target = new SapProgressWorkOrder(workOrder);

            ProgressWorkOrder progressWorkOrder = target.ProcessWorkOrderRequest();

            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].PurposeCode, target.MapCallPurpose);
            Assert.AreEqual(DateTime.Today.ToString(SapProgressWorkOrder.SAP_DATE_FORMAT), target.BasicFinish);
            Assert.AreEqual("c1", target.Location);
        }

        [TestMethod]
        public void TestConstructorSetsMeterSetToYWhenServiceInstallationsAndWorkDescriptionValid()
        {
            foreach (var workDescriptionId in WorkDescription.NEW_SERVICE_INSTALLATION)
            {
                var order = new WorkOrder {
                    WorkDescription = new WorkDescription { Id = workDescriptionId },
                    DateCompleted = DateTime.Now
                };
                order.CrewAssignments.Add(new CrewAssignment());
                order.ServiceInstallations.Add(new ServiceInstallation());

                var target = new SapProgressWorkOrder(order);

                Assert.AreEqual("Y", target.SetMeter);
            }
        }

        [TestMethod]
        public void TestConstructorSetsMeterSetToNWhenNoServiceInstallations()
        {
            foreach (var workDescriptionId in WorkDescription.NEW_SERVICE_INSTALLATION)
            {
                var order = new WorkOrder {
                    WorkDescription = new WorkDescription { Id = workDescriptionId },
                    DateCompleted = DateTime.Now
                };
                order.CrewAssignments.Add(new CrewAssignment());

                var target = new SapProgressWorkOrder(order);

                Assert.AreEqual("N", target.SetMeter);
            }
        }

        [TestMethod]
        public void TestConstructorDoesNotSetMeterSetWhenCancelled()
        {
            foreach (var workDescriptionId in WorkDescription.NEW_SERVICE_INSTALLATION)
            {
                var order = new WorkOrder {
                    WorkDescription = new WorkDescription { Id = workDescriptionId },
                    CancelledAt = DateTime.Now,
                    CrewAssignments = new[] { new CrewAssignment { DateStarted = DateTime.Now } }
                };

                var target = new SAPProgressWorkOrder(order);

                Assert.IsNull(target.SetMeter);
            }
        }
    }
}
