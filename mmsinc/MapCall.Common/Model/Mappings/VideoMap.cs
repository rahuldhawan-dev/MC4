using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class VideoMap : ClassMap<Video>
    {
        public VideoMap()
        {
            Id(x => x.Id);

            Map(x => x.SproutVideoId)
               .Length(Video.MAX_SPROUT_VIDEO_ID_LENGTH)
               .Not.Nullable();
            Map(x => x.Title)
               .Length(Video.MAX_TITLE_LENGTH)
               .Not.Nullable();

            Map(x => x.LinkedId).Column("DataLinkID").Not.Nullable();

            References(x => x.DataType);
        }
    }

    public class VideoViewMap : NHibernate.Mapping.AbstractAuxiliaryDatabaseObject
    {
        #region Exposed Methods

        public override string SqlCreateString(NHibernate.Dialect.Dialect dialect, NHibernate.Engine.IMapping p,
            string defaultCatalog, string defaultSchema)
        {
            return Migrations.CreateVideosTableBug2666.CREATE_SQL;
        }

        public override string SqlDropString(NHibernate.Dialect.Dialect dialect, string defaultCatalog,
            string defaultSchema)
        {
            return Migrations.CreateVideosTableBug2666.DROP_SQL;
        }

        #endregion
    }

    public class VideoLinkMap : ClassMap<VideoLink>
    {
        #region Constructors

        public VideoLinkMap()
        {
            Table(Migrations.CreateVideosTableBug2666.VIEW_NAME);

            Id(x => x.Id);
            Map(x => x.LinkedId);
            References(x => x.DataType);
            References(x => x.Video, "Id").ReadOnly();
            DiscriminateSubClassesOnColumn("TableName").AlwaysSelectWithValue();

            SchemaAction.None();
        }

        #endregion
    }

    public class TrainingModuleVideoMap : SubclassMap<TrainingModuleVideo>
    {
        #region Constructors

        public TrainingModuleVideoMap()
        {
            DiscriminatorValue(TrainingModuleMap.TABLE_NAME);
            References(x => x.TrainingModule, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EquipmentVideoMap : SubclassMap<EquipmentVideo>
    {
        public EquipmentVideoMap()
        {
            DiscriminatorValue(EquipmentMap.TABLE_NAME);
            References(x => x.Equipment, "LinkedId").ReadOnly();
        }
    }

    public class FacilityVideoMap : SubclassMap<FacilityVideo>
    {
        #region Constructors

        public FacilityVideoMap()
        {
            DiscriminatorValue(FacilityMap.TABLE_NAME);
            References(x => x.Facility, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class FacilityProcessVideoMap : SubclassMap<FacilityProcessVideo>
    {
        #region Constructors

        public FacilityProcessVideoMap()
        {
            DiscriminatorValue(FacilityProcessMap.TABLE_NAME);
            References(x => x.FacilityProcess, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class StandardOperatingProcedureVideoMap : SubclassMap<StandardOperatingProcedureVideo>
    {
        #region Constructors

        public StandardOperatingProcedureVideoMap()
        {
            DiscriminatorValue(StandardOperatingProcedureMap.TABLE_NAME);
            References(x => x.StandardOperatingProcedure, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TailgateTalkTopicVideoMap : SubclassMap<TailgateTalkTopicVideo>
    {
        #region Constructors

        public TailgateTalkTopicVideoMap()
        {
            DiscriminatorValue(TailgateTalkTopicMap.TABLE_NAME);
            References(x => x.TailgateTalkTopic, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TailgateTalkVideoMap : SubclassMap<TailgateTalkVideo>
    {
        #region Constructors

        public TailgateTalkVideoMap()
        {
            DiscriminatorValue(TailgateTalkMap.TABLE_NAME);
            References(x => x.TailgateTalk, "LinkedId").ReadOnly();
        }

        #endregion
    }
}
