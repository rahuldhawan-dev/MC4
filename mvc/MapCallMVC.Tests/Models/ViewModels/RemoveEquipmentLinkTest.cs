using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels {
    [TestClass]
    public class RemoveEquipmentLinkTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        #region Fields

        private RemoveEquipmentLink _target;

        #endregion

        #region Init/Cleanup
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new RemoveEquipmentLink(_container);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityAddsNewEquipmentLink()
        {
            var linkType = GetEntityFactory<LinkType>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();
            var equipmentLink = GetEntityFactory<EquipmentLink>().Create(new {
                Equipment = equipment,
                LinkType = linkType,
                Url = "nonsense"
            });
            equipment.Links.Add(equipmentLink);
            _target.EquipmentLink = equipmentLink.Id;

            _target.MapToEntity(equipment);

            Assert.AreEqual(0, equipment.Links.Count());
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.EquipmentLink);
        }
    }
}
