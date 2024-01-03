using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class LockoutFormQuestionCategoryMap : ClassMap<LockoutFormQuestionCategory>
    {
        public const string TABLE_NAME = "LockoutFormQuestionCategories";

        public LockoutFormQuestionCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Description);
        }
    }
}
