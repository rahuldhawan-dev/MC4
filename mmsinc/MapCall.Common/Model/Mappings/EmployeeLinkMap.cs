using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeLinkMap : ClassMap<EmployeeLink>
    {
        public const string TABLE_NAME = "EmployeeLink";

        public EmployeeLinkMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "EmployeeLinkId");

            Map(x => x.LinkedId, "DataLinkId");
            Map(x => x.LinkedBy, "CreatedBy");
            Map(x => x.LinkedOn, "CreatedOn");

            References(x => x.Employee, "tblEmployeeId").Not.LazyLoad();
            References(x => x.DataType);
        }
    }
}
