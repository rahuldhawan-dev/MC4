using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TapImageMap : ClassMap<TapImage>
    {
        public TapImageMap()
        {
            Id(x => x.Id, "TapImageID");

            Map(x => x.ApartmentNumber)
               .Length(TapImage.StringLengths.APARTMENT_NUMBER)
               .Nullable();

            Map(x => x.Block)
               .Length(TapImage.StringLengths.BLOCK)
               .Nullable();

            Map(x => x.CrossStreet)
               .Length(TapImage.StringLengths.CROSS_STREET)
               .Nullable();

            Map(x => x.CreatedAt)
               .Not.Nullable();

            Map(x => x.DateCompleted)
               .Nullable();

            Map(x => x.Directory, "fld")
               .Length(TapImage.StringLengths.DIRECTORY)
               .Not.Nullable();

            Map(x => x.FileName, "FileList")
               .Length(TapImage.StringLengths.FILE_NAME)
               .Not.Nullable();

            Map(x => x.IsDefaultImageForService, "IsDefault")
               .Not.Nullable();

            Map(x => x.LengthOfService)
               .Length(TapImage.StringLengths.LENGTH_OF_SERVICE)
               .Nullable();

            Map(x => x.Lot)
               .Length(TapImage.StringLengths.LOT)
               .Nullable();

            Map(x => x.MainSize)
               .Length(TapImage.StringLengths.MAIN_SIZE)
               .Nullable();

            Map(x => x.PremiseNumber)
               .Length(TapImage.StringLengths.PREMISE_NUMBER)
               .Nullable();

            Map(x => x.ServiceNumber)
               .Length(TapImage.StringLengths.SERVICE_NUMBER)
               .Nullable();

            Map(x => x.ServiceType)
               .Length(TapImage.StringLengths.SERVICE_TYPE)
               .Nullable();

            Map(x => x.Street)
               .Length(TapImage.StringLengths.STREET)
               .Nullable();

            Map(x => x.StreetNumber)
               .Length(TapImage.StringLengths.STREET_NUMBER)
               .Nullable();

            Map(x => x.StreetPrefix)
               .Length(TapImage.StringLengths.STREET_PREFIX)
               .Nullable();

            Map(x => x.StreetSuffix)
               .Length(TapImage.StringLengths.STREET_SUFFIX)
               .Nullable();

            Map(x => x.TownSection)
               .Length(TapImage.StringLengths.TOWN_SECTION)
               .Nullable();

            Map(x => x.OfficeReviewRequired).Not.Nullable();

            Map(x => x.ServiceMaterialIsNull).Formula("(SELECT CASE WHEN ServiceMaterialId IS NULL THEN 1 ELSE 0 END)");

            References(x => x.OperatingCenter)
               .Not.Nullable();

            References(x => x.Service)
               .Nullable();

            References(x => x.Town)
               .Not.Nullable();

            References(x => x.ServiceMaterial).Nullable();
            References(x => x.ServiceSize).Nullable();

            References(x => x.PreviousServiceMaterial).Nullable();
            References(x => x.PreviousServiceSize).Nullable();

            References(x => x.CustomerSideMaterial).Nullable();
            References(x => x.CustomerSideSize).Nullable();

            Map(x => x.HasAsset).Formula("(CASE WHEN ServiceId is not null THEN 1 ELSE 0 END)");
            Map(x => x.HasValidPremiseNumber)
               .Formula("(CASE WHEN isnumeric(PremiseNumber) = 1 AND len(PremiseNumber) = 10 THEN 1 ELSE 0 END)");
        }
    }
}
