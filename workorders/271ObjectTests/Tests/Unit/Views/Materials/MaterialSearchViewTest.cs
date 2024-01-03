using System.Collections.Generic;
using System.Linq;
using LINQTo271.Views.Materials;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Rhino.Mocks;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Materials
{
    [TestClass]
    public class MaterialSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private ITextBox _txtDescription, _txtPartNumber;
        private IDropDownList _ddlActive, _ddlDoNotOrder;
        private TestMaterialSearchView _target;
        
        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _txtDescription)
                .DynamicMock(out _txtPartNumber)
                .DynamicMock(out _ddlActive)
                .DynamicMock(out _ddlDoNotOrder);

            _target = new TestMaterialSearchViewBuilder()
                .WithTxtDescription(_txtDescription)
                .WithTxtPartNumber(_txtPartNumber)
                .WithDdlActive(_ddlActive)
                .WithDdlDoNotOrder(_ddlDoNotOrder);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestGenerateExpressionDoesNotFilterAnything()
        {
            _mocks.ReplayAll();

            var match = new Material() { PartNumber = "XXX333" };
            var mismatch = new Material() { PartNumber = "foo"};
            IEnumerable<Material> materials = new[] { match, mismatch };
            var result =
                materials.Where(_target.GenerateExpression().Compile()).ToList();

            Assert.IsTrue(result.Contains(match));
            Assert.IsTrue(result.Contains(mismatch));
        }

        [TestMethod]
        public void TestGenerateExpressionFiltersPartNumber()
        {
            SetupResult.For(_txtPartNumber.Text).Return("X33");
            _mocks.ReplayAll();

            var match = new Material() { PartNumber = "XXX333" };
            var mismatch = new Material() { PartNumber = "foo" };
            IEnumerable<Material> materials = new[] { match, mismatch };
            var result =
                materials.Where(_target.GenerateExpression().Compile()).ToList();

            Assert.IsTrue(result.Contains(match));
            Assert.IsFalse(result.Contains(mismatch));
        }

        [TestMethod]
        public void TestGenerateExpressionFiltersDescription()
        {
            SetupResult.For(_txtDescription.Text).Return("fo");
            _mocks.ReplayAll();

            var match = new Material() { Description = "foo" };
            var mismatch = new Material() { Description = "blergh" };
            IEnumerable<Material> materials = new[] { match, mismatch };
            var result =
                materials.Where(_target.GenerateExpression().Compile()).ToList();

            Assert.IsTrue(result.Contains(match));
            Assert.IsFalse(result.Contains(mismatch));
        }

        [TestMethod]
        public void TestGenerateExpressionFiltersActive()
        {
            SetupResult.For(_ddlActive.SelectedValue).Return("True");
            _mocks.ReplayAll();

            var match = new Material() {
                Description = "foo",
                IsActive = true
            };
            var mismatch = new Material() {
                Description = "blergh",
                IsActive = false
            };
            IEnumerable<Material> materials = new[] {
                match, mismatch
            };

            var result =
                materials.Where(_target.GenerateExpression().Compile()).ToList();

            Assert.IsTrue(result.Contains(match));
            Assert.IsFalse(result.Contains(mismatch));
        }


        [TestMethod]
        public void TestGenerateExpressionFiltersDoNotOrder()
        {
            SetupResult.For(_ddlDoNotOrder.SelectedValue).Return("True");
            _mocks.ReplayAll();

            var match = new Material()
            {
                Description = "foo",
                DoNotOrder = true
            };
            var mismatch = new Material()
            {
                Description = "blergh",
                DoNotOrder = false
            };
            IEnumerable<Material> materials = new[] {
                match, mismatch
            };

            var result =
                materials.Where(_target.GenerateExpression().Compile()).ToList();

            Assert.IsTrue(result.Contains(match));
            Assert.IsFalse(result.Contains(mismatch));
        }

        [TestMethod]
        public void TestGenerateExpressionFiltersActiveInActive()
        {
            SetupResult.For(_ddlActive.SelectedValue).Return("False");
            _mocks.ReplayAll();

            var match = new Material()
            {
                Description = "foo",
                IsActive = true
            };
            var mismatch = new Material()
            {
                Description = "blergh",
                IsActive = false
            };
            IEnumerable<Material> materials = new[] {
                match, mismatch
            };

            var result =
                materials.Where(_target.GenerateExpression().Compile()).ToList();

            Assert.IsFalse(result.Contains(match));
            Assert.IsTrue(result.Contains(mismatch));
        }
    }

    internal class TestMaterialSearchViewBuilder : TestDataBuilder<TestMaterialSearchView>
    {
        #region Private Members

        private ITextBox _txtPartNumber, _txtDescription;
        private IDropDownList _ddlActive, _ddlDoNotOrder;
        
        #endregion

        #region Exposed Methods
        
        public override TestMaterialSearchView Build()
        {
            var obj = new TestMaterialSearchView();
            if (_txtDescription != null)
                obj.SetTxtDescription(_txtDescription);
            if (_txtPartNumber != null)
                obj.SetTxtPartNumber(_txtPartNumber);
            if (_ddlActive != null)
                obj.SetDdlActive(_ddlActive);
            if (_ddlDoNotOrder != null)
                obj.SetDdlDoNotOrder(_ddlDoNotOrder);
            return obj;
        }

        public TestMaterialSearchViewBuilder WithTxtDescription(ITextBox txtDescription)
        {
            _txtDescription = txtDescription;
            return this;
        }

        public TestMaterialSearchViewBuilder WithTxtPartNumber(ITextBox txtPartNumber)
        {
            _txtPartNumber = txtPartNumber;
            return this;
        }

        public TestMaterialSearchViewBuilder WithDdlActive(IDropDownList ddlActive)
        {
            _ddlActive = ddlActive;
            return this;
        }

        public TestMaterialSearchViewBuilder WithDdlDoNotOrder(
            IDropDownList ddlDoNotOrder)
        {
            _ddlDoNotOrder = ddlDoNotOrder;
            return this;
        }

        #endregion
    }

    internal class TestMaterialSearchView : MaterialSearchView
    {
        public void SetTxtPartNumber(ITextBox txt)
        {
            txtPartNumber = txt;
        }

        public void SetTxtDescription(ITextBox txt)
        {
            txtDescription = txt;
        }

        public void SetDdlActive(IDropDownList ddl)
        {
            ddlActive = ddl;
        }

        public void SetDdlDoNotOrder(IDropDownList ddl)
        {
            ddlDoNotOrder = ddl;
        }
    }
}
