using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.RazorTable
{
    [TestClass]
    public class RazorTableTemplateColumnTest
    {
        #region Fields

        private RazorTableTemplateColumn<Model> _target;
        private Model _model;
        private HtmlHelper<Model> _htmlHelper;
        private string _templateText;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _templateText = "some text";
            _target = new RazorTableTemplateColumn<Model>(TemplateMethod);
            _model = new Model {Name = "Dude"};
            _htmlHelper = new FakeMvcHttpHandler(new Container()).CreateHtmlHelper(_model);
        }

        #endregion

        #region Private Methods

        private IHtmlString TemplateMethod(Model model)
        {
            return new HtmlString(_templateText);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorThrowsForNullTemplateParameter()
        {
            MyAssert.Throws<ArgumentNullException>(() => new RazorTableTemplateColumn<Model>(null));
        }

        [TestMethod]
        public void TestConstructorSetsTemplateProperty()
        {
            Func<Model, IHtmlString> template = (x) => { return new HtmlString(""); };
            var target = new RazorTableTemplateColumn<Model>(template);
            Assert.AreSame(template, target.Template);
        }

        [TestMethod]
        public void TestRenderCellDoesNotEscapeHtmlAgain()
        {
            _templateText = "<br>";
            var result = _target.RenderCell(_htmlHelper).ToString();
            Assert.AreEqual("<td><br></td>", result);
        }

        #endregion

        #region Helper classes

        private class Model
        {
            public string Name { get; set; }
        }

        #endregion
    }
}
