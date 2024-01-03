using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ModuleTest
    {
        [TestMethod]
        public void TestDescriptionReturnsApplicationNameAndModuleName()
        {
            var module = new Module {
                Name = "SomeModule",
                Application = new Application {
                    Name = "SomeApplication"
                }
            };

            Assert.AreEqual("SomeApplication - SomeModule", module.Description);
        }

        [TestMethod]
        public void TestToStringReturnsApplicationNameAndModuleName()
        {
            var module = new Module {
                Name = "SomeModule",
                Application = new Application {
                    Name = "SomeApplication"
                }
            };

            Assert.AreEqual("SomeApplication - SomeModule", module.Description);
        }
    }
}
