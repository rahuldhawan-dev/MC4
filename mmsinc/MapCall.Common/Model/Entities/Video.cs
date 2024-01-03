using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public interface IThingWithVideos
    {
        int Id { get; }

        IEnumerable<IVideoLink> LinkedVideos { get; }

        // used so we can get a list of DataTypes
        // for creating notes
        string TableName { get; }
    }

    [Serializable]
    public class Video : IEntity
    {
        #region Consts

        public const int MAX_SPROUT_VIDEO_ID_LENGTH = 18,
                         MAX_TITLE_LENGTH = 256; // Sprout seems to have unlimited title length, but that's dumb.

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        /// <summary>
        /// This is the 16 character id used by sproutvideo.
        /// </summary>
        public virtual string SproutVideoId { get; set; }

        public virtual string Title { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual int LinkedId { get; set; }

        #endregion
    }

    public interface IVideoLink : IEntity
    {
        Video Video { get; set; }
        DataType DataType { get; set; }
        int LinkedId { get; }
    }

    [Serializable]
    public class VideoLink : IVideoLink
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Video Video { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual int LinkedId { get; set; }
        public virtual string Title { get; set; }

        #endregion
    }

    [Serializable]
    public class EquipmentVideo : VideoLink
    {
        public virtual Equipment Equipment { get; set; }
    }

    [Serializable]
    public class FacilityVideo : VideoLink
    {
        public virtual Facility Facility { get; set; }
    }

    [Serializable]
    public class FacilityProcessVideo : VideoLink
    {
        public virtual FacilityProcess FacilityProcess { get; set; }
    }

    [Serializable]
    public class StandardOperatingProcedureVideo : VideoLink
    {
        public virtual StandardOperatingProcedure StandardOperatingProcedure { get; set; }
    }

    [Serializable]
    public class TailgateTalkVideo : VideoLink
    {
        public virtual TailgateTalk TailgateTalk { get; set; }
    }

    [Serializable]
    public class TailgateTalkTopicVideo : VideoLink
    {
        public virtual TailgateTalkTopic TailgateTalkTopic { get; set; }
    }

    [Serializable]
    public class TrainingModuleVideo : VideoLink
    {
        public virtual TrainingModule TrainingModule { get; set; }
    }
}
