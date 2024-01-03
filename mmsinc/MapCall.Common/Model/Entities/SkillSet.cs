using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SkillSet : IEntity
    {
        #region Constants

        public readonly struct StringLengths
        {
            public const int NAME = 50,
                             ABBREVIATION = 5,
                             DESCRIPTION = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Description { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}