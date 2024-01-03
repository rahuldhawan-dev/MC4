using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateEnvironmentalPermitTest : EnvironmentalPermitTestBase<CreateEnvironmentalPermit>
    {
        [TestMethod]
        public void TestMapToEntityDoesNotMapAnyOperatingCentersWhenViewModelOperatingCentersIsNull()
        {
            // In other words, make sure this doesn't crash because the OperatingCenters property is null.
            _viewModel.OperatingCenters = null;
        
            var entity = new EnvironmentalPermit();
            _viewModel.MapToEntity(entity);
        
            Assert.IsFalse(entity.OperatingCenters.Any());
        }

        [TestMethod]
        public void TestCreateMapToEntityAddsOperatingCenters()
        {
            var operatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateList(4);
        
            _viewModel.OperatingCenters = new[] { operatingCenters[0].Id, operatingCenters[3].Id };
        
            _viewModel.MapToEntity(_entity);
        
            Assert.AreEqual(2, _entity.OperatingCenters.Count);
            Assert.IsTrue(_entity.OperatingCenters.Contains(operatingCenters[0]));
            Assert.IsTrue(_entity.OperatingCenters.Contains(operatingCenters[3]));
        }

        [TestMethod]
        public void TestMapToEntityMapsFacilities()
        {
            var facility = GetEntityFactory<Facility>().Create();
            _viewModel = _viewModelFactory.BuildWithOverrides<CreateEnvironmentalPermit, EnvironmentalPermit>(
                _entity,
                new {
                    Facilities = new List<int> { facility.Id }
                });

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(1, _entity.Facilities.Count);
            Assert.AreSame(facility, _entity.Facilities.Single());
        }

        [TestMethod]
        public void TestMapToEntityMapsEquipment()
        {
            var equipment = GetEntityFactory<Equipment>().Create();
            _viewModel = _viewModelFactory.BuildWithOverrides<CreateEnvironmentalPermit, EnvironmentalPermit>(
                _entity,
                new {
                    Equipment = new List<int> { equipment.Id }
                });

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(1, _entity.Equipment.Count);
            Assert.AreSame(equipment, _entity.Equipment.Single());
        }
    }
}
