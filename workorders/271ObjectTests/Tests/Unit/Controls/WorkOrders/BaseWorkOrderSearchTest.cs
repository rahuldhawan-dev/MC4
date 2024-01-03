using System;
using System.Collections.Generic;
using LINQTo271.Controls.WorkOrders;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for BaseWorkOrderSearchTest.
    /// </summary>
    [TestClass]
    public class BaseWorkOrderSearchTest : EventFiringTestClass
    {
        #region Private Members

        private ILabel lblError;
        protected ITableRow trWorkOrderNumber,
                            trOldWorkOrderNumber,
                            trTown,
                            trTownSection,
                            trStreetNumber,
                            trApartmentAddtl,
                            trStreet,
                            trNearestCrossStreet,
                            trAssetType,
                            trDescriptionOfWork;

        private IDropDownList ddlTown,
                              ddlTownSection,
                              ddlStreet,
                              ddlNearestCrossStreet,
                              ddlAssetType,
                              ddlOperatingCenter;
        private ITextBox txtWorkOrderNumber,
                           txtOldWorkOrderNumber,
                           txtStreetNumber,
                           txtApartmentAddtl;
        private IListBox lstDescriptionOfWork;
        
        private ISecurityService _securityService;

        private TestBaseWorkOrderSearch _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out ddlTown)
                .DynamicMock(out ddlTownSection)
                .DynamicMock(out ddlStreet)
                .DynamicMock(out ddlNearestCrossStreet)
                .DynamicMock(out ddlAssetType)
                .DynamicMock(out txtWorkOrderNumber)
                .DynamicMock(out txtOldWorkOrderNumber)
                .DynamicMock(out txtStreetNumber)
                .DynamicMock(out txtApartmentAddtl)
                .DynamicMock(out lstDescriptionOfWork)
                .DynamicMock(out lblError)
                .DynamicMock(out trWorkOrderNumber)
                .DynamicMock(out trOldWorkOrderNumber)
                .DynamicMock(out trTown)
                .DynamicMock(out trTownSection)
                .DynamicMock(out trStreetNumber)
                .DynamicMock(out trApartmentAddtl)
                .DynamicMock(out trStreet)
                .DynamicMock(out trNearestCrossStreet)
                .DynamicMock(out trAssetType)
                .DynamicMock(out trDescriptionOfWork)
                .DynamicMock(out ddlOperatingCenter)
                .DynamicMock(out _securityService);


            _target = new TestBaseWorkOrderSearchBuilder()
                .WithDDLTown(ddlTown)
                .WithDDLTownSection(ddlTownSection)
                .WithDDLStreet(ddlStreet)
                .WithDDLNearestCrossStreet(ddlNearestCrossStreet)
                .WithDDLAssetType(ddlAssetType)
                .WithTXTWorkOrderNumber(txtWorkOrderNumber)
                .WithTXTOldWorkOrderNumber(txtOldWorkOrderNumber)
                .WithTXTStreetNumber(txtStreetNumber)
                .WithTXTApartmentAddtl(txtApartmentAddtl)
                .WithLSTDescriptionOfWork(lstDescriptionOfWork)
                .WithErrorLabel(lblError)
                .WithTRWorkOrderNumber(trWorkOrderNumber)
                .WithTROldWorkOrderNumber(trOldWorkOrderNumber)
                .WithTRTown(trTown)
                .WithTRTownSection(trTownSection)
                .WithTRStreetNumber(trStreetNumber)
                .WithTRApartmentAddtl(trApartmentAddtl)
                .WithTRStreet(trStreet)
                .WithTRNearestCrossStreet(trNearestCrossStreet)
                .WithTRAssetType(trAssetType)
                .WithTRDescriptionOfWork(trDescriptionOfWork)
                .WithDDLOperatingCenter(ddlOperatingCenter)
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
        public void TestSecurityServicePropertyReturnsMockedValueIfPresent()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_securityService,
                _target.SecurityService);
        }

        [TestMethod]
        public void TestSecurityServicePropertyGetsSecurityServiceSingletonInstance()
        {
            _mocks.ReplayAll();

            _target.SetSecurityService(null);

            Assert.AreSame(SecurityService.Instance,
                _target.SecurityService);
        }

        [TestMethod]
        public void TestWorkOrderNumberPropertyReturnsIntegerValueIfEntered()
        {
            var expected = 1;

            using (_mocks.Record())
            {
                SetupResult.For(txtWorkOrderNumber.TryGetIntValue()).Return(
                    expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.WorkOrderNumber);
            }
        }

        [TestMethod]
        public void TestWorkOrderNumberPropertyReturnsNullIfNoValueEntered()
        {
            using (_mocks.Record())
            {
                SetupResult.For(txtWorkOrderNumber.TryGetIntValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.WorkOrderNumber);
            }
        }
        
        [TestMethod]
        public void TestStreetNumberReturnsStringValueOfTXTStreetNumber()
        {
            var expected = "123";
            
            using (_mocks.Record())
            {
                SetupResult.For(txtStreetNumber.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StreetNumber);
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtStreetNumber.Text).Return(string.Empty);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(string.Empty, _target.StreetNumber);
            }
        }

        [TestMethod]
        public void TestApartmentAddtlReturnsStringValueOfTXTSupplementNo()
        {
            var expected = "123";
            
            using (_mocks.Record())
            {
                SetupResult.For(txtApartmentAddtl.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.ApartmentAddtl);
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(txtApartmentAddtl.Text).Return(string.Empty);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(string.Empty, _target.ApartmentAddtl);
            }
        }

        [TestMethod]
        public void TestTownIDPropertyReturnsSelectedValueOfDDLTown()
        {
            var expected = 1;
            using (_mocks.Record())
            {
                SetupResult.For(ddlTown.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.TownID);
            }
        }

        [TestMethod]
        public void TestTownIDPropertyReturnsNullWhenNoItemSelectedForDDLTown()
        {
            using (_mocks.Record())
            {
                SetupResult.For(ddlTown.GetSelectedValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.TownID);
            }
        }

        [TestMethod]
        public void TestTownSectionIDPropertyReturnsSelectedValueOfDDLTownSection()
        {
            var expected = 1;
            using (_mocks.Record())
            {
                SetupResult.For(ddlTownSection.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.TownSectionID);
            }
        }

        [TestMethod]
        public void TestTownSectionIDPropertyReturnsNullWhenNoItemSelectedForDDLTownSection()
        {
            using (_mocks.Record())
            {
                SetupResult.For(ddlTownSection.GetSelectedValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.TownSectionID);
            }
        }

        [TestMethod]
        public void TestStreetIDPropertyReturnsSelectedValueOfDDLStreet()
        {
            var expected = 1;
            using (_mocks.Record())
            {
                SetupResult.For(ddlStreet.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StreetID);
            }
        }

        [TestMethod]
        public void TestStreetIDPropertyReturnsNullWhenNoItemSelectedForDDLStreet()
        {
            using (_mocks.Record())
            {
                SetupResult.For(ddlStreet.GetSelectedValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.StreetID);
            }
        }

        [TestMethod]
        public void TestNearestCrossStreetIDPropertyReturnsSelectedValueOfDDLNearestCrossStreet()
        {
            var expected = 1;
            using (_mocks.Record())
            {
                SetupResult.For(ddlNearestCrossStreet.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.NearestCrossStreetID);
            }
        }

        [TestMethod]
        public void TestNearestCrossStreetIDPropertyReturnsNullWhenNoItemSelectedForDDLNearestCrossStreet()
        {
            using (_mocks.Record())
            {
                SetupResult.For(ddlNearestCrossStreet.GetSelectedValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.NearestCrossStreetID);
            }
        }

        [TestMethod]
        public void TestAssetTypeIDPropertyReturnsSelectedValueOfDDLAssetType()
        {
            var expected = 1;
            using (_mocks.Record())
            {
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.AssetTypeID);
            }
        }

        [TestMethod]
        public void TestOperatingCenterIDPropertyReturnsSelectedValueofDDLOperatingCenter()
        {
            var expected = 1;
            using(_mocks.Record())
            {
                SetupResult.For(ddlOperatingCenter.GetSelectedValue()).Return(expected);
            }

            using(_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.OperatingCenterID);
            }
        }


        [TestMethod]
        public void TestAssetTypeIDPropertyReturnsNullWhenNoItemSelectedForDDLAssetType()
        {
            using (_mocks.Record())
            {
                SetupResult.For(ddlAssetType.GetSelectedValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.AssetTypeID);
            }
        }

        [TestMethod]
        public void TestDescriptionOfWorkIDsPropertyReturnsSelectedValuesFromLSTDescriptionOfWork()
        {
            var expected = new List<int> {
                1, 2, 3
            };
            using (_mocks.Record())
            {
                SetupResult.For(lstDescriptionOfWork.GetSelectedValues())
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DescriptionOfWorkIDs);
            }
        }

        [TestMethod]
        public void TestDescriptionOfWorkIDsPropertyReturnsNullWhenNoItemsSelectedForLSTDescriptionOfWork()
        {
            using (_mocks.Record())
            {
                SetupResult.For(lstDescriptionOfWork.GetSelectedValues())
                    .Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.DescriptionOfWorkIDs);
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadHidesAssetTypeRowIfShowAssetTypeSetToFalse()
        {
            _target.ShowAssetType = false;

            using (_mocks.Record())
            {
                trAssetType.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadHidesDescriptionOfWorkRowIfShowDescriptionOfWorkSetToFalse()
        {
            _target.ShowDescriptionOfWork = false;

            using (_mocks.Record())
            {
                trDescriptionOfWork.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadHidesNearestCrossStreetRowIfShowNearestCrossStreetSetToFalse()
        {
            _target.ShowNearestCrossStreet = false;

            using (_mocks.Record())
            {
                trNearestCrossStreet.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }
        
        [TestMethod]
        public void TestPageLoadHidesStreetRowIfShowStreetSetToFalse()
        {
            _target.ShowStreet = false;

            using (_mocks.Record())
            {
                trStreet.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadHidesStreetNumberRowIfShowStreetNumberSetToFalse()
        {
            _target.ShowStreetNumber = false;

            using (_mocks.Record())
            {
                trStreetNumber.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadHidesTownRowIfShowTownSetToFalse()
        {
            _target.ShowTown = false;

            using (_mocks.Record())
            {
                trTown.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadHidesTownSectionRowIfShowTownSectionSetToFalse()
        {
            _target.ShowTownSection = false;

            using (_mocks.Record())
            {
                trTownSection.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadHidesWorkOrderNumberRowIfShowWorkOrderNumberSetToFalse()
        {
            _target.ShowWorkOrderNumber = false;

            using (_mocks.Record())
            {
                trWorkOrderNumber.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestClearsErrorLabelTextOnPageLoad()
        {
            _target = new TestBaseWorkOrderSearchBuilder()
                .WithErrorLabel(lblError);

            using (_mocks.Record())
            {
                lblError.Text = String.Empty;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestDisplaySearchErrorSetsTextOfErrorLabel()
        {
            var expected = "Hey, don't do that!";

            using (_mocks.Record())
            {
                lblError.Text = expected;
            }

            using (_mocks.Playback())
            {
                _target.DisplaySearchError(expected);
            }
        }

        #endregion
    }

    internal class TestBaseWorkOrderSearchBuilder : TestDataBuilder<TestBaseWorkOrderSearch>
    {
        #region Private Members

        protected ITableRow _trWorkOrderNumber,
                            _trOldWorkOrderNumber,
                            _trTown,
                            _trTownSection,
                            _trStreetNumber,
                            _trApartmentAddtl,
                            _trStreet,
                            _trNearestCrossStreet,
                            _trAssetType,
                            _trDescriptionOfWork;
        private ILabel _lblError;
        private ITextBox _txtWorkOrderNumber = new MvpTextBox(),
                         _txtOldWorkOrderNumber = new MvpTextBox(),
                         _txtStreetNumber = new MvpTextBox(),
                         _txtApartmentAddtl = new MvpTextBox();
        private IDropDownList _ddlTown = new MvpDropDownList(),
                              _ddlTownSection = new MvpDropDownList(),
                              _ddlStreet = new MvpDropDownList(),
                              _ddlNearestCrossStreet = new MvpDropDownList(),
                              _ddlAssetType = new MvpDropDownList(),
                              _ddlOperatingCenter = new MvpDropDownList();

        private IListBox _lstDescriptionOfWork = new MvpListBox();

        private ISecurityService _securityService;

        #endregion

        #region Exposed Methods

        public override TestBaseWorkOrderSearch Build()
        {
            var obj = new TestBaseWorkOrderSearch();
            if (_ddlTown != null)
                obj.SetDDLTown(_ddlTown);
            if (_ddlTownSection != null)
                obj.SetDDLTownSection(_ddlTownSection);
            if (_ddlStreet != null)
                obj.SetDDLStreet(_ddlStreet);
            if (_ddlNearestCrossStreet != null)
                obj.SetDDLNearestCrossStreet(_ddlNearestCrossStreet);
            if (_ddlAssetType != null)
                obj.SetDDLAssetType(_ddlAssetType);
            if (_lstDescriptionOfWork != null)
                obj.SetLSTDescriptionOfWork(_lstDescriptionOfWork);
            if (_txtWorkOrderNumber != null)
                obj.SetTXTWorkOrderNumber(_txtWorkOrderNumber);
            if (_txtStreetNumber != null)
                obj.SetTXTStreetNumber(_txtStreetNumber);
            if(_txtApartmentAddtl != null)
                obj.SetTXTApartmentAddtl(_txtApartmentAddtl);
            if (_lblError != null)
                obj.SetErrorLabel(_lblError);
            if (_trWorkOrderNumber != null)
                obj.SetTRWorkOrderNumber(_trWorkOrderNumber);
            if (_trOldWorkOrderNumber != null)
                obj.SetTROldWorkOrderNumber(_trOldWorkOrderNumber);
            if (_trTown != null)
                obj.SetTRTown(_trTown);
            if (_trTownSection != null)
                obj.SetTRTownSection(_trTownSection);
            if (_trStreetNumber != null)
                obj.SetTRStreetNumber(_trStreetNumber);
            if(_trApartmentAddtl != null)
                obj.setTRApartmentAddtl(_trApartmentAddtl);
            if (_trStreet != null)
                obj.SetTRStreet(_trStreet);
            if (_trNearestCrossStreet != null)
                obj.SetTRNearestCrossStreet(_trNearestCrossStreet);
            if (_trAssetType != null)
                obj.SetTRAssetType(_trAssetType);
            if (_trDescriptionOfWork != null)
                obj.SetTRDescriptionOfWork(_trDescriptionOfWork);
            if (_ddlOperatingCenter != null)
                obj.SetDDLOperatingCenter(_ddlOperatingCenter);
            if(_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestBaseWorkOrderSearchBuilder WithDDLTown(IDropDownList ddlTown)
        {
            _ddlTown = ddlTown;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithDDLTownSection(IDropDownList section)
        {
            _ddlTownSection = section;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithDDLStreet(IDropDownList street)
        {
            _ddlStreet = street;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithDDLNearestCrossStreet(IDropDownList street)
        {
            _ddlNearestCrossStreet = street;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithDDLAssetType(IDropDownList type)
        {
            _ddlAssetType = type;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithLSTDescriptionOfWork(IListBox work)
        {
            _lstDescriptionOfWork = work;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTXTWorkOrderNumber(ITextBox number)
        {
            _txtWorkOrderNumber = number;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTXTOldWorkOrderNumber(ITextBox number)
        {
            _txtOldWorkOrderNumber = number;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTXTStreetNumber(ITextBox number)
        {
            _txtStreetNumber = number;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTXTApartmentAddtl(ITextBox number)
        {
            _txtApartmentAddtl = number;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithErrorLabel(ILabel lbl)
        {
            _lblError = lbl;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRWorkOrderNumber(ITableRow trWorkOrderNumber)
        {
            _trWorkOrderNumber = trWorkOrderNumber;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTROldWorkOrderNumber(ITableRow trOldWorkOrderNumber)
        {
            _trOldWorkOrderNumber = trOldWorkOrderNumber;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRTown(ITableRow trTown)
        {
            _trTown = trTown;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRTownSection(ITableRow trTownSection)
        {
            _trTownSection = trTownSection;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRStreetNumber(ITableRow trStreetNumber)
        {
            _trStreetNumber = trStreetNumber;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRApartmentAddtl(ITableRow trApartmentAddtl)
        {
            _trApartmentAddtl = trApartmentAddtl;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRStreet(ITableRow trStreet)
        {
            _trStreet = trStreet;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRNearestCrossStreet(ITableRow trNearestCrossStreet)
        {
            _trNearestCrossStreet = trNearestCrossStreet;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRAssetType(ITableRow trAssetType)
        {
            _trAssetType = trAssetType;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithTRDescriptionOfWork(ITableRow trDescriptionOfWork)
        {
            _trDescriptionOfWork = trDescriptionOfWork;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithDDLOperatingCenter(IDropDownList ddlOperatingCenter)
        {
            _ddlOperatingCenter = ddlOperatingCenter;
            return this;
        }

        public TestBaseWorkOrderSearchBuilder WithSecurityService(ISecurityService service)
        {
            _securityService = service;
            return this;
        }
        #endregion


    }

    internal class TestBaseWorkOrderSearch : BaseWorkOrderSearch
    {
        #region Exposed Methods

        public void SetDDLTown(IDropDownList town)
        {
            ddlTown = town;
        }

        public void SetDDLTownSection(IDropDownList section)
        {
            ddlTownSection = section;
        }

        public void SetDDLStreet(IDropDownList street)
        {
            ddlStreet = street;
        }

        public void SetDDLNearestCrossStreet(IDropDownList street)
        {
            ddlNearestCrossStreet = street;
        }

        public void SetDDLAssetType(IDropDownList type)
        {
            ddlAssetType = type;
        }

        public void SetLSTDescriptionOfWork(IListBox work)
        {
            lstDescriptionOfWork = work;
        }

        public void SetTXTWorkOrderNumber(ITextBox number)
        {
            txtWorkOrderNumber = number;
        }
        
        public void SetTXTStreetNumber(ITextBox number)
        {
            txtStreetNumber = number;
        }

        public void SetTXTApartmentAddtl(ITextBox number)
        {
            txtApartmentAddtl = number;
        }

        public void SetErrorLabel(ILabel lbl)
        {
            lblError = lbl;
        }

        public void SetTRWorkOrderNumber(ITableRow tr)
        {
            trWorkOrderNumber = tr;
        }

        public void SetTROldWorkOrderNumber(ITableRow tr)
        {
            trOldWorkOrderNumber = tr;
        }

        public void SetTRTown(ITableRow tr)
        {
            trTown = tr;
        }

        public void SetTRTownSection(ITableRow tr)
        {
            trTownSection = tr;
        }

        public void SetTRStreet(ITableRow tr)
        {
            trStreet = tr;
        }

        public void SetTRStreetNumber(ITableRow tr)
        {
            trStreetNumber = tr;
        }

        public void setTRApartmentAddtl(ITableRow tr)
        {
            trApartmentAddtl = tr;
        }

        public void SetTRNearestCrossStreet(ITableRow tr)
        {
            trNearestCrossStreet = tr;
        }

        public void SetTRAssetType(ITableRow tr)
        {
            trAssetType = tr;
        }

        public void SetTRDescriptionOfWork(ITableRow tr)
        {
            trDescriptionOfWork = tr;
        }

        public void SetDDLOperatingCenter(IDropDownList ddl)
        {
            ddlOperatingCenter = ddl;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        #endregion
    }
}
