
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class WorkDescriptionRepositoryTest : ContractorsControllerTestBase<WorkDescription, WorkDescriptionRepository>
    {
        // TODO: Rewrite
        //[TestMethod]
        //public void GetByAssetTypeIdShouldFindAllWorkDescriptionsForAssetType()
        //{
        //    //var assetType = GetFactory<ValveAssetTypeFactory>().B();
        //    var expected = GetFactory<WorkDescriptionFactory>().CreateList(3); //, new {AssetType = assetType});
        //    var notExpected = GetFactory<WorkDescriptionFactory>().Create(new { 
        //        Description="Blargh", 
        //        AssetType = GetFactory<HydrantAssetTypeFactory>().Create()});

        //    var actual = Repository.GetByAssetTypeId((int)AssetTypeEnum.Valve).ToArray();

        //    Assert.AreEqual(expected.Count(), actual.Length);
        //    Assert.IsFalse(actual.Contains(notExpected));
        //    foreach (var wd in expected)
        //    {
        //        Assert.IsTrue(actual.Contains(wd));
        //    }
        //}
    }
}
