using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    public abstract class FacilitySubAreaTest<TViewModel> : ViewModelTestBase<FacilitySubArea, TViewModel> where TViewModel : FacilitySubAreaViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;
        public Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Area);
            _vmTester.CanMapBothWays(x => x.Description);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Area, GetEntityFactory<FacilityArea>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Area);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, FacilitySubArea.MAX_DESCRIPTION_LENGTH);
        }

        #endregion
    }

    [TestClass]
    public class CreateFacilitySubAreaTest : FacilitySubAreaTest<CreateFacilitySubArea> {}

    [TestClass]
    public class EditFacilitySubAreaTest : FacilitySubAreaTest<EditFacilitySubArea> {}
}
