using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EstimatingProjectMap : ClassMap<EstimatingProject>
    {
        public EstimatingProjectMap()
        {
            Table("EstimatingProjects");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.ProjectType).Not.Nullable();
            References(x => x.Town).Column("MunicipalityId").Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Estimator).Not.Nullable();
            References(x => x.Contractor).Nullable();

            Map(x => x.ProjectNumber).Not.Nullable().Length(30);
            Map(x => x.WBSNumber).Nullable().Length(18);
            Map(x => x.ProjectName).Not.Nullable().Length(50);
            Map(x => x.Street).Not.Nullable().Length(30);
            Map(x => x.Description).Nullable().Length(MakeDescriptionFieldLongerForBug1968.STRING_LENGTH);
            Map(x => x.EstimateDate).Not.Nullable();
            Map(x => x.Remarks).Nullable();
            Map(x => x.JDEPayrollNumber).Length(AddJDEPayrollNumberToEstimatingProjectsForBug2340.LENGTH).Nullable();
            Map(x => x.OverheadPercentage)
               .Column(CreateTablesForBug1774.ColumnNames.EstimatingProjects.OVERHEAD_PERCENTAGE)
               .Not.Nullable();
            Map(x => x.ContingencyPercentage)
               .Column(CreateTablesForBug1774.ColumnNames.EstimatingProjects.CONTINGENCY_PERCENTAGE)
               .Not.Nullable();
            Map(x => x.LumpSum).Column(CreateTablesForBug1774.ColumnNames.EstimatingProjects.LUMP_SUM)
                               .Nullable();
            Map(x => x.CreatedBy)
               .DbSpecificFormula(
                    "(select top 1 P.FullName From AuditLogEntries ale join tblPermissions P on P.RecId = ale.UserId where ale.AuditEntryType = 'INSERT' and ale.EntityName = 'EstimatingProject' and ale.EntityId = Id)",
                    "(select P.FullName From AuditLogEntries ale join tblPermissions P on P.RecId = ale.UserId where ale.AuditEntryType = 'INSERT' and ale.EntityName = 'EstimatingProject' and ale.EntityId = Id LIMIT 1)");

            HasMany(x => x.OtherCosts)
               .KeyColumn(CreateTablesForBug1780.ColumNames.EstimatingProjectOtherCosts.ESTIMATING_PROJECT_ID)
               .Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.ContractorLaborCosts)
               .KeyColumn("EstimatingProjectId")
               .Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.Materials)
               .KeyColumn(CreateTablesForBug1775.ColumnNames.EstimatingProjectsMaterials.ESTIMATING_PROJECT_ID)
               .Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.CompanyLaborCosts)
               .KeyColumn(AddNJAWLaborTableForBug1778.ColumnNames.ESTIMATING_PROJECT_ID)
               .Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.Permits)
               .KeyColumn(CreateTablesForBug1779.ColumnNames.ESTIMATING_PROJECT_ID)
               .Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.EstimatingProjectDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.EstimatingProjectNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
