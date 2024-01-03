using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class MostRecentlyInstalledServiceTest
    {
        [TestMethod]
        public void Test_ToJSONObject_ReturnsMostRecentlyInstalledServiceAsAnonymousObjectWithIdValues()
        {
            var actual = new MostRecentlyInstalledService {
                Premise = new Premise { Id = 1 },
                Service = new Service { Id = 2 },
                ServiceMaterial = new ServiceMaterial { Id = 3 },
                ServiceSize = new ServiceSize { Id = 4 },
                CustomerSideMaterial = new ServiceMaterial { Id = 5 },
                CustomerSideSize = new ServiceSize { Id = 6 }
            };

            dynamic result = MostRecentlyInstalledService.ToJSONObject(actual);
            
            Assert.AreEqual(actual.Premise.Id, result.PremiseId);
            Assert.AreEqual(actual.Service.Id, result.ServiceId);
            Assert.AreEqual(actual.ServiceMaterial.Id, result.ServiceMaterialId);
            Assert.AreEqual(actual.ServiceSize.Id, result.ServiceSizeId);
            Assert.AreEqual(actual.CustomerSideMaterial.Id, result.CustomerSideMaterialId);
            Assert.AreEqual(actual.CustomerSideSize.Id, result.CustomerSideSizeId);
        }

        [TestMethod]
        public void Test_ToJSONObject_DoesNotErrorWhenASizeOrMaterialAreNull()
        {
            var actual = new MostRecentlyInstalledService {
                Premise = new Premise { Id = 1 },
                Service = new Service { Id = 2 }
            };
            
            dynamic result = MostRecentlyInstalledService.ToJSONObject(actual);

            Assert.IsNull(result.ServiceMaterialId);
            Assert.IsNull(result.ServiceSizeId);
            Assert.IsNull(result.CustomerSideMaterialId);
            Assert.IsNull(result.CustomerSideSizeId);
        }
    }
}
