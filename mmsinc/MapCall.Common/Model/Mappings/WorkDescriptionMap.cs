using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WorkDescriptionMap : ClassMap<WorkDescription>
    {
        #region Constructors

        public WorkDescriptionMap()
        {
            Id(x => x.Id).Column("WorkDescriptionID").GeneratedBy.Assigned();

            //References(x => x.WorkDescription);
            References(x => x.AssetType);
            References(x => x.WorkCategory).Nullable();
            References(x => x.FirstRestorationAccountingCode).Not.Nullable();
            References(x => x.FirstRestorationProductCode).Not.Nullable();
            References(x => x.SecondRestorationAccountingCode);
            References(x => x.SecondRestorationProductCode);
            References(x => x.AccountingType);
            References(x => x.PlantMaintenanceActivityType).Not.Nullable();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.TimeToComplete).Not.Nullable();
            Map(x => x.FirstRestorationCostBreakdown).Not.Nullable();
            Map(x => x.SecondRestorationCostBreakdown);
            Map(x => x.ShowBusinessUnit).Not.Nullable();
            Map(x => x.ShowApprovalAccounting).Not.Nullable();
            Map(x => x.EditOnly).Not.Nullable();
            Map(x => x.Revisit).Not.Nullable();
            Map(x => x.MaintenanceActivityType).Length(WorkDescription.StringLengths.MAINT_ACT_TYPE).Nullable();
            Map(x => x.IsActive).Not.Nullable().Default("true");
            Map(x => x.MarkoutRequired).Nullable();
            Map(x => x.MaterialsRequired).Nullable();
            Map(x => x.JobSiteCheckListRequired).Nullable();
            Map(x => x.DigitalAsBuiltRequired).Not.Nullable();

            //HasMany(x => x.WorkDescriptions).KeyColumn("WorkDescriptionID");
            //HasMany(x => x.WorkOrderDescriptionChanges).KeyColumn("FromWorkDescriptionID");
            //HasMany(x => x.WorkOrderDescriptionChanges).KeyColumn("ToWorkDescriptionID");
            //HasMany(x => x.WorkOrders).KeyColumn("WorkDescriptionID");
        }

        #endregion
    }
}
