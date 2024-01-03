using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EasementMap : ClassMap<Easement>
    {
        #region Constructors

        public EasementMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("EasementID");

            DynamicUpdate();

            Map(x => x.DateRecorded).Nullable();
            Map(x => x.DeedBook).Nullable().Length(50);
            Map(x => x.DeedPage).Nullable().Length(50);
            Map(x => x.Wbs).Nullable().Length(20).Column("Task_WorkOrder");
            Map(x => x.WorkOrderCompletionDate).Nullable();
            Map(x => x.PayFrequency).Nullable().Length(50);
            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.EasementDescription).Nullable();
            Map(x => x.EasementRequirements).Nullable().Length(50);
            Map(x => x.PropertyAddress).Nullable().Length(50);
            Map(x => x.PropertyZip).Nullable().Length(50);
            Map(x => x.BlockLot).Nullable().Length(50);
            Map(x => x.Fee).Nullable();
            Map(x => x.OwnerVendorName).Nullable().Length(50);
            Map(x => x.OwnerName).Nullable().Length(50);
            Map(x => x.OwnerAddress).Nullable().Length(50);
            Map(x => x.OwnerZip).Nullable().Length(50);
            Map(x => x.OwnerPhone).Nullable().Length(16);
            Map(x => x.Gsp).Nullable();
            Map(x => x.RecordNumber).Nullable().Length(Easement.StringLengths.RECORD_NUMBER);
            Map(x => x.StreetNumber).Nullable();

            References(x => x.OperatingCenter).Nullable().Column("OpCodeID");
            References(x => x.Category).Nullable();
            References(x => x.Reason).Nullable();
            References(x => x.Type).Nullable();
            References(x => x.PayMonth).Nullable();
            References(x => x.Town).Nullable().Column("PropertyTown");
            References(x => x.State).Nullable().Column("PropertyState");
            References(x => x.Coordinate).Nullable();
            References(x => x.FeeFrequency).Nullable();
            References(x => x.OwnerTown).Nullable().Column("OwnerTown");
            References(x => x.OwnerState).Nullable().Column("OwnerState");
            References(x => x.TownSection).Nullable();
            References(x => x.Street).Nullable();
            References(x => x.CrossStreet).Nullable();
            References(x => x.Status).Nullable();
            References(x => x.GrantorType).Nullable();

            HasMany(x => x.EasementNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.EasementDocuments).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
