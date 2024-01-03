using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MMSINC.Results;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class MapIconControllerTest : MapCallMvcControllerTestBase<MapIconController, MapIcon>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.AllowsAnonymousAccess("~/MapIcon/Index/");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because this isn't a normal index action.
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;
            var icons = GetFactory<MapIconFactory>().CreateList(3);

            var result = (ViewResult)_target.Index();
            var resultModel = (IconSet)result.Model;

            Assert.AreEqual("_Index", result.ViewName);
            Assert.IsTrue(resultModel.Icons.Contains(icons[0]));
            Assert.IsTrue(resultModel.Icons.Contains(icons[1]));
            Assert.IsTrue(resultModel.Icons.Contains(icons[2]));
        }

        [TestMethod]
        public void TestIndexReturnsViewFragmentWithSpecifiedIconSet()
        {
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;
            var iconSet = GetFactory<IconSetFactory>().Create(new {
                Id = (int)IconSets.Antennae
            });
            var icons = GetFactory<MapIconFactory>().CreateList(3);
            foreach (var icon in icons)
            {
                iconSet.Icons.Add(icon);
            }
            Session.Save(iconSet);

            var result = (ViewResult)_target.Index(IconSets.Antennae);
            var resultModel = (IconSet)result.Model;

            Assert.AreEqual("_Index", result.ViewName);
            icons.ForEach(i => MyAssert.Contains(resultModel.Icons, i));
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // noop: There is no search model for this controller. Other index tests cover this functionality.
        }

        #endregion
    }
}
