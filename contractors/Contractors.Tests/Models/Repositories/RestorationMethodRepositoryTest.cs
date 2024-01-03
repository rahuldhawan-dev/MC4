using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class RestorationMethodRepositoryTest : ContractorsControllerTestBase<RestorationMethod, RestorationMethodRepository>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            Repository = _container.GetInstance<RestorationMethodRepository>();
        }

        #endregion

        #region GetByRestorationTypeID

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