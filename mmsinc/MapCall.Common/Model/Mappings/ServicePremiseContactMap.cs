using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServicePremiseContactMap : ClassMap<ServicePremiseContact>
    {
        public ServicePremiseContactMap()
        {
            Table("ServicePremiseContacts");
            Id(x => x.Id);
            Map(x => x.ContactDate).Not.Nullable();
            Map(x => x.CertifiedLetterSent).Not.Nullable();
            Map(x => x.NotifiedCustomerServiceCenter).Not.Nullable();
            Map(x => x.ContactInformation).Length(int.MaxValue).Nullable(); // ntext column
            Map(x => x.CommunicationResults).Length(int.MaxValue).Nullable(); // ntext column
            References(x => x.ContactMethod, "ServicePremiseContactMethodId").Not.Nullable();
            References(x => x.ContactType, "ServicePremiseContactTypeId").Not.Nullable();
            References(x => x.Service, "ServiceId").Not.Nullable();
            References(x => x.CreatedBy, "CreatedByUserId").Not.Nullable();
        }
    }

    public class ServicePremiseContactMethodMap : EntityLookupMap<ServicePremiseContactMethod>
    {
        public ServicePremiseContactMethodMap()
        {
            Table("ServicePremiseContactMethods");
        }
    }

    public class ServicePremiseContactTypeMap : EntityLookupMap<ServicePremiseContactType>
    {
        public ServicePremiseContactTypeMap()
        {
            Table("ServicePremiseContactTypes");
        }
    }
}
