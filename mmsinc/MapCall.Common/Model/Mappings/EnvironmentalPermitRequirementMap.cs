using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalPermitRequirementMap : ClassMap<EnvironmentalPermitRequirement>
    {
        public EnvironmentalPermitRequirementMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EnvironmentalPermit).Column("PermitId").Not.Nullable();
            References(x => x.RequirementType).Not.Nullable();
            References(x => x.ValueUnit).Not.Nullable();
            References(x => x.ValueDefinition).Not.Nullable();
            References(x => x.TrackingFrequency).Not.Nullable();
            References(x => x.ReportingFrequency).Not.Nullable();
            References(x => x.ProcessOwner).Nullable();
            References(x => x.ReportingOwner).Not.Nullable();
            References(x => x.CommunicationType).Nullable();

            Map(x => x.Requirement).Not.Nullable().Length(EnvironmentalPermitRequirement.StringLengths.REQUIREMENT);
            Map(x => x.ReportingFrequencyDetails);
            Map(x => x.ReportDataStorageLocation)
               .Length(EnvironmentalPermitRequirement.StringLengths.REPORT_DATA_STORAGE_LOCATION);
            Map(x => x.ReportCreationInstructions)
               .Length(EnvironmentalPermitRequirement.StringLengths.REPORT_CREATION_INSTRUCTIONS);
            Map(x => x.ReportSendTo);
            Map(x => x.Notes);
            Map(x => x.CommunicationEmail).Nullable()
                                          .Length(EnvironmentalPermitRequirement.StringLengths.COMMUNICATION_EMAIL);
            Map(x => x.CommunicationLink).Nullable()
                                         .Length(EnvironmentalPermitRequirement.StringLengths.COMMUNICATION_LINK);
        }
    }
}
