using System.Linq;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for OperatingCenterStockedMaterialTestTest
    /// </summary>
    [TestClass]
    public class OperatingCenterStockedMaterialIntegrationTest : WorkOrdersTestClass<OperatingCenterStockedMaterial>
    {
        #region Exposed Static Methods

        internal static TestOperatingCenterStockedMaterialBuilder GetValidOperatingCenterStockedMaterial()
        {
            return new TestOperatingCenterStockedMaterialBuilder();
        }

        public static void DeleteOperatingCenterStockedMaterial(OperatingCenterStockedMaterial entity)
        {
            var material = entity.Material;
            OperatingCenterStockedMaterialRepository.Delete(entity);
            MaterialTest.DeleteMaterial(material);
        }

        public static Material GetValidMaterial(string partNumber, string description)
        {
            return new Material { PartNumber = partNumber, Description = description };
        }

        #endregion

        #region Private Methods

        protected override OperatingCenterStockedMaterial GetValidObject()
        {
            return GetValidOperatingCenterStockedMaterial();
        }

        protected override OperatingCenterStockedMaterial GetValidObjectFromDatabase()
        {
            var stockedMaterial = GetValidObject();
            OperatingCenterStockedMaterialRepository.Insert(stockedMaterial);
            return stockedMaterial;
        }

        protected override void DeleteObject(OperatingCenterStockedMaterial entity)
        {
            DeleteOperatingCenterStockedMaterial(entity);
        }

        private void DeleteMaterial(Material material)
        {
            MaterialRepository.Delete(material);
        }

        private MaterialHelper GetActiveAndInactiveMaterials()
        {
            var activeMaterial = GetValidMaterial("ACTIVE123", "I am active");
            activeMaterial.IsActive = true;
            MaterialRepository.Insert(activeMaterial);
            var inactiveMaterial = GetValidMaterial("INACTIVE789", "I am inactive");
            inactiveMaterial.IsActive = false;
            MaterialRepository.Insert(inactiveMaterial);

            var opc = OperatingCenterRepository.SelectAllAsList().First();
            var ocsm = new OperatingCenterStockedMaterial
            {
                Material = activeMaterial,
                OperatingCenter = opc
            };
            OperatingCenterStockedMaterialRepository.Insert(ocsm);

            var inactiveOcsm = new OperatingCenterStockedMaterial
            {
                Material = inactiveMaterial,
                OperatingCenter = opc
            };
            OperatingCenterStockedMaterialRepository.Insert(inactiveOcsm);
            return new MaterialHelper
            {
                OperatingCenter = opc,
                ActiveMaterial = activeMaterial,
                InactiveMaterial = inactiveMaterial,
                ActiveOperatingCenterStockedMaterial = ocsm,
                InactiveOperatingCenterStockedMaterial = inactiveOcsm
            };
        }

        private void DeleteMaterialHelper(MaterialHelper helper)
        {
            // Delete everything but the OperatingCenter itself.
            DeleteObject(helper.ActiveOperatingCenterStockedMaterial);
            // DeleteMaterial(helper.ActiveMaterial);

            DeleteObject(helper.InactiveOperatingCenterStockedMaterial);
            //  DeleteMaterial(helper.InactiveMaterial);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLookupMaterialByStockNumberReturnsOnlyActiveMaterialsByDefault()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                var result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByStockNumber(target.OperatingCenter.OperatingCenterID,
                            target.ActiveMaterial.PartNumber);

                Assert.IsTrue(result.Contains(target.ActiveMaterial));

                result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByStockNumber(target.OperatingCenter.OperatingCenterID,
                            target.InactiveMaterial.PartNumber);

                Assert.IsFalse(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        [TestMethod]
        public void TestLookupMaterialByStockNumberReturnsOnlyActiveMaterialsWhenActiveMaterialsOnlyOverloadIsSetToTrue()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                // ReSharper disable RedundantArgumentDefaultValue
                var result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByStockNumber(target.OperatingCenter.OperatingCenterID,
                            target.ActiveMaterial.PartNumber, true);

                Assert.IsTrue(result.Contains(target.ActiveMaterial));

                result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByStockNumber(target.OperatingCenter.OperatingCenterID,
                            target.InactiveMaterial.PartNumber, true);
                // ReSharper restore RedundantArgumentDefaultValue

                Assert.IsFalse(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        [TestMethod]
        public void TestLookupMaterialByStockNumberReturnsAllMaterialsWhenActiveMaterialsOnlyOverloadIsSetToFalse()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                // ReSharper disable RedundantArgumentDefaultValue
                var result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByStockNumber(target.OperatingCenter.OperatingCenterID,
                            target.ActiveMaterial.PartNumber, false);

                Assert.IsTrue(result.Contains(target.ActiveMaterial));

                result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByStockNumber(target.OperatingCenter.OperatingCenterID,
                            target.InactiveMaterial.PartNumber, false);
                // ReSharper restore RedundantArgumentDefaultValue

                Assert.IsTrue(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }


        [TestMethod]
        public void TestLookupMaterialByDescriptionReturnsOnlyActiveMaterialsByDefault()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                var result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByDescription(target.OperatingCenter.OperatingCenterID,
                            target.ActiveMaterial.Description);

                Assert.IsTrue(result.Contains(target.ActiveMaterial));

                result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByDescription(target.OperatingCenter.OperatingCenterID,
                            target.InactiveMaterial.Description);

                Assert.IsFalse(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        [TestMethod]
        public void TestLookupMaterialByDescriptionReturnsOnlyActiveMaterialsWhenActiveMaterialsOnlyOverloadIsSetToTrue()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                // ReSharper disable RedundantArgumentDefaultValue
                var result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByDescription(target.OperatingCenter.OperatingCenterID,
                            target.ActiveMaterial.Description, true);

                Assert.IsTrue(result.Contains(target.ActiveMaterial));

                result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByDescription(target.OperatingCenter.OperatingCenterID,
                            target.InactiveMaterial.Description, true);
                // ReSharper restore RedundantArgumentDefaultValue

                Assert.IsFalse(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        [TestMethod]
        public void TestLookupMaterialByDescriptionReturnsAllMaterialsWhenActiveMaterialsOnlyOverloadIsSetToFalse()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                // ReSharper disable RedundantArgumentDefaultValue
                var result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByDescription(target.OperatingCenter.OperatingCenterID,
                            target.ActiveMaterial.Description, false);

                Assert.IsTrue(result.Contains(target.ActiveMaterial));

                result =
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByDescription(target.OperatingCenter.OperatingCenterID,
                            target.InactiveMaterial.Description, false);
                // ReSharper restore RedundantArgumentDefaultValue

                Assert.IsTrue(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        #endregion

        private sealed class MaterialHelper
        {
            public Material ActiveMaterial;
            public Material InactiveMaterial;
            public OperatingCenterStockedMaterial ActiveOperatingCenterStockedMaterial;
            public OperatingCenterStockedMaterial InactiveOperatingCenterStockedMaterial;
            public OperatingCenter OperatingCenter;
        }
    }

    internal class TestOperatingCenterStockedMaterialBuilder : TestDataBuilder<OperatingCenterStockedMaterial>
    {
        #region Private Members

        private Material _material = MaterialTest.GetValidMaterial();
        private OperatingCenter _operatingCenter =
            OperatingCenterIntegrationTest.GetValidOperatingCenter();

        #endregion

        #region Exposed Methods

        public override OperatingCenterStockedMaterial Build()
        {
            var ocsm = new OperatingCenterStockedMaterial();
            if (_material != null)
                ocsm.Material = _material;
            if (_operatingCenter != null)
                ocsm.OperatingCenter = _operatingCenter;
            return ocsm;
        }

        public TestOperatingCenterStockedMaterialBuilder WithMaterial(Material material)
        {
            _material = material;
            return this;
        }

        public TestOperatingCenterStockedMaterialBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        #endregion
    }
}