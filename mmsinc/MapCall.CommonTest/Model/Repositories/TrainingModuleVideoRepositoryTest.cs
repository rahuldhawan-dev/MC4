using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class TrainingModuleVideoRepositoryTest : Common.Testing.MapCallMvcSecuredRepositoryTestBase<Video,
        TrainingModuleVideoRepositoryTest.TestTrainingModuleVideoRepository, User>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            var u = base.CreateUser();
            u.Email = "hey@you.com";
            return u;
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        // Can't really test most of this because it's hitting an external web api in which we don't
        // know what data will be there. Not yet anyway.

        [TestMethod]
        public void TestConvertDynamidJsonToConcreteVideoWorksCorrectly()
        {
            var notActuallyJson = new {
                id = "hashvalueofsorts",
                embed_code = "some html of sorts ?type=sd",
                title = "Titles!",
                created_at = "2015-10-16T07:44:13-06:00",
                tags = new string[] { },
                assets = new {
                    thumbnails = new object[] { }
                }
            };
            var simulatedRawResponse = System.Web.Helpers.Json.Encode(notActuallyJson);
            var jsonObj = System.Web.Helpers.Json.Decode(simulatedRawResponse);
            var vid = Repository.ConvertDynamidJsonToConcreteVideo(jsonObj);

            Assert.AreEqual("hashvalueofsorts", vid.Id);
            Assert.AreEqual("some html of sorts ?vemail=hey@you.com&type=sd", vid.EmbedCode);
            Assert.AreEqual("Titles!", vid.Title);
            Assert.AreEqual("10/16/2015 9:44:13 AM", vid.CreatedAt);
        }

        [TestMethod]
        public void TestConvertDynamicJsonToConcreteVideoAppendsUserEmailAddressToEmbedCode()
        {
            var notActuallyJson = new {
                id = "hashvalueofsorts",
                embed_code = "this could be html with a querystring in its src?",
                title = "Titles!",
                created_at = "2015-10-16T07:44:13-06:00",
                tags = new string[] { },
                assets = new {
                    thumbnails = new object[] { }
                }
            };
            var simulatedRawResponse = System.Web.Helpers.Json.Encode(notActuallyJson);
            var jsonObj = System.Web.Helpers.Json.Decode(simulatedRawResponse);
            var vid = Repository.ConvertDynamidJsonToConcreteVideo(jsonObj);

            Assert.IsTrue(vid.EmbedCode.Contains("vemail=hey@you.com&"));
        }

        #endregion

        #region Test classes

        public class TestTrainingModuleVideoRepository : VideoRepository
        {
            public TestTrainingModuleVideoRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
                IAuthenticationService<User> authenticationService) : base(roleRepo, session, container,
                authenticationService) { }
        }

        #endregion
    }
}
