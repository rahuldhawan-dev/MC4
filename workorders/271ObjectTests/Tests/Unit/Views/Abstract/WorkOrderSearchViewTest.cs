using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Common;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrderSearchViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private IBaseWorkOrderSearch baseSearch;

        private TestWorkOrderSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out baseSearch);

            _target = new TestWorkOrderSearchViewBuilder()
                .WithBaseSearch(baseSearch);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestWorkOrderNumberPropertyReturnsWorkOrderNumberFromBaseSearchControl()
        {
            var expected = 1;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.WorkOrderNumber).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.WorkOrderNumber);
            }
        }

        [TestMethod]
        public void TestTownIDPropertyReturnsTownIDFromBaseSearchControl()
        {
            var expected = 3;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.TownID).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.TownID);
            }
        }

        [TestMethod]
        public void TestTownSectionIDPropertyReturnsTownSectionIDFromBaseSearchControl()
        {
            var expected = 4;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.TownSectionID).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.TownSectionID);
            }
        }

        [TestMethod]
        public void TestStreetIDPropertyReturnsStreetIDFromBaseSearchControl()
        {
            var expected = 5;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.StreetID).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StreetID);
            }
        }

        [TestMethod]
        public void TestNearestCrossStreetIDPropertyReturnsNearestCrossStreetIDFromBaseSearchControl()
        {
            var expected = 6;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.NearestCrossStreetID).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.NearestCrossStreetID);
            }
        }

        [TestMethod]
        public void TestAssetTypeIDPropertyReturnsAssetTypeIDFromBaseSearchControl()
        {
            var expected = 7;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.AssetTypeID).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.AssetTypeID);
            }
        }

        [TestMethod]
        public void TestDescriptionOfWorkIDsPropertyReturnsDescriptionOfWorkIDsFromBaseSearchControl()
        {
            var expected = new List<int> {
                7, 8, 9
            };

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.DescriptionOfWorkIDs).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DescriptionOfWorkIDs);
            }
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestGeneratedExpressionFiltersByTownSectionWhenTownSectionSelected()
        {
            TownSection selectedTownSection = new TownSection
            {
                TownSectionID = 1
            },
                        otherTownSection = new TownSection
                        {
                            TownSectionID = 2
                        };


            WorkOrder withSelectedTownSection =
                new TestWorkOrderBuilder().WithTownSection(selectedTownSection),
                      withOtherTownSection =
                          new TestWorkOrderBuilder().WithTownSection(otherTownSection);

            Func<WorkOrder, bool> expr;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.TownSectionID).Return(selectedTownSection.TownSectionID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(withSelectedTownSection));
                Assert.IsFalse(expr(withOtherTownSection));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByStreetWhenStreetSelected()
        {
            Street selectedStreet = new Street
            {
                StreetID = 1
            },
                        otherStreet = new Street
                        {
                            StreetID = 2
                        };


            WorkOrder withSelectedStreet =
                new TestWorkOrderBuilder().WithStreet(selectedStreet),
                      withOtherStreet =
                          new TestWorkOrderBuilder().WithStreet(otherStreet);

            Func<WorkOrder, bool> expr;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.StreetID).Return(selectedStreet.StreetID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(withSelectedStreet));
                Assert.IsFalse(expr(withOtherStreet));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByNearestCrossStreetWhenNearestCrossStreetSelected()
        {
            Street selectedStreet = new Street
            {
                StreetID = 1
            },
                        otherStreet = new Street
                        {
                            StreetID = 2
                        };


            WorkOrder withSelectedStreet =
                new TestWorkOrderBuilder().WithNearestCrossStreet((selectedStreet)),
                      withOtherStreet =
                          new TestWorkOrderBuilder().WithNearestCrossStreet((otherStreet));

            Func<WorkOrder, bool> expr;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.NearestCrossStreetID).Return(selectedStreet.StreetID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(withSelectedStreet));
                Assert.IsFalse(expr(withOtherStreet));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByWorkDescriptionWhenWorkDescriptionSelected()
        {
            var expectedWorkDescriptionID = new List<int> {
                1
            };

            WorkDescription selectedWorkDescription = new WorkDescription {
                                WorkDescriptionID = expectedWorkDescriptionID[0]
                            },
                            otherWorkDescription = new WorkDescription {
                                WorkDescriptionID = 2
                            };


            WorkOrder withSelectedWorkDescription =
                          new TestWorkOrderBuilder()
                              .WithWorkDescription(selectedWorkDescription),
                      withOtherWorkDescription =
                          new TestWorkOrderBuilder()
                              .WithWorkDescription(otherWorkDescription);
            Func<WorkOrder, bool> expr;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.DescriptionOfWorkIDs).Return(expectedWorkDescriptionID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(withSelectedWorkDescription));
                Assert.IsFalse(expr(withOtherWorkDescription));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByStreetNumberWhenStreetNumberSelected()
        {
            const string selectedStreetNumber = "1",
                         otherStreetNumber = "2";

            WorkOrder withSelectedStreetNumber =
                        new TestWorkOrderBuilder()
                            .WithStreetNumber(selectedStreetNumber),
                      withOtherStreetNumber =
                        new TestWorkOrderBuilder()
                            .WithStreetNumber(otherStreetNumber);

            Func<WorkOrder, bool> expr;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.StreetNumber).Return(selectedStreetNumber);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(withSelectedStreetNumber));
                Assert.IsFalse(expr(withOtherStreetNumber));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByTownWhenTownValueSelected()
        {
            Town selectedTown = new Town {
                     TownID = 1
                 },
                 otherTown = new Town {
                     TownID = 2
                 };

            WorkOrder withSelectedTown =
                          new TestWorkOrderBuilder().WithTown(selectedTown),
                      withOtherTown =
                          new TestWorkOrderBuilder().WithTown(otherTown);

            Func<WorkOrder, bool> expr;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.TownID).Return(selectedTown.TownID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(withSelectedTown));
                Assert.IsFalse(expr(withOtherTown));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByOrderNumberWhenTxtOrderNumberHasValue()
        {
            WorkOrder wo1 = new WorkOrder { WorkOrderID = 1 },
                      wo2 = new WorkOrder { WorkOrderID = 2 },
                      wo3 = new WorkOrder { WorkOrderID = 3 };
            CrewAssignment ca1 = new CrewAssignment
            {
                AssignedFor = DateTime.Today.Date,
                WorkOrder = wo1
            },
                           ca2 = new CrewAssignment
                           {
                               AssignedFor = DateTime.Today.Date,
                               WorkOrder = wo2
                           },
                           ca3 = new CrewAssignment
                           {
                               AssignedFor = DateTime.Today.Date,
                               WorkOrder = wo3
                           };
            IEnumerable<WorkOrder> orders = new[] {
                wo1, wo2, wo3
            };

            IEnumerable<WorkOrder> result;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.WorkOrderNumber).Return(2);
            }

            using (_mocks.Playback())
            {
                result =
                    orders.Where(_target.GenerateExpression().Compile()).ToList();
            }

            Assert.IsFalse(result.Contains(wo1));
            Assert.IsTrue(result.Contains(wo2));
            Assert.IsFalse(result.Contains(wo3));
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByAssetTypeWhenAssetTypeValueSelected()
        {
            AssetType valveType = new TestAssetTypeBuilder<Valve>(),
                      hydrantType = new TestAssetTypeBuilder<Hydrant>();
            WorkOrder assetIsValve = new TestWorkOrderBuilder()
                          .WithAssetType(valveType),
                      assetIsHydrant = new TestWorkOrderBuilder()
                          .WithAssetType(hydrantType);
            Func<WorkOrder, bool> expr;

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.AssetTypeID)
                    .Return(valveType.AssetTypeID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsTrue(expr(assetIsValve));
                Assert.IsFalse(expr(assetIsHydrant));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(baseSearch.AssetTypeID)
                    .Return(hydrantType.AssetTypeID);
            }

            using (_mocks.Playback())
            {
                expr = _target.GenerateExpression().Compile();

                Assert.IsFalse(expr(assetIsValve));
                Assert.IsTrue(expr(assetIsHydrant));
            }
        }

        [TestMethod]
        public void TestDisplaySearchErrorCallsDisplaySearchErrorOnBaseSearchControl()
        {
            var expected = "Hey, don't do that!";

            using (_mocks.Record())
            {
                baseSearch.DisplaySearchError(expected);
            }

            using (_mocks.Playback())
            {
                _target.DisplaySearchError(expected);
            }
        }

        #endregion
    }

    internal class TestWorkOrderSearchViewBuilder : TestDataBuilder<TestWorkOrderSearchView>
    {
        #region Private Members

        private IBaseWorkOrderSearch _baseSearch;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderSearchView Build()
        {
            var obj = new TestWorkOrderSearchView();
            if (_baseSearch != null)
                obj.SetBaseSearchControl(_baseSearch);
            return obj;
        }

        public TestWorkOrderSearchViewBuilder WithBaseSearch(IBaseWorkOrderSearch baseSearch)
        {
            _baseSearch = baseSearch;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderSearchView : WorkOrderSearchView
    {
        #region Properties

        public override WorkOrderPhase Phase
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Private Methods

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            // noop
        }

        #endregion

        #region Exposed Methods

        public void SetBaseSearchControl(IBaseWorkOrderSearch control)
        {
            baseSearch = control;
        }

        #endregion
    }
}
