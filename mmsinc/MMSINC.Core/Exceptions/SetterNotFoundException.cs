using System;

namespace MMSINC.Exceptions
{
    public class SetterNotFoundException : PropertyNotFoundException
    {
        #region Constants

        private const string MESSAGE_FORMAT =
            "Public setter for property '{0}' on type '{1}' was not found.";

        #endregion

        #region Properties

        public override string Message
        {
            get { return String.Format(MESSAGE_FORMAT, PropertyName, TargetType); }
        }

        #endregion

        #region Constructors

        public SetterNotFoundException(Type targetType, string propertyName)
            : base(targetType, propertyName) { }

        #endregion
    }
}
