using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ShortCycleWorkOrderSafetyBriefMap : ClassMap<ShortCycleWorkOrderSafetyBrief>
    {
        public ShortCycleWorkOrderSafetyBriefMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.FSR, "FSRId").Not.Nullable();
            Map(x => x.DateCompleted).Not.Nullable();
            Map(x => x.IsPPEInGoodCondition).Not.Nullable();
            Map(x => x.HasCompletedDailyStretchingRoutine).Not.Nullable();
            Map(x => x.HasPerformedInspectionOnVehicle).Not.Nullable();

            // Types
            HasManyToMany(x => x.LocationTypes)
               .Table("ShortCycleWorkOrderSafetyBriefLocations")
               .ParentKeyColumn("ShortCycleWorkOrderSafetyBriefId")
               .ChildKeyColumn("SafetyBriefLocationTypeId");
            HasManyToMany(x => x.HazardTypes)
               .Table("ShortCycleWorkOrderSafetyBriefHazards")
               .ParentKeyColumn("ShortCycleWorkOrderSafetyBriefId")
               .ChildKeyColumn("SafetyBriefHazardTypeId");
            HasManyToMany(x => x.PPETypes)
               .Table("ShortCycleWorkOrderSafetyBriefPPE")
               .ParentKeyColumn("ShortCycleWorkOrderSafetyBriefId")
               .ChildKeyColumn("SafetyBriefPPETypeId");
            HasManyToMany(x => x.ToolTypes)
               .Table("ShortCycleWorkOrderSafetyBriefTools")
               .ParentKeyColumn("ShortCycleWorkOrderSafetyBriefId")
               .ChildKeyColumn("SafetyBriefToolTypeId");
        }
    }
}
