using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TaskGroupCategory : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             TYPE = 25,
                             ABBREVIATION = 4;
        }

        public struct Display
        {
            public const string CATEGORY_TYPE = "Category Type";
        }

        #endregion

        #region Constructor

        public TaskGroupCategory()
        {
            TaskGroups = new List<TaskGroup>();
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        [View(TaskGroupCategory.Display.CATEGORY_TYPE)]
        public virtual string Type { get; set; }

        public virtual string Abbreviation { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual IList<TaskGroup> TaskGroups { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Type;
        }

        #endregion
    }
}
