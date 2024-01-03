using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringProjectStatusMap : ClassMap<RecurringProjectStatus>
    {
        #region Constants

        public const string TABLE_NAME = "RecurringProjectStatuses";

        #endregion

        #region Constructors

        public RecurringProjectStatusMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Description);
            //HasMany(x => x.RecurringProjects).KeyColumn("RPProjectStatusID");
        }

        #endregion
    }
}
