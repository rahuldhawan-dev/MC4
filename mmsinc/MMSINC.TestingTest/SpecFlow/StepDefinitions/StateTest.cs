using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using Moq;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MMSINC.TestingTest.SpecFlow.StepDefinitions
{
    //[TestClass]
    //public class StateTest : StepDefinitionTest<TestUser>
    //{
    //    #region Init/Cleanup

    //    private TestUser _testUser;

    //    [TestInitialize]
    //    public void TestInitialize()
    //    {
    //        BaseTestInitialize();
    //        _testUser = CreateCachedTestObject<TestUser>("bob", overrides: new {
    //            Email = "foo@bar.baz",
    //            CreatedAt = DateTime.Now
    //        });
    //    }

    //    [TestCleanup]
    //    public void TestCleanup()
    //    {
    //        BaseTestCleanup();
    //    }

    //    protected override Type StepDefinitionClass
    //    {
    //        get { return typeof(State); }
    //    }

    //    protected override Assembly ModelAssembly
    //    {
    //        get { return typeof(TestUser).Assembly; }
    //    }

    //    #endregion

    //    #region Page Content

    //    private void SetupElementWithText(string text, bool visible)
    //    {
    //        var div = new Mock<IDiv>();
    //        div.Setup(x => x.Text).Returns(text);
    //        div.Setup(x => x.Exists).Returns(true);
    //        div.Setup(x => x.IsVisible()).Returns(visible);
    //        var divObj = div.Object;
    //        _browser.Setup(x => x.ContainsText(text)).Returns(true);
    //        _browser.Setup(x => x.IElement(ConstraintMatch(Find.ByText(text)))).Returns(divObj);
    //    }

    //    [TestMethod]
    //    public void TestIShouldSeeText()
    //    {
    //        TestFor("Then I should see \"foo\"", t => {
    //            t.ShallPass(() => SetupElementWithText("foo", true));

    //            t.NoneShallPass(
    //                () => _browser.Setup(x => x.ContainsText("foo")).Returns(false),
    //                () => SetupElementWithText("foo", false));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeTheCurrentHostnameAndText()
    //    {
    //        TestFor("Then I should see the current hostname and \"foo\"", t => {
    //            t.ShallPass(() => _browser.Setup(x => x.ContainsText(Environment.MachineName.ToLowerInvariant() + "foo")).Returns(true));

    //            t.NoneShallPass(() => _browser.Setup(x => x.ContainsText(Environment.MachineName.ToLowerInvariant() + "foo")).Returns(false));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeTextInFrame()
    //    {
    //        var frame = new Mock<IFrame>();

    //        TestFor("Then I should see \"foo\" in frame bar", t => {
    //            t.ShallPass(() => {
    //                _browser.Setup(x => x.IFrame("bar")).Returns(frame.Object);
    //                frame.Setup(x => x.ContainsText("foo")).Returns(true);
    //            });

    //            // TODO: what happens when the frame isn't found?
    //            t.NoneShallPass(() => {
    //                _browser.Setup(x => x.IFrame("bar")).Returns(frame.Object);
    //                frame.Setup(x => x.ContainsText("foo")).Returns(false);
    //            });
    //        });
    //    }

    //    private void SetupDisplayForWithValue(string fieldName, string value)
    //    {
    //        var label = new Mock<ILabel>();
    //        var labelObj = label.Object;
    //        var div = new Mock<IDiv>();
    //        var field = new Mock<IElement>();

    //        _browser.Setup(x => x.TryGetElement(ConstraintMatch(Find.ByFor(fieldName)), out labelObj)).Returns(true);
    //        label.Setup(x => x.IAncestor(ConstraintMatch(FindBy.Class("field-pair")))).Returns(div.Object);
    //        div.Setup(x => x.IChild(ConstraintMatch(FindBy.Class("field")))).Returns(field.Object);
    //        field.SetupGet(x => x.Exists).Returns(true);
    //        field.SetupGet(x => x.Text).Returns(value);
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeTextIn()
    //    {
    //        ILabel labelObj;

    //        foreach (var conjunctiveAdverb in new[] {"Given I can", "When I", "Then I should"})
    //        {
    //            TestFor(conjunctiveAdverb + " see a display for foo with \"bar\"", t => {
    //                t.ShallPass(() => SetupDisplayForWithValue("foo", "bar"));

    //                t.NoneShallPass(
    //                    // text doesn't match
    //                    () => SetupDisplayForWithValue("foo", "baz"),
    //                    // label not found
    //                    () =>
    //                        _browser.Setup(x => x.TryGetElement(ConstraintMatch(Find.ByFor("foo")), out labelObj))
    //                            .Returns(false));
    //            });
    //        }
    //    }

    //    #region I should/n't see the button

    //    public static Mock<IButton> SetupButtonWithAttribute<TParent>(string attr, string value, Mock<TParent> parent, bool exists = true, bool visible = true)
    //        where TParent : class, Testing.WatiN.IElementContainer
    //    {
    //        var button = new Mock<IButton>();

    //        parent.Setup(x => x.IElementOfType<IButton>(
    //                It.Is<Constraint>(c => c.Matches(new TestAttributeBag {{attr, value}}, new ConstraintContext()))))
    //            .Returns(button.Object);
    //        button.SetupGet(x => x.Exists).Returns(exists);
    //        button.Setup(x => x.IsVisible()).Returns(visible);

    //        return button;
    //    }

    //    private Mock<IButton> SetupButtonWithAttribute(string attr, string value, bool exists = true, bool visible = true)
    //    {
    //        return SetupButtonWithAttribute(attr, value, _browser, exists, visible);
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeButton()
    //    {
    //        var id = "foo";
    //        TestFor(String.Format("Then I should see the button \"{0}\"", id), t => {
    //            t.ShallPass(() => SetupButtonWithAttribute("id", id));

    //            t.NoneShallPass(() => SetupButtonWithAttribute("id", id, false),
    //                () => SetupButtonWithAttribute("id", id, true, false));
    //        });

    //        TestFor(String.Format("Then I should not see the button \"{0}\"", id), t => {
    //            t.ShallPass(() => SetupButtonWithAttribute("id", id, false),
    //                () => SetupButtonWithAttribute("id", id, true, false));

    //            t.NoneShallPass(() => SetupButtonWithAttribute("id", id));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeButtonUnderTab()
    //    {
    //        var id = "foo";
    //        var tabName = "foo tab";
    //        TestFor(String.Format("Then I should see the button \"{0}\" under the \"{1}\" tab", id, tabName), t => {
    //            t.ShallPass(() => {
    //                var tab = SetupTab(tabName);
    //                SetupButtonWithAttribute("id", id, tab);
    //            });

    //            t.NoneShallPass(() => {
    //                var tab = SetupTab(tabName);
    //                SetupButtonWithAttribute("id", id, tab, false);
    //            }, () => {
    //                var tab = SetupTab(tabName);
    //                SetupButtonWithAttribute("id", id, tab, true, false);
    //            });
    //        });

    //        TestFor(String.Format("Then I should not see the button \"{0}\" under the \"{1}\" tab", id, tabName), t => {
    //            t.ShallPass(() => {
    //                var tab = SetupTab(tabName);
    //                SetupButtonWithAttribute("id", id, tab, false);
    //            }, () => {
    //                var tab = SetupTab(tabName);
    //                SetupButtonWithAttribute("id", id, tab, true, false);
    //            });

    //            t.NoneShallPass(() => {
    //                var tab = SetupTab(tabName);
    //                SetupButtonWithAttribute("id", id, tab);
    //            });
    //        });
    //    }

    //    #endregion

    //    #region ThenIShouldSeeTextInField

    //    [TestMethod]
    //    public void TestThenIShouldSeeTextInField()
    //    {
    //        ILabel labelObj;

    //        TestFor("Then I should see a display for test user: \"bob\"'s Email", t => {
    //            t.ShallPass(() => SetupDisplayForWithValue("Email", _testUser.Email));

    //            t.NoneShallPass(
    //                // text doesn't match
    //                () => SetupDisplayForWithValue("Email", "not the test user's email address"),
    //                // label not found
    //                () =>
    //                    _browser.Setup(x => x.TryGetElement(ConstraintMatch(Find.ByFor("Email")), out labelObj))
    //                        .Returns(false));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeTextInFieldConsidersFormatIfModelPropertyHasOneSpecified()
    //    {
    //        TestFor("Then I should see a display for test user: \"bob\"'s CreatedAt", t => {
    //            t.ShallPass(() => SetupDisplayForWithValue("CreatedAt", String.Format("{0:d}", _testUser.CreatedAt)));

    //            t.NoneShallPass(
    //                // unformatted
    //                () => SetupDisplayForWithValue("CreatedAt", _testUser.CreatedAt.ToString()),
    //                // wrong format
    //                () => SetupDisplayForWithValue("CreatedAt", String.Format("{0:MM/dd/yy H:mm:ss zzz}", _testUser.CreatedAt)));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeTextInFieldAllowsForEmptyOrNullValues()
    //    {
    //        TestFor("Then I should see a display for test user: \"bob\"'s UpdatedAt",
    //            t => t.ShallPass(() => SetupDisplayForWithValue("UpdatedAt", String.Empty)));
    //    }

    //    #endregion

    //    #region ThenIShouldSeeNamedDataInDisplayFor

    //    [TestMethod]
    //    public void TestThenIShouldSeeNamedDataInDisplayFor()
    //    {
    //        TestFor("Then I should see a display for Buh with test user \"bob\"'s Email", t => t.ShallPass(() => SetupDisplayForWithValue("Buh", _testUser.Email)));
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeNamedDataInDisplayFor2ElectricBoogaloo()
    //    {
    //        TestFor("Then I should see a display for Buh with test user \"bob\"", t => t.ShallPass(() => SetupDisplayForWithValue("Buh", _testUser.ToString())));
    //    }

    //    #endregion

    //    // skip a few...

    //    private Mock<IDiv> SetupTab(string label, bool exists = true)
    //    {
    //        return InputTest.SetupTab(label, _browser, exists);
    //    }

    //    private void SetupTableWithValueInColumn(string columnName, string value, string tableId = null, bool tableExists = true)
    //    {
    //        SetupTableWithValueInColumn(columnName, value, _browser, tableId, tableExists);
    //    }

    //    private void SetupTableWithValueInColumn<TParent>(string columnName, string value, Mock<TParent> parent, string tableId = null, bool tableExists = true)
    //        where TParent : class, Testing.WatiN.IElementContainer
    //    {
    //        var table = new Mock<ITable>();
    //        var tableRows = new Mock<ITableRowCollection>();
    //        var headerRows = new Mock<ITableRowCollection>();
    //        var headerRow = new Mock<ITableRow>();
    //        var headerCells = new Mock<IElementCollection<Element, IElement>>();
    //        var headerCell = new Mock<ITableCell>();
    //        var headerCellArr = new[] {headerCell.Object};
    //        var tableRow = new Mock<ITableRow>();
    //        var tableRowArr = new[] {tableRow.Object};
    //        var tableCells = new Mock<ITableCellCollection>();
    //        var tableCell = new Mock<ITableCell>();
    //        var tableCellArr = new[] {tableCell.Object};

    //        if (String.IsNullOrWhiteSpace(tableId))
    //        {
    //            parent.Setup(x => x.ITables.First()).Returns(table.Object);
    //        }
    //        else
    //        {
    //            parent.Setup(x => x.IElementOfType<ITable>(
    //                //It.Is<Constraint>(c => c.ToString() == Find.ById(tableId).ToString())))
    //                It.IsAny<Constraint>()))
    //                .Returns(table.Object);
    //        }
    //        table.Setup(x => x.Exists).Returns(tableExists);
    //        table.Setup(x => x.ITableRows).Returns(tableRows.Object);
    //        tableRows.Setup(x => x.Filter(ConstraintMatch(TableRow.IsHeaderRow()))).Returns(headerRows.Object);
    //        headerRows.SetupGet(x => x.Count).Returns(1);
    //        headerRows.Setup(x => x[0]).Returns(headerRow.Object);
    //        headerRow.Setup(x => x.IElementsWithTag("th")).Returns(headerCells.Object);
    //        headerCells.Setup(x => x.GetEnumerator()).Returns(headerCellArr.TakeWhile(_ => true).GetEnumerator());
    //        headerCell.SetupGet(x => x.Text).Returns(columnName);
    //        headerCells.SetupGet(x => x.Count).Returns(1);
    //        tableRows.Setup(x => x.GetEnumerator()).Returns(tableRowArr.TakeWhile(_ => true).GetEnumerator());
    //        tableRow.SetupGet(x => x.ITableCells).Returns(tableCells.Object);
    //        tableCells.Setup(x => x.Filter(ConstraintMatch(Find.ByIndex(0)))).Returns(tableCells.Object);
    //        tableCells.Setup(x => x.GetEnumerator()).Returns(tableCellArr.TakeWhile(_ => true).GetEnumerator());
    //        tableCell.SetupGet(x => x.Text).Returns(value);
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeTextInTheNamedColumn()
    //    {
    //        TestFor("Then I should see test user \"bob\"'s Email in the \"Email\" column", t => {
    //            t.ShallPass(() => SetupTableWithValueInColumn("Email", _testUser.Email, _browser));

    //            t.NoneShallPass( // text doesn't match
    //                () => SetupTableWithValueInColumn("Email", "not the test user's email address", _browser));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeFormattedTextInTheNamedColumn()
    //    {
    //        TestFor("Then I should see test user \"bob\"'s CreatedAt as a date in the \"Created At\" column",t => {
    //            t.ShallPass(() => SetupTableWithValueInColumn("Created At", String.Format("{0:d}", _testUser.CreatedAt)));

    //            t.NoneShallPass(
    //                // wrong value
    //                () => SetupTableWithValueInColumn("Created At", String.Format("{0:d}", _testUser.CreatedAt.Value.AddDays(1))),
    //                // wrong format
    //                () => SetupTableWithValueInColumn("Created At", String.Format("{0:M/d/yyyy h:mm tt}", _testUser.CreatedAt)),
    //                // unformatted
    //                () => SetupTableWithValueInColumn("Created At", _testUser.CreatedAt.ToString()));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeNamedTypesPropertyFormattedAsInTheNamedTablesNamedColumn()
    //    {
    //        TestFor("Then I should see test user \"bob\"'s CreatedAt as a date in the table fooTable's \"Created At\" column",
    //            t => {
    //                t.ShallPass(
    //                    () => SetupTableWithValueInColumn("Created At", String.Format("{0:d}", _testUser.CreatedAt), "fooTable"));
    //            });
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeNamedTypesPropertyInTheNamedTablesNamedColumn()
    //    {
    //        TestFor(
    //            "Then I should see test user \"bob\"'s Email in the table fooTable's \"Email\" column",
    //            t =>
    //                t.ShallPass(() => {
    //                    SetupTableWithValueInColumn("Email", _testUser.Email, "fooTable");
    //                })

    //            &&

    //            t.NoneShallPass(
    //                () => { // table does not exist
    //                    var tab = SetupTab("fooTab");
    //                    SetupTableWithValueInColumn("Email", _testUser.Email, "fooTable",  false);
    //                },
    //                () => { // I prefer to pay in cash at the bar
    //                    var tab = SetupTab("fooTab", false);
    //                    SetupTableWithValueInColumn("Email", _testUser.Email, tab, "fooTable");
    //                }
    //            ));
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeNamedTypesPropertyInTheNamedTablesNamedColumnUnderTab()
    //    {
    //        TestFor(
    //            "Then I should see test user \"bob\"'s Email in the table fooTable's \"Email\" column under the \"fooTab\" tab",
    //            t =>
    //                t.ShallPass(() => {
    //                    var tab = SetupTab("fooTab");
    //                    SetupTableWithValueInColumn("Email", _testUser.Email, tab, "fooTable");
    //                })

    //            &&

    //            t.NoneShallPass(
    //                () => { // table does not exist
    //                    var tab = SetupTab("fooTab");
    //                    SetupTableWithValueInColumn("Email", _testUser.Email, tab, "fooTable",  false);
    //                },
    //                () => { // I prefer to pay in cash at the bar
    //                    var tab = SetupTab("fooTab", false);
    //                    SetupTableWithValueInColumn("Email", _testUser.Email, tab, "fooTable");
    //                }
    //            ));
    //    }

    //    #region Links

    //    private void SetupSecureLinkToPageFor(string action, string className, int identity, bool exists)
    //    {
    //        // it's not calling a .net library method to generate the url, so we do need to
    //        // generate it ourselves in the test to compare with
    //        var link = new Mock<ILink>(); 
    //        _browser.Setup(x =>
    //            x.ILink(ConstraintMatch(Find.ByUrl(
    //                new Regex(String.Format("http://localhost:8888/{0}/{1}/{2}\\?{3}=.+", className.ToLower(), action,
    //                    identity, FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME)))))).Returns(link.Object);
    //        link.SetupGet(x => x.Exists).Returns(exists);
    //    }

    //    [TestMethod]
    //    public void TestThenIShouldSeeASecureLinkToThePageFor()
    //    {
    //        TestFor("Then I should see a secure link to the Fldsmdfr page for test user: \"bob\"", t =>
    //        {
    //            t.ShallPass(() => SetupSecureLinkToPageFor("Fldsmdfr", _testUser.GetType().Name, _testUser.Id, true));
    //            t.NoneShallPass(() => SetupSecureLinkToPageFor("Fldsmdfr", _testUser.GetType().Name, _testUser.Id, false));
    //        });

    //        var negatives = new[] {"should not", "shouldn't", "shant"};
    //        foreach (var negative in negatives)
    //        {
    //            TestFor(String.Format("Then I {0} see a secure link to the Fldsmdfr page for test user: \"bob\"", negative ), t => {
    //                t.ShallPass(
    //                    () => SetupSecureLinkToPageFor("Fldsmdfr", _testUser.GetType().Name, _testUser.Id, false));
    //                t.NoneShallPass(
    //                    () => SetupSecureLinkToPageFor("Fldsmdfr", _testUser.GetType().Name, _testUser.Id, true));
    //            });
    //        }
    //    }

    //    #endregion

    //    #region Selects

    //    private void SetupSelectWithValue(string select, string value)
    //    {
    //        var selectList = new Mock<ISelectList>();

    //        var option = new Mock<IOption>();
    //        option.Setup(x => x.Exists).Returns(true);
    //        _browser.Setup(x => x.ISelectList(ConstraintMatch(Find.ById(select)))).Returns(selectList.Object);
    //        selectList.Setup(x => x.Exists).Returns(true);
    //        selectList.Setup(x => x.Option(value)).Returns(option.Object);
    //    }

    //    [TestMethod]
    //    public void TestThenStringShouldBeSelectedInTheDropDown()
    //    {
    //        TestFor("Then \"Foo\" should not be selected in the ElementName dropdown", t => {
    //            t.ShallPass(() => SetupSelectWithValue("ElementName", "Foo"));

    //            t.NoneShallPass(
    //                () => SetupSelectWithValue("ElementName", "Bar"));
    //        });
    //    }

    //    #endregion

    //    #endregion
    //}
}
