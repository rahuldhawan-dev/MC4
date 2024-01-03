using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ViolationCertificateMap : ClassMap<ViolationCertificate>
    {
        #region Constants

        public const string SQL_EXPIRED =
                                "(CASE WHEN (DATEDIFF(\"D\", CertificateDate, GETDATE()) > 365) THEN 1 ELSE 0 END)",
                            SQL_LITE_EXPIRED =
                                "(CASE WHEN ((julianday('now') - julianday(CertificateDate)) > 365) THEN 1 ELSE 0 END)";

        #endregion

        public ViolationCertificateMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter)
               .Formula("(Select e.OperatingCenterId FROM tblEmployee e WHERE e.tblEmployeeId = EmployeeId)")
               .ReadOnly();
            References(x => x.Employee).Not.Nullable();

            Map(x => x.CertificateDate).Not.Nullable();
            Map(x => x.Comments).Nullable();
            Map(x => x.Expired).DbSpecificFormula(SQL_EXPIRED, SQL_LITE_EXPIRED);

            HasMany(x => x.ViolationCertificateDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ViolationCertificateNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
