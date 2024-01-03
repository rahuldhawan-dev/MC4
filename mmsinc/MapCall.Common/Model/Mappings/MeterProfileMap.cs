using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterProfileMap : ClassMap<MeterProfile>
    {
        public MeterProfileMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MeterProfileID");

            References(x => x.Type).Column("MeterTypeID");
            References(x => x.Size).Column("MeterSizeID");
            References(x => x.Manufacturer).Column("MeterManufacturerID");
            References(x => x.DialCount).Column("NumberOfDials");
            References(x => x.UnitOfMeasure);
            References(x => x.Output).Column("MeterOutputs");

            Map(x => x.ProfileName).Length(MeterProfile.StringLenths.PROFILE_NAME);
            Map(x => x.EstimatedMaximumFlow).Precision(53);
            Map(x => x.AWWALowerLimitPercentage).Precision(18).Scale(2);
            Map(x => x.AWWAUpperLimitPercentage).Precision(18).Scale(2);
            Map(x => x.TestComments).Column("MeterTestComments").Length(MeterProfile.StringLenths.TEST_COMMENTS);
            Map(x => x.TestPointsMinimum).Precision(10);
        }
    }
}
