using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable CheckNamespace
namespace MMSINC.Exceptions
    // ReSharper restore CheckNamespace
{
    [Serializable]
    public class DomainLogicException : Exception
    {
        #region Constants

        public const string MESSAGE_FORMAT = "  Class {0}, id {1}";

        #endregion

        #region Private Members

        private readonly string _modelClass, _passedMessage;
        private readonly object _modelId;
        private readonly bool _generateMessage;

        #endregion

        #region Properties

        public override string Message
        {
            get
            {
                return (_generateMessage)
                    ? base.Message + String.Format(MESSAGE_FORMAT,
                        _modelClass, _modelId)
                    : (_passedMessage ?? base.Message);
            }
        }

        #endregion

        #region Constructors

        public DomainLogicException(string message)
            : base(message)
        {
            _passedMessage = message;
        }

        public DomainLogicException(string message, string modelClass,
            object modelId)
            : this(message)
        {
            _modelClass = modelClass;
            _modelId = modelId;
            _generateMessage = true;
        }

        protected DomainLogicException(SerializationInfo info,
            StreamingContext context)
        {
            _passedMessage = info.GetString("_passedMessage");

            if (String.IsNullOrEmpty(info.GetString("_modelClass")) ||
                info.GetValue("_modelId", typeof(object)) == null)
                return;

            _modelClass = info.GetString("_modelClass");
            _modelId = info.GetValue("_modelId", typeof(object));
            _generateMessage = true;
        }

        #endregion

        #region Methods

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true
        )]
        public override void GetObjectData(SerializationInfo info,
            StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_passedMessage", _passedMessage);
            info.AddValue("_modelClass", _modelClass);
            info.AddValue("_modelId", _modelId);
        }

        #endregion
    }
}
