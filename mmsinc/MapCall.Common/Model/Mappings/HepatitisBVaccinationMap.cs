using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HepatitisBVaccinationMap : ClassMap<HepatitisBVaccination>
    {
        #region Constructors

        public HepatitisBVaccinationMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.ResponseDate).Not.Nullable();

            References(x => x.OperatingCenter)
               .Formula("(Select e.OperatingCenterId FROM tblEmployee e WHERE e.tblEmployeeId = EmployeeId)")
               .ReadOnly();

            References(x => x.Employee).Not.Nullable();
            References(x => x.HepatitisBVaccineStatus).Not.Nullable();

            HasMany(x => x.HepatitisBVaccinationDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.HepatitisBVaccinationNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
