using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BusinessUnitMap : ClassMap<BusinessUnit>
    {
        #region Constructors

        public BusinessUnitMap()
        {
            Id(x => x.Id, "BusinessUnitID");

            Map(x => x.AuthorizedStaffingLevelBargainingUnit).Nullable();
            Map(x => x.AuthorizedStaffingLevelManagement).Nullable();
            Map(x => x.AuthorizedStaffingLevelNonBargainingUnit).Nullable();
            Map(x => x.AuthorizedStaffingLevelTotal).Nullable();
            Map(x => x.BU);
            Map(x => x.Description).Nullable().Length(BusinessUnit.StringLengths.DESCRIPTION);
            Map(x => x.Is271Visible);
            Map(x => x.Order, "[Order]");
            Map(x => x.IsActive).Nullable();

            References(x => x.Area, "BusinessUnitAreaId").Nullable();
            References(x => x.OperatingCenter);
            References(x => x.Department);
            References(x => x.EmployeeResponsible, "EmployeeResponsible").Nullable();
            
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
