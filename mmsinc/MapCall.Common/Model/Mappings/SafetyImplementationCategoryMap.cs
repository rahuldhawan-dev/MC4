using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SafetyImplementationCategoryMap : ClassMap<SafetyImplementationCategory>
    {
        public SafetyImplementationCategoryMap()
        {
            Table("SafetyImplementationCategories");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(50);
            HasMany(x => x.BappTeamIdeas).KeyColumn("SafetyImplementationCategoryId");
        }
    }
}
