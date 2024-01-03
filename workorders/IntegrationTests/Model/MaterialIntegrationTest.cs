using System;
using System.Linq;
using MMSINC.Common;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MaterialTestTest
    /// </summary>
    [TestClass]
    public class MaterialIntegrationTest : WorkOrdersTestClass<Material>
    {
        #region Exposed Static Methods

        public static Material GetValidMaterial()
        {
            return new Material { PartNumber = "PN1234" };
        }

        public static void DeleteMaterial(Material entity)
        {
            MaterialRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override Material GetValidObject()
        {
            return GetValidMaterial();
        }

        protected override Material GetValidObjectFromDatabase()
        {
            var material = GetValidObject();
            MaterialRepository.Insert(material);
            return material;
        }

        protected override void DeleteObject(Material entity)
        {
            DeleteMaterial(entity);
        }

        private MaterialHelper GetActiveAndInactiveMaterials()
        {
            var activeMaterial = GetValidObject();
            activeMaterial.IsActive = true;
            MaterialRepository.Insert(activeMaterial);
            var inactiveMaterial = GetValidObject();
            inactiveMaterial.IsActive = false;
            MaterialRepository.Insert(inactiveMaterial);

            var opc = OperatingCenterRepository.SelectAllAsList().First();
            var ocsm = new OperatingCenterStockedMaterial
            {
                Material = activeMaterial,
                OperatingCenter = opc
            };
            OperatingCenterStockedMaterialRepository.Insert(ocsm);

            var inactiveOcsm = new OperatingCenterStockedMaterial {
                Material = inactiveMaterial,
                OperatingCenter = opc
            };
            OperatingCenterStockedMaterialRepository.Insert(inactiveOcsm);
            return new MaterialHelper {
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
            DeleteOperatingCenterStockedMaterialRepository(helper.ActiveOperatingCenterStockedMaterial);
            DeleteOperatingCenterStockedMaterialRepository(helper.InactiveOperatingCenterStockedMaterial);
            DeleteObject(helper.ActiveMaterial);
            DeleteObject(helper.InactiveMaterial);
        }

        private void DeleteOperatingCenterStockedMaterialRepository(OperatingCenterStockedMaterial ocsm)
        {
            OperatingCenterStockedMaterialRepository.Delete(ocsm);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewMaterial()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.DoesNotThrow(() => MaterialRepository.Insert(target));

                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(Material));

                DeleteObject(target);
            }
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestCannotSaveWithoutPartNumber()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.PartNumber = null;

                MyAssert.Throws(() => MaterialRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Attempting to save a Material without a PartNumber should throw an exception");
            }
        }

        [TestMethod]
        public void TestGetStockedMaterialsByOperatingCenterReturnsOnlyActiveMaterialsByDefault()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();

                var result =
                    MaterialRepository.GetStockedMaterialsByOperatingCenter(
                        target.OperatingCenter.OperatingCenterID).ToArray();

                Assert.IsTrue(result.Contains(target.ActiveMaterial));
                Assert.IsFalse(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        [TestMethod]
        public void TestGetStockedMaterialsByOperatingCenterReturnsOnlyActiveMaterialsWhenActiveMaterialsOnlyOverloadIsSetToTrue()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                // ReSharper disable RedundantArgumentDefaultValue
                var result = MaterialRepository.GetStockedMaterialsByOperatingCenter(target.OperatingCenter.OperatingCenterID, true).ToArray();
                // ReSharper restore RedundantArgumentDefaultValue

                Assert.IsTrue(result.Contains(target.ActiveMaterial));
                Assert.IsFalse(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        [TestMethod]
        public void TestGetStockedMaterialsByOperatingCenterReturnsBothActiveAndInactiveMaterialsWhenActiveMaterialsOnlyOverloadIsSetToFalse()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                // ReSharper disable RedundantArgumentDefaultValue
                var result = MaterialRepository.GetStockedMaterialsByOperatingCenter(target.OperatingCenter.OperatingCenterID, false).ToArray();
                // ReSharper restore RedundantArgumentDefaultValue

                Assert.IsTrue(result.Contains(target.ActiveMaterial));
                Assert.IsTrue(result.Contains(target.InactiveMaterial));

                DeleteMaterialHelper(target);
            }
        }

        [TestMethod]
        public void TestGetFilteredDataReturnsOnlyMaxResults()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetActiveAndInactiveMaterials();
                var expr = PredicateBuilder.True<Material>();
                var results = new TestMaterialRepository().TestGetFilteredData(expr);
                Assert.AreEqual(MaterialRepository.MAX_RESULTS, results.Count());
            }
        }

        #region Helper class
        
        private sealed class MaterialHelper
        {
            public Material ActiveMaterial;
            public Material InactiveMaterial;
            public OperatingCenterStockedMaterial ActiveOperatingCenterStockedMaterial;
            public OperatingCenterStockedMaterial InactiveOperatingCenterStockedMaterial;
            public OperatingCenter OperatingCenter;
        }

        #endregion
    }

    internal class TestMaterialRepository : MaterialRepository
    {
        public  System.Collections.Generic.IEnumerable<Material> TestGetFilteredData(System.Linq.Expressions.Expression<Func<Material, bool>> filterExpression)
        {
            return GetFilteredData(filterExpression);
        }
    }
}