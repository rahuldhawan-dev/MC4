using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DriversLicensesRestrictionMap : ClassMap<DriversLicensesRestriction>
    {
        public DriversLicensesRestrictionMap()
        {
            Table("DriversLicensesRestrictions");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.DriversLicense).Column("LicenseId").Nullable();
            References(x => x.DriversLicenseRestriction).Column("RestrictionId").Nullable();
        }
    }
}
