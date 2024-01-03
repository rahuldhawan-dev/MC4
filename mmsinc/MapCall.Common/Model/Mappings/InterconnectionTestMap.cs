using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionTestMap : ClassMap<InterconnectionTest>
    {
        #region Constructors

        public InterconnectionTestMap()
        {
            Id(x => x.Id, "InterconnectionTestId").GeneratedBy.Identity();

            References(x => x.Facility);
            References(x => x.Employee);
            References(x => x.Contractor);
            References(x => x.WorkOrder);
            References(x => x.InterconnectionInspectionRating);

            Map(x => x.InspectionDate).Nullable();
            Map(x => x.MaxFlowMGDAchieved).Nullable().Precision(10);
            Map(x => x.AllValvesOperational).Nullable();
            Map(x => x.InspectionComments).Nullable().Length(InterconnectionTest.StringLengths.INSPECTION_COMMENTS);
            Map(x => x.RepresentativeOnSite).Nullable().Length(InterconnectionTest.StringLengths.REPRESENTATIVE_ON_SITE);

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
