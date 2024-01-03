using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class DriversLicenseViewModelTest : InMemoryDatabaseTest<DriversLicense, RepositoryBase<DriversLicense>>
    {
        #region Private Members

        private IList<DriversLicenseEndorsement> _endorsements;
        private IList<DriversLicenseRestriction> _restrictions;
        private IViewModelFactory _viewModelFactory;

        #endregion

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(DriversLicenseEndorsementFactory).Assembly)
                             .GetInstance<TestDataFactoryService>();
        }

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
            _endorsements = GetEntityFactory<DriversLicenseEndorsement>().CreateList(6);
            _restrictions = GetEntityFactory<DriversLicenseRestriction>().CreateList(6);
        }

        #endregion

        [TestMethod]
        public void TestCreateSetsEndorsements()
        {
            var license = GetEntityFactory<DriversLicense>().Build();
            var model = _viewModelFactory.BuildWithOverrides<CreateDriversLicense, DriversLicense>(license, new {
                Endorsements = _endorsements.Map(x => x.Id).ToArray()
            });

            license = model.MapToEntity(license);

            foreach (var endorsement in _endorsements)
            {
                Assert.IsTrue(license.Endorsements.Any(x => x.DriversLicenseEndorsement == endorsement));
            }
        }

        [TestMethod]
        public void TestCreateSetsRestrictions()
        {
            var license = GetEntityFactory<DriversLicense>().Build();
            var model = _viewModelFactory.BuildWithOverrides<CreateDriversLicense, DriversLicense>(license, new {
                Restrictions = _restrictions.Map(x => x.Id).ToArray()
            });

            license = model.MapToEntity(license);

            foreach (var restriction in _restrictions)
            {
                Assert.IsTrue(license.Restrictions.Any(x => x.DriversLicenseRestriction == restriction));
            }
        }

        [TestMethod]
        public void TestUpdateUpdatesEndorsements()
        {
            var license = GetEntityFactory<DriversLicense>().Build();
            license = _viewModelFactory.BuildWithOverrides<CreateDriversLicense, DriversLicense>(license, new {
                Endorsements = _endorsements.Map(x => x.Id).Take(3).ToArray()
            }).MapToEntity(license);

            var expected = _endorsements.Map(x => x.Id).Skip(1).Take(3).ToArray();

            var model = _viewModelFactory.BuildWithOverrides<EditDriversLicense, DriversLicense>(license, new {
                Endorsements = expected
            });

            license = model.MapToEntity(license);

            foreach (var id in expected)
            {
                Assert.IsTrue(license.Endorsements.Any(x => x.DriversLicenseEndorsement.Id == id));
            }
            Assert.IsFalse(license.Endorsements.Any(x => x.DriversLicenseEndorsement.Id == _endorsements.First().Id));
        }

        [TestMethod]
        public void TestUpdateUpdatesRestrictions()
        {
            var license = GetEntityFactory<DriversLicense>().Build();
            license = _viewModelFactory.BuildWithOverrides<CreateDriversLicense, DriversLicense>(license, new {
                Restrictions = _restrictions.Map(x => x.Id).Take(3).ToArray()
            }).MapToEntity(license);

            var expected = _restrictions.Map(x => x.Id).Skip(1).Take(3).ToArray();

            var model = _viewModelFactory.BuildWithOverrides<EditDriversLicense, DriversLicense>(license, new {
                Restrictions = expected
            });

            license = model.MapToEntity(license);

            foreach (var id in expected)
            {
                Assert.IsTrue(license.Restrictions.Any(x => x.DriversLicenseRestriction.Id == id));
            }
            Assert.IsFalse(license.Restrictions.Any(x => x.DriversLicenseRestriction.Id == _restrictions.First().Id));
        }
    }
}
