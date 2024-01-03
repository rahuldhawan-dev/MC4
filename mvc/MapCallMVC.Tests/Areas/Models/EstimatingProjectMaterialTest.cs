using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Models
{
    [TestClass]
    public class EstimatingProjectMaterialTest : MapCallMvcInMemoryDatabaseTestBase<EstimatingProjectMaterial>
    {
        #region Fields

        private EstimatingProjectMaterial _entity;
        private EstimatingProjectMaterialViewModel _target;
        private ViewModelTester<EstimatingProjectMaterialViewModel, EstimatingProjectMaterial> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IMaterialRepository>().Use<MaterialRepository>();
            e.For<IEstimatingProjectRepository>().Use<EstimatingProjectRepository>();
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<EstimatingProjectMaterial>().Create();
            _target = new EstimatingProjectMaterialViewModel(_container);
            _vmTester = new ViewModelTester<EstimatingProjectMaterialViewModel, EstimatingProjectMaterial>(_target, _entity);
            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.SetupGet(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestViewModelMapSetsPropertiesAndIds()
        {
            var estimatingProject = GetEntityFactory<EstimatingProject>().Create();
            var material = GetEntityFactory<Material>().Create();

            var estimatingProjectMaterial = GetEntityFactory<EstimatingProjectMaterial>().Create(new {
                EstimatingProject = estimatingProject,
               Material = material,
               Quantity = 5
            });

            var target = new EstimatingProjectMaterialViewModel(_container);
            target.Map(estimatingProjectMaterial);

            Assert.AreEqual(material.Id, estimatingProjectMaterial.Material.Id);
            Assert.AreEqual(estimatingProject.Id, estimatingProjectMaterial.EstimatingProject.Id);
        }

        [TestMethod]
        public void TestViewModelMapToEntitySetsProperties()
        {
            var estimatingProject = GetEntityFactory<EstimatingProject>().Create();
            var material = GetEntityFactory<Material>().Create();

            var target = new EstimatingProjectMaterialViewModel(_container) {
                Material = material.Id,
                EstimatingProject = estimatingProject.Id,
                Quantity = 5
            };
            var entity = new EstimatingProjectMaterial();

            target.MapToEntity(entity);

            Assert.AreEqual(estimatingProject.Id, entity.EstimatingProject.Id);
            Assert.AreEqual(material.Id, entity.Material.Id);
        }

        #endregion
    }
}
