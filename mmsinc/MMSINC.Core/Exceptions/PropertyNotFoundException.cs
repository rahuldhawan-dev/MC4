using System;

namespace MMSINC.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        #region Constants

        public const string PUBLIC_MESSAGE_FORMAT =
                                "Public property '{0}' on type '{1}' was not found.",
                            PRIVATE_MESSAGE_FORMAT =
                                "Private/protected property '{0}' on type '{1}' was not found.";

        #endregion

        #region Properties

        public Type TargetType { get; protected set; }
        public string PropertyName { get; protected set; }
        public bool PublicProperty { get; protected set; }

        public override string Message
        {
            get { return String.Format(MessageFormat, PropertyName, TargetType); }
        }

        public string MessageFormat
        {
            get
            {
                return PublicProperty
                    ? PUBLIC_MESSAGE_FORMAT
                    : PRIVATE_MESSAGE_FORMAT;
            }
        }

        #endregion

        #region Constructors

        public PropertyNotFoundException(Type targetType, string propertyName,
            bool publicProperty = true)
        {
            TargetType = targetType;
            PropertyName = propertyName;
            PublicProperty = publicProperty;
        }

        #endregion
    }
}
