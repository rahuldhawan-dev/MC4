using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TankInspectionMap : ClassMap<TankInspection>
    {
        public TankInspectionMap()
        {
            //General Tab
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Equipment).Nullable();
            References(x => x.ProductionWorkOrder).Not.Nullable();
            References(x => x.TankObservedBy).Not.Nullable();
            References(x => x.PublicWaterSupply).Nullable();
            Map(x => x.TankCapacity).Not.Nullable(); 
            Map(x => x.TankAddress).Length(TankInspection.StringLengths.COMMENT_STRING_LENGTH).Nullable(); 
            References(x => x.Town).Not.Nullable();
            References(x => x.Coordinate).Not.Nullable();
            Map(x => x.ZipCode).Length(TankInspection.StringLengths.ZIP_CODE).Nullable();
            Map(x => x.ObservationDate).Nullable();
            Map(x => x.LastObserved).Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.TankInspectionType).Not.Nullable();
            //Questions
            HasMany(x => x.TankInspectionQuestions).KeyColumn("TankInspectionId").Inverse().Cascade.AllDeleteOrphan();
            //Notes&Docs
            HasMany(x => x.Documents).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
