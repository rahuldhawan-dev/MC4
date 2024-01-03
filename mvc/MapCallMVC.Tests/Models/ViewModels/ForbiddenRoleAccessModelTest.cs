using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class ForbiddenRoleAccessModelTest
    {
        [TestMethod]
        public void TestConstructorSetsRequiredRolesPropertyToNewInstance()
        {
            var first = new ForbiddenRoleAccessModel().RequiredRoles;
            Assert.IsNotNull(first);
            var second = new ForbiddenRoleAccessModel().RequiredRoles;
            Assert.IsNotNull(second);
            Assert.AreNotSame(first, second);
        }
    }
}
