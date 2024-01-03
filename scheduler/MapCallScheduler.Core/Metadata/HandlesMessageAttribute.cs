using System;
using MMSINC.Interface;

namespace MapCallScheduler.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlesMessageAttribute : Attribute
    {
        #region Properties

        public Func<IMailMessage, bool> Predicate { get; }

        #endregion

        #region Constructors

        public HandlesMessageAttribute(Func<IMailMessage, bool> predicate)
        {
            Predicate = predicate;
        }

        #endregion
    }
}
