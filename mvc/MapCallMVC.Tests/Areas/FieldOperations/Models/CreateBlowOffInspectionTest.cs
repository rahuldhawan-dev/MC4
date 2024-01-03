using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateBlowOffInspectionTest : BlowOffInspectionViewModelTest<CreateBlowOffInspection>
    {
        [TestMethod]
        public void TestMapToEntitySetsInspectedBy()
        {
            // need to evict because MapToEntity causes a flush 
            // which causes an nhibernate error because nhibernate
            // is stupid.
            Session.Evict(_entity);
            _entity.InspectedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.InspectedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsGallonsFlowed()
        {
            _viewModel.GPM = 400;
            _viewModel.MinutesFlowed = 2;
            _entity.GallonsFlowed = null;
            _vmTester.MapToEntity();
            Assert.AreEqual(800, _entity.GallonsFlowed);
        }

        [TestMethod]
        public void TestValidationFailsIfValveIsNotInspectable()
        {
            _entity.Valve.Status.Id = AssetStatus.Indices.CANCELLED;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.INACTIVE;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.RETIRED;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.REMOVED;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.ACTIVE;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Valve);
        }
    }
}