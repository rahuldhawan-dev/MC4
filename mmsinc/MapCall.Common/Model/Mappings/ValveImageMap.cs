using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ValveImageMap : ClassMap<ValveImage>
    {
        public ValveImageMap()
        {
            Id(x => x.Id, "ValveImageID");

            Map(x => x.CrossStreet)
               .Length(ValveImage.StringLengths.CROSS_STREET)
               .Nullable();

            Map(x => x.CrossStreetPrefix, "XStreetPrefix")
               .Length(ValveImage.StringLengths.CROSS_STREET_PREFIX)
               .Nullable();

            Map(x => x.CrossStreetSuffix, "XStreetSuffix")
               .Length(ValveImage.StringLengths.CROSS_STREET_SUFFIX)
               .Nullable();

            Map(x => x.CreatedAt)
               .Not.Nullable();

            Map(x => x.DateCompleted)
               .Length(ValveImage.StringLengths.DATE_COMPLETED)
               .Nullable();

            Map(x => x.Directory, "fld")
               .Length(ValveImage.StringLengths.DIRECTORY)
               .Not.Nullable();

            Map(x => x.FileName, "FileList")
               .Length(ValveImage.StringLengths.FILENAME)
               .Not.Nullable();

            Map(x => x.IsDefaultImageForValve, "IsDefault")
               .Not.Nullable();

            Map(x => x.Location)
               .Length(ValveImage.StringLengths.LOCATION)
               .Nullable();

            Map(x => x.ValveSize, "MainSize")
               .Length(ValveImage.StringLengths.VALVE_SIZE)
               .Nullable();

            Map(x => x.NumberOfTurns)
               .Length(ValveImage.StringLengths.NUMBER_OF_TURNS)
               .Nullable();

            Map(x => x.Street)
               .Length(ValveImage.StringLengths.STREET)
               .Nullable();

            Map(x => x.StreetNumber)
               .Length(ValveImage.StringLengths.STREET_NUMBER)
               .Nullable();

            Map(x => x.StreetPrefix)
               .Length(ValveImage.StringLengths.STREET_PREFIX)
               .Nullable();

            Map(x => x.StreetSuffix)
               .Length(ValveImage.StringLengths.STREET_SUFFIX)
               .Nullable();

            Map(x => x.TownSection)
               .Length(ValveImage.StringLengths.TOWNSECTION)
               .Nullable();

            Map(x => x.ValveNumber)
               .Length(ValveImage.StringLengths.VALVE_NUMBER)
               .Nullable();

            Map(x => x.OfficeReviewRequired).Not.Nullable();
            Map(x => x.ApartmentNumber).Length(ValveImage.StringLengths.APARTMENT_NUMBER).Nullable();

            References(x => x.OpenDirection, "ValveOpenDirectionId")
               .Nullable();
            References(x => x.NormalPosition, "ValveNormalPositionId")
               .Nullable();
            References(x => x.OperatingCenter)
               .Not.Nullable();
            References(x => x.Town)
               .Not.Nullable();
            References(x => x.Valve)
               .Nullable();

            Map(x => x.HasAsset).Formula("(CASE WHEN ValveId is not null THEN 1 ELSE 0 END)");
            Map(x => x.HasValidValveNumber)
               .DbSpecificFormula(
                    "(CASE WHEN left(ValveNumber, 1) = 'V' OR left(ValveNumber, 1) = 'H' THEN 1 ELSE 0 END)",
                    "(CASE WHEN substr(ValveNumber, 1, 1) = 'V' OR substr(ValveNumber, 1, 1) = 'H' THEN 1 ELSE 0 END)");
        }
    }
}
