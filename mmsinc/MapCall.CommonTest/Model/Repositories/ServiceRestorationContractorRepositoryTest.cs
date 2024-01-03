using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ServiceRestorationContractorRepositoryTest : InMemoryDatabaseTest<ServiceRestorationContractor,
        ServiceRestorationContractorRepository>
    {
        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsByOperatingCenterId()
        {
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "FOO"});
            var opc2 = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "BAR"});
            var src = GetEntityFactory<ServiceRestorationContractor>().Create(new {
                OperatingCenter = opc1, Contractor = "Buh?", FinalRestoration = true, PartialRestoration = true
            });
            var invalid = GetEntityFactory<ServiceRestorationContractor>().Create(new {
                OperatingCenter = opc2, Contractor = "Buh?", FinalRestoration = true, PartialRestoration = true
            });

            Session.Save(opc1);
            Session.Flush();

            var result = Repository.GetByOperatingCenterId(opc1.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Contains(invalid));
        }
    }
}
