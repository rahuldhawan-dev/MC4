using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using Moq;

namespace MMSINC.Core.MvcTest.Results
{
    [TestClass]
    public class AutoCompleteResultTest
    {
        #region Fields

        private TestAutoCompleteResult _target;

        #endregion

        #region Init/cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestAutoCompleteResult();
            _target.DataProperty = "DataProp";
            _target.LabelProperty = "LabelProp";
            _target.ValueProperty = "ValueProp";
        }

        #endregion

        #region Tests

        #region Constructors

        [TestMethod]
        public void TestConstructorSetsDataInDataOverload()
        {
            var expected = new[] {"string"};
            var target = new AutoCompleteResult(expected, "data", "label");
            Assert.AreSame(expected, target.Data);
        }

        [TestMethod]
        public void TestConstructorSetsDataPropertyValue()
        {
            var expected = new[] {"string"};
            var target = new AutoCompleteResult(expected, "data", "label");
            Assert.AreSame("data", target.DataProperty);
        }

        [TestMethod]
        public void TestConstructorSetsLabelPropertyValue()
        {
            var expected = new[] {"string"};
            var target = new AutoCompleteResult(expected, "data", "label");
            Assert.AreSame("label", target.LabelProperty);
        }

        [TestMethod]
        public void TestConstructorSetsValuePropertyValue()
        {
            var expected = new[] {"string"};
            var target = new AutoCompleteResult(expected, "data", "label");
            Assert.IsNull(target.ValueProperty);
            target = new AutoCompleteResult(expected, "data", "label", "value");
            Assert.AreEqual("value", target.ValueProperty);
        }

        [TestMethod]
        public void TestConstructorsSetJsonBehaviorToAllowGetByDefault()
        {
            var target = new AutoCompleteResult();
            Assert.AreEqual(JsonRequestBehavior.AllowGet, target.JsonRequestBehavior);

            target = new AutoCompleteResult(null, null, null);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, target.JsonRequestBehavior);
        }

        #endregion

        #region Properties

        [TestMethod]
        public void TestDataPropertyGetsAndSets()
        {
            var expected = new[] {"some string"};
            _target.Data = expected;
            Assert.AreSame(expected, _target.Data);
        }

        [TestMethod]
        public void TestJsonRequestBehaviorPropertyGetsAndSets()
        {
            _target.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Assert.AreEqual(JsonRequestBehavior.AllowGet, _target.JsonRequestBehavior);
        }

        [TestMethod]
        public void TestDataPropertyPropertyGetsAndSets()
        {
            _target.DataProperty = "Text";
            Assert.AreEqual("Text", _target.DataProperty);
        }

        [TestMethod]
        public void TestLabelPropertyPropertyGetsAndSets()
        {
            _target.LabelProperty = "Text";
            Assert.AreEqual("Text", _target.LabelProperty);
        }

        [TestMethod]
        public void TestValuePropertyPropertyGetsAndSets()
        {
            _target.ValueProperty = "Text";
            Assert.AreEqual("Text", _target.ValueProperty);
        }

        #endregion

        #region CreateJsonResult

        [TestMethod]
        public void TestCreateJsonResultReturnsJsonResultObject()
        {
            _target.Data = new List<object>();
            MyAssert.IsInstanceOfType<JsonResult>(_target.CallCreateJsonResult());
        }

        [TestMethod]
        public void TestCreateJsonResultSetsJsonBehaviorOnJsonResultObject()
        {
            _target.Data = new List<object>();
            _target.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var result = (JsonResult)_target.CallCreateJsonResult();
            Assert.AreEqual(_target.JsonRequestBehavior, result.JsonRequestBehavior);
        }

        [TestMethod]
        public void TestCreateJsonResultReturnsExpectedSerializableData()
        {
            _target.Data = new[] {new Item {DataProp = 32, LabelProp = "Label", ValueProp = "Value"}};
            var result = (IEnumerable<object>)((JsonResult)_target.CallCreateJsonResult()).Data;

            Assert.AreEqual(1, result.Count());
            var serialized = (Dictionary<string, object>)result.Single();
            Assert.AreEqual(32, serialized["data"]);
            Assert.AreEqual("Label", serialized["label"]);
            Assert.AreEqual("Value", serialized["value"]);
        }

        #endregion

        #region Execute

        [TestMethod]
        public void TestExecuteExecutesTheResultFromCreateJsonResult()
        {
            var actionResult = new Mock<ActionResult>();
            var context = new Mock<ControllerContext>();
            _target.TestJsonResult = actionResult.Object;
            _target.ExecuteResult(context.Object);

            actionResult.Verify(x => x.ExecuteResult(context.Object));
        }

        #endregion

        #endregion

        #region Helper classes

        private class Item
        {
            public int DataProp { get; set; }
            public string LabelProp { get; set; }
            public string ValueProp { get; set; }
        }

        private class TestAutoCompleteResult : AutoCompleteResult
        {
            public ActionResult TestJsonResult { get; set; }

            protected internal override ActionResult CreateJsonResult()
            {
                return TestJsonResult ?? base.CreateJsonResult();
            }

            public ActionResult CallCreateJsonResult()
            {
                return CreateJsonResult();
            }
        }

        #endregion
    }

    [TestClass]
    public class CalendarResultTest : InMemoryDatabaseTest<TestUser, RepositoryBase<TestUser>>
    {
        private TestCalendarResult _target;

        #region Init/Cleanup

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(TestUserFactory).Assembly);
        }

        #endregion

        [TestMethod]
        public void TestCalendarResultUsesSuppliedConversionFnToConvertEntitiesToCalendarItems()
        {
            var users = GetEntityFactory<TestUser>().CreateArray(3);
            _target = new TestCalendarResult(Repository.GetAll(), u => new CalendarItem {
                title = u.Email,
                start = u.CreatedAt.Value
            });
            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            _target.ExecuteResult(controller.ControllerContext);
            var resultData = ((IEnumerable<CalendarItem>)_target.Data).ToArray();

            for (var i = 0; i < resultData.Length; ++i)
            {
                Assert.AreEqual(users[i].Email, resultData[i].title);
                Assert.AreEqual(users[i].CreatedAt, resultData[i].start);
            }
        }

        private class TestCalendarResult : CalendarResult<TestUser>
        {
            public TestCalendarResult(IEnumerable<TestUser> source, Func<TestUser, CalendarItem> conversionFn) : base(
                source, conversionFn)
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
        }
    }
}
