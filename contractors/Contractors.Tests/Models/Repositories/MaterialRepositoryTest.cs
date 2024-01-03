using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;
using MaterialRepository = Contractors.Data.Models.Repositories.MaterialRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class MaterialRepositoryTest : ContractorsControllerTestBase<Material, MaterialRepository>
    {
        #region Private Members

        private IRepository<MaterialUsed> _materialUsedRepository;
        private IWorkOrderRepository _workOrderRepository;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Repository = _container.GetInstance<MaterialRepository>();
            _materialUsedRepository = _container.GetInstance<MaterialUsedRepository>();
            _workOrderRepository = _container.GetInstance<WorkOrderRepository>();
            _container.Inject(_materialUsedRepository);
            _container.Inject(_workOrderRepository);
        }

        #endregion

        [TestMethod]
        public void TestMaterialSearchSearchesByPartNumberAndOperatingCenter()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var otherOperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var materials = new[] {
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675309", Description = "some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675310", Description = "some other thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675311", Description = "nothing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234567", Description = "also some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234568", Description = "also nothing"})
            };
            var otherMaterial = GetFactory<MaterialFactory>().Create(new {
                PartNumber = "other operating center",
                Description = "Operating Center Material"
            });
            materials.Each(m =>
            {
                operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = m});
                Session.Merge(operatingCenter);
            });
            otherOperatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = otherOperatingCenter, Material = otherMaterial});
            Session.Merge(otherOperatingCenter);
            Session.Flush();
            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            _currentUser.Contractor.OperatingCenters.Add(otherOperatingCenter);
            Session.Merge(_currentUser.Contractor);

            var results =
                Repository.GetBySearchAndOperatingCenterId("867", operatingCenter.Id).ToArray();
            Assert.AreEqual(3, results.Length);
            Assert.AreEqual(materials[0], results[0]);
            Assert.AreEqual(materials[1], results[1]);
            Assert.AreEqual(materials[2], results[2]);

            results = Repository.GetBySearchAndOperatingCenterId("some", operatingCenter.Id).ToArray();
            Assert.AreEqual(3, results.Length);
            Assert.AreEqual(materials[0], results[0]);
            Assert.AreEqual(materials[1], results[1]);
            Assert.AreEqual(materials[3], results[2]);

            results = Repository.GetBySearchAndOperatingCenterId("DONTFINDME", operatingCenter.Id).ToArray();
            Assert.AreEqual(0, results.Length);

            results = Repository.GetBySearchAndOperatingCenterId(otherMaterial.PartNumber, operatingCenter.Id).ToArray();
            Assert.AreEqual(0, results.Length);

            results = Repository.GetBySearchAndOperatingCenterId(otherMaterial.PartNumber, otherOperatingCenter.Id).ToArray();
            Assert.AreEqual(1, results.Length);
        }

        [TestMethod]
        public void TestMaterialSearchSearchesByPartNumberAndWorkOrderId()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var workOrder =
                GetFactory<WorkOrderFactory>().Create(new {
                    AssignedContractor = _currentUser.Contractor,
                    OperatingCenter = operatingCenter
                });
            var otherOperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var materials = new[] {
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675309", Description = "some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675310", Description = "some other thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "8675311", Description = "nothing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234567", Description = "also some thing"}),
                GetFactory<MaterialFactory>().Create(new {PartNumber = "1234568", Description = "also nothing"})
            };
            var materialUsed =
                GetFactory<MaterialUsedFactory>().Create(
                    new {Material = materials[0], WorkOrder = workOrder});
            var otherMaterial = GetFactory<MaterialFactory>().Create(new
            {
                PartNumber = "other operating center",
                Description = "Operating Center Material"
            });
            materials.Each(m =>
            {
                operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = m});
                Session.Merge(operatingCenter);
            });
            otherOperatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = otherOperatingCenter, Material = otherMaterial});
            Session.Merge(otherOperatingCenter);

            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            _currentUser.Contractor.OperatingCenters.Add(otherOperatingCenter);
            Session.Merge(_currentUser.Contractor);
            Session.Flush();
            var results =
                Repository.GetBySearchAndWorkOrderId("867", materialUsed.WorkOrder.Id).ToArray();

            Assert.AreEqual(3, results.Length);
            Assert.AreEqual(materials[0], results[0]);
            Assert.AreEqual(materials[1], results[1]);
            Assert.AreEqual(materials[2], results[2]);

            results = Repository.GetBySearchAndOperatingCenterId("some", operatingCenter.Id).ToArray();
            Assert.AreEqual(3, results.Length);
            Assert.AreEqual(materials[0], results[0]);
            Assert.AreEqual(materials[1], results[1]);
            Assert.AreEqual(materials[3], results[2]);

            results = Repository.GetBySearchAndOperatingCenterId("DONTFINDME", operatingCenter.Id).ToArray();
            Assert.AreEqual(0, results.Length);

            results = Repository.GetBySearchAndOperatingCenterId(otherMaterial.PartNumber, operatingCenter.Id).ToArray();
            Assert.AreEqual(0, results.Length);

            results = Repository.GetBySearchAndOperatingCenterId(otherMaterial.PartNumber, otherOperatingCenter.Id).ToArray();
            Assert.AreEqual(1, results.Length);
        }
    }
}
