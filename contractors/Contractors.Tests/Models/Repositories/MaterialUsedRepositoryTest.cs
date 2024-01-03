using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class MaterialUsedRepositoryTest : ContractorsControllerTestBase<MaterialUsed>
    {
        #region Private Members

        private MaterialUsedRepository _target;

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
            _target = _container.GetInstance<MaterialUsedRepository>();
        }

        #endregion

        [TestMethod]
        public void TestLinqPropertyOnlyAllowsAccessToMaterialsForOrdersThatTheCurrentUserCanAccess()
        {
            // GetAll() uses Linq
            var orders = new[] {
                GetFactory<WorkOrderFactory>().Create(new {
                    AssignedContractor = _currentUser.Contractor
                }),
                GetFactory<WorkOrderFactory>().Create()
            };

            var materials = new[] {
                GetFactory<MaterialUsedFactory>().Create(new {
                    WorkOrder = orders[0]
                }),
                GetFactory<MaterialUsedFactory>().Create(new {
                    WorkOrder = orders[1]
                })
            };

            var found = _target.GetAll().ToArray();

            Assert.AreEqual(1, found.Length);
            Assert.AreEqual(materials[0], found[0]);
        }

        [TestMethod]
        public void TestCriteriaPropertyOnlyAllowsAccessToMaterialsForOrdersThatTheCurrentUserCanAccess()
        {
            // Find(int id) uses Criteria
            var orders = new[] {
                GetFactory<WorkOrderFactory>().Create(new {
                    AssignedContractor = _currentUser.Contractor
                }),
                GetFactory<WorkOrderFactory>().Create()
            };

            var materials = new[] {
                GetFactory<MaterialUsedFactory>().Create(new {
                    WorkOrder = orders[0]
                }),
                GetFactory<MaterialUsedFactory>().Create(new {
                    WorkOrder = orders[1]
                })
            };

            Assert.IsNotNull(_target.Find(materials[0].Id));
            Assert.IsNull(_target.Find(materials[1].Id));
        }
    }
}
