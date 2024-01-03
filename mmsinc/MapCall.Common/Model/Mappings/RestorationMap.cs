using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RestorationMap : ClassMap<Restoration>
    {
        #region Constructors

        public RestorationMap()
        {
            Id(x => x.Id, "RestorationID");

            Map(x => x.LinearFeetOfCurb);
            Map(x => x.RestorationNotes).AsTextField().Nullable(); // Required on new records though.
            Map(x => x.PartialRestorationNotes).AsTextField().Nullable();
            Map(x => x.FinalRestorationNotes).AsTextField().Nullable();
            Map(x => x.PartialRestorationInvoiceNumber)
               .Length(Restoration.StringLengths.PARTIAL_INVOICE_NUMBER)
               .Nullable();
            Map(x => x.PartialRestorationDate);

            Map(x => x.PartialRestorationPriorityUpcharge).Nullable();
            Map(x => x.PartialPavingSquareFootage).Nullable();
            Map(x => x.PartialRestorationTrafficControlCost, "TrafficControlCostPartialRestoration");
            Map(x => x.FinalRestorationInvoiceNumber)
               .Length(Restoration.StringLengths.FINAL_INVOICE_NUMBER)
               .Nullable();
            Map(x => x.FinalRestorationDate);
            Map(x => x.FinalRestorationPriorityUpcharge).Nullable();
            Map(x => x.FinalPavingSquareFootage).Nullable();
            Map(x => x.FinalRestorationTrafficControlCost, "TrafficControlCostFinalRestoration");
            Map(x => x.EightInchStabilizeBaseByCompanyForces).Not.Nullable();
            Map(x => x.TotalAccruedCost);
            Map(x => x.PartialRestorationActualCost);
            Map(x => x.FinalRestorationActualCost);
            Map(x => x.PavingSquareFootage);
            Map(x => x.PartialRestorationPurchaseOrderNumber)
               .Length(Restoration.StringLengths.PARTIAL_PO_NUM)
               .Nullable();
            Map(x => x.FinalRestorationPurchaseOrderNumber)
               .Length(Restoration.StringLengths.FINAL_PO_NUM)
               .Nullable();
            Map(x => x.PartialRestorationTrafficControlInvoiceNumber)
               .Length(Restoration.StringLengths.PARTIAL_AND_FINAL_TRAFFIC_CONTROL_INVOICE_NUMBER)
               .Nullable();
            Map(x => x.FinalRestorationTrafficControlInvoiceNumber)
               .Length(Restoration.StringLengths.PARTIAL_AND_FINAL_TRAFFIC_CONTROL_INVOICE_NUMBER)
               .Nullable();
            Map(x => x.AssignedContractorAt).Nullable();
            Map(x => x.CreatedByContractorAt).Nullable();
            Map(x => x.PartialRestorationDueDate).Nullable();
            Map(x => x.FinalRestorationDueDate).Nullable();
            Map(x => x.PartialRestorationApprovedAt).Nullable();
            Map(x => x.FinalRestorationApprovedAt).Nullable();
            Map(x => x.WBSNumber)
               .Length(Restoration.StringLengths.WBS_NUMBER)
               .Nullable();
            Map(x => x.CompletedByOthers).Nullable();
            Map(x => x.CompletedByOthersNotes).AsTextField().Nullable();
            Map(x => x.TrafficControlRequired).Nullable();
            Map(x => x.DateRecompleted).Nullable();
            Map(x => x.DateReopened).Nullable();
            Map(x => x.DateRescheduled).Nullable();
            Map(x => x.InitialPurchaseOrderNumber).Nullable().Length(Restoration.StringLengths.INITIAL_PO_NUM);
            Map(x => x.PartialRestorationBreakoutBilling).AsTextField().Nullable();
            Map(x => x.AcknowledgedByContractor).Not.Nullable();

            // Map(x => x.PartialPavingBreakOutEightInches);
            // Map(x => x.PartialPavingBreakOutTenInches);
            // Map(x => x.PartialSawCutting);
            // Map(x => x.DaysToPartialPaveHole);

            //  Map(x => x.FinalPavingBreakOutEightInches);
            // Map(x => x.FinalPavingBreakOutTenInches);
            //  Map(x => x.FinalSawCutting);
            //  Map(x => x.DaysToFinalPaveHole);
            //   Map(x => x.DateApproved);
            //  Map(x => x.ApprovedByID);
            //  Map(x => x.RejectedByID);
            //   Map(x => x.DateRejected);
            //   Map(x => x.SawCutByCompanyForces).Not.Nullable();

            References(x => x.WorkOrder)
               .Nullable(); // This used to not be nullable but now they can make restorations without workorders.
            References(x => x.RestorationType).Not.Nullable();
            References(x => x.ResponsePriority).Not.Nullable();
            References(x => x.CreatedByContractor).Nullable();
            References(x => x.AssignedContractor).Nullable(); // Nullable but required for new restorations.
            References(x => x.PartialRestorationPriorityUpchargeType).Nullable();
            References(x => x.FinalRestorationPriorityUpchargeType).Nullable();
            References(x => x.PartialRestorationCompletedBy, "PartialRestorationCompletedByContractorId").Nullable();
            References(x => x.FinalRestorationCompletedBy, "FinalRestorationCompletedByContractorId").Nullable();
            References(x => x.Town).Nullable();
            References(x => x.OperatingCenter).Nullable();

            // DO NOT CASCADE THIS! When deleting a restoration, it will make nhibernate try to delete
            // the RestorationMethod too.
            HasManyToMany(x => x.PartialRestorationMethods)
               .Table("PartialRestorationsRestorationMethods")
               .ParentKeyColumn("RestorationId")
               .ChildKeyColumn("RestorationMethodId");

            // DO NOT CASCADE THIS! When deleting a restoration, it will make nhibernate try to delete
            // the RestorationMethod too.
            HasManyToMany(x => x.FinalRestorationMethods)
               .Table("FinalRestorationsRestorationMethods")
               .ParentKeyColumn("RestorationId")
               .ChildKeyColumn("RestorationMethodId");

            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
