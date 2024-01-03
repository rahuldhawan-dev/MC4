using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for DropDownListEditTemplateTest
    /// </summary>
    [TestClass]
    public class DropDownListEditTemplateTest : EventFiringTestClass
    {
        #region Private Members

        private IEntityBoundField _parent;
        private IDropDownList _dropDownList;
        private DropDownListEditTemplate _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks
               .DynamicMock(out _parent)
               .DynamicMock(out _dropDownList);
            _target =
                new DropDownListEditTemplateBuilder().WithParent(_parent).WithBoundDropDownList(_dropDownList);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestExtractValuesCreatesDictionaryFromParentFieldAndDropDownListSelectedValue()
        {
            const string expectedValueField = "foo", expectedValue = "bar";
            SetupResult.For(_parent.SelectedValueField).Return(expectedValueField);
            SetupResult.For(_dropDownList.SelectedValue).Return(expectedValue);

            _mocks.ReplayAll();

            var result = _target.ExtractValues(null);

            Assert.IsTrue(result.Contains(expectedValueField));
            Assert.AreEqual(expectedValue, result[expectedValueField]);
        }

        [TestMethod]
        public void TestInstantiateInSetsDataBoundHandlerOfDropDownList()
        {
            _dropDownList = new MockDropDownList();
            _target =
                new DropDownListEditTemplateBuilder().WithParent(_parent).WithBoundDropDownList(_dropDownList);

            _mocks.ReplayAll();

            _target.InstantiateIn(new Control());

            Assert.IsTrue(((MockDropDownList)_dropDownList).DataBoundIsSet);
        }

        [TestMethod]
        public void TestInstantiateInSetsDataSourceOfDropDownListToBoundObjectDataSource()
        {
            var objectDataSource = new ObjectDataSource();
            _dropDownList = new MockDropDownList();
            _target =
                new DropDownListEditTemplateBuilder().WithParent(_parent).WithBoundDropDownList(_dropDownList)
                                                     .WithBoundObjectDataSource(objectDataSource);

            _mocks.ReplayAll();

            _target.InstantiateIn(new Control());

            Assert.AreSame(objectDataSource, _dropDownList.DataSource);
        }

        [TestMethod]
        public void TestInstantiateInAddsDataSourceAndDropDownListToContainter()
        {
            var container = new Control();
            var objectDataSource = new ObjectDataSource();
            _dropDownList = new MockDropDownList();
            _target =
                new DropDownListEditTemplateBuilder().WithParent(_parent).WithBoundDropDownList(_dropDownList)
                                                     .WithBoundObjectDataSource(objectDataSource);

            _mocks.ReplayAll();

            _target.InstantiateIn(container);

            Assert.IsTrue(container.Controls.Contains(objectDataSource));
            Assert.IsTrue(container.Controls.Contains((Control)_dropDownList));
        }

        [TestMethod]
        public void TestInstantiateInAddsRequiredFieldValidatorIfParentRequiredPropertyIsTrue()
        {
            SetupResult.For(_parent.Required).Return(true);
            var container = new Control();
            var validator = new RequiredFieldValidator();
            var objectDataSource = new ObjectDataSource();
            _dropDownList = new MockDropDownList();
            _target =
                new DropDownListEditTemplateBuilder().WithParent(_parent).WithBoundDropDownList(_dropDownList)
                                                     .WithBoundObjectDataSource(objectDataSource)
                                                     .WithValidator(validator);

            _mocks.ReplayAll();

            _target.InstantiateIn(container);

            Assert.IsTrue(container.Controls.Contains(validator));
        }

        [TestMethod]
        public void TestInstantiateInDoesNotAddRequiredFieldValidatorIfParentRequiredPropertyIsFalse()
        {
            SetupResult.For(_parent.Required).Return(false);
            var container = new Control();
            var validator = new RequiredFieldValidator();
            var objectDataSource = new ObjectDataSource();
            _dropDownList = new MockDropDownList();
            _target =
                new DropDownListEditTemplateBuilder().WithParent(_parent).WithBoundDropDownList(_dropDownList)
                                                     .WithBoundObjectDataSource(objectDataSource)
                                                     .WithValidator(validator);

            _mocks.ReplayAll();

            _target.InstantiateIn(container);

            Assert.IsFalse(container.Controls.Contains(validator));
        }

        [TestMethod]
        public void TestBuildsBoundDropDownListWithProperValues()
        {
            const string expectedDataTextField = "Foo", expectedDataValueField = "Bar";
            SetupResult.For(_parent.DataTextField).Return(expectedDataTextField);
            SetupResult.For(_parent.DataValueField).Return(expectedDataValueField);
            SetupResult.For(_parent.DataField).Return("Foobar");
            var objectDataSource = new ObjectDataSource();
            _target =
                new DropDownListEditTemplateBuilder().WithParent(_parent).WithBoundObjectDataSource(objectDataSource);

            _mocks.ReplayAll();

            var dropDownList = _target.BoundDropDownList;

            Assert.AreEqual(expectedDataTextField, dropDownList.DataTextField);
            Assert.AreEqual(expectedDataValueField, dropDownList.DataValueField);
            Assert.AreSame(objectDataSource, dropDownList.DataSource);
            Assert.IsTrue(dropDownList.AppendDataBoundItems);
            Assert.AreEqual(_target.ControlID, dropDownList.ID);
        }

        [TestMethod]
        public void TestBuildsRequiredFieldValidatorWithProperValues()
        {
            var expectedText =
                typeof(DropDownListEditTemplate).GetField("REQUIRED_TEXT",
                    BindingFlags.Static |
                    BindingFlags.NonPublic).GetValue(null);
            SetupResult.For(_parent.DataField).Return("Foobar");

            _mocks.ReplayAll();

            var validator = _target.Validator;

            Assert.AreEqual(expectedText, validator.Text);
            Assert.AreEqual(_target.ControlID, validator.ControlToValidate);
        }

        [TestMethod]
        public void TestBuildsBoundObjectDataSourceWithProperValues()
        {
            const string expectedTypeName = "Foo", expectedSelectMethod = "Bar";
            SetupResult.For(_parent.TypeName).Return(expectedTypeName);
            SetupResult.For(_parent.SelectMethod).Return(expectedSelectMethod);

            _mocks.ReplayAll();

            var objectDataSource = _target.BoundObjectDataSource;

            Assert.AreEqual(expectedTypeName, objectDataSource.TypeName);
            Assert.AreEqual(expectedSelectMethod, objectDataSource.SelectMethod);
        }

        [TestMethod]
        public void TestBuildsControlIDPropertyWithProperValues()
        {
            var dataField = "Foo";
            var idFormat =
                typeof(DropDownListEditTemplate).GetField("CONTROL_ID_FORMAT",
                    BindingFlags.Static |
                    BindingFlags.NonPublic).GetValue(null).ToString();
            SetupResult.For(_parent.DataField).Return(dataField);
            var expectedID = String.Format(idFormat, dataField);

            _mocks.ReplayAll();

            Assert.AreEqual(expectedID, _target.ControlID);
        }
    }

    internal class DropDownListEditTemplateBuilder : TestDataBuilder<DropDownListEditTemplate>
    {
        #region Constants

        internal const string PARENT_DATA_FIELD_NAME = "Foo";

        private static readonly Type _targetType =
            typeof(DropDownListEditTemplate);

        #endregion

        #region Private Members

        private IDropDownList _dropDownList;

        private IEntityBoundField _parent = new EntityBoundField {
            DataField = PARENT_DATA_FIELD_NAME
        };

        private ObjectDataSource _objectDataSource;
        private RequiredFieldValidator _requiredFieldValidator;

        #endregion

        #region Private Methods

        private void SetDropDownList(DropDownListEditTemplate template)
        {
            var ddlAccessor =
                _targetType.GetField(
                    "_boundDropDownList",
                    BindingFlags.Instance | BindingFlags.NonPublic);
            ddlAccessor.SetValue(template, _dropDownList);
        }

        private void SetObjectDataSource(DropDownListEditTemplate template)
        {
            var odsAccessor = _targetType.GetField("_boundObjectDataSource",
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            odsAccessor.SetValue(template, _objectDataSource);
        }

        private void SetValidator(DropDownListEditTemplate template)
        {
            var valAccessor = _targetType.GetField("_validator",
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            valAccessor.SetValue(template, _requiredFieldValidator);
        }

        #endregion

        #region Exposed Methods

        public override DropDownListEditTemplate Build()
        {
            var template = new DropDownListEditTemplate(_parent);
            if (_dropDownList != null)
                SetDropDownList(template);
            if (_objectDataSource != null)
                SetObjectDataSource(template);
            if (_requiredFieldValidator != null)
                SetValidator(template);
            return template;
        }

        public DropDownListEditTemplateBuilder WithParent(IEntityBoundField newParent)
        {
            _parent = newParent;
            return this;
        }

        public DropDownListEditTemplateBuilder WithBoundDropDownList(IDropDownList dropDownList)
        {
            _dropDownList = dropDownList;
            return this;
        }

        public DropDownListEditTemplateBuilder WithBoundObjectDataSource(ObjectDataSource objectDataSource)
        {
            _objectDataSource = objectDataSource;
            return this;
        }

        public DropDownListEditTemplateBuilder WithValidator(RequiredFieldValidator validator)
        {
            _requiredFieldValidator = validator;
            return this;
        }

        #endregion
    }

    internal class MockDropDownList : Control, IDropDownList
    {
        #region Properties

        public bool AppendDataBoundItems { get; set; }
        public string DataTextField { get; set; }
        public string DataValueField { get; set; }
        public object DataSource { get; set; }

        public string[] DataKeyNames
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string DataMember
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string DataSourceID
        {
            set { throw new NotImplementedException(); }
        }

        string IDataBoundControl.DataSourceID { get; set; }

        public IDataSource DataSourceObject
        {
            get { throw new NotImplementedException(); }
        }

        public bool DataBoundIsSet
        {
            get { return DataBound != null; }
        }

        public int SelectedIndex
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object SelectedDataKey
        {
            get { throw new NotImplementedException(); }
        }

        public string SelectedValue
        {
            get { throw new NotImplementedException(); }
        }

        public ListItemCollection Items
        {
            get { throw new NotImplementedException(); }
        }

        public ListItem SelectedItem
        {
            get { throw new NotImplementedException(); }
        }

        public object DataItem
        {
            get
            {
                IDataItemContainer dataItemContainer = (IDataItemContainer)DataItemContainer;
                return dataItemContainer != null ? dataItemContainer.DataItem : null;
            }
        }

        public SortDirection SortDirection
        {
            get { return SortDirection.Ascending; }
        }

        public string SortExpression
        {
            get { return null; }
        }

        public int PageSize
        {
            get { return -1; }
            set { }
        }

        public int PageIndex
        {
            get { return -1; }
            set { }
        }

        #endregion

        #region Events

        public event EventHandler DataBound;

        #endregion

        #region Exposed Methods

        public TReturn GetSelectedValue<TReturn>(Func<ListItem, TReturn> fn)
        {
            throw new NotImplementedException();
        }

        public int? GetSelectedValue()
        {
            throw new NotImplementedException();
        }

        public bool? GetBooleanValue()
        {
            throw new NotImplementedException();
        }

        public string GetStringValue()
        {
            throw new NotImplementedException();
        }

        public void SetSortDirection(SortDirection direction) { }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
