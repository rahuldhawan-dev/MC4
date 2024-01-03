using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AsBuiltImageMap : ClassMap<AsBuiltImage>
    {
        #region Constructors

        public AsBuiltImageMap()
        {
            Id(x => x.Id, "AsBuiltImageID");

            References(x => x.Town).Not.Nullable();
            References(x => x.TownSection).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            Map(x => x.Comments).Length(AsBuiltImage.StringLengths.COMMENTS).Nullable();
            Map(x => x.CreatedAt)
               .Not.Nullable();
            Map(x => x.DateInstalled);
            Map(x => x.PhysicalInService).Nullable();
            Map(x => x.StreetPrefix)
               .Length(AsBuiltImage.StringLengths.STREET_PREFIX);
            Map(x => x.Street)
               .Length(AsBuiltImage.StringLengths.STREET);
            Map(x => x.StreetSuffix)
               .Length(AsBuiltImage.StringLengths.STREET_SUFFIX);
            Map(x => x.CrossStreetPrefix, "XStreetPrefix")
               .Length(AsBuiltImage.StringLengths.XSTREET_PREFIX);
            Map(x => x.CrossStreet)
               .Length(AsBuiltImage.StringLengths.CROSS_STREET);
            Map(x => x.CrossStreetSuffix, "XStreetSuffix")
               .Length(AsBuiltImage.StringLengths.XSTREET_SUFFIX);
            Map(x => x.ProjectName)
               .Length(AsBuiltImage.StringLengths.PROJECT_NAME);
            Map(x => x.MapPage)
               .Length(AsBuiltImage.StringLengths.MAP_PAGE);
            Map(x => x.TaskNumber)
               .Length(AsBuiltImage.StringLengths.TASK_NUMBER);
            Map(x => x.FileName, "FileList")
               .Length(AsBuiltImage.StringLengths.FILE_LIST)
               .Not.Nullable();
            Map(x => x.Directory, "fld")
               .Length(AsBuiltImage.StringLengths.FLD)
               .Not.Nullable();
            Map(x => x.CoordinatesModifiedOn);
            Map(x => x.OfficeReviewRequired).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.ApartmentNumber).Length(AsBuiltImage.StringLengths.APARTMENT_NUMBER).Nullable();

            References(x => x.UpdatedBy).Nullable();
        }

        #endregion
    }
}
