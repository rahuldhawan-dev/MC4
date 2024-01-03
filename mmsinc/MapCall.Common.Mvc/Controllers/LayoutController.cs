using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Validation;

namespace MapCall.Common.Controllers
{
    [AllowAnonymous]
    public class LayoutController : MMSINC.Controllers.ControllerBase
    {
        public ActionResult Index()
        {
            return View(new LayoutModel());
        }

        public ActionResult Cascade(int id)
        {
            var sli = new SelectListItem {Value = id.ToString(), Text = $"You selected {id}"};
            // This is so when dummy cascading data comes in, it always returns multiple results.
            var dummyValue = id + 100;
            var otherDummyValue = id + 200;
            var sli2 = new SelectListItem {Value = dummyValue.ToString(), Text = $"Dummy value {dummyValue}"};
            var sli3 = new SelectListItem {Value = otherDummyValue.ToString(), Text = $"Dummy value {otherDummyValue}"};
            return new CascadingActionResult(new[] {sli, sli2, sli3}, "Text", "Value");
        }

        public ActionResult CascadeNoResults(int id)
        {
            return new CascadingActionResult(Enumerable.Empty<SelectListItem>(), "Text", "Value");
        }

        public ActionResult CascadeBothParentsRequired(int? parent1, int? parent2)
        {
            SelectListItem sli;
            if (parent1.HasValue && parent2.HasValue)
            {
                sli = new SelectListItem {Value = "1", Text = "Both parents have values."};
            }
            else if (parent1.HasValue)
            {
                sli = new SelectListItem {Value = "1", Text = "Only first parent has value."};
            }
            else if (parent2.HasValue)
            {
                sli = new SelectListItem {Value = "1", Text = "Only second parent has value."};
            }
            else
            {
                sli = new SelectListItem {Value = "1", Text = "Both parents missing values."};
            }

            return new CascadingActionResult(new[] {sli}, "Text", "Value");
        }

        public ActionResult CascadeNoParentRequired(int? parent)
        {
            SelectListItem sli;
            if (parent == null)
            {
                sli = new SelectListItem {Value = "1", Text = "Parent does not have value."};
            }
            else
            {
                sli = new SelectListItem {Value = "1", Text = "Parent does have value."};
            }

            return new CascadingActionResult(new[] {sli}, "Text", "Value");
        }

        public ActionResult CascadeMultiple(int[] ids)
        {
            var results = ids.Select(x => new SelectListItem {Value = x.ToString(), Text = $"You selected {x}"})
                             .ToList();
            return new CascadingActionResult(results, "Text", "Value");
        }

        public ActionResult GetAutoCompleteDependsOnOptions(string partial, int id)
        {
            var results = new[] { new { id = 1, description = "one" }, new { id = 2, description = "two" } };
            return new AutoCompleteResult(results, "id", "description");
        }

        public class LayoutModel
        {
            #region Properties

            #region Cascading

            [DropDown]
            public int? RegularDropDown { get; set; }

            [DropDown]
            public int? RegularDropDownPreSelectedValue { get; set; }

            [Description("This should be on --Select--")]
            [DropDown("Layout", "Cascade", DependsOn = "RegularDropDownPreSelectedValue",
                PromptText = "Select a thing above")]
            public int? CascadeDropDownWithNoSelectedValue { get; set; }

            [Description("This should not have an empty Select value.")]
            [MultiSelect("Layout", "Cascade", DependsOn = "RegularDropDownPreSelectedValue",
                PromptText = "Select a thing above")]
            public int[] CascadeListBox { get; set; }

            #region Cascading when all parents are required and are not populated

            [DropDown]
            public int? CascadeParent1 { get; set; }

            [DropDown]
            public int? CascadeParent2 { get; set; }

            [DropDown]
            public int? CascadeParent3 { get; set; }

            [DropDown]
            public int? CascadeParent4 { get; set; }

            [DropDown]
            public int? CascadeParent5 { get; set; }

            [DropDown]
            public int? CascadeParent6 { get; set; }

