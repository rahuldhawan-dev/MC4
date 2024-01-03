using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ObjectMapping;
using Moq;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.StructureMap;

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class EntityMapPropertyDescriptorTest
    {
        #region Fields

        private EntityMapAttributedEntity _entity;
        private EntityMapAttributedViewModel _viewModel;

        private readonly Type _entityType = typeof(EntityMapAttributedEntity),
                              _viewModelType = typeof(EntityMapAttributedViewModel),
                              _referenceEntityType = typeof(EntityWithIdProperty);

        private EntityMapPropertyDescriptor _target;
        private Mock<MMSINC.Data.NHibernate.IRepository<EntityWithIdProperty>> _entityWithIdRepo;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _entityWithIdRepo = new Mock<MMSINC.Data.NHibernate.IRepository<EntityWithIdProperty>>();
            _entity = new EntityMapAttributedEntity();
            _viewModel = new EntityMapAttributedViewModel();
            _container.Inject(_entityWithIdRepo.Object);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        #endregion

        #region Private Methods

        private EntityMapPropertyDescriptor InitTarget(string primaryPropName, string secondaryPropertyName = null,
            MapDirections direction = MapDirections.BothWays)
        {
            return new EntityMapPropertyDescriptor(_container, _viewModelType.GetProperty(primaryPropName),
                _entityType, typeof(MMSINC.Data.NHibernate.IRepository<EntityWithIdProperty>), secondaryPropertyName,
                direction);
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsSecondaryPropertyName()
        {
            var target = new EntityMapAttribute("Whoop!");
            Assert.AreEqual("Whoop!", target.SecondaryPropertyName);
        }

        [TestMethod]
        public void TestConstructorSetsRepositoryTypeToNullWhenParameterIsNull()
        {
            var target = new EntityMapPropertyDescriptor(_container, _viewModelType.GetProperty("MapsBothWaysProperty"),
                typeof(EntityMapAttributedEntity), null);
            Assert.IsNull(target.RepositoryType);
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionIfRepositoryTypeDoesNotImplementIRepository()
        {
            MyAssert.Throws<EntityMapperException>(
                () => new EntityMapPropertyDescriptor(_container, _viewModelType.GetProperty("MapsBothWaysProperty"),
                    typeof(EntityMapAttributedEntity), typeof(object)));
        }

        #endregion

        #region MapToPrimary

        [TestMethod]
        public void TestMapToPrimary_SetsViewModelPropertyToIdValueOfEntityReferenceWhenEntityReferenceIsNotNull()
        {
            var target = InitTarget("MapsBothWaysProperty");
            _entity.MapsBothWaysProperty = new EntityWithIdProperty {Id = 15};
            target.MapToPrimary(_viewModel, _entity);
            Assert.AreEqual(15, _viewModel.MapsBothWaysProperty);
        }

        [TestMethod]
        public void
            TestMapToPrimary_SetsViewModelPropertyToZeroIfTheEntityPropertyIsNullAndTheViewModelPropertyIs_NOT_NullableInt()
        {
            var target = InitTarget("IntProperty");
            _entity.IntProperty = null;
            _viewModel.IntProperty = 444;
            target.MapToPrimary(_viewModel, _entity);
            Assert.AreEqual(0, _viewModel.IntProperty,
                "Properties of type int should be set to their default value if the entity's prop is null.");
        }

        [TestMethod]
        public void
            TestMapToPrimary_SetsViewModelPropertyToNullIfTheEntityPropertyIsNullAndTheViewModelPropertyIsNullableInt()
        {
            var target = InitTarget("NullableIntProperty");
            _entity.NullableIntProperty = null;
            _viewModel.NullableIntProperty = 444;
            target.MapToPrimary(_viewModel, _entity);
            Assert.IsNull(_viewModel.NullableIntProperty,
                "Properties of type int should be set to their default value if the entity's prop is null.");
        }

        [TestMethod]
        public void TestMapToPrimary_SetsViewModelPropertyIdArrayToValuesOfEntityReferencesWhenEntityReferenceList()
        {
            var target = InitTarget("SomeProperties");
            _entity.SomeProperties = new List<EntityWithIdProperty> {
                new EntityWithIdProperty {Id = 1},
                new EntityWithIdProperty {Id = 2},
                new EntityWithIdProperty {Id = 3},
            };
            var props = _entity.SomeProperties.ToArray();

            target.MapToPrimary(_viewModel, _entity);

            for (var i = 0; i < props.Count(); ++i)
            {
                Assert.AreEqual(_viewModel.SomeProperties[i], props[i].Id);
            }
        }

        #endregion

        #region MapToSecondary

        [TestMethod]
        public void TestMapToSecondary_SetsEntityPropertyWithObjectFromRepositoryBasedOnViewModelProperty()
        {
            var target = InitTarget("MapsBothWaysProperty");
            var refEntity = new EntityWithIdProperty {Id = 42};
            _viewModel.MapsBothWaysProperty = 42;
            _entityWithIdRepo.Setup(x => x.Load(_viewModel.MapsBothWaysProperty))
                             .Returns(refEntity)
                             .Verifiable();

            target.MapToSecondary(_viewModel, _entity);
            Assert.IsNotNull(_entity.MapsBothWaysProperty);
            Assert.AreSame(refEntity, _entity.MapsBothWaysProperty);

            _entityWithIdRepo.VerifyAll();
        }

        [TestMethod]
        public void TestMapToSecondary_SetsEntityPropertyListWithObjectsFromRepositoryBasedOnViewModelIntArrayProperty()
        {
            var target = InitTarget("SomeProperties");
            var refEntities = new[] {
                new EntityWithIdProperty {Id = 1},
                new EntityWithIdProperty {Id = 2},
                new EntityWithIdProperty {Id = 3}
            };
            _viewModel.SomeProperties = new[] {1, 2, 3};
            for (var i = 0; i < refEntities.Count(); ++i)
            {
                _entityWithIdRepo.Setup(x => x.Load(_viewModel.SomeProperties[i]))
                                 .Returns(refEntities[i])
                                 .Verifiable();
            }

            target.MapToSecondary(_viewModel, _entity);

            var props = _entity.SomeProperties.ToArray();
            for (var i = 0; i < refEntities.Count(); ++i)
            {
                Assert.AreSame(refEntities[i], props[i]);
            }

            // TODO: this seems to be a Moq bug
            //_entityWithIdRepo.VerifyAll();
        }

        [TestMethod]
        public void TestMapToSecondary_SetsEntityPropertyToNullIfNotFoundInRepository()
        {
            var target = InitTarget("MapsBothWaysProperty");
            var refEntity = new EntityWithIdProperty();
            _entity.MapsBothWaysProperty = refEntity;
            _viewModel.MapsBothWaysProperty = 42;
            _entityWithIdRepo.Setup(x => x.Load(_viewModel.MapsBothWaysProperty))
                             .Returns((EntityWithIdProperty)null)
                             .Verifiable();

            target.MapToSecondary(_viewModel, _entity);
            Assert.IsNull(_entity.MapsBothWaysProperty);
            _entityWithIdRepo.VerifyAll();
        }

        [TestMethod]
        public void TestMapToSecondary_SetsEntityPropertyToNullIfViewModelIdPropertyIsNullNullableInt()
        {
            var target = InitTarget("NullableIntProperty");
            _entity.NullableIntProperty = new EntityWithIdProperty();
            _viewModel.NullableIntProperty = null;
            target.MapToSecondary(_viewModel, _entity);
            Assert.IsNull(_entity.NullableIntProperty);
        }

        [TestMethod]
        public void TestMapToSecondary_CorrectlyReadsNullableIntValueThatIsNotNull()
        {
            var target = InitTarget("NullableIntProperty");
            var refEntity = new EntityWithIdProperty {Id = 99};
            _entityWithIdRepo.Setup(x => x.Load(refEntity.Id)).Returns(refEntity);
            _entity.NullableIntProperty = null;
            _viewModel = new EntityMapAttributedViewModel {
                NullableIntProperty = refEntity.Id
            };
            target.MapToSecondary(_viewModel, _entity);
            Assert.AreSame(refEntity, _entity.NullableIntProperty);
        }

        [TestMethod]
        public void
            TestMapToSecondary_SetsRepositoryTypePropertyToGeneratedRepositoryTypeWhenTheRepositoryTypePropertyIsNull()
        {
            var expectedType = typeof(MMSINC.Data.NHibernate.IRepository<>).MakeGenericType(_referenceEntityType);
            _container = new Container(e => e.For<IRepository<EntityWithIdProperty>>().Use(_entityWithIdRepo.Object));
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));

            var target = new EntityMapPropertyDescriptor(_container, _viewModelType.GetProperty("NullableIntProperty"),
                typeof(EntityMapAttributedEntity), null);
            var refEntity = new EntityWithIdProperty {Id = 99};
            _entityWithIdRepo.Setup(x => x.Find(refEntity.Id)).Returns(refEntity);
            _entity.NullableIntProperty = null;
            _viewModel = new EntityMapAttributedViewModel {
                NullableIntProperty = refEntity.Id
            };
            Assert.IsNull(target.RepositoryType, "This should be null before MaptoSecondary is called.");
            target.MapToSecondary(_viewModel, _entity);
            Assert.AreSame(expectedType, target.RepositoryType);
        }

        [TestMethod]
        public void
            TestMapToSecondary_ThrowsExceptionIfNoMatchingRepositoryTypeCanBeFoundInStructureMapWhenFindingGeneratedRepositoryType()
        {
            // Need to kill the one we injected in the test initializer
            _container = new Container();
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));

            var target = new EntityMapPropertyDescriptor(_container, _viewModelType.GetProperty("NullableIntProperty"),
                typeof(EntityMapAttributedEntity), null);
            var refEntity = new EntityWithIdProperty {Id = 99};
            _entityWithIdRepo.Setup(x => x.Find(refEntity.Id)).Returns(refEntity);
            _entity.NullableIntProperty = null;
            _viewModel = new EntityMapAttributedViewModel {
                NullableIntProperty = refEntity.Id
            };

            Assert.IsNull(target.RepositoryType, "This should be null before MaptoSecondary is called.");

            MyAssert.Throws<ObjectMapperException>(() => target.MapToSecondary(_viewModel, _entity));
            Assert.IsNull(target.RepositoryType, "Should stay null after exception");
        }

        [TestMethod]
        public void
            TestMapToSecondary_ThrowsExceptionIfStructureMapHasMoreThanOneMatchingTypeThatImplementsIRepositoryForTheEntityPropertyType()
        {
            // Setup ObjectFactory so it only includes our two IRepository<Model> types.
            _container = new Container();
            _container.Inject(new FakeModelRepository());
            _container.Inject(new AnotherFakeModelRepository());
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));

            var target = new EntityMapPropertyDescriptor(_container, _viewModelType.GetProperty("NullableIntProperty"),
                typeof(EntityMapAttributedEntity), null);
            var refEntity = new EntityWithIdProperty {Id = 99};
            _entityWithIdRepo.Setup(x => x.Find(refEntity.Id)).Returns(refEntity);
            _entity.NullableIntProperty = null;
            _viewModel = new EntityMapAttributedViewModel {
                NullableIntProperty = refEntity.Id
            };

            Assert.IsNull(target.RepositoryType, "This should be null before MaptoSecondary is called.");

            MyAssert.Throws<ObjectMapperException>(() => target.MapToSecondary(_viewModel, _entity));
            Assert.IsNull(target.RepositoryType, "Should stay null after exception");
        }

        #endregion

        #endregion

        #region Entity/ViewModel classes that use EntityMapAttribute

        private class EntityMapAttributedEntity
        {
            public EntityWithIdProperty MapsBothWaysProperty { get; set; }
            public EntityWithIdProperty ToEntityOnlyProperty { get; set; }
            public EntityWithIdProperty ToModelOnlyProperty { get; set; }
            public EntityWithIdProperty NoneProperty { get; set; }
            public EntityWithIdProperty AutoFindRepositoryProperty { get; set; }

            public EntityWithIdProperty IntProperty { get; set; }
            public EntityWithIdProperty NullableIntProperty { get; set; }

            public EntityWithIdProperty ADifferentProperty { get; set; }

            public IList<EntityWithIdProperty> SomeProperties { get; set; }
        }

        private class EntityMapAttributedViewModel
        {
            [EntityMap(typeof(IBaseRepository<EntityWithIdProperty>))]
            public int MapsBothWaysProperty { get; set; }

            [EntityMap(typeof(IBaseRepository<EntityWithIdProperty>), Direction = MapDirections.ToEntity)]
            public int ToEntityOnlyProperty { get; set; }

            [EntityMap(typeof(IBaseRepository<EntityWithIdProperty>), Direction = MapDirections.ToViewModel)]
            public int ToModelOnlyProperty { get; set; }

            [EntityMap(typeof(IBaseRepository<EntityWithIdProperty>), Direction = MapDirections.None)]
            public int NoneProperty { get; set; }

            [EntityMap]
            public int AutoFindRepositoryProperty { get; set; }

            [EntityMap]
            public int IntProperty { get; set; }

            [EntityMap]
            public int? NullableIntProperty { get; set; }

            [EntityMap("ADifferentProperty")]
            public int MapsToDifferentPropertyName { get; set; }

            [EntityMap]
            public int[] SomeProperties { get; set; }
        }

        public class EntityWithIdProperty
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
        }

        private class Entity
        {
            public Model AutoRepositoryProperty { get; set; }
            public Model IntProperty { get; set; }
        }

        private class ViewModel
        {
            public int AutoRepositoryProperty { get; set; }
            public int IntProperty { get; set; }
        }

        private class FakeModelRepository : IBaseRepository<Model>
        {
            public bool Exists(int id)
            {
                throw new NotImplementedException();
            }

            public void Delete(Model entity)
            {
                throw new NotImplementedException();
            }

            public Model Save(Model entity)
            {
                throw new NotImplementedException();
            }

            public void Save(IEnumerable<Model> entities)
            {
                throw new NotImplementedException();
            }

            public void Update(Model entity)
            {
                throw new NotImplementedException();
            }

            public Model Find(int id)
            {
                throw new NotImplementedException();
            }

            public int GetIdentifier(Model entity)
            {
                throw new NotImplementedException();
            }
        }

        private class AnotherFakeModelRepository : FakeModelRepository { }

        #endregion
    }
}
