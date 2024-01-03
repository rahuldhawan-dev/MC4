using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MaintenancePlanTaskTypeMap : ClassMap<MaintenancePlanTaskType>
    {
        public MaintenancePlanTaskTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description, "Description").Length(MaintenancePlanTaskType.StringLengths.DESCRIPTION).Not.Nullable();
            Map(x => x.Abbreviation).Length(MaintenancePlanTaskType.StringLengths.ABBREVIATION).Nullable();
            Map(x => x.IsActive, "IsActive").Not.Nullable();
        }
    }
}
