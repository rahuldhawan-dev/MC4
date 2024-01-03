using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LockoutFormAnswer : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual LockoutForm LockoutForm { get; set; }

        public virtual LockoutFormQuestion LockoutFormQuestion { get; set; }

        // This is nullable so that when the form is created, the management questions at the time
        // are created with the record so that the user can answer them when returning to service. 
        public virtual bool? Answer { get; set; }
        public virtual string Comments { get; set; }

        #endregion

        #region Constructors

        public LockoutFormAnswer() { }

        #endregion
    }
}
