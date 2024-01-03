using System;
using System.Linq;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditEnvironmentalPermitTest : EnvironmentalPermitTestBase<EditEnvironmentalPermit>
    {
        [TestMethod]
        public void TestValidationThrowsErrorForRequiresRequirementsWhenNoRequirements()
        {
            _viewModel.RequiresRequirements = true;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.RequiresRequirements, EditEnvironmentalPermit.REQUIRES_REQUIREMENTS_VALIDATION_ERROR);

            _viewModel.RequirementCount = 1;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.RequiresRequirements);
        }

        [TestMethod]
        public void TestEditMapToEntityAddsRemovesOperatingCentersAppropriately()
        {
            var expectedToRemove = GetFactory<UniqueOperatingCenterFactory>().Create();
            var expectedToAdd = GetFactory<UniqueOperatingCenterFactory>().Create();
            _entity.OperatingCenters.Add(expectedToRemove);
            _viewModel.OperatingCenters = new[] { expectedToAdd.Id };
        
            _viewModel.MapToEntity(_entity);
        
            Assert.AreEqual(1, _entity.OperatingCenters.Count);
            Assert.IsTrue(_entity.OperatingCenters.Contains(expectedToAdd));
            Assert.IsFalse(_entity.OperatingCenters.Contains(expectedToRemove));
        }
        
        [TestMethod]
        public void TestMapToEntityDoesNotMapAnyOperatingCentersWhenViewModelOperatingCentersIsNull()
        {
            // In other words, make sure this doesn't crash because the OperatingCenters property is null.
            _viewModel.OperatingCenters = null;
        
            _viewModel.MapToEntity(_entity);
        
            Assert.IsFalse(_entity.OperatingCenters.Any());
        }

        [TestMethod]
        public void TestPermitExpiresSetsToTrueWhenPermitExpirationDateExists()
        {
            _entity.PermitExpirationDate = DateTime.Now;
        
            _viewModel.Map(_entity);
        
            Assert.IsTrue(_viewModel.PermitExpires);
        }
        
        [TestMethod]
        public void TestPermitExpiresIsFalseWhenExperationDateDoesNotExist()
        {
            _viewModel.Map(_entity);
        
            Assert.IsFalse(_viewModel.PermitExpires);
        }
    }
}
