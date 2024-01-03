using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DriversLicenseEndorsementMap : ClassMap<DriversLicenseEndorsement>
    {
        public DriversLicenseEndorsementMap()
        {
            Table("DriversLicenseEndorsements");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Letter).Not.Nullable().Length(1);
            Map(x => x.Title).Not.Nullable().Length(55);
            HasMany(x => x.DriversLicensesEndorsements).KeyColumn("EndorsementId");
        }
    }
}
