using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HelpCategoryMap : ClassMap<HelpCategory>
    {
        public HelpCategoryMap()
        {
            Table("HelpCategories");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("HelpCategoryID");
            Map(x => x.Description).Not.Nullable().Length(255);
        }
    }
}
