using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class OperatorLicenseMapTest : InMemoryDatabaseTest<OperatorLicense>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
        }

        #region Tests

        [TestMethod]
        public void
            TestDeletingALicencedOperatorDeletesTheLinkToPublicWaterSupplyButDoesNotDeleteThePublicWaterSupplyItself()
        {
            //Assemble
            var publicWaterSupply = GetFactory<PublicWaterSupplyFactory>().Create();
            var operatorLicense = GetFactory<OperatorLicenseFactory>().Create();
            var pwsidOperator = GetFactory<PublicWaterSupplyLicensedOperatorFactory>().Create(new
                {LicensedOperator = operatorLicense, PublicWaterSupply = publicWaterSupply});
            //Session.Flush();
            Session.Evict(pwsidOperator);
            Session.Evict(publicWaterSupply);
            Session.Evict(operatorLicense);
            // Sanity here
            operatorLicense = Session.Query<OperatorLicense>().Single(x => x.Id == operatorLicense.Id);
            Session.Flush();
            Session.Delete(operatorLicense);
            Session.Flush();
            //Assert
            publicWaterSupply = Session.Query<PublicWaterSupply>().SingleOrDefault(x => x.Id == publicWaterSupply.Id);
            Assert.IsNotNull(publicWaterSupply,
                "The public water supply should not have been deleted when the operator license was deleted.");
            pwsidOperator = Session.Query<PublicWaterSupplyLicensedOperator>()
                                   .SingleOrDefault(x => x.Id == pwsidOperator.Id);
            Assert.IsNull(pwsidOperator,
                "The PublicWaterSupplyLicensedOperator should have been deleted when the operator license was deleted.");
        }

        #endregion
    }
}
