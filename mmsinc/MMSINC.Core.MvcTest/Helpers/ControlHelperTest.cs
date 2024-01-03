using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Validation;
using Moq;
using StructureMap;

// ReSharper disable Mvc.ActionNotResolved, Mvc.ControllerNotResolved, Mvc.AreaNotResolved

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class ControlHelperTest
    {
        #region Consts

        private const string VALIDATION_ATTRIBUTE = "data-val";

        #endregion

        #region Fields

        private ControlHelper<Model> _target;
        private ViewDataDictionary<Model> _viewData;
        private ViewContext _viewContext;
        private FakeMvcApplicationTester _appTester;
        private Model _model;
        private CascadingController _cascadingController;
        private IContainer _container;
        private Mock<IRepository<EntityLookupModel>> _mockEntityRepo;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            // FormBuilder.SecureFormsEnabled = false;
            _container = new Container();
            _appTester = new FakeMvcApplicationTester(_container);
            _appTester.RegisterArea("SomeArea", (context) => {
                context.MapRoute(
                    "SomeArea_default",
                    "SomeArea/{controller}/{action}/{id}",
                    new {action = "Index", id = UrlParameter.Optional}
                );
            });

            // Controllers need to exist for any of the ControlHelper methods that
            // generate urls.
            _cascadingController = new CascadingController();
            _appTester.ControllerFactory.RegisterController(_cascadingController);
            _appTester.ControllerFactory.RegisterController(new FileUploadController());
            _appTester.ControllerFactory.RegisterController(new AutoCompleteController());

            _model = new Model();

            _mockEntityRepo = new Mock<IRepository<EntityLookupModel>>();
            _container.Inject(_mockEntityRepo.Object);
            InitializeForModel(_model);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        private void InitializeForModel(Model model)
        {
            var htmlHelper = _appTester.CreateRequestHandler().CreateHtmlHelper(model);
            _viewContext = htmlHelper.ViewContext;
            var controller = new FileUploadController();
            _appTester.ControllerFactory.RegisterController(controller);
            _viewData = htmlHelper.ViewData;
            _target = new ControlHelper<Model>(_viewContext, _viewData, _appTester.Routes, _container);
        }

        private void InitializeForCascading()
        {
            _viewData["Container"] = _model;
        }

        #endregion

        #region Private Methods

        private void AddModelStateValue(string key, object rawValue, string attemptedValue)
        {
            var state = new ModelState();
            state.Value = new ValueProviderResult(rawValue, attemptedValue,
                System.Globalization.CultureInfo.CurrentCulture);
            _viewData.ModelState.Add(key, state);
        }

        private void ClearModelState()
        {
            _viewData.ModelState.Clear();
        }

        private void AssertBuilderIsUninitialized(ControlBuilder builder)
        {
            Assert.IsNull(builder.Id);
            Assert.IsNull(builder.Name);
            Assert.IsFalse(builder.HtmlAttributes.Any());
        }

        #endregion

        #region Tests

        #region GetModelValue

        [TestMethod]
        public void TestGetModelValueReturnsModelStateValueInsteadOfModelValueIfModelStateValueExists()
        {
            var expectedModelState = "I'm a model state value.";
            var expectedModelValue = "I am just a model value.";
            _model.StringProp = expectedModelValue;

            AddModelStateValue("StringProp", expectedModelState, expectedModelState);

            var mockCache = new Mock<IBuilderData>();
            mockCache.Setup(x => x.Expression).Returns("StringProp");
            mockCache.Setup(x => x.FullExpression).Returns("StringProp");
            var meta = ModelMetadataProviders.Current.GetMetadataForProperty(() => _model.StringProp, _model.GetType(),
                "StringProp");
            mockCache.Setup(x => x.ModelMetadata).Returns(meta);

            var result = _target.GetModelValue(mockCache.Object, typeof(string));
            Assert.AreEqual(expectedModelState, result);

            ClearModelState();

            result = _target.GetModelValue(mockCache.Object, typeof(string));
            Assert.AreEqual(expectedModelValue, result);
        }

        [TestMethod]
        public void TestGetModelValueReturnsNullIfNoModelStateOrValueIsSetOnModel()
        {
            var mockCache = new Mock<IBuilderData>();
            mockCache.Setup(x => x.Expression).Returns("StringProp");
            mockCache.Setup(x => x.FullExpression).Returns("StringProp");
            var meta = ModelMetadataProviders.Current.GetMetadataForProperty(() => _model.StringProp, _model.GetType(),
                "StringProp");
            mockCache.Setup(x => x.ModelMetadata).Returns(meta);
            Assert.IsNull(_target.GetModelValue(mockCache.Object, typeof(string)));
        }

        [TestMethod]
        public void TestGetModelValueReturnsNullIfNoModelStateAndParentModelIsNull()
        {
            InitializeForModel(null);
            var mockCache = new Mock<IBuilderData>();
            mockCache.Setup(x => x.Expression).Returns("StringProp");
            mockCache.Setup(x => x.FullExpression).Returns("StringProp");
            var meta = ModelMetadataProviders.Current.GetMetadataForProperty(() => _model.StringProp, _model.GetType(),
                "StringProp");
            mockCache.Setup(x => x.ModelMetadata).Returns(meta);
            Assert.IsNull(_target.GetModelValue(mockCache.Object, typeof(string)));
        }

        #endregion

        #region InitializeControlBuilder

        [TestMethod]
        public void TestInitializeControlBuilderSetsUnobtrusiveValidationAttributes()
        {
            var cb = _target.InitializeControlBuilder<TestControlBuilder, string>(x => x.RequiredProp).Builder;
            Assert.IsTrue(cb.HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));

            cb = _target.InitializeControlBuilder<TestControlBuilder>("RequiredProp").Builder;
            Assert.IsTrue(cb.HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));
        }

        [TestMethod]
        public void TestInitializeControlBuilderSetsUnobtrusiveValidationAttributesOnNestedProperties()
        {
            var cb = _target.InitializeControlBuilder<TestControlBuilder, string>(x => x.NestedProp.RequiredStringProp)
                            .Builder;
            Assert.IsTrue(cb.HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));

            cb = _target.InitializeControlBuilder<TestControlBuilder>("NestedProp.RequiredStringProp").Builder;
            Assert.IsTrue(cb.HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));
        }

        [TestMethod]
        public void
            TestInitializeControlBuilderSetsUnobtrusiveValidationAttributesOnNestedPropertiesWhenTheModelIsNull()
        {
            InitializeForModel(null);
            var cb = _target.InitializeControlBuilder<TestControlBuilder, string>(x => x.NestedProp.RequiredStringProp)
                            .Builder;
            Assert.IsTrue(cb.HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));

            cb = _target.InitializeControlBuilder<TestControlBuilder>("NestedProp.RequiredStringProp").Builder;
            Assert.IsTrue(cb.HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));
        }

        [TestMethod]
        public void
            TestPasswordReturnsTextBoxBuilderWithoutUnobtrusiveValidationAttributesWhenThePropertyIsNotPartOfTheModel()
        {
            var cb = _target.InitializeControlBuilder<TestControlBuilder>("ImNotReal").Builder;
            Assert.IsFalse(cb.HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));
        }

        [TestMethod]
        public void TestInitializeControlBuilderSetsIdAttribute()
        {
            var cb = _target.InitializeControlBuilder<TestControlBuilder, string>(x => x.NestedProp.RequiredStringProp)
                            .Builder;
            Assert.AreEqual("NestedProp_RequiredStringProp", cb.HtmlAttributes["id"]);

            cb = _target.InitializeControlBuilder<TestControlBuilder>("NestedProp.RequiredStringProp").Builder;
            Assert.AreEqual("NestedProp_RequiredStringProp", cb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestInitializeControlBuilderSetsNameAttribute()
        {
            var cb = _target.InitializeControlBuilder<TestControlBuilder, string>(x => x.NestedProp.RequiredStringProp)
                            .Builder;
            Assert.AreEqual("NestedProp.RequiredStringProp", cb.HtmlAttributes["name"]);

            cb = _target.InitializeControlBuilder<TestControlBuilder>("NestedProp.RequiredStringProp").Builder;
            Assert.AreEqual("NestedProp.RequiredStringProp", cb.HtmlAttributes["name"]);
        }

        #endregion

        #region ActionLink

        [TestMethod]
        public void TestActionLinkReturnsActionLinkWithTitleAndHrefToSpecifiedAction()
        {
            var result = _target.ActionLink("link text", "action", "controller", null, null);

            Assert.AreEqual("link text", result.Text);
            Assert.AreEqual("/controller/action", result.HtmlAttributes["href"]);
        }

        #endregion

        #region AutoComplete

        [TestMethod]
        public void TestAutoCompleteWithoutArgumentsReturnsUninitializedAutoCompleteBuilderInstance()
        {
            var result = _target.AutoComplete();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.ActionParameterName);
            Assert.AreEqual(result.HttpMethod, "GET", "Must be GET, the default value when no value is set.");
            Assert.IsNull(result.Url);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void TestAutoCompleteReturnsAutoCompleteBuilderWithNameAndIdAttributes()
        {
            var tb = _target.AutoCompleteFor(x => x.StringProp);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);

            tb = _target.AutoComplete("StringProp");
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestAutoCompleteReturnsAutoCompleteBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.AutoCompleteFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.AutoComplete("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestAutoCompleteReturnsAutoCompleteBuilderWithValueSet()
        {
            _model.StringProp = "HEY SUP";
            var tb = _target.AutoCompleteFor(x => x.StringProp);
            Assert.AreEqual(_model.StringProp, tb.Value);

            tb = _target.AutoComplete("StringProp");
            Assert.AreEqual(_model.StringProp, tb.Value);
        }

        [TestMethod]
        public void TestAutoCompleteReturnsAutoCompleteBuilderWithFormattedValueSet()
        {
            var expectedDate = new DateTime(2014, 6, 26);
            var expectedDateAsString = "6/26/2014";
            _model.FormattedDateProp = expectedDate;
            var tb = _target.AutoCompleteFor(x => x.FormattedDateProp);
            Assert.AreEqual(expectedDateAsString, tb.Value);

            tb = _target.AutoComplete("FormattedDateProp");
            Assert.AreEqual(expectedDateAsString, tb.Value);
        }

        [TestMethod]
        public void TestAutoCompleteIsReturnedWithErrorCssClassIfModelStateHasErrorForExpression()
        {
            var tb = _target.AutoCompleteFor(x => x.RequiredProp);
            Assert.IsFalse(tb.HtmlAttributes.ContainsKey("class"));

            _viewData.ModelState.AddModelError("RequiredProp", "Nope :(");
            tb = _target.AutoCompleteFor(x => x.RequiredProp);
            Assert.AreEqual(HtmlHelper.ValidationInputCssClassName, tb.HtmlAttributes["class"]);
        }

        [TestMethod]
        public void TestAutoCompleteReturnsAutoCompleteBuilderWithUrlSet()
        {
            var tb = _target.AutoCompleteFor(x => x.AutoCompleteProp);
            Assert.AreEqual("/SomeArea/AutoComplete/YouAutoCompleteMe", tb.Url);
        }

        [TestMethod]
        public void
            TestAutoCompleteReturnsBuilderWithDisplayTextSetWhenDisplayPropertyHasValueAndModelPropertyHasValueAndCanGetValueFromTheDatabase()
        {
            var expectedEntity = new EntityLookupModel {Description = "See me!"};
            _mockEntityRepo.Setup(x => x.Find(42)).Returns(expectedEntity);
            _model.AutoCompleteEntityWithDisplayProperty = 42;

            var result = _target.AutoCompleteFor(x => x.AutoCompleteEntityWithDisplayProperty);
            Assert.AreEqual("See me!", result.DisplayText);
        }

        [TestMethod]
        public void
            TestAutoCompleteReturnsBuilderWithoutDisplayTextIfDisplayPropertyHasValueButModelPropertyDoesNotHaveValue()
        {
            _model.AutoCompleteEntityWithDisplayProperty = null;

            var result = _target.AutoCompleteFor(x => x.AutoCompleteEntityWithDisplayProperty);
            Assert.IsNull(result.DisplayText);
        }

        [TestMethod]
        public void TestAutoCompleteThrowsErrorIfDisplayPropertyIsSetButPropertyDoesNotHaveEntityMustExistAttribute()
        {
            _model.AutoCompleteEntityWithDisplayPropertyButNoEntityMustExistAttribute = 42;

            MyAssert.ThrowsWithMessage<InvalidOperationException>(
                () => _target.AutoCompleteFor(x =>
                    x.AutoCompleteEntityWithDisplayPropertyButNoEntityMustExistAttribute),
                "MMSINC.Core.MvcTest.Helpers.ControlHelperTest+Model.AutoCompleteEntityWithDisplayPropertyButNoEntityMustExistAttribute has an AutoComplete with DisplayProperty but is missing the required EntityMustExist attribute.");
        }

        [TestMethod]
        public void TestAutoCompleteHowDealWithNullEntity()
        {
            _mockEntityRepo.Setup(x => x.Find(42)).Returns((EntityLookupModel)null);
            _model.AutoCompleteEntityWithDisplayProperty = 42;

            MyAssert.ThrowsWithMessage<InvalidOperationException>(
                () => _target.AutoCompleteFor(x => x.AutoCompleteEntityWithDisplayProperty),
                "MMSINC.Core.MvcTest.Helpers.ControlHelperTest+EntityLookupModel with id 42 could not be found.");
        }

        #endregion

        #region BoolDropDownList

        [TestMethod]
        public void TestBoolDropDownListRendersValuesBasedOnAppliedBoolFormatAttribute()
        {
            var expected =
                "<select id=\"PropWithBoolFormat\" name=\"PropWithBoolFormat\">" +
                "<option value=\"\">Uh uh</option>" +
                "<option value=\"True\">Sure</option>" +
                "<option value=\"False\">Nah</option>" +
                "</select>";

            _appTester.ModelFormatterProvider.Clear();
            var result = _target.BoolDropDown("PropWithBoolFormat");
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());

            // Also test the Expression overload
            result = _target.BoolDropDownFor(x => x.PropWithBoolFormat);
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());
        }

        [TestMethod]
        public void TestBoolDropDownListRendersDefaultValuesWhenNoBoolFormatAttributeIsFound()
        {
            var expected =
                "<select id=\"PropWithoutBoolFormat\" name=\"PropWithoutBoolFormat\">" +
                "<option value=\"\">-- Select --</option>" +
                "<option value=\"True\">True</option>" +
                "<option value=\"False\">False</option>" +
                "</select>";

            _appTester.ModelFormatterProvider.Clear();
            var result = _target.BoolDropDown("PropWithoutBoolFormat");
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());

            // Also test the Expression overload
            result = _target.BoolDropDownFor(x => x.PropWithoutBoolFormat);
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());
        }

        [TestMethod]
        public void TestBoolDropDownListRendersAnEmptyOptionIfNullTextIsEmptyString()
        {
            var expected =
                "<select id=\"PropWithoutBoolFormat\" name=\"PropWithoutBoolFormat\">" +
                "<option value=\"\"></option>" +
                "<option value=\"True\">Yes</option>" +
                "<option value=\"False\">No</option>" +
                "</select>";

            var result = _target.BoolDropDown("PropWithoutBoolFormat", "Yes", "No", string.Empty);
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());

            // Also test the Expression overload
            result = _target.BoolDropDownFor(x => x.PropWithoutBoolFormat, "Yes", "No", string.Empty);
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());
        }

        [TestMethod]
        public void TestBoolDropDownListRendersADefaultSelectOptionIfNullTextIsNull()
        {
            var expected =
                "<select id=\"PropWithoutBoolFormat\" name=\"PropWithoutBoolFormat\">" +
                "<option value=\"\">-- Select --</option>" +
                "<option value=\"True\">Yes</option>" +
                "<option value=\"False\">No</option>" +
                "</select>";

            var result = _target.BoolDropDown("PropWithoutBoolFormat", "Yes", "No", null);
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());

            // Also test that the optional param defaults to null
            result = _target.BoolDropDown("PropWithoutBoolFormat", "Yes", "No");
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());

            // Also test the Expression overload
            result = _target.BoolDropDownFor(x => x.PropWithoutBoolFormat, "Yes", "No", null);
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());

            // Also test the Expression overload and optional param defaults to null
            result = _target.BoolDropDownFor(x => x.PropWithoutBoolFormat, "Yes", "No");
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());
        }

        [TestMethod]
        public void TestBoolDropDownListRendersSelectedValue()
        {
            var expected =
                "<select id=\"PropWithoutBoolFormat\" name=\"PropWithoutBoolFormat\">" +
                "<option value=\"\">-- Select --</option>" +
                "<option selected=\"selected\" value=\"True\">True</option>" +
                "<option value=\"False\">False</option>" +
                "</select>";

            _appTester.ModelFormatterProvider.Clear();
            _model.PropWithoutBoolFormat = true;
            var result = _target.BoolDropDown("PropWithoutBoolFormat");
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());

            // Also test the Expression overload
            result = _target.BoolDropDownFor(x => x.PropWithoutBoolFormat);
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());
        }

        [TestMethod]
        public void TestBoolDropDownRendersHtmlAttributes()
        {
            var expected =
                "<select foo=\"bar\" id=\"PropWithoutBoolFormat\" name=\"PropWithoutBoolFormat\">" +
                "<option value=\"\">-- Select --</option>" +
                "<option value=\"True\">Yes</option>" +
                "<option value=\"False\">No</option>" +
                "</select>";

            var result = _target.BoolDropDown("PropWithoutBoolFormat", "Yes", "No", null).With(new {foo = "bar"});
            Assert.AreEqual(expected, result.ToStringWithoutLineBreaks());
        }

        #endregion

        #region Button

        [TestMethod]
        public void TestButtonWithoutParamArgumentsReturnsAnEmptyButtonBuilder()
        {
            var result = _target.Button();
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.IsNull(result.Text);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.HtmlAttributes.Any());
            Assert.AreEqual(ButtonType.Button, result.Type);
        }

        [TestMethod]
        public void TestButtonReturnsButtonBuilderAsTypeButtonWithText()
        {
            var result = _target.Button("Text");
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.AreEqual("Text", result.Text);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.HtmlAttributes.Any());
            Assert.AreEqual(ButtonType.Button, result.Type);
        }

        #endregion

        #region CheckBox

        [TestMethod]
        public void TestCheckBoxReturnsCheckBoxBuilderWithValueSetToALowerCaseStringOfTheWordTrue()
        {
            Assert.AreEqual("true", _target.CheckBoxFor(x => x.IntProp).Value);
        }

        [TestMethod]
        public void TestCheckBoxWithoutArgumentsReturnsUninitializedCheckBoxBuilderInstance()
        {
            var result = _target.CheckBox();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.Checked);
        }

        #endregion

        #region CheckBoxList

        [TestMethod]
        public void TestCheckBoxListWithoutArgumentsReturnsUninitializedCheckBoxBuilderInstance()
        {
            var result = _target.CheckBoxList();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.EmptyText);
            Assert.IsFalse(result.Items.Any());
            Assert.IsFalse(result.SelectedValues.Any());
        }

        [TestMethod]
        public void TestCheckBoxListReturnsBuilderWithNullEmptyTextValue()
        {
            Assert.IsNull(_target.CheckBoxListFor(x => x.StringProp).EmptyText);
            Assert.IsNull(_target.CheckBoxList("StringProp").EmptyText);
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderWithNameAndIdAttributes()
        {
            var slb = _target.CheckBoxListFor(x => x.StringProp);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["id"]);

            slb = _target.CheckBoxList("StringProp");
            Assert.AreEqual("StringProp", slb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.CheckBoxListFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.CheckBoxList("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderWithoutSelectedValuesIfModelPropertyIsNull()
        {
            _model.StringProp = null;
            Assert.IsFalse(_target.CheckBoxListFor(x => x.StringProp).SelectedValues.Any());
            Assert.IsFalse(_target.CheckBoxList("StringProp").SelectedValues.Any());
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderWithSelectedValuesPropertySetWithSingleValue()
        {
            var expected = "sup";
            _model.StringProp = expected;

            var result = _target.CheckBoxListFor(x => x.StringProp);
            Assert.IsTrue(result.SelectedValues.Contains(expected));

            result = _target.CheckBoxList("StringProp");
            Assert.IsTrue(result.SelectedValues.Contains(expected));
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderWithSelectedValuesPropertySetToMultipleValues()
        {
            var expected = new List<object>();
            expected.Add(1);
            expected.Add(2);

            _model.ListProp = expected;
            var result = _target.CheckBoxListFor(x => x.ListProp);
            Assert.AreEqual(2, result.SelectedValues.Count);
            Assert.IsTrue(result.SelectedValues.Contains(1));
            Assert.IsTrue(result.SelectedValues.Contains(2));

            result = _target.CheckBoxList("ListProp");
            Assert.AreEqual(2, result.SelectedValues.Count);
            Assert.IsTrue(result.SelectedValues.Contains(1));
            Assert.IsTrue(result.SelectedValues.Contains(2));
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderWithEmptyItemsIfNoItemsAreInViewData()
        {
            _viewData.Clear();
            Assert.IsFalse(_target.CheckBoxListFor(x => x.StringProp).Items.Any());
            Assert.IsFalse(_target.CheckBoxList("StringProp").Items.Any());
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderWithSelectListItemsFromViewData()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["StringProp"] = items;

            Assert.AreSame(expected, _target.CheckBoxListFor(x => x.StringProp).Items.Single());
            Assert.AreSame(expected, _target.CheckBoxList("StringProp").Items.Single());
        }

        [TestMethod]
        public void
            TestCheckBoxListReturnsCheckBoxListBuilderWithSelectListItemsCreatedFromDropDownItemCollectionsFromViewData()
        {
            var expected = new DropDownItemModel {
                Text = "Some Text",
                Value = 1
            };

            // The "key" on a ListBoxItem is its value attribute,
            // The "value" on a ListBoxItem is its text value.
            var items = SelectListItemConverter.Convert(new[] {expected}, x => x.Value, x => x.Text);
            _viewData["StringProp"] = items;

            var result = _target.CheckBoxListFor(x => x.StringProp);
            Assert.AreEqual(expected.Text, result.Items.Single().Text);
            Assert.AreEqual(expected.Value.ToString(), result.Items.Single().Value);

            result = _target.CheckBoxList("StringProp");
            Assert.AreEqual(expected.Text, result.Items.Single().Text);
            Assert.AreEqual(expected.Value.ToString(), result.Items.Single().Value);
        }

        [TestMethod]
        public void
            TestCheckBoxListReturnsCheckBoxListBuilderWithItemsFromSpecificViewDataKeyWhenSelectAttributeIsAvailableForProperty()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["SomeKey"] = items;
            var badItems = new List<SelectListItem>();
            _viewData["SelectAttributeProp"] = badItems;

            Assert.AreSame(expected, _target.CheckBoxListFor(x => x.SelectAttributeProp).Items.Single());
            Assert.AreSame(expected, _target.CheckBoxList("SelectAttributeProp").Items.Single());
        }

        [TestMethod]
        public void TestCheckBoxListReturnsCheckBoxListBuilderForStringExpressionThatDoesNotExistForModel()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["ImNotReal"] = items;
            var result = _target.CheckBoxList("ImNotReal");
            Assert.AreSame(expected, result.Items.Single());
            Assert.IsFalse(result.SelectedValues.Any());
        }

        #endregion

        #region DatePicker

        [TestMethod]
        public void TestDatePickertWithoutArgumentsReturnsUninitializedDatePickerBuilderInstance()
        {
            var result = _target.DatePicker();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.IncludeTimePicker);
        }

        [TestMethod]
        public void TestDatePickerReturnsDatePickerBuilderWithNameAndIdAttributes()
        {
            var tb = _target.DatePickerFor(x => x.DateProp);
            Assert.AreEqual("DateProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("DateProp", tb.HtmlAttributes["id"]);

            tb = _target.DatePicker("DateProp");
            Assert.AreEqual("DateProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("DateProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestDatePickerReturnsDatePickerBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true",
                _target.DatePickerFor(x => x.RequiredDateProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.DatePicker("RequiredDateProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestDatePickerReturnsDatePickerBuilderWithValueSet()
        {
            _model.DateProp = DateTime.Today;
            var expected = _model.DateProp.ToString();
            var tb = _target.DatePickerFor(x => x.DateProp);
            Assert.AreEqual(expected, tb.Value);

            tb = _target.DatePicker("DateProp");
            Assert.AreEqual(expected, tb.Value);
        }

        [TestMethod]
        public void TestDatePickerReturnsDatePickerBuilderWithFormattedValueSet()
        {
            var expectedDate = new DateTime(2014, 6, 26, 4, 0, 4);
            var expectedDateAsString = "6/26/2014";
            _model.FormattedDateProp = expectedDate;
            var tb = _target.DatePickerFor(x => x.FormattedDateProp);
            Assert.AreEqual(expectedDateAsString, tb.Value);

            tb = _target.DatePicker("FormattedDateProp");
            Assert.AreEqual(expectedDateAsString, tb.Value);
        }

        [TestMethod]
        public void TestDatePickerIsReturnedWithErrorCssClassIfModelStateHasErrorForExpression()
        {
            var tb = _target.DatePickerFor(x => x.RequiredDateProp);
            Assert.IsFalse(tb.HtmlAttributes.ContainsKey("class"));

            _viewData.ModelState.AddModelError("RequiredDateProp", "Nope :(");
            tb = _target.DatePickerFor(x => x.RequiredDateProp);
            Assert.AreEqual(HtmlHelper.ValidationInputCssClassName, tb.HtmlAttributes["class"]);
        }

        #endregion

        #region DropDown

        [TestMethod]
        public void TestDropDownWithoutArgumentsReturnsUninitializedSelectListBuilderInstanceWithTypeSetToDropDown()
        {
            var result = _target.DropDown();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.EmptyText);
            Assert.IsFalse(result.Items.Any());
            Assert.IsFalse(result.SelectedValues.Any());
            Assert.AreEqual(SelectListType.DropDown, result.Type);
        }

        [TestMethod]
        public void TestDropDownReturnsBuilderWithTypeSetToDropDownList()
        {
            var slb = _target.DropDownFor(x => x.StringProp);
            Assert.AreEqual(SelectListType.DropDown, slb.Type);

            slb = _target.DropDown("StringProp");
            Assert.AreEqual(SelectListType.DropDown, slb.Type);
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithNameAndIdAttributes()
        {
            var slb = _target.DropDownFor(x => x.StringProp);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["id"]);

            slb = _target.DropDown("StringProp");
            Assert.AreEqual("StringProp", slb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.DropDownFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.DropDown("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithoutSelectedValuesIfModelPropertyIsNull()
        {
            _model.StringProp = null;
            Assert.IsFalse(_target.DropDownFor(x => x.StringProp).SelectedValues.Any());
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithSelectedValuesPropertySetWithSingleValue()
        {
            var expected = "sup";
            _model.StringProp = expected;

            var result = _target.DropDownFor(x => x.StringProp);
            Assert.IsTrue(result.SelectedValues.Contains(expected));

            result = _target.DropDown("StringProp");
            Assert.IsTrue(result.SelectedValues.Contains(expected));
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithSelectedValuesPropertySetToMultipleValues()
        {
            var expected = new List<object>();
            expected.Add(1);
            expected.Add(2);

            _model.ListProp = expected;
            var result = _target.DropDownFor(x => x.ListProp);
            Assert.AreEqual(2, result.SelectedValues.Count);
            Assert.IsTrue(result.SelectedValues.Contains(1));
            Assert.IsTrue(result.SelectedValues.Contains(2));

            result = _target.DropDown("ListProp");
            Assert.AreEqual(2, result.SelectedValues.Count);
            Assert.IsTrue(result.SelectedValues.Contains(1));
            Assert.IsTrue(result.SelectedValues.Contains(2));
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithSelectedValuesPropertySetForIntValues()
        {
            var expected = 3;
            _model.IntProp = expected;

            var result = _target.DropDownFor(x => x.IntProp);
            Assert.IsTrue(result.SelectedValues.Contains(expected));

            result = _target.DropDown("IntProp");
            Assert.IsTrue(result.SelectedValues.Contains(expected));
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithEmptyItemsIfNoItemsAreInViewData()
        {
            _viewData.Clear();
            Assert.IsFalse(_target.DropDownFor(x => x.StringProp).Items.Any());
            Assert.IsFalse(_target.DropDown("StringProp").Items.Any());
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithSelectListItemsFromViewData()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["StringProp"] = items;

            Assert.AreSame(expected, _target.DropDownFor(x => x.StringProp).Items.Single());
            Assert.AreSame(expected, _target.DropDown("StringProp").Items.Single());
        }

        [TestMethod]
        public void
            TestDropDownReturnsSelectListBuilderWithSelectListItemsCreatedFromDropDownItemCollectionsFromViewData()
        {
            var expected = new DropDownItemModel {
                Text = "Some Text",
                Value = 1
            };

            // The "key" on a DropDownItem is its value attribute,
            // The "value" on a DropDownItem is its text value.
            var items = SelectListItemConverter.Convert(new[] {expected}, x => x.Value, x => x.Text);
            _viewData["StringProp"] = items;

            var result = _target.DropDownFor(x => x.StringProp);
            Assert.AreEqual(expected.Text, result.Items.Single().Text);
            Assert.AreEqual(expected.Value.ToString(), result.Items.Single().Value);

            result = _target.DropDown("StringProp");
            Assert.AreEqual(expected.Text, result.Items.Single().Text);
            Assert.AreEqual(expected.Value.ToString(), result.Items.Single().Value);
        }

        [TestMethod]
        public void TestDropDownCanUseEntityLookupCollections()
        {
            var expected = new EntityLookupModel {
                Description = "Neat",
                Id = 134
            };
            var items = new[] {expected};
            _viewData["StringProp"] = items;

            var result = _target.DropDownFor(x => x.StringProp);
            Assert.AreEqual(expected.Description, result.Items.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), result.Items.Single().Value);

            result = _target.DropDown("StringProp");
            Assert.AreEqual(expected.Description, result.Items.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), result.Items.Single().Value);
        }

        [TestMethod]
        public void
            TestDropDownReturnsSelectListBuilderWithItemsFromSpecificViewDataKeyWhenSelectAttributeIsAvailableForProperty()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["SomeKey"] = items;
            var badItems = new List<SelectListItem>();
            _viewData["SelectAttributeProp"] = badItems;

            Assert.AreSame(expected, _target.DropDownFor(x => x.SelectAttributeProp).Items.Single());
            Assert.AreSame(expected, _target.DropDown("SelectAttributeProp").Items.Single());
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderForStringExpressionThatDoesNotExistForModel()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["ImNotReal"] = items;
            var result = _target.DropDown("ImNotReal");
            Assert.AreSame(expected, result.Items.Single());
            Assert.IsFalse(result.SelectedValues.Any());
        }

        [TestMethod]
        public void
            TestDropDownReturnsSelectListBuilderWithEmptyTextSetToSelectAttributeDefaultLabelTextIfAttributeExists()
        {
            Assert.AreEqual("Some item label", _target.DropDownFor(x => x.SelectAttributeProp).EmptyText);
            Assert.AreEqual("Some item label", _target.DropDown("SelectAttributeProp").EmptyText);
        }

        //[TestMethod]
        //public void TestDropDownThrowsExceptionIfViewDataDoesNotHaveContainerKey()
        //{
        //    _viewData.Remove("Container");
        //    MyAssert.Throws<InvalidOperationException>(() => _target.DropDownFor(x => x.CascadingProperty));
        //    MyAssert.Throws<InvalidOperationException>(() => _target.DropDown("CascadingProperty"));
        //}

        [TestMethod]
        public void TestDropDownReturnsEmptyItemsForCascadesIfViewDataHasContainerValueButValueIsNull()
        {
            _viewData["Container"] = null;
            Assert.IsFalse(_target.DropDownFor(x => x.CascadingProperty).Items.Any());
            Assert.IsFalse(_target.DropDown("CascadingProperty").Items.Any());
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithoutItemsIfCascadingParentIsNull()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;

            Assert.IsFalse(_target.DropDownFor(x => x.CascadingProperty).Items.Any());
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithItemsFromCascadingActionIfPropertyHasCascadingAttribute()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;
            _model.CascadingParentProperty = 32;

            // There will be two items, the first item is the empty item, the last item is the one we're testing.
            var result = _target.DropDownFor(x => x.CascadingProperty).Items.Last();
            Assert.AreEqual("Some text", result.Text);
            Assert.AreEqual("Some value", result.Value);
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithItemsFromCascadingActionIfPropertyHasCascadingAttributeWithDependentRequirementsSetToNoneAndParentValueIsNull()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;
            _model.CascadingParentPropertyThatIsNotRequired = null;

            // There will be two items, the first item is the empty item, the last item is the one we're testing.
            var result = _target.DropDownFor(x => x.CascadingPropertyWithoutRequiredParent).Items.Last();
            Assert.AreEqual("Some text", result.Text);
        }

        [TestMethod]
        public void TestDropDownHasPrerenderedAttributeWhenItemsAlreadyExist()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;
            _model.CascadingParentPropertyThatIsNotRequired = null;

            Assert.IsTrue(_target.DropDownFor(x => x.CascadingPropertyWithoutRequiredParent).HtmlAttributes.ContainsKey("data-cascading-prerendered"));
        }

        [TestMethod]
        public void TestDropDownDisposesControllerForCascades()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;
            _model.CascadingParentProperty = 32;

            Assert.IsFalse(_cascadingController.Disposed);
            _target.DropDownFor(x => x.CascadingProperty);
            Assert.IsTrue(_cascadingController.Disposed);
        }

        private void AssertApplyCascadingHtmlAttributeSet(string htmlAttributeName, string expected,
            string message = null)
        {
            // RouteValueDictionary is what's used internally by the helper, so 
            // let's use that here, too.
            InitializeForCascading();
            var result = _target.DropDownFor(x => x.CascadingProperty);
            Assert.AreEqual(expected, result.HtmlAttributes[htmlAttributeName],
                "Must be equal for lambda expression. " + message);

            result = _target.DropDown("CascadingProperty");
            Assert.AreEqual(expected, result.HtmlAttributes[htmlAttributeName],
                "Must be equal for string expression. " + message);
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithExpectedCascadingHtmlAttributes()
        {
            AssertApplyCascadingHtmlAttributeSet("data-cascading", "true");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-action", "/Cascading/CascadeAction");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-dependson", "#CascadingParentProperty");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-dependentsrequired", "all");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-errortext", "Some error");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-loadingtext", "Some loading text");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-prompttext", "Some prompt text");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-httpmethod", "Some http method");
            AssertApplyCascadingHtmlAttributeSet("data-cascading-actionparam", "id");
        }

        [TestMethod]
        public void TestDropDownReturnsSelectListBuilderWithCorrectDependsOnValueForNestedProperties()
        {
            _viewData.TemplateInfo.HtmlFieldPrefix = "SomeParentProperty";
            AssertApplyCascadingHtmlAttributeSet("data-cascading-dependson",
                "#SomeParentProperty_CascadingParentProperty");
        }

        [TestMethod]
        public void TestDropDownReturnsExpectedHtmlAttributesWhenDealingWithAMultiParameterCascade()
        {
            InitializeForCascading();
            _cascadingController.Result = new CascadingActionResult(Enumerable.Empty<object>());
            var result = _target.DropDownFor(x => x.MultiParameterCascadeProperty);
            Assert.AreEqual("id,somethingElse", result.HtmlAttributes["data-cascading-actionparam"],
                "Must be equal for lambda expression. Should have no spaces between parameters, comma only.");
            Assert.AreEqual("#CascadingParentProperty,#AreaCascadingProperty",
                result.HtmlAttributes["data-cascading-dependson"],
                "Must be equal for lambda expression. Should have no spaces between selectors, comma only.");

            result = _target.DropDown("MultiParameterCascadeProperty");
            Assert.AreEqual("id,somethingElse", result.HtmlAttributes["data-cascading-actionparam"],
                "Must be equal for string expression. Should have no spaces between parameters, comma only.");
            Assert.AreEqual("#CascadingParentProperty,#AreaCascadingProperty",
                result.HtmlAttributes["data-cascading-dependson"],
                "Must be equal for string expression. Should have no spaces between selectors, comma only.");
        }

        [TestMethod]
        public void TestApplyCascadingHtmlAttributesSesActionWhenCascadingAttributeHasArea()
        {
            InitializeForCascading();
            var result = _target.DropDownFor(x => x.AreaCascadingProperty);
            Assert.AreEqual("/SomeArea/Cascading/CascadeAction", result.HtmlAttributes["data-cascading-action"],
                "Must be equal for lambda expression.");
            result = _target.DropDown("AreaCascadingProperty");
            Assert.AreEqual("/SomeArea/Cascading/CascadeAction", result.HtmlAttributes["data-cascading-action"],
                "Must be equal for string expression.");
        }

        #endregion

        #region FileUpload

        [TestMethod]
        public void TestFileUploadWithoutArgumentsReturnsUninitializedFileUploadBuilder()
        {
            var result = _target.FileUpload();
            AssertBuilderIsUninitialized(result);
            Assert.IsFalse(result.AllowedExtensions.Any());
            Assert.IsNull(result.ButtonText);
            Assert.IsNull(result.OnComplete);
            Assert.IsNull(result.Url);
        }

        [TestMethod]
        public void TestFileUploadReturnsFileUploadBuilderWithNameAndIdAttributes()
        {
            var tb = _target.FileUploadFor(x => x.FileUploadProp);
            Assert.AreEqual("FileUploadProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("FileUploadProp", tb.HtmlAttributes["id"]);

            tb = _target.FileUpload("FileUploadProp");
            Assert.AreEqual("FileUploadProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("FileUploadProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestFileUploadReturnsFileUploadBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true",
                _target.FileUploadFor(x => x.RequiredFileUploadProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.FileUpload("RequiredFileUploadProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestFileUploadIsReturnedWithErrorCssClassIfModelStateHasErrorForExpression()
        {
            var tb = _target.FileUploadFor(x => x.RequiredFileUploadProp);
            Assert.IsFalse(tb.HtmlAttributes.ContainsKey("class"));

            _viewData.ModelState.AddModelError("RequiredFileUploadProp", "Nope :(");
            tb = _target.FileUploadFor(x => x.RequiredFileUploadProp);
            Assert.AreEqual(HtmlHelper.ValidationInputCssClassName, tb.HtmlAttributes["class"]);
        }

        [TestMethod]
        public void TestFileUploadReturnsFileUploadBuilderWithUrlSet()
        {
            var tb = _target.FileUploadFor(x => x.FileUploadProp);
            Assert.AreEqual("/FileUpload/UploadMe", tb.Url);

            tb = _target.FileUpload("FileUploadProp");
            Assert.AreEqual("/FileUpload/UploadMe", tb.Url);
        }

        #endregion

        #region Hidden

        [TestMethod]
        public void TestHiddenWithoutArgumentsReturnsUninitializedHiddenInputBuilder()
        {
            var result = _target.Hidden();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void TestHiddenReturnsHiddenInputBuilderWithNameAndIdAttributes()
        {
            var tb = _target.HiddenFor(x => x.StringProp);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);

            tb = _target.Hidden("StringProp");
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestHiddenReturnsHiddenInputBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.HiddenFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.Hidden("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestHiddenReturnsHiddenInputBuilderWithValueSet()
        {
            _model.StringProp = "HEY SUP";
            var tb = _target.HiddenFor(x => x.StringProp);
            Assert.AreEqual(_model.StringProp, tb.Value);

            tb = _target.Hidden("StringProp");
            Assert.AreEqual(_model.StringProp, tb.Value);
        }

        [TestMethod]
        public void TestHiddenReturnsHiddenInputBuilderWithFormattedValueSet()
        {
            var expectedDate = new DateTime(2014, 6, 26);
            var expectedDateAsString = "6/26/2014";
            _model.FormattedDateProp = expectedDate;
            var tb = _target.HiddenFor(x => x.FormattedDateProp);
            Assert.AreEqual(expectedDateAsString, tb.Value);

            tb = _target.Hidden("FormattedDateProp");
            Assert.AreEqual(expectedDateAsString, tb.Value);
        }

        [TestMethod]
        public void TestHiddenIsReturnedWithErrorCssClassIfModelStateHasErrorForExpression()
        {
            var tb = _target.HiddenFor(x => x.RequiredProp);
            Assert.IsFalse(tb.HtmlAttributes.ContainsKey("class"));

            _viewData.ModelState.AddModelError("RequiredProp", "Nope :(");
            tb = _target.HiddenFor(x => x.RequiredProp);
            Assert.AreEqual(HtmlHelper.ValidationInputCssClassName, tb.HtmlAttributes["class"]);
        }

        #endregion

        #region ListBox

        [TestMethod]
        public void TestListBoxWithoutArgumentsReturnsUninitializedListBoxBuilderInstanceWithTypeSetToListBox()
        {
            var result = _target.ListBox();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.EmptyText);
            Assert.IsFalse(result.Items.Any());
            Assert.IsFalse(result.SelectedValues.Any());
            Assert.AreEqual(SelectListType.ListBox, result.Type);
        }

        [TestMethod]
        public void TestListBoxReturnsBuilderWithTypeSetToListBox()
        {
            var slb = _target.ListBoxFor(x => x.StringProp);
            Assert.AreEqual(SelectListType.ListBox, slb.Type);

            slb = _target.ListBox("StringProp");
            Assert.AreEqual(SelectListType.ListBox, slb.Type);
        }

        [TestMethod]
        public void TestListBoxReturnsBuilderWithNullEmptyTextValue()
        {
            Assert.IsNull(_target.ListBoxFor(x => x.StringProp).EmptyText);
            Assert.IsNull(_target.ListBox("StringProp").EmptyText);
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithNameAndIdAttributes()
        {
            var slb = _target.ListBoxFor(x => x.StringProp);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["id"]);

            slb = _target.ListBox("StringProp");
            Assert.AreEqual("StringProp", slb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", slb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.ListBoxFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.ListBox("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithoutSelectedValuesIfModelPropertyIsNull()
        {
            _model.StringProp = null;
            Assert.IsFalse(_target.ListBoxFor(x => x.StringProp).SelectedValues.Any());
            Assert.IsFalse(_target.ListBox("StringProp").SelectedValues.Any());
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithSelectedValuesPropertySetWithSingleValue()
        {
            var expected = "sup";
            _model.StringProp = expected;

            var result = _target.ListBoxFor(x => x.StringProp);
            Assert.IsTrue(result.SelectedValues.Contains(expected));

            result = _target.ListBox("StringProp");
            Assert.IsTrue(result.SelectedValues.Contains(expected));
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithSelectedValuesPropertySetToMultipleValues()
        {
            var expected = new List<object>();
            expected.Add(1);
            expected.Add(2);

            _model.ListProp = expected;
            var result = _target.ListBoxFor(x => x.ListProp);
            Assert.AreEqual(2, result.SelectedValues.Count);
            Assert.IsTrue(result.SelectedValues.Contains(1));
            Assert.IsTrue(result.SelectedValues.Contains(2));

            result = _target.ListBox("ListProp");
            Assert.AreEqual(2, result.SelectedValues.Count);
            Assert.IsTrue(result.SelectedValues.Contains(1));
            Assert.IsTrue(result.SelectedValues.Contains(2));
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithSelectedValuesPropertySetForIntValues()
        {
            var expected = 3;
            _model.IntProp = expected;

            var result = _target.ListBoxFor(x => x.IntProp);
            Assert.IsTrue(result.SelectedValues.Contains(expected));

            result = _target.ListBox("IntProp");
            Assert.IsTrue(result.SelectedValues.Contains(expected));
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithEmptyItemsIfNoItemsAreInViewData()
        {
            _viewData.Clear();
            Assert.IsFalse(_target.ListBoxFor(x => x.StringProp).Items.Any());
            Assert.IsFalse(_target.ListBox("StringProp").Items.Any());
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithSelectListItemsFromViewData()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["StringProp"] = items;

            Assert.AreSame(expected, _target.ListBoxFor(x => x.StringProp).Items.Single());
            Assert.AreSame(expected, _target.ListBox("StringProp").Items.Single());
        }

        [TestMethod]
        public void
            TestListBoxReturnsSelectListBuilderWithSelectListItemsCreatedFromDropDownItemCollectionsFromViewData()
        {
            var expected = new DropDownItemModel {
                Text = "Some Text",
                Value = 1
            };

            // The "key" on a ListBoxItem is its value attribute,
            // The "value" on a ListBoxItem is its text value.
            var items = SelectListItemConverter.Convert(new[] {expected}, x => x.Value, x => x.Text);
            _viewData["StringProp"] = items;

            var result = _target.ListBoxFor(x => x.StringProp);
            Assert.AreEqual(expected.Text, result.Items.Single().Text);
            Assert.AreEqual(expected.Value.ToString(), result.Items.Single().Value);

            result = _target.ListBox("StringProp");
            Assert.AreEqual(expected.Text, result.Items.Single().Text);
            Assert.AreEqual(expected.Value.ToString(), result.Items.Single().Value);
        }

        [TestMethod]
        public void
            TestListBoxReturnsSelectListBuilderWithItemsFromSpecificViewDataKeyWhenSelectAttributeIsAvailableForProperty()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["SomeKey"] = items;
            var badItems = new List<SelectListItem>();
            _viewData["SelectAttributeProp"] = badItems;

            Assert.AreSame(expected, _target.ListBoxFor(x => x.SelectAttributeProp).Items.Single());
            Assert.AreSame(expected, _target.ListBox("SelectAttributeProp").Items.Single());
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderForStringExpressionThatDoesNotExistForModel()
        {
            var expected = new SelectListItem();
            var items = new List<SelectListItem>();
            items.Add(expected);
            _viewData["ImNotReal"] = items;
            var result = _target.ListBox("ImNotReal");
            Assert.AreSame(expected, result.Items.Single());
            Assert.IsFalse(result.SelectedValues.Any());
        }

        //[TestMethod]
        //public void TestListBoxThrowsExceptionIfViewDataDoesNotHaveContainerKey()
        //{
        //    _viewData.Remove("Container");
        //    MyAssert.Throws<InvalidOperationException>(() => _target.ListBoxFor(x => x.CascadingProperty));
        //    MyAssert.Throws<InvalidOperationException>(() => _target.ListBox("CascadingProperty"));
        //}

        [TestMethod]
        public void TestListBoxReturnsEmptyItemsForCascadesIfViewDataHasContainerValueButValueIsNull()
        {
            _viewData["Container"] = null;
            Assert.IsFalse(_target.ListBoxFor(x => x.CascadingProperty).Items.Any());
            Assert.IsFalse(_target.ListBox("CascadingProperty").Items.Any());
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithoutItemsIfCascadingParentIsNull()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;

            Assert.IsFalse(_target.ListBoxFor(x => x.CascadingProperty).Items.Any());
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithItemsFromCascadingActionIfPropertyHasCascadingAttribute()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;
            _model.CascadingParentProperty = 32;

            // There will be two items, the first item is the empty item, the last item is the one we're testing.
            var result = _target.ListBoxFor(x => x.CascadingProperty).Items.Last();
            Assert.AreEqual("Some text", result.Text);
            Assert.AreEqual("Some value", result.Value);
        }

        [TestMethod]
        public void TestListBoxDisposesControllerForCascades()
        {
            var expected = new CascadeItemModel {
                Text = "Some text",
                Value = "Some value"
            };
            _cascadingController.Result = new CascadingActionResult(new[] {expected}, "Text", "Value");
            _viewData["Container"] = _model;
            _model.CascadingParentProperty = 32;

            Assert.IsFalse(_cascadingController.Disposed);
            _target.ListBoxFor(x => x.CascadingProperty);
            Assert.IsTrue(_cascadingController.Disposed);
        }

        private void AssertApplyCascadingHtmlAttributeSetForListBox(string htmlAttributeName, string expected)
        {
            // RouteValueDictionary is what's used internally by the helper, so 
            // let's use that here, too.
            InitializeForCascading();
            var result = _target.ListBoxFor(x => x.CascadingProperty);
            Assert.AreEqual(expected, result.HtmlAttributes[htmlAttributeName], "Must be equal for lambda expression.");

            result = _target.ListBox("CascadingProperty");
            Assert.AreEqual(expected, result.HtmlAttributes[htmlAttributeName], "Must be equal for string expression.");
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithExpectedCascadingHtmlAttributes()
        {
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading", "true");
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading-action", "/Cascading/CascadeAction");
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading-dependson", "#CascadingParentProperty");
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading-errortext", "Some error");
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading-loadingtext", "Some loading text");
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading-prompttext", "Some prompt text");
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading-httpmethod", "Some http method");
            AssertApplyCascadingHtmlAttributeSetForListBox("data-cascading-actionparam", "id");
        }

        [TestMethod]
        public void TestListBoxReturnsSelectListBuilderWithCorrectDependsOnValueForNestedProperties()
        {
            _viewData.TemplateInfo.HtmlFieldPrefix = "SomeParentProperty";
            AssertApplyCascadingHtmlAttributeSet("data-cascading-dependson",
                "#SomeParentProperty_CascadingParentProperty");
        }

        [TestMethod]
        public void TestApplyCascadingHtmlAttributesSesActionWhenCascadingAttributeHasAreaForListBox()
        {
            _appTester.RegisterArea("SomeArea", (context) => {
                context.MapRoute(
                    "SomeArea_default",
                    "SomeArea/{controller}/{action}/{id}",
                    new {action = "Index", id = UrlParameter.Optional}
                );
            });

            InitializeForCascading();
            var result = _target.ListBoxFor(x => x.AreaCascadingProperty);
            Assert.AreEqual("/SomeArea/Cascading/CascadeAction", result.HtmlAttributes["data-cascading-action"],
                "Must be equal for lambda expression.");
            result = _target.ListBox("AreaCascadingProperty");
            Assert.AreEqual("/SomeArea/Cascading/CascadeAction", result.HtmlAttributes["data-cascading-action"],
                "Must be equal for lambda expression.");
        }

        #endregion

        #region Password

        [TestMethod]
        public void TestPasswordWithoutArgumentsReturnsUninitializedTextBoxBuilderWithTypeSetToPassword()
        {
            var result = _target.Password();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.Value);
            Assert.AreEqual(TextBoxType.Password, result.Type);
        }

        [TestMethod]
        public void TestPasswordReturnsTextBoxBuilderWithTypeSetToPassword()
        {
            Assert.AreEqual(TextBoxType.Password, _target.PasswordFor(x => x.StringProp).Type);
            Assert.AreEqual(TextBoxType.Password, _target.Password("StringProp").Type);
        }

        [TestMethod]
        public void TestPasswordReturnsTextBoxBuilderWithNameAndIdAttributes()
        {
            var tb = _target.PasswordFor(x => x.StringProp);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);

            tb = _target.Password("StringProp");
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestPasswordReturnsTextBoxBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.PasswordFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.Password("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestPasswordReturnsTextBoxBuilderWithValueSet()
        {
            _model.StringProp = "HEY SUP";
            var tb = _target.PasswordFor(x => x.StringProp);
            Assert.AreEqual(_model.StringProp, tb.Value);

            tb = _target.Password("StringProp");
            Assert.AreEqual(_model.StringProp, tb.Value);
        }

        [TestMethod]
        public void TestPasswordReturnsTextBoxBuilderWithFormattedValueSet()
        {
            var expectedDate = new DateTime(2014, 6, 26);
            var expectedDateAsString = "6/26/2014";
            _model.FormattedDateProp = expectedDate;
            var tb = _target.PasswordFor(x => x.FormattedDateProp);
            Assert.AreEqual(expectedDateAsString, tb.Value);

            tb = _target.Password("FormattedDateProp");
            Assert.AreEqual(expectedDateAsString, tb.Value);
        }

        [TestMethod]
        public void TestPasswordIsReturnedWithErrorCssClassIfModelStateHasErrorForExpression()
        {
            var tb = _target.PasswordFor(x => x.RequiredProp);
            Assert.IsFalse(tb.HtmlAttributes.ContainsKey("class"));

            _viewData.ModelState.AddModelError("RequiredProp", "Nope :(");
            tb = _target.PasswordFor(x => x.RequiredProp);
            Assert.AreEqual(HtmlHelper.ValidationInputCssClassName, tb.HtmlAttributes["class"]);
        }

        #endregion

        #region RangePicker

        [TestMethod]
        public void TestRangePickerReturnsRangePickerBuilderWithNameAndIdAttributes()
        {
            var tb = _target.RangePickerFor(x => x.IntRangeProp);
            Assert.AreEqual("IntRangeProp", tb.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp_Start", tb.StartBuilder.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp.Start", tb.StartBuilder.HtmlAttributes["name"]);
            Assert.AreEqual("IntRangeProp_Operator", tb.OperatorBuilder.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp.Operator", tb.OperatorBuilder.HtmlAttributes["name"]);
            Assert.AreEqual("IntRangeProp_End", tb.EndBuilder.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp.End", tb.EndBuilder.HtmlAttributes["name"]);

            tb = _target.RangePicker("IntRangeProp");
            Assert.AreEqual("IntRangeProp", tb.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp_Start", tb.StartBuilder.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp.Start", tb.StartBuilder.HtmlAttributes["name"]);
            Assert.AreEqual("IntRangeProp_Operator", tb.OperatorBuilder.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp.Operator", tb.OperatorBuilder.HtmlAttributes["name"]);
            Assert.AreEqual("IntRangeProp_End", tb.EndBuilder.HtmlAttributes["id"]);
            Assert.AreEqual("IntRangeProp.End", tb.EndBuilder.HtmlAttributes["name"]);
        }

        [TestMethod]
        public void TestRangePickerReturnsRangePickerBuilderWithDatePickersIfRangePropIsRangeOfDate()
        {
            var tb = _target.RangePickerFor(x => x.DateRangeProp);
            Assert.IsInstanceOfType(tb.StartBuilder, typeof(DatePickerBuilder));
            Assert.IsInstanceOfType(tb.EndBuilder, typeof(DatePickerBuilder));

            tb = _target.RangePicker("DateRangeProp");
            Assert.IsInstanceOfType(tb.StartBuilder, typeof(DatePickerBuilder));
            Assert.IsInstanceOfType(tb.EndBuilder, typeof(DatePickerBuilder));
        }

        [TestMethod]
        public void TestRangePickerReturnsRangePickerBuilderWithTextBoxesIfRangePropIsNOTRangeOfDate()
        {
            var tb = _target.RangePickerFor(x => x.IntProp);
            Assert.IsInstanceOfType(tb.StartBuilder, typeof(TextBoxBuilder));
            Assert.IsInstanceOfType(tb.EndBuilder, typeof(TextBoxBuilder));

            tb = _target.RangePicker("IntProp");
            Assert.IsInstanceOfType(tb.StartBuilder, typeof(TextBoxBuilder));
            Assert.IsInstanceOfType(tb.EndBuilder, typeof(TextBoxBuilder));
        }

        [TestMethod]
        public void TestRangePickerReturnsRangePickerBuilderWithUnobtrusiveValidationAttributesOnAllChildBuilders()
        {
            var tb = _target.RangePickerFor(x => x.IntRangeProp);
            Assert.IsTrue(tb.StartBuilder.HtmlAttributes.ContainsKey("data-val"));
            Assert.IsTrue(tb.OperatorBuilder.HtmlAttributes.ContainsKey("data-val"));
            Assert.IsTrue(tb.EndBuilder.HtmlAttributes.ContainsKey("data-val"));

            tb = _target.RangePicker("IntRangeProp");
            Assert.IsTrue(tb.StartBuilder.HtmlAttributes.ContainsKey("data-val"));
            Assert.IsTrue(tb.OperatorBuilder.HtmlAttributes.ContainsKey("data-val"));
            Assert.IsTrue(tb.EndBuilder.HtmlAttributes.ContainsKey("data-val"));
        }

        #endregion

        #region ResetButton

        [TestMethod]
        public void TestResetButtonWithoutParamArgumentsReturnsAnEmptyButtonBuilderAsTypeReset()
        {
            var result = _target.ResetButton();
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.AreEqual("Reset", result.Text, "Reset is the default text.");
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.HtmlAttributes.Any());
            Assert.AreEqual(ButtonType.Reset, result.Type);
        }

        [TestMethod]
        public void TestResetButtonReturnsButtonBuilderAsTypeResetWithText()
        {
            var result = _target.ResetButton("Text");
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.AreEqual("Text", result.Text);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.HtmlAttributes.Any());
            Assert.AreEqual(ButtonType.Reset, result.Type);
        }

        #endregion

        #region SubmitButton

        [TestMethod]
        public void TestSubmitButtonWithoutParamArgumentsReturnsAnEmptyButtonBuilderAsTypeSubmit()
        {
            var result = _target.SubmitButton();
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.IsNull(result.Text);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.HtmlAttributes.Any());
            Assert.AreEqual(ButtonType.Submit, result.Type);
        }

        [TestMethod]
        public void TestSubmitButtonReturnsButtonBuilderWithSubmitTypeAndText()
        {
            var result = _target.SubmitButton("Text");
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.AreEqual("Text", result.Text);
            Assert.IsNull(result.Value);
            Assert.IsFalse(result.HtmlAttributes.Any());
            Assert.AreEqual(ButtonType.Submit, result.Type);
        }

        #endregion

        #region TextArea

        [TestMethod]
        public void TestTextAreaWithoutArgumentsReturnsUninitializedTextBoxBuilderWithTypeSetToTextArea()
        {
            var result = _target.TextArea();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.Value);
            Assert.AreEqual(TextBoxType.TextArea, result.Type);
        }

        [TestMethod]
        public void TestTextAreaReturnsTextBoxBuilderWithTypeSetToTextArea()
        {
            Assert.AreEqual(TextBoxType.TextArea, _target.TextAreaFor(x => x.StringProp).Type);
            Assert.AreEqual(TextBoxType.TextArea, _target.TextArea("StringProp").Type);
        }

        [TestMethod]
        public void TestTextAreaReturnsTextBoxBuilderWithNameAndIdAttributes()
        {
            var tb = _target.TextAreaFor(x => x.StringProp);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);

            tb = _target.TextArea("StringProp");
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestTextAreaReturnsTextBoxBuilderWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.TextAreaFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.TextArea("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestTextAreaReturnsTextBoxBuilderWithValueSet()
        {
            _model.StringProp = "HEY SUP";
            var tb = _target.TextAreaFor(x => x.StringProp);
            Assert.AreEqual(_model.StringProp, tb.Value);

            tb = _target.TextArea("StringProp");
            Assert.AreEqual(_model.StringProp, tb.Value);
        }

        [TestMethod]
        public void TestTextAreaReturnsTextBoxBuilderWithFormattedValueSet()
        {
            var expectedDate = new DateTime(2014, 6, 26);
            var expectedDateAsString = "6/26/2014";
            _model.FormattedDateProp = expectedDate;
            var tb = _target.TextAreaFor(x => x.FormattedDateProp);
            Assert.AreEqual(expectedDateAsString, tb.Value);

            tb = _target.TextArea("FormattedDateProp");
            Assert.AreEqual(expectedDateAsString, tb.Value);
        }

        [TestMethod]
        public void TestTextAreaIsReturnedWithErrorCssClassIfModelStateHasErrorForExpression()
        {
            var tb = _target.TextAreaFor(x => x.RequiredProp);
            Assert.IsFalse(tb.HtmlAttributes.ContainsKey("class"));

            _viewData.ModelState.AddModelError("RequiredProp", "Nope :(");
            tb = _target.TextAreaFor(x => x.RequiredProp);
            Assert.AreEqual(HtmlHelper.ValidationInputCssClassName, tb.HtmlAttributes["class"]);
        }

        #endregion

        #region TextBox

        [TestMethod]
        public void TestTextBoxWithoutArgumentsReturnsUninitializedTextBoxBuilderWithTypeSetToText()
        {
            var result = _target.TextBox();
            AssertBuilderIsUninitialized(result);
            Assert.IsNull(result.Value);
            Assert.AreEqual(TextBoxType.Text, result.Type);
        }

        [TestMethod]
        public void TestTextBoxReturnsTextBoxBuilderWithTypeSetToText()
        {
            Assert.AreEqual(TextBoxType.Text, _target.TextBoxFor(x => x.StringProp).Type);
            Assert.AreEqual(TextBoxType.Text, _target.TextBox("StringProp").Type);
        }

        [TestMethod]
        public void TestTextBoxReturnsTextBoxWithNameAndIdAttributes()
        {
            var tb = _target.TextBoxFor(x => x.StringProp);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);

            tb = _target.TextBox("StringProp");
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestTextBoxReturnsTextBoxWithUnobtrusiveValidationAttributes()
        {
            Assert.AreEqual("true", _target.TextBoxFor(x => x.RequiredProp).HtmlAttributes[VALIDATION_ATTRIBUTE]);
            Assert.AreEqual("true", _target.TextBox("RequiredProp").HtmlAttributes[VALIDATION_ATTRIBUTE]);
        }

        [TestMethod]
        public void TestTextBoxReturnsTextBoxBuilderWithValueSet()
        {
            _model.StringProp = "HEY SUP";
            var tb = _target.TextBoxFor(x => x.StringProp);
            Assert.AreEqual(_model.StringProp, tb.Value);

            tb = _target.TextBox("StringProp");
            Assert.AreEqual(_model.StringProp, tb.Value);
        }

        [TestMethod]
        public void TestTextBoxReturnsTextBoxBuilderWithFormattedValueSet()
        {
            var expectedDate = new DateTime(2014, 6, 26);
            var expectedDateAsString = "6/26/2014";
            _model.FormattedDateProp = expectedDate;
            var tb = _target.TextBoxFor(x => x.FormattedDateProp);
            Assert.AreEqual(expectedDateAsString, tb.Value);

            tb = _target.TextBox("FormattedDateProp");
            Assert.AreEqual(expectedDateAsString, tb.Value);
        }

        [TestMethod]
        public void TestTextBoxIsReturnedWithErrorCssClassIfModelStateHasErrorForExpression()
        {
            var tb = _target.TextBoxFor(x => x.RequiredProp);
            Assert.IsFalse(tb.HtmlAttributes.ContainsKey("class"));

            _viewData.ModelState.AddModelError("RequiredProp", "Nope :(");
            tb = _target.TextBoxFor(x => x.RequiredProp);
            Assert.AreEqual(HtmlHelper.ValidationInputCssClassName, tb.HtmlAttributes["class"]);
        }

        #endregion

        #region ValueButton

        [TestMethod]
        public void TestValueButtonReturnsButtonBuilderWithNameAndIdAttributes()
        {
            var tb = _target.ValueButtonFor(x => x.StringProp);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);

            tb = _target.ValueButton("StringProp");
            Assert.AreEqual("StringProp", tb.HtmlAttributes["name"]);
            Assert.AreEqual("StringProp", tb.HtmlAttributes["id"]);
        }

        [TestMethod]
        public void TestValueButtonReturnsButtonBuilderWithoutUnobtrusiveValidationAttributes()
        {
            Assert.IsFalse(_target.ValueButtonFor(x => x.RequiredProp).HtmlAttributes
                                  .ContainsKey(VALIDATION_ATTRIBUTE));
            Assert.IsFalse(_target.ValueButton("RequiredProp").HtmlAttributes.ContainsKey(VALIDATION_ATTRIBUTE));
        }

        [TestMethod]
        public void TestValueButtonReturnsButtonBuilderWithValueAndTextPropertiesSetToTheSameValue()
        {
            _model.StringProp = "Sup";
            var result = _target.ValueButtonFor(x => x.StringProp);
            Assert.AreEqual(_model.StringProp, result.Text);
            Assert.AreEqual(_model.StringProp, result.Value);

            result = _target.ValueButton("StringProp");
            Assert.AreEqual(_model.StringProp, result.Text);
            Assert.AreEqual(_model.StringProp, result.Value);
        }

        #endregion

        #endregion

        #region Test classes

        private class Model
        {
            public string StringProp { get; set; }

            [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
            public DateTime FormattedDateProp { get; set; }

            [Required]
            public string RequiredProp { get; set; }

            public NestedModel NestedProp { get; set; }

            public List<object> ListProp { get; set; }

            public int IntProp { get; set; }

            [Select(ControllerViewDataKey = "SomeKey", DefaultItemLabel = "Some item label")]
            public int SelectAttributeProp { get; set; }

            public int? CascadingParentProperty { get; set; }

            [DropDown(Action = "CascadeAction", Controller = "Cascading", DependsOn = "CascadingParentProperty",
                HttpMethod = "Some http method", ErrorText = "Some error", LoadingText = "Some loading text",
                PromptText = "Some prompt text")]
            public int CascadingProperty { get; set; }

            [DropDown(Action = "CascadeAction", Controller = "Cascading", Area = "SomeArea",
                DependsOn = "CascadingParentProperty")]
            public int AreaCascadingProperty { get; set; }

            [DropDown(Action = "MultiParameterCascadeAction", Controller = "Cascading",
                DependsOn = "CascadingParentProperty, AreaCascadingProperty")]
            public int? MultiParameterCascadeProperty { get; set; }

            public int? CascadingParentPropertyThatIsNotRequired { get; set; }

            [DropDown(Action = "CascadeAction", Controller = "Cascading",
                DependsOn = "CascadingParentPropertyThatIsNotRequired", DependentsRequired = DependentRequirement.None,
                HttpMethod = "Some http method", ErrorText = "Some error", LoadingText = "Some loading text",
                PromptText = "Some prompt text")]
            public int CascadingPropertyWithoutRequiredParent { get; set; }

            [BoolFormat(True = "Sure", False = "Nah", Null = "Uh uh")]
            public bool? PropWithBoolFormat { get; set; }

            public bool? PropWithoutBoolFormat { get; set; }

            public DateTime DateProp { get; set; }

            [DateTimePicker]
            public DateTime DateTimeProp { get; set; }

            [Required]
            public DateTime? RequiredDateProp { get; set; }

            public MMSINC.Data.Range<DateTime> DateRangeProp { get; set; }

            public MMSINC.Data.Range<int> IntRangeProp { get; set; }

            #region AutoComplete

            [AutoComplete("SomeArea", "AutoComplete", "YouAutoCompleteMe")]
            public string AutoCompleteProp { get; set; }

            [EntityMustExist(typeof(EntityLookupModel))]
            [AutoComplete("SomeArea", "AutoComplete", "YouAutoCompleteMe",
                DisplayProperty = nameof(EntityLookupModel.Description))]
            public int? AutoCompleteEntityWithDisplayProperty { get; set; }

            [AutoComplete("SomeArea", "AutoComplete", "YouAutoCompleteMe",
                DisplayProperty = nameof(EntityLookupModel.Description))]
            public int? AutoCompleteEntityWithDisplayPropertyButNoEntityMustExistAttribute { get; set; }

            #endregion

            [FileUpload(FileTypes.Jpeg, Action = "UploadMe", Controller = "FileUpload")]
            public AjaxFileUpload FileUploadProp { get; set; }

            [Required]
            [FileUpload(FileTypes.Jpeg, Action = "UploadMe", Controller = "FileUpload")]
            public AjaxFileUpload RequiredFileUploadProp { get; set; }
        }

        private class NestedModel
        {
            [Required]
            public string RequiredStringProp { get; set; }
        }

        private class DropDownItemModel
        {
            public object Text { get; set; }
            public int Value { get; set; }
        }

        // needs to be public for Mock.
        public class EntityLookupModel : IEntityLookup
        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                throw new NotImplementedException();
            }

            public int Id { get; set; }
            public string Description { get; set; }
        }

        private class TestControlBuilder : ControlBuilder<TestControlBuilder>
        {
            protected override string CreateHtmlString()
            {
                throw new NotImplementedException();
            }
        }

        private class CascadingController : Controller
        {
            public CascadingActionResult Result { get; set; }
            public int? ParentValueUsed { get; private set; }
            public bool Disposed { get; private set; }

            public int? MultiParamFirstValue { get; private set; }
            public int? MultiParamSecondValue { get; private set; }

            public ActionResult CascadeAction(int id)
            {
                ParentValueUsed = id;
                return Result;
            }

            public ActionResult MultiParameterCascadeAction(int? id, int? somethingElse)
            {
                MultiParamFirstValue = id;
                MultiParamSecondValue = somethingElse;
                return Result;
            }

            protected override void Dispose(bool disposing)
            {
                Disposed = true;
                base.Dispose(disposing);
            }
        }

        private class AutoCompleteController : Controller
        {
            public ActionResult YouAutoCompleteMe(int someParam)
            {
                return null;
            }
        }

        private class FileUploadController : Controller
        {
            public ActionResult UploadMe()
            {
                return null;
            }
        }

        private class CascadeItemModel
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        #endregion
    }
}
