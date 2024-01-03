using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using MMSINC.Testing.ClassExtensions;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    [TestClass]
    public class CreateCommunityRightToKnowTest : CommunityRightToKnowTestBase<CreateCommunityRightToKnow>
    {
        #region Fields

        protected Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsSubmissionDateToNow()
        {
            var now = DateTime.Now;
            _dateTimeProvider
               .Setup(dt => dt.GetCurrentDate())
               .Returns(now);

            _vmTester.MapToEntity();

            Assert.AreEqual(now, _entity.SubmissionDate);
        }

        #endregion
    }
}
