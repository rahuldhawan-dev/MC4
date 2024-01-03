using FluentNHibernate.Mapping;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GrievanceMap : ClassMap<Grievance>
    {
        public const int NUMBER_LENGTH = 50;
        public const string TABLE_NAME = FixTableAndColumnNamesForBug1623.NewTableNames.GRIEVANCES;

        public GrievanceMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id)
               .GeneratedBy.Identity();

            Map(x => x.DateReceived)
               .Not.Nullable();
            Map(x => x.EstimatedImpactValue).Nullable();
            Map(x => x.Number).Nullable();
            Map(x => x.IncidentDate).Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.DescriptionOfOutcome).Nullable();
            Map(x => x.UnionDueDate).Nullable();
            Map(x => x.ManagementDueDate).Nullable();

            References(x => x.Contract, FixTableAndColumnNamesForBug1623.NewColumnNames.Grievances.CONTRACT_ID);
            References(x => x.Categorization,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Grievances.CATEGORIZATION_ID);
            References(x => x.GrievanceCategory, "GrievanceCategoryId");
            References(x => x.Status, FixTableAndColumnNamesForBug1623.NewColumnNames.Grievances.STATUS_ID);
            References(x => x.OperatingCenter,
                    FixTableAndColumnNamesForBug1623.NewColumnNames.Common.OPERATING_CENTER_ID)
               .Not.Nullable();
            References(x => x.LaborRelationsBusinessPartner, "LaborRelationsBusinessPartnerEmployeeId").Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.GrievanceEmployees)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
