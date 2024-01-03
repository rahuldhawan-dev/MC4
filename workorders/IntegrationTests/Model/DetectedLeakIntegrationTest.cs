using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for DetectedLeakTestTest
    /// </summary>
    [TestClass]
    public class DetectedLeakIntegrationTest : WorkOrdersTestClass<DetectedLeak>
    {
        #region Exposed Static Methods

        public static DetectedLeak GetValidDetectedLeak()
        {
            return new DetectedLeak {
                WorkOrder = WorkOrderIntegrationTest.GetValidWorkOrder().Build(),
                WorkAreaType =
                    WorkAreaTypeIntegrationTest.GetValidWorkAreaType()
            };
        }

        public static void DeleteDetectedLeak(DetectedLeak entity)
        {
            var order = entity.WorkOrder;
            var area = entity.WorkAreaType;
            DetectedLeakRepository.Delete(entity);
            WorkOrderIntegrationTest.DeleteWorkOrder(order);
            WorkAreaTypeIntegrationTest.DeleteWorkAreaType(area);
        }

        #endregion

        #region Private Methods

        protected override DetectedLeak GetValidObject()
        {
            return GetValidDetectedLeak();
        }

        protected override DetectedLeak GetValidObjectFromDatabase()
        {
            var leak = GetValidObject();
            DetectedLeakRepository.Insert(leak);
            return leak;
        }

        protected override void DeleteObject(DetectedLeak entity)
        {
            DeleteDetectedLeak(entity);
        }

        #endregion

        // TODO: Are these tests even necessary?
        [TestMethod]
        public void TestSaveWithAccessPointsAndContacts()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.AccessPointsAndContacts = "This is a test string.";

                MyAssert.DoesNotThrow(
                    () => DetectedLeakRepository.Insert(target),
                    "Error saving LeakReportingSource object with value set for AccessPointsAndContacts.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithSoundRecorded()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.SoundRecorded = true;

                MyAssert.DoesNotThrow(
                    () => DetectedLeakRepository.Insert(target),
                    "Error saving DetectedLeak object with value set for SoundRecorded.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithLeakReportingSource()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                var source = LeakReportingSourceTest.GetValidLeakReportingSource();
                target.LeakReportingSource =
                    source;

                MyAssert.DoesNotThrow(
                    () => DetectedLeakRepository.Insert(target),
                    "Error saving DetectedLeak object with attached LeakReportingSource object.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithMileage()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.Mileage = (decimal)1.1;

                MyAssert.DoesNotThrow(
                    () => DetectedLeakRepository.Insert(target),
                    "Error saving DetectedLeak object with value set for Mileage.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithHydrantsSounded()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.HydrantsSounded = 4;

                MyAssert.DoesNotThrow(() => DetectedLeakRepository.Insert(target),
                                      "Error saving DetectedLeak object with value set for HydrantsSounded.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithMainsSounded()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.MainsSounded = 4;

                MyAssert.DoesNotThrow(() => DetectedLeakRepository.Insert(target),
                                      "Error saving DetectedLeak object with value set for MainsSounded.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithServicesSounded()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.ServicesSounded = 4;

                MyAssert.DoesNotThrow(() => DetectedLeakRepository.Insert(target),
                                      "Error saving DetectedLeak object with value set for ServicesSounded.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithSurveyStartingPoint()
        {
            using (_simulator.SimulateRequest())
            {
                var valve = ValveTest.GetValidValve();
                var target = GetValidObject();
                target.SurveyStartingPoint = valve;

                MyAssert.DoesNotThrow(() => DetectedLeakRepository.Insert(target),
                                      "Error saving DetectedLeak object with attached Valve (SurveyStartingPoint) object.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithSurveyEndingPoint()
        {
            using (_simulator.SimulateRequest())
            {
                var valve = ValveTest.GetValidValve();
                var target = GetValidObject();
                target.SurveyEndingPoint = valve;

                MyAssert.DoesNotThrow(() => DetectedLeakRepository.Insert(target),
                                      "Error saving DetectedLeak object with attached Valve (SurveyEndingPoint) object.");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestSaveWithMapPage()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                const string mappage = "1234567890";
                target.MapPage = mappage;

                MyAssert.DoesNotThrow(() => DetectedLeakRepository.Insert(target),
                                      "Error saving DetectedLeak object with value for MapPage.");
                Assert.AreEqual(mappage, target.MapPage);

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestSaveWithEquipmentUsed()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                const string equipmentused = "1234567890123456789012345";
                target.EquipmentUsed = equipmentused;

                MyAssert.DoesNotThrow(() => DetectedLeakRepository.Insert(target),
                                      "Error saving DetectedLeak object with value for EquipmentUsed.");

                DeleteObject(target);
            }
        }
    }
}