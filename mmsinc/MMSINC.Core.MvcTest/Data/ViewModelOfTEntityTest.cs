using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class ViewModelOfTEntityTest
    {
        #region Fields

        private TestViewModel _target;
        private Mock<IObjectMapper> _mapper;

        #endregion

        #region Initializers

        [TestInitialize]
        public void InitializeTest()
        {
            _mapper = new Mock<IObjectMapper>();
            _target = new TestViewModel(new Container());
        }

        #endregion

        #region Constructor

        [TestMethod]
        public void TestConstructorDoesNotThrowForNullArgument()
        {
            MyAssert.DoesNotThrow(() => new TestViewModel(null));
            MyAssert.DoesNotThrow(() => new TestViewModel(null, null));
        }

        [TestMethod]
        public void TestConstructorSetsOriginalPropertyToPassedInEntity()
        {
            var expected = new Entity();
            var target = new TestViewModel(null, expected);
            Assert.AreSame(expected, target.OriginalTest);
        }

        #endregion

        #region CreateViewModelMapper

        [TestMethod]
        public void TestCreateViewModelMapperCreatesInstanceUsingEntityType()
        {
            var mapper = (AutoObjectMapper)_target.Mapper;
            Assert.AreSame(typeof(Entity), mapper.SecondaryType);
        }

        [TestMethod]
        public void TestCreateViewModelMapperCreatesInstanceWithDerivedViewModelType()
        {
            var mapper = (AutoObjectMapper)_target.Mapper;
            Assert.AreSame(typeof(TestViewModel), mapper.PrimaryType);
        }

        #endregion

        #region Map

        [TestMethod]
        public void TestMapThrowsForNullEntity()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.Map(null));
        }

        [TestMethod]
        public void TestMapUsesMapperToMap()
        {
            var entity = new Entity();
            _target.Mapper = _mapper.Object;
            _target.Map(entity);
            _mapper.Verify(x => x.MapToPrimary(_target, entity));
        }

        #endregion

        #region MapToEntity

        [TestMethod]
        public void TestMapToEntityThrowsForNullEntity()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.MapToEntity(null));
        }

        [TestMethod]
        public void TestMapToEntityUsesMapperToMap()
        {
            var entity = new Entity();
            _target.Mapper = _mapper.Object;
            _target.MapToEntity(entity);
            _mapper.Verify(x => x.MapToSecondary(_target, entity));
        }

        #endregion

        #region Mapper

        [TestMethod]
        public void TestMapperReturnsMapperFromCreateViewModelMapperCallIfNull()
        {
            _target.Mapper = _mapper.Object;
            Assert.AreSame(_mapper.Object, _target.Mapper);
        }

        #endregion

        #region ToString

        [TestMethod]
        public void TestToStringReturnsToStringOfOriginalIfImplemented()
        {
            var entity = new EntityWithToString();
            var target = new TestViewModelWithToString(new Container(), entity);

            Assert.AreEqual(EntityWithToString.TO_STRING, target.ToString());
        }

        #endregion
    }

    #region Test classes

    public class Entity : IValidatableObject
    {
        public string StringProp { get; set; }
        public int IntProp { get; set; }
        public bool BoolProp { get; set; } // Property not in ViewModel.

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class TestViewModel : ViewModel<Entity>
    {
        public TestViewModel(IContainer container, Entity entity = null) : base(container)
        {
            if (entity != null)
            {
                Map(entity);
            }
        }

        public Entity OriginalTest
        {
            get { return Original; }
        }

        public string StringProp { get; set; }
        public int IntProp { get; set; }
        public double DoubleProp { get; set; } // Property not in Entity.
    }

    public class EntityWithToString : Entity, IValidatableObject
    {
        public const string TO_STRING = "TestToString";

        public override string ToString()
        {
            return TO_STRING;
        }
    }

    public class TestViewModelWithToString : ViewModel<EntityWithToString>
    {
        public TestViewModelWithToString(IContainer container, EntityWithToString entity) : base(container)
        {
            if (entity != null)
            {
                Map(entity);
            }
        }
    }

    #endregion
}
