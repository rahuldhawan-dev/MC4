using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceLineProtectionInvestigationMap : ClassMap<ServiceLineProtectionInvestigation>
    {
        public const string
            //- Pending Review - new record, not service fields for company service entered
            //- Renewal Required - if lead / galv / tubeloy entered for company service material
            //- No Action Required - if not(lead / galv / tubeloy)
            //- Renewal Completed - a renewal completed date has been entered
            STATUS_SQL = "(CASE" +
                         " WHEN (RenewalCompleted is not null) " +
                         "  THEN 4 " +
                         " WHEN (CompanyServiceLineMaterialId is null) " +
                         "  THEN 1 " +
                         " WHEN (CompanyServiceLineMaterialId in (6,7,10))" +
                         "  THEN 2" +
                         " ELSE " +
                         "   3" +
                         " END" +
                         ")";

        public ServiceLineProtectionInvestigationMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.CustomerName).Not.Nullable();
            Map(x => x.StreetNumber).Not.Nullable();
            Map(x => x.CustomerAddress2).Nullable();
            Map(x => x.CustomerZip).Not.Nullable();
            Map(x => x.PremiseNumber).Not.Nullable();
            Map(x => x.AccountNumber).Nullable();
            Map(x => x.CustomerPhone).Nullable();
            Map(x => x.DateInstalled).Nullable();
            Map(x => x.TheNotes, "Notes").Nullable();
            Map(x => x.RenewalCompleted).Nullable();

            References(x => x.Street).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.CustomerCity, "CustomerCity").Not.Nullable();
            References(x => x.CustomerServiceMaterial, "CustomerServiceLineMaterialId").Not.Nullable();
            References(x => x.CustomerServiceSize, "CustomerServiceLineSizeId").Nullable();
            References(x => x.WorkType, "ServiceLineProtectionWorkTypeId").Not.Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.Contractor).Not.Nullable();
            References(x => x.Service).Nullable();
            References(x => x.CompanyServiceMaterial, "CompanyServiceLineMaterialId").Nullable();
            References(x => x.CompanyServiceSize, "CompanyServiceLineSizeId").Nullable();
            
            References(x => x.Status).DbSpecificFormula(STATUS_SQL, STATUS_SQL);

            HasMany(x => x.ServiceLineProtectionInvestigationNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade
                                                                   .None();
            HasMany(x => x.ServiceLineProtectionInvestigationDocuments).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
