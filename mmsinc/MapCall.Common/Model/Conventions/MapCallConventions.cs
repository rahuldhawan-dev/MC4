using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using MMSINC.ClassExtensions.StringExtensions;

namespace MapCall.Common.Model.Conventions
{
    public class MapCallConventions : IClassConvention, IIdConvention, IReferenceConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType.Name.Pluralize());
        }

        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
        }

        public void Apply(IManyToOneInstance instance)
        {
            if (string.IsNullOrWhiteSpace(((IManyToOneInspector)instance).Formula))
            {
                instance.Column(instance.Property.Name + "Id");
            }
        }
    }
}
