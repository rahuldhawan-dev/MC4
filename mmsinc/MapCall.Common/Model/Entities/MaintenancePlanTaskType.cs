using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MaintenancePlanTaskType : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             ABBREVIATION = 4;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string Abbreviation { get; set; }

        /// <summary>
        /// This gets the Type Code based on the Id of the current instance.
        /// Example: P001, P002, etc
        /// </summary>
        public virtual string Code => $"P{Id:000}";

        #endregion
        
        #region Exposed Methods

        public override string ToString() => Description;

        #endregion
    }
}
