using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditCrewTest : ViewModelTestBase<Crew, EditCrew>
    {
        #region Fields

        private ViewModelTester<EditCrew, Crew> _vmTester;
        private EditCrew _viewModel;
        private Crew _entity;
        private OperatingCenter _operatingCenter;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<ICrewRepository>().Use<CrewRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<Crew>().Create();
            _operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            _viewModel = _viewModelFactory.BuildWithOverrides<EditCrew, Crew>(_entity, new {
                OperatingCenter = _operatingCenter.Id
            });
            _vmTester = new ViewModelTester<EditCrew, Crew>(_viewModel, _entity);
        }

        #endregion

        #region Test

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            var crew = GetEntityFactory<Crew>().Create(new { Availability = (decimal)2.5, Description = "Foo" });
            _entity.Availability = crew.Availability;

            _vmTester.MapToViewModel();

            Assert.AreEqual(crew.Availability, _viewModel.Availability);

            _entity.Availability = 0;
            _vmTester.MapToEntity();

            Assert.AreEqual(crew.Availability, _entity.Availability);
           
            _entity.Description = crew.Description;

            _vmTester.MapToViewModel();

            Assert.AreEqual(crew.Description, _viewModel.Description);

            _entity.Description = null;
            _vmTester.MapToEntity();

            Assert.AreSame(crew.Description, _entity.Description);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Availability);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.Description,
                    Crew.StringLengths.CREW_NAME);
        }

        #endregion
    }
}
