using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Admin.Models.ViewModels.RoleGroups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Admin.Models.ViewModels.RoleGroups
{
    [TestClass]
    public class CreateRoleGroupRoleTest : ViewModelTestBase<RoleGroupRole, CreateRoleGroupRole>
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // noop, the mapping is done manually by the parent RoleGroupViewModel.
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Action);
            ValidationAssert.PropertyIsRequired(x => x.Module);
        }
        
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop
        }
        
        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }

        #endregion

        #endregion
    }
}
