using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels.Streets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels.Streets
{
    [TestClass]
    public class CreateStreetTest : BaseStreetViewModelTest<CreateStreet>
    {
        #region Tests
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.Town);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();
            ValidationAssert.EntityMustExist(x => x.Town, GetEntityFactory<Town>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.County);
            ValidationAssert.PropertyIsRequired(x => x.Town);
        }

        [TestMethod]
        public void TestValidationFailsIfTheStreetAlreadyExistsForATown()
        {
            var prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "N" });
            var suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St" });
            var town = GetEntityFactory<Town>().Create();
            var existingStreet = GetEntityFactory<Street>().Create(new {
                Town = town, 
                Name = "South",
                Prefix = prefix,
                Suffix = suffix
            });
            town.Streets.Add(existingStreet);
            _entity.Town = town;
            _viewModel.Town = town.Id;
            _viewModel.County = town.County.Id;
            _viewModel.State = town.State.Id;
            _viewModel.Prefix = prefix.Id;
            _viewModel.Suffix = suffix.Id;
            _viewModel.Name = "South";

            ValidationAssert.ModelStateHasNonPropertySpecificError("A record already exists for this street for this town.");

            _viewModel.Name = "Something Else";

            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public void TestValidationIsTestingStreetNameAgainstTheFullStNameAndIsCaseInsensitive()
        {
            // There's nothing stopping a user from setting the Name to the whole "N South St" 
            // name and ignoring the prefix/suffix values, so let's make sure they can't make
            // duplicates that way.
            var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;
            _viewModel.Town = town.Id;
            _viewModel.County = town.County.Id;
            _viewModel.State = town.State.Id;
            var existingStreet = GetEntityFactory<Street>().Create(new {
                Town = town, 
                Name = "n sOuTh ST",
                Prefix = (StreetPrefix)null,
                Suffix = (StreetSuffix)null
            });
            town.Streets.Add(existingStreet);

            var prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "N" });
            var suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St" });
            _viewModel.Prefix = prefix.Id;
            _viewModel.Suffix = suffix.Id;
            _viewModel.Name = "south";

            ValidationAssert.ModelStateHasNonPropertySpecificError("A record already exists for this street for this town.");

            _viewModel.Name = "Something Else";

            ValidationAssert.ModelStateIsValid();
        }

        #endregion
    }
}
