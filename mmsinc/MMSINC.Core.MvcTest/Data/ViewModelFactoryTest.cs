using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class ViewModelFactoryTest
    {
        private IContainer _container;
        private IViewModelFactory _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _target = _container.GetInstance<ViewModelFactory>();
        }

        [TestMethod]
        public void TestBuildBuildsViewModel()
        {
            Assert.IsNotNull(_target.Build<TestViewModel>());
        }

        [TestMethod]
        public void TestBuildBuildsWithEntityIfProvided()
        {
            var entity = new Entity {
                StringProp = "foo"
            };

            Assert.AreEqual(entity.StringProp, _target.Build<TestViewModel, Entity>(entity).StringProp);
        }

        [TestMethod]
        public void TestBuildWithOverridesBuildsWithAdditionalOverrides()
        {
            Assert.AreEqual("foo", _target.BuildWithOverrides<TestViewModel>(new {
                StringProp = "foo"
            }).StringProp);

            Assert.AreEqual("foo", _target.BuildWithOverrides<TestViewModel, Entity>(new Entity(), new {
                StringProp = "foo"
            }).StringProp);
        }

        [TestMethod]
        public void TestBuildWithOverridesThrowsExceptionWhenOverridePropertyNotFound()
        {
            bool thrown;

            void DoTest(Action fn)
            {
                thrown = false;
                try
                {
                    fn();
                }
                catch (Exception e)
                {
                    thrown = true;
                    MyAssert.Contains("TestViewModel", e.Message, "Exception should contain view model class name");
                    MyAssert.Contains("ThisIsNotARealProperty", e.Message,
                        "Exception should contain errant property name");
                }

                Assert.IsTrue(thrown, "Exception was not thrown as expected");
            }

            DoTest(() =>
                _target.BuildWithOverrides<TestViewModel>(new {
                    ThisIsNotARealProperty = "meh"
                }));
            DoTest(() =>
                _target.BuildWithOverrides<TestViewModel, Entity>(new Entity(), new {
                    ThisIsNotARealProperty = "meh"
                }));
        }
    }
}
