using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using MMSINC.Validation;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class EntityMustExistAttributeTest
    {
        #region Fields

        private EntityMustExistAttribute _target;
        private ValidationContext _validationContext;
        private MockRepository _repo;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new EntityMustExistAttribute(typeof(MockEntity));
            _repo = new MockRepository();
            _container = new Container();
            _container.Inject<IRepository<MockEntity>>(_repo);
            _validationContext = new ValidationContext(new { }, new StructureMapServiceProvider(_container), null);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorThrowsForNullRepositoryType()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target = new EntityMustExistAttribute(null));
        }

        [TestMethod]
        public void TestConstructorSetsTypeProperty()
        {
            _target = new EntityMustExistAttribute(typeof(MockEntity));
            Assert.AreSame(typeof(IRepository<MockEntity>), _target.RepositoryType);
        }

        [TestMethod]
        public void TestIsValidReturnsTrueIfObjectExistsInRepository()
        {
            _repo.Items[42] = new MockEntity();
            Assert.AreEqual(ValidationResult.Success, _target.GetValidationResult(42, _validationContext));
        }

        [TestMethod]
        public void TestIsValidReturnsFalseIfObjectDoesNotExistInRepository()
        {
            Assert.AreNotEqual(ValidationResult.Success, _target.GetValidationResult(124121421, _validationContext));
        }

        [TestMethod]
        public void TestGetValidationResultReturnsFormattedErrorMessage()
        {
            var propName = "Yowsah!";
            var expected = _target.FormatErrorMessage(propName);
            var result = _target.GetValidationResult(3413413,
                new ValidationContext(new MockEntity(), new StructureMapServiceProvider(_container), null)
                    {DisplayName = "Yowsah!"});
            Assert.AreEqual(expected, result.ErrorMessage);
        }

        [TestMethod]
        public void TestProcessAddsSelfToMetadataAdditionalValues()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            _target.Process(metadata);
            Assert.AreEqual(_target,
                metadata.AdditionalValues[
                    EntityMustExistAttribute.ADDITIONAL_VALUES_KEY]);
        }

        [TestMethod]
        public void TestGetAttributeForModelReturnsAttributeFromMetadata()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            _target.Process(metadata);
            var result = EntityMustExistAttribute.GetAttributeForModel(metadata);
            Assert.AreSame(_target, result);
        }

        #endregion

        #region Test Classes

        public class MockEntity : IEntity
        {
            public int Id { get; set; }
        }

        public class MockRepository : IRepository<MockEntity>
        {
            public readonly Dictionary<int, MockEntity> Items = new Dictionary<int, MockEntity>();

            public bool Exists(int id)
            {
                return Items.ContainsKey(id);
            }

            public void Delete(MockEntity entity)
            {
                throw new NotImplementedException();
            }

            public MockEntity Save(MockEntity entity)
            {
                throw new NotImplementedException();
            }

            public void Save(IEnumerable<MockEntity> entities)
            {
                throw new NotImplementedException();
            }

            public void Update(MockEntity entity)
            {
                throw new NotImplementedException();
            }

            public MockEntity Find(int id)
            {
                throw new NotImplementedException();
            }

            public int GetIdentifier(MockEntity entity)
            {
                throw new NotImplementedException();
            }

            public IQueryable<MockEntity> Linq { get; }
            public ICriteria Criteria { get; }

            public int GetCountForCriterion(ICriterion criterion, IDictionary<string, string> aliases = null,
                ICriterion additionalCriterion = null)
            {
                throw new NotImplementedException();
            }

            public int GetCountForCriteria(ICriteria criteria)
            {
                throw new NotImplementedException();
            }

            public int GetCountForSearchSet<T>(ISearchSet<T> search) where T : class
            {
                throw new NotImplementedException();
            }

            public ICriteria Search(ICriterion criterion, IDictionary<string, string> aliases = null,
                ICriterion additionalCriterion = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, ICriteria criteria,
                Action<ISearchMapper> searchMapperCallback = null,
                int? maxResults = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, IQueryOver query,
                Action<ISearchMapper> searchMapperCallback = null)
            {
                throw new NotImplementedException();
            }

            public IQueryable<MockEntity> GetAll()
            {
                throw new NotImplementedException();
            }

            public IQueryable<MockEntity> GetAllSorted()
            {
                throw new NotImplementedException();
            }

            public IQueryable<MockEntity> GetAllSorted(Expression<Func<MockEntity, object>> sort)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TAs> GetAllAs<TAs>(Expression<Func<MockEntity, TAs>> expression)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<MockEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriterion filter,
                string sort = null,
                bool sortAsc = true)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<MockEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriteria criteria,
                string sort = null,
                bool sortAsc = true)
            {
                throw new NotImplementedException();
            }

            public MockEntity Load(int id)
            {
                throw new NotImplementedException();
            }

            public IQueryable<MockEntity> Where(Expression<Func<MockEntity, bool>> p)
            {
                throw new NotImplementedException();
            }

            public bool Any(Expression<Func<MockEntity, bool>> p)
            {
                throw new NotImplementedException();
            }

            public Dictionary<int, MockEntity> FindManyByIds(IEnumerable<int> ids)
            {
                throw new NotImplementedException();
            }

            public void ClearSession()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
