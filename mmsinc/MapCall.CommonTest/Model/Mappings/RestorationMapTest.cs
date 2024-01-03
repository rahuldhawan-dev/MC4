using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class RestorationMapTest : InMemoryDatabaseTest<Restoration>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        private Restoration EvictAndRequery(Restoration model)
        {
            // Evict and requery to ensure the database is being queried.
            Session.Evict(model);
            return Session.Query<Restoration>().Single(x => x.Id == model.Id);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void
            TestDeletingARestorationMUSTNOTDeleteTheAssociatedRestorationMethodAssociatedWithPartialRestorationMethods()
        {
            var restMethod = GetFactory<RestorationMethodFactory>().Create();
            var restoration = GetFactory<RestorationFactory>().Create();
            restoration.PartialRestorationMethods.Add(restMethod);
            Session.Save(restoration);

            // This existing should cause the test to fail out right due to constraint errors
            // if the mapping is setup incorrectly.
            var anotherRestorationWithSameMethod = GetFactory<RestorationFactory>().Create();
            anotherRestorationWithSameMethod.PartialRestorationMethods.Add(restMethod);
            Session.Save(anotherRestorationWithSameMethod);

            Session.Flush();
            restoration = EvictAndRequery(restoration);
            anotherRestorationWithSameMethod = EvictAndRequery(anotherRestorationWithSameMethod);

            Session.Delete(restoration);
            Session.Flush();
        }

        [TestMethod]
        public void
            TestDeletingARestorationMUSTNOTDeleteTheAssociatedRestorationMethodAssociatedWithFinalRestorationMethods()
        {
            var restMethod = GetFactory<RestorationMethodFactory>().Create();
            var restoration = GetFactory<RestorationFactory>().Create();
            restoration.FinalRestorationMethods.Add(restMethod);
            Session.Save(restoration);

            // This existing should cause the test to fail out right due to constraint errors
            // if the mapping is setup incorrectly.
            var anotherRestorationWithSameMethod = GetFactory<RestorationFactory>().Create();
            anotherRestorationWithSameMethod.FinalRestorationMethods.Add(restMethod);
            Session.Save(anotherRestorationWithSameMethod);

            Session.Flush();
            restoration = EvictAndRequery(restoration);
            anotherRestorationWithSameMethod = EvictAndRequery(anotherRestorationWithSameMethod);

            Session.Delete(restoration);
            Session.Flush();
        }

        #endregion
    }
}
