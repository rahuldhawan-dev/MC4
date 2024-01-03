using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class RestorationTest
    {
        #region Fields

        private Restoration _target;
        private Contractor _contractor;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _contractor = new Contractor();
            _target = new Restoration();
            _target.RestorationType = new RestorationType();
        }

        #endregion

        #region RelatedRestorations

        [TestMethod]
        public void TestRelatedRestorationsReturnsEmptyIEnumerableIfWorkOrderIsNull()
        {
            _target.WorkOrder = null;
            Assert.IsNotNull(_target.RelatedRestorations);
        }

        #endregion

        #region EstimatedRestorationFootage

        [TestMethod]
        public void TestEstimatedRestorationFootageReturnsLinearFeetOfCurbWhenRestorationTypeContainsCURB()
        {
            _target.LinearFeetOfCurb = 42;
            _target.PavingSquareFootage = 11;
            _target.RestorationType = new RestorationType {
                Description = "CURB YOUR ENTHUSIASM"
            };

            Assert.AreEqual(42m, _target.EstimatedRestorationFootage);
        }

        [TestMethod]
        public void TestEstimatedRestorationFootageReturnsLinearFeetOfCurbWhenRestorationTypeDoesNotContainCURB()
        {
            var target = new Restoration();
            target.LinearFeetOfCurb = 42;
            target.PavingSquareFootage = 11;
            target.RestorationType = new RestorationType {
                Description = "CARB YOAR ENTHASIUSM"
            };

            Assert.AreEqual(11m, target.EstimatedRestorationFootage);
        }

        #endregion

        #region MeasurementType

        [TestMethod]
        public void TestMeasurementTypeReturnsTheSameThingAsRestorationTypeMeasurementType()
        {
            _target.RestorationType = new RestorationType {
                Description = "Not the C URB word!"
            };

            Assert.AreEqual(RestorationMeasurementTypes.SquareFt, _target.RestorationType.MeasurementType,
                "Sanity check");
            Assert.AreEqual(RestorationMeasurementTypes.SquareFt, _target.MeasurementType);

            _target.RestorationType.Description = "Curb your enthusiasm";

            Assert.AreEqual(RestorationMeasurementTypes.LinearFt, _target.RestorationType.MeasurementType,
                "Sanity check");
            Assert.AreEqual(RestorationMeasurementTypes.LinearFt, _target.MeasurementType);
        }

        #endregion

        #region AccrualValue

        [TestMethod]
        public void TestAccrualValueReturnsTotalAccruedValueIfPartialRestorationActualCostIsNullOrZero()
        {
            var expected = 999m;
            _target.TotalAccruedCost = expected;

            _target.PartialRestorationActualCost = null;
            Assert.AreEqual(expected, _target.AccrualValue);

            _target.PartialRestorationActualCost = 0;
            Assert.AreEqual(expected, _target.AccrualValue);
        }

        [TestMethod]
        public void TestAccrualValueReturnsExpectedValueWhenPartialSquareFootageIsZero()
        {
            var workOrder = new WorkOrder();
            var opc = new OperatingCenter();
            workOrder.OperatingCenter = opc;
            var restorationType = new RestorationType { Description = "curb" };
            var restorationTypeCost = new RestorationTypeCost { FinalCost = 12, OperatingCenter = opc };
            restorationType.RestorationTypeCosts.Add(restorationTypeCost);
            _target.WorkOrder = workOrder;
            _target.OperatingCenter = opc;
            _target.RestorationType = restorationType;
            _target.PartialPavingSquareFootage = 0;
            _target.PartialRestorationActualCost = 10;
            _target.LinearFeetOfCurb = 13;
            Assert.AreEqual(156m, _target.AccrualValue);
        }

        [TestMethod]
        public void TestAccrualValueReturnsExpectedValueWhenPartialSquareFootageIsGreaterThanZero()
        {
            var workOrder = new WorkOrder();
            var opc = new OperatingCenter();
            workOrder.OperatingCenter = opc;
            var restorationType = new RestorationType { Description = "curb" };
            var restorationTypeCost = new RestorationTypeCost { FinalCost = 12, OperatingCenter = opc };
            restorationType.RestorationTypeCosts.Add(restorationTypeCost);
            _target.WorkOrder = workOrder;
            _target.OperatingCenter = opc;
            _target.RestorationType = restorationType;
            _target.PartialPavingSquareFootage = 2;
            _target.PartialRestorationActualCost = 10;
            _target.LinearFeetOfCurb = 13;
            Assert.AreEqual(24m, _target.AccrualValue);
        }

        #endregion

        #region CalculateAccruedCost

        [TestMethod]
        public void TestCACReturnsZeroIfThereAreNoMatchingRestorationCostTypes()
        {
            var target = new Restoration {
                OperatingCenter = new OperatingCenter(),
                RestorationType = new RestorationType {Description = "Not curb"},
                PavingSquareFootage = 42m,
                LinearFeetOfCurb = 11m,
            };
            var rtc = new RestorationTypeCost();
            target.RestorationType.RestorationTypeCosts.Add(rtc);
            Assert.AreEqual(0m, target.CalculateAccruedCost());
        }

        [TestMethod]
        public void TestCACUsesEstimatedRestorationFootageWhenCalculating()
        {
            var opc = new OperatingCenter();
            var target = new Restoration {
                OperatingCenter = opc,
                RestorationType = new RestorationType {Description = "Not curb"},
                PavingSquareFootage = 42m,
                LinearFeetOfCurb = 10m,
            };

            target.RestorationType.RestorationTypeCosts.Add(new RestorationTypeCost {
                OperatingCenter = opc,
                Cost = 2d
            });

            Assert.AreEqual(84m, target.CalculateAccruedCost());
        }

        #endregion

        #region AssignContractor

        [TestMethod]
        public void TestAssignContractorThrowsExceptionIfRestorationTypeIsNull()
        {
            _target.RestorationType = null;
            MyAssert.Throws<InvalidOperationException>(() => _target.AssignContractor(_contractor, DateTime.Now));
        }

        [TestMethod]
        public void TestAssignContractorThrowsExceptionIfContractorIsNull()
        {
            MyAssert.Throws<NotSupportedException>(() => _target.AssignContractor(null, DateTime.Now));
        }

        [TestMethod]
        public void TestAssignContractorWorksAsExpected()
        {
            var expectedDate = new DateTime(1984, 4, 24);
            _target.RestorationType = new RestorationType {
                PartialRestorationDaysToComplete = 2,
                FinalRestorationDaysToComplete = 5
            };
            _target.AssignContractor(_contractor, expectedDate);

            Assert.AreSame(_contractor, _target.AssignedContractor);
            Assert.AreEqual(expectedDate, _target.AssignedContractorAt);
            Assert.AreEqual(new DateTime(1984, 4, 26), _target.PartialRestorationDueDate);
            Assert.AreEqual(new DateTime(1984, 4, 29), _target.FinalRestorationDueDate);
        }

        //if (wo.Restorations != null && wo.Restorations.Any())
        //{
        //    var lastRestoration = wo.Restorations.OrderByDescending(x => x.Id).First();
        //    AssignedContractor = lastRestoration.AssignedContractor?.Id;
        //    WBSNumber = lastRestoration.WBSNumber;
        //    PartialRestorationPurchaseOrderNumber = lastRestoration.PartialRestorationPurchaseOrderNumber;
        //    FinalRestorationPurchaseOrderNumber = lastRestoration.FinalRestorationPurchaseOrderNumber;
        //}

        #endregion
    }
}
