using System;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MapCall.Common.Testing;


namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class TailgateTalkTopicTest<TViewModel> : ViewModelTestBase<TailgateTalkTopic, TViewModel>  where TViewModel : TailgateTalkTopicViewModel
    {
        [TestMethod]
        public void TestDescriptionReturnsIdAndTopic()
        {
            var id = 108;
            var topic = "Duran Duran is neither a Duran or a Duran";
            var target = new TailgateTalkTopic() {Topic = topic};
            target.SetPropertyValueByName("Id", id);

            Assert.AreEqual(String.Format("{0} - {1}", id, topic), target.Description);
        }
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Category, GetEntityFactory<TailgateTopicCategory>().Create());
            _vmTester.CanMapBothWays(x => x.Month, GetEntityFactory<TailgateTopicMonth>().Create());
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.Topic);
            _vmTester.CanMapBothWays(x => x.TargetDeliveryDate);
            _vmTester.CanMapBothWays(x => (int)x.TopicLevel);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.IsActive);
            ValidationAssert.PropertyIsRequired(x => x.Topic);
            ValidationAssert.PropertyIsRequired(x => x.Category);
            ValidationAssert.PropertyIsRequired(x => x.Month);
            ValidationAssert.PropertyIsRequired(x => x.TargetDeliveryDate);
            ValidationAssert.PropertyIsRequired(x => x.TopicLevel);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.Category, GetEntityFactory<TailgateTopicCategory>().Create());
            ValidationAssert.EntityMustExist(x => x.Month, GetEntityFactory<TailgateTopicMonth>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }
    }
}