            [DropDown("Layout", "CascadeBothParentsRequired", DependsOn = "CascadeParent1,CascadeParent2",
                DependentsRequired = DependentRequirement.All, PromptText = "Select a thing above")]
            public int? ChildRequiresBothNonPopulatedParents { get; set; }

            [DropDown("Layout", "CascadeBothParentsRequired", DependsOn = "CascadeParent1,CascadeParent2",
                DependentsRequired = DependentRequirement.One, PromptText = "Select a thing above")]
            public int? ChildRequiresAtleastOnePopulatedParent { get; set; }

            [DropDown("Layout", "CascadeBothParentsRequired", DependsOn = "CascadeParent3,CascadeParent4",
                DependentsRequired = DependentRequirement.One, PromptText = "Select a thing above")]
            public int? ChildPrepopulatesWithOnePopulatedParent { get; set; }

            [DropDown("Layout", "CascadeBothParentsRequired", DependsOn = "CascadeParent5,CascadeParent6",
                DependentsRequired = DependentRequirement.All, PromptText = "Select a thing above")]
            public int? ChildHasOneParentPrepopulatedButNeedsBoth { get; set; }

            #endregion

            #region Cascading when no parents are required

            [DropDown]
            public int? CascadeParentNotRequired { get; set; }

            [View(Description =
                "Selecting a value from CascadeParentNotRequired should filter, but it is not required to use this dropdown.")]
            [DropDown("Layout", "CascadeNoParentRequired", DependsOn = "CascadeParentNotRequired",
                DependentsRequired = DependentRequirement.None, PromptText = "Select from CascadeBothParentsRequired")]
            public int? ChildDoesNotRequireParent { get; set; }

            #endregion

            #region Cascading when there are no results

            [DropDown]
            public int? ParentThatWillNotReturnResults { get; set; }

            [DropDown("Layout", "CascadeNoResults", DependsOn = "ParentThatWillNotReturnResults",
                PromptText = "Select something from 'Parent That Will Not Return Results'")]
            public int? DropDownWithoutResults { get; set; }

            [MultiSelect("Layout", "CascadeNoResults", DependsOn = "ParentThatWillNotReturnResults",
                PromptText = "Select something from 'Parent That Will Not Return Results'")]
            public int? MultiSelectWithoutResults { get; set; }

            [CheckBoxList("Layout", "CascadeNoResults", DependsOn = "ParentThatWillNotReturnResults",
                PromptText = "Select something from 'Parent That Will Not Return Results'")]
            public int? CheckBoxListWithoutResults { get; set; }

            #endregion

            #endregion

            [Description("This text is from a description attribute.")]
            public string TextGoesHere { get; set; }

            [Multiline]
            public string MultilineText { get; set; }

            public int WholeNumbersOnly { get; set; }
            public bool IsItTrue { get; set; }
            public decimal Pi { get; set; }

            [DropDown]
            public int DropDown { get; set; }

            [DropDown]
            public int DropDownWithError { get; set; }
            
            [AutoComplete("Layout", "GetAutoCompleteDependsOnOptions", DependsOn = "DropDown")]
            public int? AutoCompleteDependsOn { get; set; }
            
            public DateTime TodaysDate { get; set; }
            public DateRange DateRange { get; set; }
            public RequiredDateRange RequiredDateRange { get; set; }
            public NumericRange NumericRange { get; set; }

            public DateRange EqualsDateRange { get; set; }

            [DateTimePicker, View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)]
            public DateTime DateTimePicker { get; set; }

            [Select(SelectType.CheckBoxList)]
            public List<int> CheckBoxing { get; set; }

            [UIHint("StringArray")]
            public string[] MultiInput { get; set; }

            [MultiString]
            public string[] MultiString { get; set; }

            public TableModelResultSet TableResultSet { get; set; }

            [ComboBox]
            public int? ComboBox { get; set; }

            public string ComboBoxTwoRequiresThis { get; set; }

            [ComboBox, RequiredWhen("ComboBoxTwoRequiresThis", ComparisonType.NotEqualTo, null)]
            public int? ComboBoxTwo { get; set; }

            #region CheckBoxList stuff

            [DropDown]
            public int? ParentOfCheckBoxList { get; set; }

