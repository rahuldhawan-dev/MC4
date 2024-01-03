using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CommercialDriversLicenseProgramStatusMap : ClassMap<CommercialDriversLicenseProgramStatus>
    {
        #region Constants

        public const string TABLE_NAME = "CommercialDriversLicenseProgramStatuses";

        #endregion

        #region Constructors

        public CommercialDriversLicenseProgramStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Description).Not.Nullable(); //.Unique();
        }

        #endregion
    }
}
