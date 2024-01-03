using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class RestorationMethodRepositoryTest : InMemoryDatabaseTest<RestorationMethod, RestorationMethodRepository>
    {
        #region Tests

        [TestMethod]
        public void GetByRestorationTypeIDShouldFindAllMethodsForTheGivenType()
        {
            var expected = GetFactory<RestorationMethodFactory>().CreateArray(3);
            var extra = GetFactory<RestorationMethodFactory>().Create();
            var restorationType = GetFactory<RestorationTypeFactory>().Create();

            foreach (var restorationMethod in expected)
            {
                restorationMethod.RestorationTypes.Add(restorationType);
                Session.SaveOrUpdate(restorationMethod);
            }

            Session.Flush();
            Session.Clear();

            var actual = Repository.GetByRestorationTypeID(restorationType.Id).ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }

            Assert.IsFalse(expected.Contains(extra));
        }

        #endregion
    }
}
