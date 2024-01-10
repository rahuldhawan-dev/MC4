using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ReportViewingMap : ClassMap<ReportViewing>
    {
        public ReportViewingMap()
        {
            Id(x => x.Id, "ReportViewingID").GeneratedBy.Identity().Not.Nullable();
            Map(x => x.DateViewed).Not.Nullable();
            Map(x => x.ReportName).Not.Nullable();
            // This is odd because the old 271 system called things employees that were actually users.
            // TODO: Clean up once all the 271 reports are moved to MVC.
            References(x => x.User, "EmployeeID").Not.Nullable();
        }
    }
}
