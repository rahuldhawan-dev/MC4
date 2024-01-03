using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LookupMap : ClassMap<Lookup>
    {
        public const string TABLE_NAME = "Lookup",
                            DISCRIMINATOR = "LookupType";

        public LookupMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "LookupId");

            Map(x => x.Description, "LookupValue");
            DiscriminateSubClassesOnColumn(DISCRIMINATOR).AlwaysSelectWithValue();
        }
    }

    public class DepartmentNameMap : SubclassMap<DepartmentName>
    {
        public DepartmentNameMap()
        {
            DiscriminatorValue("DepartmentName");
        }
    }

    public class EmergencyResponsePriorityMap : SubclassMap<EmergencyResponsePriority>
    {
        public EmergencyResponsePriorityMap()
        {
            DiscriminatorValue("EmergencyResponsePriority");
        }
    }

    public class LicenseTypeMap : SubclassMap<LicenseType>
    {
        public LicenseTypeMap()
        {
            DiscriminatorValue("License_Type");
        }
    }

    public class SOPCategoryMap : SubclassMap<SOPCategory>
    {
        public SOPCategoryMap()
        {
            DiscriminatorValue("SOP_Category");
        }
    }

    public class SOPSectionMap : SubclassMap<SOPSection>
    {
        public SOPSectionMap()
        {
            DiscriminatorValue("Section_Number");
        }
    }

    public class SOPStatusMap : SubclassMap<SOPStatus>
    {
        public SOPStatusMap()
        {
            DiscriminatorValue("SOP_Status");
        }
    }

    public class SOPSubSectionMap : SubclassMap<SOPSubSection>
    {
        public SOPSubSectionMap()
        {
            DiscriminatorValue("Sub_Section_Number");
        }
    }

    public class SOPSystemMap : SubclassMap<SOPSystem>
    {
        public SOPSystemMap()
        {
            DiscriminatorValue("SOP_System");
        }
    }

    public class TailgateTopicCategoryMap : SubclassMap<TailgateTopicCategory>
    {
        public TailgateTopicCategoryMap()
        {
            DiscriminatorValue("TailgateCategory");
        }
    }

    public class TailgateTopicMonthMap : SubclassMap<TailgateTopicMonth>
    {
        public TailgateTopicMonthMap()
        {
            DiscriminatorValue("Target_Month");
        }
    }
}
