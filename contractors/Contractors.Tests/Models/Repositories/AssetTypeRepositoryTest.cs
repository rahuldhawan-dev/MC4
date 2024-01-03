using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class AssetTypeRepositoryTest : ContractorsControllerTestBase<AssetType, AssetTypeRepository>
    {
        // TODO: REFACTOR OUT DUPLICATION
        [TestMethod]
        public void TestLinqOnlyAllowsAccessToTheAssetTypesBelongingToTheOperatingCentersThatTheCurrentUserCanAccess()
        {
            var currentContractor = GetFactory<ContractorFactory>().Create();
            var currentUser = GetFactory<ContractorUserFactory>().Create(new {Contractor = currentContractor});
            var currentOperatingCenter = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ1"});
            var extraOperatingCenter = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ2"});
            var expected = new[] {
                GetFactory<ValveAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create()
            };
            var extraAssetTypes = new[] {
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create()
            };

            _container.Inject((_authenticationService = new MockAuthenticationService<ContractorUser>(currentUser)).Object);

            currentContractor.OperatingCenters.Add(currentOperatingCenter);
            expected.Each(t =>
                currentOperatingCenter.OperatingCenterAssetTypes.Add(
                    new OperatingCenterAssetType {
                        OperatingCenter =
                            currentOperatingCenter,
                        AssetType = t
                    }));
            Session.SaveOrUpdate(currentOperatingCenter);
            Session.SaveOrUpdate(currentContractor);
            Session.SaveOrUpdate(currentUser);

            extraAssetTypes.Each(t => extraOperatingCenter.OperatingCenterAssetTypes.Add(
                    new OperatingCenterAssetType {
                        OperatingCenter =
                            extraOperatingCenter,
                        AssetType = t
                    }));
            Session.SaveOrUpdate(extraOperatingCenter);

            Session.Flush();
            Session.Clear();

            Repository = _container.GetInstance<AssetTypeRepository>();

            // .GetAll uses the Linq property (for now at least)
            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Description, actual[i].Description);
            }
        }

        // TODO: REFACTOR OUT DUPLICATION
        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToTheAssetTypesBelongingToTheOperatingCentersThatTheCurrentUserCanAccess()
        {
            var currentContractor = GetFactory<ContractorFactory>().Create();
            var currentUser = GetFactory<ContractorUserFactory>().Create(new {Contractor = currentContractor});
            var currentOperatingCenter = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ1"});
            var extraOperatingCenter = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ2"});
            var expected = new[] {
                GetFactory<ValveAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create()
            };
            var extraAssetTypes = new[] {
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create()
            };

            _container.Inject((_authenticationService = new MockAuthenticationService<ContractorUser>(currentUser)).Object);

            currentContractor.OperatingCenters.Add(currentOperatingCenter);
            expected.Each(t =>
                currentOperatingCenter.OperatingCenterAssetTypes.Add(
                    new OperatingCenterAssetType {
                        OperatingCenter =
                            currentOperatingCenter,
                        AssetType = t
                    }));
            Session.SaveOrUpdate(currentOperatingCenter);
            Session.SaveOrUpdate(currentContractor);
            Session.SaveOrUpdate(currentUser);

            extraAssetTypes.Each(t =>
                extraOperatingCenter.OperatingCenterAssetTypes.Add(
                    new OperatingCenterAssetType {
                        OperatingCenter =
                            extraOperatingCenter,
                        AssetType = t
                    }));
            Session.SaveOrUpdate(extraOperatingCenter);

            Session.Flush();
            Session.Clear();

            Repository = _container.GetInstance<AssetTypeRepository>();

            // .Search uses the Criteria property (for now at least)
            var search = Repository.Search(Restrictions.Conjunction());
            var actual = search.List<AssetType>().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Description, actual[i].Description);
            }
        }
    }
}
