using _271ObjectTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Exceptions;
using WorkOrders.Model;

namespace IntegrationTests.Model
{
    [TestClass]
    public class SewerOpeningIntegrationTest : WorkOrdersTestClass<SewerOpening>
    {
        public const int REFERENCE_SEWER_OPENING_ID = 3509;
        
        protected override SewerOpening GetValidObjectFromDatabase()
        {
            return SewerOpeningRepository.GetEntity(REFERENCE_SEWER_OPENING_ID);
        }

        protected override void DeleteObject(SewerOpening entity)
        {
            throw new DomainLogicException("Cannot delete Sewer Opening objects in this context.");
        }

        /// <summary>
        /// These properties are used in the <see cref="MapCall.Common.Utility.ArcCollectorLinkGenerator"/>.
        /// If the mappings are changed in MapCall.Common.Model.Entities, they can be missed in WorkOrders.Model.
        /// This will catch the properties used in the generator if they aren't updated in WorkOrders.Model
        ///
        /// This hits real data from the db. If it's failing oddly, the data may have changed for the
        /// referenced SewerOpening
        /// </summary>
        [TestMethod]
        public void TestPropertiesUsedForArcCollectorLinkGeneratorMapCorrectly()
        {
            using (_simulator.SimulateRequest())
            {
                var sewerOpening = GetValidObjectFromDatabase();

                Assert.AreEqual(sewerOpening.LastUpdatedById, sewerOpening.LastUpdatedBy.EmployeeID);
                Assert.AreEqual(sewerOpening.AssetStatusID, sewerOpening.AssetStatus.AssetStatusID);
                Assert.AreEqual(sewerOpening.SewerOpeningMaterialId, sewerOpening.SewerOpeningMaterial.Id);
            }
        }
    }
}
