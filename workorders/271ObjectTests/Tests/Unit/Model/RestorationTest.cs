using System;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for RestorationTest
    /// </summary>
    [TestClass]
    public class RestorationTest
    {
        #region Constants

        private const int RESTORATION_TYPE_ID = 1,
                  INITIAL_RESTORATION_METHOD_ID = 2,
                  FINAL_RESTORATION_METHOD_ID = 3;

        #endregion

        #region Private Members

        private IRepository<Restoration> _repository;
        private Restoration _target;
        private RestorationType _type;
        private WorkOrder _order;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationIntegrationTestInitialize()
        {
            _repository = new MockRestorationRepository();
            _order = new TestWorkOrderBuilder();
            _type = new RestorationType();
            _target = new TestRestorationBuilder()
                .WithWorkOrder(_order)
                .WithRestorationType(_type);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewRestoration()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target = new TestRestorationBuilder()
                .WithWorkOrder(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutRestorationType()
        {
            _target = new TestRestorationBuilder()
                .WithRestorationType(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestSaveWithPavingSquareFootage()
        {
            var expected = 20;
            _target.PavingSquareFootage = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PavingSquareFootage);
        }

        [TestMethod]
        public void TestSaveWithLinearFeetOfCurb()
        {
            var expected = 20;
            _target.LinearFeetOfCurb = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.LinearFeetOfCurb);
        }

        [TestMethod]
        public void TestSaveWithRestorationNotes()
        {
            var expected = "Test Notes";
            _target.RestorationNotes = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.RestorationNotes);
        }

        [TestMethod]
        public void TestSaveWithPartialRestorationInvoiceNumber()
        {
            var expected = "123456789012";
            _target.PartialRestorationInvoiceNumber = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PartialRestorationInvoiceNumber);
        }

        [TestMethod]
        public void TestSaveWithPartialRestorationDate()
        {
            var expected = DateTime.Today;
            _target.PartialRestorationDate = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PartialRestorationDate);
        }

        [TestMethod]
        public void TestSaveWithPartialRestorationCompletedBy()
        {
            var expected = "1234567890";
            _target.PartialRestorationCompletedBy = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PartialRestorationCompletedBy);
        }

        [TestMethod]
        public void TestSaveWithPartialPavingBreakOutEightInches()
        {
            var expected = 1;
            _target.PartialPavingBreakOutEightInches = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PartialPavingBreakOutEightInches);
        }

        [TestMethod]
        public void TestSaveWithPartialPavingBreakOutTenInches()
        {
            var expected = 2;
            _target.PartialPavingBreakOutTenInches = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PartialPavingBreakOutTenInches);
        }

        [TestMethod]
        public void TestSaveWithPartialSawCutting()
        {
            var expected = 3;
            _target.PartialSawCutting = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PartialSawCutting);
        }

        [TestMethod]
        public void TestSaveWithPartialPavingSquareFootage()
        {
            var expected = 40;
            _target.PartialPavingSquareFootage = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.PartialPavingSquareFootage);
        }

        [TestMethod]
        public void TestSaveWithDaysToPartialPaveHole()
        {
            var expected = 4;
            _target.DaysToPartialPaveHole = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.DaysToPartialPaveHole);
        }

        [TestMethod]
        public void TestSaveWithTrafficControlCostPartialRestoration()
        {
            var expected = 4;
            _target.TrafficControlCostPartialRestoration = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.TrafficControlCostPartialRestoration);
        }

        [TestMethod]
        public void TestSaveWithFinalRestorationInvoiceNumber()
        {
            var expected = "123456789012";
            _target.FinalRestorationInvoiceNumber = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalRestorationInvoiceNumber);
        }

        [TestMethod]
        public void TestSaveWithFinalRestorationDate()
        {
            var expected = DateTime.Today;
            _target.FinalRestorationDate = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalRestorationDate);
        }

        [TestMethod]
        public void TestSaveWithFinalRestorationCompletedBy()
        {
            var expected = "1234567890";
            _target.FinalRestorationCompletedBy = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalRestorationCompletedBy);
        }

        [TestMethod]
        public void TestSaveWithFinalPavingBreakOutEightInches()
        {
            var expected = 1;
            _target.FinalPavingBreakOutEightInches = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalPavingBreakOutEightInches);
        }

        [TestMethod]
        public void TestSaveWithFinalPavingBreakOutTenInches()
        {
            var expected = 1;
            _target.FinalPavingBreakOutTenInches = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalPavingBreakOutTenInches);
        }

        [TestMethod]
        public void TestSaveWithFinalSawCutting()
        {
            var expected = 2;
            _target.FinalSawCutting = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalSawCutting);
        }

        [TestMethod]
        public void TestSaveWithFinalPavingSquareFootage()
        {
            var expected = 20;
            _target.FinalPavingSquareFootage = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalPavingSquareFootage);
        }

        [TestMethod]
        public void TestSaveWithDaysToFinalPaveHole()
        {
            var expected = 3;
            _target.DaysToFinalPaveHole = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.DaysToFinalPaveHole);
        }

        [TestMethod]
        public void TestSaveWithTrafficControlCostFinalRestoration()
        {
            var expected = 6;
            _target.TrafficControlCostFinalRestoration = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.TrafficControlCostFinalRestoration);
        }

        [TestMethod]
        public void TestSaveWithDateApproved()
        {
            var expected = DateTime.Today;
            _target.FinalRestorationApprovedAt = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.FinalRestorationApprovedAt);
        }

        [TestMethod]
        public void TestSaveWithDateRejected()
        {
            var expected = DateTime.Today;
            _target.DateRejected = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.DateRejected);
        }

        [TestMethod]
        public void TestSaveWithEightInchStabilizeBaseByCompanyForces()
        {
            var expected = true;
            _target.EightInchStabilizeBaseByCompanyForces = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.EightInchStabilizeBaseByCompanyForces);
        }

        [TestMethod]
        public void TestSaveWithSawCutByCompanyForces()
        {
            var expected = true;
            _target.SawCutByCompanyForces = expected;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(expected, _target.SawCutByCompanyForces);
        }

        [TestMethod]
        public void TestSavingWithoutResponsePriorityAutomaticallySetsToStandard()
        {
            _target.ResponsePriorityID = null;
            _target.ResponsePriority = null;

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
            Assert.AreEqual(RestorationResponsePriorityRepository.Indices.STANDARD,
                _target.ResponsePriorityID);
        }


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
            var restorationType = new RestorationType { Description = "curb" };
            var restorationTypeCost = new RestorationTypeCost { FinalCost = 12 };
            _target.RestorationType = restorationType;
            _target.RestorationTypeCost = restorationTypeCost;
            _target.PartialPavingSquareFootage = 0;
            _target.PartialRestorationActualCost = 10;
            _target.LinearFeetOfCurb = 13;
            Assert.AreEqual(156m, _target.AccrualValue);
        }

        [TestMethod]
        public void TestAccrualValueReturnsExpectedValueWhenPartialSquareFootageIsGreaterThanZero()
        {
            var restorationType = new RestorationType { Description = "curb" };
            var restorationTypeCost = new RestorationTypeCost { FinalCost = 12 };
            _target.RestorationType = restorationType;
            _target.RestorationTypeCost = restorationTypeCost;
            _target.PartialPavingSquareFootage = 2;
            _target.PartialRestorationActualCost = 10;
            _target.LinearFeetOfCurb = 13;
            Assert.AreEqual(24m, _target.AccrualValue);
        }


        //[TestMethod]
        //public void TestAdjustedAccrualReturnsCorrectValueForMeasurementTypeLinearFt()
        //{
        //    var restorationType = new RestorationType { Description = "curb"};
        //    var restorationTypeCost = new RestorationTypeCost { FinalCost = 12 };
        //    _target.RestorationType = restorationType;
        //    _target.RestorationTypeCost = restorationTypeCost;
        //    _target.PartialRestorationActualCost = 300;
        //    _target.TotalAccruedCost = 400;
        //    _target.LinearFeetOfCurb = 20;

        //    var actual = _target.AdjustedAccrual;
            
        //    Assert.AreEqual(180, actual);
        //}

        //[TestMethod]
        //public void TestAdjustedAccrualReturnsCorrectValueForMeasurementTypeSquareFootage()
        //{
        //    var restorationType = new RestorationType { Description = "not currrrbbbb!" };
        //    var restorationTypeCost = new RestorationTypeCost { FinalCost = 12 };
        //    _target.RestorationType = restorationType;
        //    _target.RestorationTypeCost = restorationTypeCost;
        //    _target.PartialRestorationActualCost = 300;
        //    _target.TotalAccruedCost = 400;
        //    _target.PavingSquareFootage = 30;
        //    _target.LinearFeetOfCurb = 0;

        //    var actual = _target.AdjustedAccrual;
            
        //    Assert.AreEqual(270.00m, decimal.Round(actual.Value,2));

        //    ////restoration size
        //    _target.PavingSquareFootage = 0;
        //    _target.SetHiddenFieldValueByName("_restorationSize", null);
        //    Assert.IsNull(_target.AdjustedAccrual);
        //    _target.PavingSquareFootage = 400;

        //    ////total accrued cost
        //    _target.TotalAccruedCost = 0;
        //    Assert.IsNull(_target.AdjustedAccrual);
        //    _target.TotalAccruedCost = 400;

        //    //final cost
        //    _target.RestorationTypeCost.FinalCost = 0;
        //    Assert.IsNull(_target.AdjustedAccrual);
        //    _target.RestorationTypeCost.FinalCost = 12;

        //    //initial actual cost
        //    _target.PartialRestorationActualCost = null;
        //    Assert.IsNull(_target.AdjustedAccrual);
        //    _target.PartialRestorationActualCost = 300;
        //}

        //[TestMethod]
        //public void TestAccrualValueReturnsTotalAccrudedCostIfTotalInitialActualCostEqualsZero()
        //{
        //    var target = new Restoration {
        //        PartialRestorationActualCost = null,
        //        TotalAccruedCost = 1.2m  
        //    };

        //    Assert.AreEqual(target.TotalAccruedCost, target.AccrualValue);

        //    target.PartialRestorationActualCost = 0;

        //    Assert.AreEqual(target.TotalAccruedCost, target.AccrualValue);
        //}

        //[TestMethod]
        //public void TestAccrualValueReturnsAdjustedAccrualIfCostIsNotEqualToZero()
        //{
        //    var restorationType = new RestorationType { Description = "not currrrbbbb!" };
        //    var restorationTypeCost = new RestorationTypeCost { FinalCost = 12 };
        //    _target.RestorationType = restorationType;
        //    _target.RestorationTypeCost = restorationTypeCost;
        //    _target.PartialRestorationActualCost = 300;
        //    _target.TotalAccruedCost = 400;
        //    _target.PavingSquareFootage = 30;
        //    _target.LinearFeetOfCurb = 0;

        //    Assert.AreEqual(_target.AdjustedAccrual, _target.AccrualValue);

        //}
    }

    internal class MockRestorationRepository : MockRepository<Restoration> { }

    internal class TestRestorationBuilder : TestDataBuilder<Restoration>
    {
        #region Private Members

        private RestorationMethod _partialRestorationMethod,
                                  _finalRestorationMethod;
        private RestorationType _restorationType;
        private WorkOrder _workOrder;

        #endregion

        #region Exposed Methods

        public override Restoration Build()
        {
            var obj = new Restoration();
            if (_workOrder != null)
                obj.WorkOrder = _workOrder;
            if (_restorationType != null)
                obj.RestorationType = _restorationType;
            //if (_partialRestorationMethod != null)
            //    obj.PartialRestorationMethod = _partialRestorationMethod;
            //if (_finalRestorationMethod != null)
            //    obj.FinalRestorationMethod = _finalRestorationMethod;
            return obj;
        }

        public TestRestorationBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        public TestRestorationBuilder WithRestorationType(RestorationType restorationType)
        {
            _restorationType = restorationType;
            return this;
        }

        public TestRestorationBuilder WithPartialRestorationMethod(RestorationMethod partialRestorationMethod)
        {
            _partialRestorationMethod = partialRestorationMethod;
            return this;
        }

        public TestRestorationBuilder WithFinalRestorationMethod(RestorationMethod finalRestorationMethod)
        {
            _finalRestorationMethod = finalRestorationMethod;
            return this;
        }

        #endregion
    }
}
