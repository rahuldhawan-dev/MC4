using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MedicalCertificateMap : ClassMap<MedicalCertificate>
    {
        #region Constants

        public const string SQL_EXPIRED = "(CASE WHEN (ExpirationDate < GETDATE()) THEN 1 ELSE 0 END)";

        #endregion

        public MedicalCertificateMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter)
               .Formula("(Select e.OperatingCenterId FROM tblEmployee e WHERE e.tblEmployeeId = EmployeeId)")
               .ReadOnly();
            References(x => x.Employee).Not.Nullable();

            Map(x => x.CertificationDate).Not.Nullable();
            Map(x => x.ExpirationDate).Not.Nullable();
            Map(x => x.Comments).Nullable();

            HasMany(x => x.MedicalCertificateDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.MedicalCertificateNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();

            Map(x => x.Expired).DbSpecificFormula(SQL_EXPIRED, SQL_EXPIRED.Replace("GETDATE()", "date('now')"));
        }
    }
}
