using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetSchemaVersionsMap : ClassMap<AspnetSchemaVersions>
    {
        public AspnetSchemaVersionsMap()
        {
            Table("aspnet_SchemaVersions");

            LazyLoad();
            ReadOnly();

            CompositeId().KeyProperty(x => x.Feature, "Feature")
                         .KeyProperty(x => x.CompatibleSchemaVersion, "CompatibleSchemaVersion");

            Map(x => x.IsCurrentVersion).Column("IsCurrentVersion").Not.Nullable();
        }
    }
}