            // TODO: Make this CheckBoxList once the actions are setup properly.
            [CheckBoxList("Layout", "Cascade", DependsOn = nameof(ParentOfCheckBoxList))]
            public int? CascadingCheckBoxList { get; set; }

            [DropDown("Layout", "CascadeMultiple", DependsOn = nameof(CascadingCheckBoxList))]
            public int? ChildOfCheckBoxList { get; set; }

            // These two are for testing that the client-side doesn't re-request 
            // data from the server when the parent has a value but filtering would
            // return no results. The server pre-renders no results, so the client
            // shouldn't be requesting the data again.
            public int ParentHasValueNotOnPage { get; set; }

            [DropDown("Layout", "CascadeNoResults", DependsOn = nameof(ParentHasValueNotOnPage))]
            public int PreRenderedChildShouldDisplayNoResultsAndNotMakeAServerRequest { get; set; }

            #endregion

            #endregion

            public LayoutModel()
            {
                TodaysDate = DateTime.Now;
                DateTimePicker = DateTime.Now;
                DateRange = new DateRange {
                    Start = DateTime.Today,
                    End = DateTime.Today.AddDays(7)
                };
                EqualsDateRange = new DateRange {
                    Start = DateTime.Today,
                    End = DateTime.Today.AddDays(22),
                    Operator = RangeOperator.Equal
                };

                // I dunno if this is supposed to be prepopulating when the page loads? -Ross 3/30/2017
                MultiInput = new[] {"Some value", "And another value"};

                MultiString = new[] {"Some value", "And another value"};

                var tableModels = new List<TableModel>();
                tableModels.Add(new TableModel {Id = 1, SomeDate = DateTime.Today, SomeString = "Neat"});
                tableModels.Add(new TableModel
                    {Id = 2, SomeDate = DateTime.Today.AddDays(1), SomeString = "Another value"});
                tableModels.Add(new TableModel
                    {Id = 3, SomeDate = DateTime.Today.AddDays(2), SomeString = "More values, more savings"});
                tableModels.Add(new TableModel {Id = 4, SomeDate = DateTime.Today.AddDays(3), SomeString = "A thing"});
                tableModels.Add(new TableModel
                    {Id = 5, SomeDate = DateTime.Today.AddDays(4), SomeString = "ioasdgoajsdog iaj"});
                tableModels.Add(new TableModel {
                    Id = 6, SomeDate = DateTime.Today.AddDays(5),
                    SomeString = "asdogahsdoug asoasoguho agoh sdgou ahsdog hgohasu"
                });
                tableModels.Add(new TableModel
                    {Id = 7, SomeDate = DateTime.Today.AddDays(6), SomeString = "WHYAREWESHOUTING"});
                tableModels.Add(new TableModel
                    {Id = 8, SomeDate = DateTime.Today.AddDays(7), SomeString = "I don't care what these say"});
                tableModels.Add(new TableModel
                    {Id = 9, SomeDate = DateTime.Today.AddDays(8), SomeString = "Blah blah blah blah"});
                tableModels.Add(new TableModel {Id = 10, SomeDate = DateTime.Today.AddDays(9), SomeString = "Yes"});
                tableModels.Add(new TableModel {Id = 11, SomeDate = DateTime.Today.AddDays(10), SomeString = "Nah"});
                tableModels.Add(new TableModel
                    {Id = 12, SomeDate = DateTime.Today.AddDays(11), SomeString = "4124215181 181825128"});
                tableModels.Add(new TableModel {Id = 13, SomeDate = DateTime.Today.AddDays(12), SomeString = "k"});
                TableResultSet = new TableModelResultSet {
                    Results = tableModels,
                    Count = 4129,
                    PageCount = 42,
                    PageSize = 10
                };

                ParentHasValueNotOnPage = 42;
                // Don't add rows in the constructor, it'll cause a stackoverflow.
            }
        }

        public class TableModel
        {
            public int Id { get; set; }
            public string SomeString { get; set; }
            public DateTime SomeDate { get; set; }
        }

        public class TableModelResultSet : SearchSet<TableModel> { }

        public LayoutController(ControllerBaseArguments args) : base(args) { }
    }
}
