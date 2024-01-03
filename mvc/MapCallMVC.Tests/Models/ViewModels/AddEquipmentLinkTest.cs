using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels 
{
    [TestClass]
    public class AddEquipmentLinkTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        #region Fields

        private AddEquipmentLink _target;

        #endregion

        #region Init/Cleanup
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AddEquipmentLink(_container);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityAddsNewEquipmentLink()
        {
            var linkType = GetEntityFactory<LinkType>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();
            _target.Url = "foo";
            _target.LinkType = linkType.Id;
            
            _target.MapToEntity(equipment);

            Assert.AreEqual(_target.Url, equipment.Links.Single().Url);
            Assert.AreEqual(_target.LinkType, equipment.Links.Single().LinkType.Id);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.Url);
            ValidationAssert.PropertyIsRequired(_target, x => x.LinkType);
        }

        [TestMethod]
        public void TestStringLengthValidation()
        {
            //valeue needs to be in the url so we create one for the test.
            var url = "http://mapcall.amwater.com/";
            url = url + url.PadRight(EquipmentLink.StringLengths.PAYMENT_METHOD_URL - url.Length, 'x');
            _target.Url = url;
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.Url, EquipmentLink.StringLengths.PAYMENT_METHOD_URL, true);
        }

        [TestMethod]
        public void TestUrlFieldMatchesSpecialUrlFormat()
        {
            ValidationAssert.PropertyMustBeUrl(_target, x => x.Url);
        }
    }
}
