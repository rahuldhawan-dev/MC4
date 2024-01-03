using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels.Streets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels.Streets
{
    [TestClass]
    public class EditStreetTest : BaseStreetViewModelTest<EditStreet>
    {
        #region Tests
              
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
            _viewModel.Prefix = prefix.Id;
            _viewModel.Suffix = suffix.Id;
            _viewModel.Name = "South";

            ValidationAssert.ModelStateHasNonPropertySpecificError("A record already exists for this street for this town.");

            _viewModel.Name = "Something Else";

            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public void TestValidationIsTestingStreetNameAgainstTheFullStName()
        {
            // There's nothing stopping a user from setting the Name to the whole "N South St" 
            // name and ignoring the prefix/suffix values, so let's make sure they can't make
            // duplicates that way.
            var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;
            var existingStreet = GetEntityFactory<Street>().Create(new {
                Town = town, 
                Name = "N SoUtH St",
                Prefix = (StreetPrefix)null,
                Suffix = (StreetSuffix)null
            });
            town.Streets.Add(existingStreet);

            var prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "N" });
            var suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St" });
            _viewModel.Prefix = prefix.Id;
            _viewModel.Suffix = suffix.Id;
            _viewModel.Name = "SOUTH";

            ValidationAssert.ModelStateHasNonPropertySpecificError("A record already exists for this street for this town.");

            _viewModel.Name = "Something Else";

            ValidationAssert.ModelStateIsValid();
        }

        #endregion
    }
}
