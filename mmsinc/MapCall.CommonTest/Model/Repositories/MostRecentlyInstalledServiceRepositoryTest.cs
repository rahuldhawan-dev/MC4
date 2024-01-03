using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class MostRecentlyInstalledServiceRepositoryTest
        : MapCallMvcInMemoryDatabaseTestBase<MostRecentlyInstalledService>
    {
        private IRepository<MostRecentlyInstalledService> _target;

        [TestInitialize]
        public void TestInitialize()
        {

            _target = _container.GetInstance<RepositoryBase<MostRecentlyInstalledService>>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IServiceRepository>().Mock();
            e.For<ITapImageRepository>().Mock();
        }

        [TestMethod]
        public void Test_ByInstallationNumberAndOperatingCenter_FiltersNonActivePremises()
        {
            var installation = "installation";
            var opCenter = GetFactory<UniqueOperatingCenterFactory>().Create();

            var activePremise = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = opCenter
            });
            var activePremiseService = GetEntityFactory<Service>().Create(new {
                Premise = activePremise
            });

            var inactivePremise = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = opCenter,
                StatusCode = typeof(InactivePremiseStatusCodeFactory)
            });
            var inactivePremiseService = GetEntityFactory<Service>().Create(new {
                Premise = inactivePremise
            });
            
            var killedPremise = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = opCenter,
                StatusCode = typeof(KilledPremiseStatusCodeFactory)
            });
            var killedPremiseService = GetEntityFactory<Service>().Create(new {
                Premise = killedPremise
            });
            
            var nonConvertedPremise = GetEntityFactory<Premise>().Create(new {
                Installation = installation,
                OperatingCenter = opCenter,
                StatusCode = typeof(NonConvertedPremiseStatusCodeFactory)
            });
            var nonConvertedPremiseService = GetEntityFactory<Service>().Create(new {
                Premise = nonConvertedPremise
            });

            var result = _target.ByInstallationNumberAndOperatingCenter(installation, opCenter.Id);
            
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(activePremise, result.Single().Premise);
        }
    }
}
