using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Views.ContractorCrewAssignments;
using LINQTo271.Views.CrewAssignments;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Views.CrewAssignments;

namespace _271ObjectTests.Tests.Unit.Views.ContractorCrewAssignments
{
    [TestClass]
    public class ContractorCrewAssignmentsSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestContractorCrewAssignmentsSearchView _target;
        private IResponse _iResponse;
        private IDropDownList ddlCrew;
        private ITextBox ccDate;
        private ISecurityService _securityService;
        
        #endregion
        
        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _iResponse)
                .DynamicMock(out ddlCrew)
                .DynamicMock(out ccDate)
                .DynamicMock(out _securityService);

            _target =
                new TestContractorCrewAssignmentsSearchViewBuilder()
                    .WithResponse(_iResponse)
                    .WithDDLCrew(ddlCrew)
                    .WithCCDate(ccDate)
                    .WithSecurityService(_securityService);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestCrewIDReturnsSelectedCrewID()
        {
            var expected = 1;
            using (_mocks.Record())
            {
                SetupResult.For(ddlCrew.GetSelectedValue()).Return(expected);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.CrewID);
            }
        }

        [TestMethod]
        public void TestDateReturnsTodayIfNull()
        {
            using (_mocks.Record())
            {

            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(DateTime.Today, _target.Date);
            }
        }

        [TestMethod]
        public void TestDateReturnsValueIfSet()
        {
            var date = DateTime.Now.AddDays(1.0);
            using (_mocks.Record())
            {
                SetupResult.For(ccDate.TryGetDateTimeValue()).Return(date);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(date, _target.Date);
            }
        }

        [TestMethod]
        public void TestSetDateSetsCorrectValue()
        {
            var expected = new DateTime(2010, 10, 10);
            using (_mocks.Record())
            {
                ccDate.Text = expected.ToString("MM-dd-yyyy");
            }
            using (_mocks.Playback())
            {
                _target.Date = expected;
            }
        }

        [TestMethod]
        public void TestSecurityServicePropertyReturnsMockedValueIfPresent()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_securityService,
                _target.GetPropertyValueByName("SecurityService"));
        }

        [TestMethod]
        public void TestSecurityServicePropertyGetsSecurityServiceSingletonInstance()
        {
            _mocks.ReplayAll();

            _target.SetSecurityService(null);

            Assert.AreSame(SecurityService.Instance,
                _target.GetPropertyValueByName("SecurityService"));
        }

        #endregion
        
        [TestMethod]
        public void TestBaseExpressionFiltersOutNonContractorCrews()
        {
            _mocks.ReplayAll();

            var contractorCrew = new Crew
            {
                ContractorID = 50
            };
            var caContractor = new CrewAssignment
            {
                Crew = contractorCrew,
            };
            var caNoContractor = new CrewAssignment
            {
                Crew = new Crew()
            };
            IEnumerable<CrewAssignment> crewAssignments = new[] {
                caContractor, caNoContractor
            };

            var result =
                crewAssignments.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsTrue(result.Contains(caContractor));
            Assert.IsFalse(result.Contains(caNoContractor));
        }
    }

    internal class TestContractorCrewAssignmentsSearchViewBuilder : TestDataBuilder<TestContractorCrewAssignmentsSearchView>
    {
        #region Private Members

        private IResponse _iResponse;
        private IDropDownList _iDdlCrew;
        private ITextBox _iCcDate;
        private ISecurityService _securityService;

        #endregion

        #region Exposed Methods

        public override TestContractorCrewAssignmentsSearchView Build()
        {
            var obj = new TestContractorCrewAssignmentsSearchView();
            if (_iResponse != null)
                obj.SetIResponse(_iResponse);
            if (_iDdlCrew != null)
                obj.SetDDLCrew(_iDdlCrew);
            if (_iCcDate != null)
                obj.SetCCDate(_iCcDate);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestContractorCrewAssignmentsSearchViewBuilder WithResponse(IResponse iResponse)
        {
            _iResponse = iResponse;
            return this;
        }

        public TestContractorCrewAssignmentsSearchViewBuilder WithDDLCrew(IDropDownList ddlCrew)
        {
            _iDdlCrew = ddlCrew;
            return this;
        }

        public TestContractorCrewAssignmentsSearchViewBuilder WithCCDate(ITextBox ccDate)
        {
            _iCcDate = ccDate;
            return this;
        }

        public TestContractorCrewAssignmentsSearchViewBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentsSearchView : ContractorCrewAssignmentsSearchView
    {
        public void SetIResponse(IResponse iResponse)
        {
            _iResponse = iResponse;
        }
        
        public void SetDDLCrew(IDropDownList ddl)
        {
            ddlCrew = ddl;
        }

        public void SetCCDate(ITextBox tb)
        {
            ccDate = tb;
        }
        
        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }
    }
}

