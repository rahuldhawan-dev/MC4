using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentNHibernate.Mapping;
using NHibernate;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    public class TestParent : IValidatableObject
    {
        #region Properties

        public virtual int Id { get; protected set; }
        public virtual IList<TestChild> Children { get; set; }

        #endregion

        #region Constructors

        public TestParent()
        {
            Children = new List<TestChild>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class TestChild : IValidatableObject
    {
        #region Properties

        public virtual int Id { get; protected set; }
        public virtual TestParent Parent { get; set; }

        #endregion

        #region Constructors

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class TestParentMap : ClassMap<TestParent>
    {
        public TestParentMap()
        {
            Id(x => x.Id);
            HasMany(x => x.Children).KeyColumn("ParentId").Cascade.All().Not.LazyLoad();
        }
    }

    public class TestChildMap : ClassMap<TestChild>
    {
        public TestChildMap()
        {
            Id(x => x.Id);
            References(x => x.Parent);
        }
    }

    public class ParentFactory : TestDataFactory<TestParent>
    {
        public ParentFactory(IContainer container) : base(container) { }

        static ParentFactory()
        {
            Defaults(new { });
        }
    }

    public class ChildFactory : TestDataFactory<TestChild>
    {
        public ChildFactory(IContainer container) : base(container) { }

        static ChildFactory()
        {
            Defaults(new {
                //Parent = typeof (ParentFactory)
            });
        }
    }
}
