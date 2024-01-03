using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using Moq;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace MMSINC.TestingTest.SpecFlow.StepDefinitions
{
    //[TestClass]
    //public class InputTest : StepDefinitionTest<TestUser>
    //{
    //    private TestUser _testUser;

    //    #region Init/Cleanup

    //    #region Properties

    //    protected override Type StepDefinitionClass
    //    {
    //        get { return typeof(Input); }
    //    }

    //    protected override Assembly ModelAssembly
    //    {
    //        get { return typeof(TestUser).Assembly; }
    //    }

    //    #endregion

    //    #region Setup/Teardown

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

    //    #endregion

    //    #region Methods

    //    private Mock<IDiv> SetupTab(string label, bool exists = true)
    //    {
    //        return SetupTab(label, _browser, exists);
    //    }

    //    public static Mock<IDiv> SetupTab(string label, Mock<ISlightlyBetterBrowser> browser, bool exists = true)
    //    {
    //        var tabsContainer = new Mock<IDiv>();
    //        var tab = new Mock<ILink>();
    //        var contentDiv = new Mock<IDiv>();

    //        browser.Setup(x => x.IElementOfType<IDiv>(
    //            It.Is<Constraint>(c => c.Matches(new TestAttributeBag {{"className", "tabs-container"}}, new ConstraintContext()))))
    //            .Returns(tabsContainer.Object);
    //        tabsContainer.SetupGet(x => x.Exists).Returns(exists);
    //        tabsContainer.Setup(x => x.IElementOfType<ILink>(
    //            It.Is<Constraint>(c => c.Matches(new TestAttributeBag {{"innertext", label}}, new ConstraintContext()))))
    //            .Returns(tab.Object);
    //        tab.SetupGet(x => x.Exists).Returns(true);
    //        tab.SetupGet(x => x.Url).Returns("someTab#whoCares");
    //        browser.Setup(x => x.IElementOfType<IDiv>(
    //            It.Is<Constraint>(c => c.Matches(new TestAttributeBag {{"id", "whoCares"}}, new ConstraintContext()))))
    //            .Returns(contentDiv.Object);
    //        contentDiv.SetupGet(x => x.Exists).Returns(true);

    //        return contentDiv;
    //    }

    //    #endregion

    //    #endregion

    //    #region Value Elements (Inputs, Selects, etc.)

    //    private Mock<ICheckBox> SetupCheckboxWithNameAndValue<TParent>(string name, string value, Mock<TParent> parent, bool exists = true)
    //        where TParent : class, Testing.WatiN.IElementContainer
    //    {
    //        var checkBox = new Mock<ICheckBox>();
    //        parent.Setup(x => x.IElementOfType<ICheckBox>(
    //            It.Is<Constraint>(c => c.ToString() == Find.ByName(name).And(Find.ByValue(value)).ToString())))
    //              //It.IsAny<Constraint>()))
    //            .Returns(checkBox.Object);
    //        checkBox.SetupGet(x => x.Exists).Returns(exists);
    //        return checkBox;
    //    }

    //    [TestMethod]
    //    public void TestWhenIClickTheCheckbox()
    //    {
    //        var name = "foo";
    //        var value = "bar";
    //        TestFor(String.Format("When I click the checkbox named {0} with the value \"{1}\"", name, value), t => {
    //            t.ShallPass(() => {
    //                var chk = SetupCheckboxWithNameAndValue(name, value, _browser);
    //                chk.SetupSet(x => x.Checked = true);
    //                _browser.Setup(x => x.FireJQueryEvent(chk.Object, JQueryEventType.Change));
    //            });

    //            t.NoneShallPass(() => SetupCheckboxWithNameAndValue(name, value, _browser, false));
    //        });
    //    }

    //    [TestMethod]
    //    public void TestWhenIClickTheCheckBoxWithNamedDataObjectThingy()
    //    {
    //        var name = "foo";
    //        TestFor(String.Format("When I click the checkbox named {0} with test user \"bob\"'s Email", name), t => {
    //            t.ShallPass(() => {
    //                var chk = SetupCheckboxWithNameAndValue(name, _testUser.Email, _browser);
    //                chk.SetupSet(x => x.Checked = true);
    //                _browser.Setup(x => x.FireJQueryEvent(chk.Object, JQueryEventType.Change));
    //            });
    //        });
    //    }

    //    [TestMethod]
    //    public void TestWhenIClickTheCheckBoxWithNamedDataObjectThingyUnderTheTab()
    //    {
    //        var name = "foo";
    //        var tabName = "bar";
    //        TestFor(String.Format("When I click the checkbox named {0} with test user \"bob\"'s Email under the \"{1}\" tab", name, tabName), t => {
    //            t.ShallPass(() => {
    //                var tab = SetupTab(tabName);
    //                var chk = SetupCheckboxWithNameAndValue(name, _testUser.Email, tab);
    //                chk.SetupSet(x => x.Checked = true);
    //                _browser.Setup(x => x.FireJQueryEvent(chk.Object, JQueryEventType.Change));
    //            });
    //        });
    //    }

    //    #endregion

    //    #region Buttons, Links, and Tabs

    //    private Mock<IButton> SetupSingleButtonWithAttribute<TParent>(string attr, string text, Mock<TParent> parent) where TParent : class, Testing.WatiN.IElementContainer
    //    {
    //        var button = new Mock<IButton>();
    //        var coll = new Mock<IButtonCollection>();
    //        parent.Setup(x => x.IButtons.Filter(
    //                It.Is<Constraint>(c => c.Matches(new TestAttributeBag {{attr, text}}, new ConstraintContext()))))
    //            .Returns(coll.Object);
    //        coll.Setup(x => x.Any()).Returns(true);
    //        coll.SetupGet(x => x.Count).Returns(1);
    //        coll.Setup(x => x.Single()).Returns(button.Object);
    //        return button;
    //    }

    //    private void SetupNButtonsWithAttribute(int count, string attr, string text)
    //    {
    //        var coll = new Mock<IButtonCollection>();
    //        _browser.SetupGet(x => x.IButtons).Returns(coll.Object);
    //        coll.Setup(
    //            x => x.Filter(It.Is<Constraint>(c => c.Matches(new TestAttributeBag {{attr, text}}, new ConstraintContext()))))
    //            .Returns(coll.Object);
    //        coll.Setup(x => x.Any()).Returns(count > 0);
    //        coll.SetupGet(x => x.Count).Returns(count);
    //    }

    //    private void SetupNButtonsWithAttributeInFrame(int count, string attr, string text, Mock<IFrame> container)
    //    {
    //        var coll = new Mock<IButtonCollection>();
    //        container.SetupGet(x => x.IButtons).Returns(coll.Object);
    //        coll.Setup(
    //            x => x.Filter(It.Is<Constraint>(c => c.Matches(new TestAttributeBag {{attr, text}}, new ConstraintContext()))))
    //            .Returns(coll.Object);
    //        coll.Setup(x => x.Any()).Returns(count > 0);
    //        coll.SetupGet(x => x.Count).Returns(count);
    //    }

    //    [TestMethod]
    //    public void TestWhenIPressAButton()
    //    {
    //        var text = "foo";
    //        var attributes = new[] {"id", "name", "value", "innertext"};
    //        var steps = new[] {
    //            "When I press {0}",
    //            //"When I press \"{0}\"",
    //            "Given I have pressed {0}",
    //            //"Given I have pressed \"{0}\""
    //        };
    //        steps.Each(step =>
    //            TestFor(String.Format(step, text), t => {
    //                attributes.Each(attr => {
    //                    t.ShallPass(() => {
    //                        var button = SetupSingleButtonWithAttribute(attr, text, _browser);
    //                        button.Setup(b => b.Click());
    //                    });

    //                    t.NoneShallPass(() => SetupNButtonsWithAttribute(2, attr, text),
    //                        () => SetupNButtonsWithAttribute(0, attr, text));
    //                });
    //            }));
    //    }

    //    [TestMethod]
    //    public void TestWhenIPressAButtonAndDoNotWait()
    //    {
    //        var text = "foo";
    //        var attributes = new[] {"id", "name", "value", "innertext"};
    //        var steps = new[] {
    //            "When I press {0} and do not wait",
    //            // having trouble matching the regex for this one:
    //            //"When I press \"{0}\" and do not wait",
    //            "Given I have pressed {0} and did not wait",
    //            //"Given I have pressed \"{0}\" and did not wait"
    //        };
    //        steps.Each(step => {
    //            attributes.Each(attr => {
    //                try
    //                {
    //                    TestFor(String.Format(step, text), t => {
    //                        t.ShallPass(() => {
    //                            var button = SetupSingleButtonWithAttribute(attr, text, _browser);
    //                            button.Setup(b => b.ClickNoWait());
    //                        });

    //                        t.NoneShallPass(() => SetupNButtonsWithAttribute(2, attr, text),
    //                            () => SetupNButtonsWithAttribute(0, attr, text));
    //                    });

    //                }
    //                catch (Exception ex)
    //                {
    //                    throw new Exception(String.Format("attribute {0} was a problem for step {1}", attr, step), ex);
    //                }
    //            });
    //        });
    //    }

    //    [TestMethod]
    //    public void TestWhenIPressAButtonInAFrame()
    //    {
    //        var text = "foo";
    //        var frameName = "someFrame";
    //        var attributes = new[] {"id", "name", "value", "innertext"};
    //        var steps = new[] {
    //            "When I press {0} in frame {1}",
    //            //"When I press \"{0}\" in frame {1}",
    //            "Given I have pressed {0} in frame {1}",
    //            //"Given I have pressed \"{0}\" in frame {1}"
    //        };
    //        steps.Each(step =>
    //            TestFor(String.Format(step, text, frameName), t => {
    //                attributes.Each(attr => {
    //                    var frame = new Mock<IFrame>();
    //                    _browser.Setup(x => x.IFrame(frameName)).Returns(frame.Object);

    //                    t.ShallPass(() => {
    //                        var button = SetupSingleButtonWithAttribute(attr, text, frame);
    //                        button.Setup(b => b.Click());
    //                    });

    //                    t.NoneShallPass(() => SetupNButtonsWithAttributeInFrame(2, attr, text, frame),
    //                        () => SetupNButtonsWithAttributeInFrame(0, attr, text, frame));
    //                });
    //            }));
    //    }

    //    [TestMethod]
    //    public void TestWhenIPressAButtonInATab()
    //    {
    //        var text = "foo";
    //        var tabLabel = "some tab";
    //        var attributes = new[] {"id", "name", "value", "innertext"};
    //        var steps = new[] {
    //            "When I press {0} under the \"{1}\" tab",
    //            //"When I press \"{0}\" under the \"{1}\" tab"
    //        };
    //        steps.Each(step =>
    //            TestFor(String.Format(step, text, tabLabel), t => attributes.Each(attr => t.ShallPass(() => {
    //                var div = SetupTab(tabLabel);
    //                var button = SetupSingleButtonWithAttribute(attr, text, div);
    //                button.Setup(b => b.Click());
    //            }))));
    //    }

    //    #endregion
    //}

    //public class TestAttributeBag : IAttributeBag, IEnumerable
    //{
    //    #region Private Members

    //    private readonly NameValueCollection _attributes;

    //    #endregion

    //    #region Constructors

    //    public TestAttributeBag()
    //    {
    //        _attributes = new NameValueCollection();
    //    }

    //    #endregion

    //    #region Exposed Methods

    //    public void Add(string name, string value)
    //    {
    //        _attributes.Add(name, value);
    //    }

    //    public string GetAttributeValue(string attributeName)
    //    {
    //        return _attributes[attributeName];
    //    }

    //    public T GetAdapter<T>() where T : class
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IEnumerator GetEnumerator()
    //    {
    //        return _attributes.GetEnumerator();
    //    }

    //    #endregion
    //}
}
