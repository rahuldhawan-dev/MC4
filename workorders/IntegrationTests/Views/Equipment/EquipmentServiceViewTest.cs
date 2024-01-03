using System;
using System.Linq;
using System.Web.Mvc;
using IntegrationTests.Model;
using LINQTo271.Views.Equipments;
using MMSINC.Exceptions;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Views.Equipment
{
    [TestClass]
    public class EquipmentServiceViewTest : WorkOrdersTestClass<Facility>
    {
        #region Constants

        public const int REFERENCE_TOWN_ID = 62, SOUTH_ORANGE = 92, OC_SOUTH_ORANGE_VILLAGE = 93;

        #endregion

        #region Private Members

        private HttpSimulator _simulator;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _simulator = new HttpSimulator();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        #region Private Methods

        protected override Facility GetValidObjectFromDatabase()
        {
            return GetValidFacility();
        }

        protected override void DeleteObject(Facility entity)
        {
            throw new DomainLogicException("Cannot delete Facility objects in this context.");
        }
        
        #endregion

        #region Exposed Static Methods

        public static Facility GetValidFacility()
        {
            return FacilityRepository.GetEntity(REFERENCE_TOWN_ID);
        }

        #endregion
        
        [TestMethod]
        public void GetEquipmentByTown()
        {
            using (_simulator.SimulateRequest())
            {
                var town = TownIntegrationTest.ReferenceTown;
                
                var facility = town.Facilities.First();
                facility.DepartmentID = DepartmentRepository.Indices.T_AND_D;
                var total = facility.Equipments.Count;
                FacilityRepository.Update(facility);

                Assert.IsTrue(total > 0, "Sanity Check: It appears that the data from dataimport has changed. We want to test that at least one piece of equipment is in the T&D department.");

                var knownCategoryValues =
                    String.Format("undefined:{0};Town:{1}", 10, town.TownID);

                var results =
                    (new EquipmentServiceView()).GetEquipmentByTown(
                        knownCategoryValues, "Town");

                Assert.AreEqual(total, results.Length, "The reference town should have " + total + " equipments");
            }
        }

        [TestMethod]
        public void TestEquipmentByTownReturnsProductionEquipmentIfOperatingCenterHasWorkOrderInvoicing()
        {
            using (_simulator.SimulateRequest())
            {
                var town = TownIntegrationTest.GetTown(SOUTH_ORANGE);
                var operatingCenter = town.OperatingCentersTowns.First(x => x.OperatingCenterID == OC_SOUTH_ORANGE_VILLAGE).OperatingCenter;
                Assert.IsTrue(operatingCenter.HasWorkOrderInvoicing, "Sanity Check: South Orange Village should allow work order invoicing or the rest of this is pointless.");
                var equipments = town.Facilities.Where(x => x.OperatingCenterID == operatingCenter.OperatingCenterID && x.DepartmentID == DepartmentRepository.Indices.PRODUCTION).SelectMany(x => x.Equipments);
                var total = equipments.Count();

                Assert.IsTrue(total > 0, "Sanity Check: It appears that the data from dataimport has changed. We want to test that at least one piece of equipment is in the Production department.");

                var knownCategoryValues = String.Format("undefined:{0};Town:{1}", 10, town.TownID);

                var results = (new EquipmentServiceView()).GetEquipmentByTown(knownCategoryValues, "Town");

                Assert.AreEqual(total, results.Length, "The reference town should have " + total + " equipments");
            }
        }
    }
}