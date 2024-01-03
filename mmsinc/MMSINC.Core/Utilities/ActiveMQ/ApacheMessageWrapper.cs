using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;

namespace MMSINC.Utilities.ActiveMQ
{
    public class ApacheMessageWrapper : IMessage
    {
        #region Private Members

        private readonly Apache.NMS.IMessage _messageImplementation;
        private string _text;

        #endregion

        #region Properties

        public string Text
        {
            get
            {
                if (_text != null)
                {
                    return _text;
                }

                switch (_messageImplementation)
                {
                    case ITextMessage m:
                        _text = m.Text;
                        break;
                    case ActiveMQMapMessage m:
                        _text = m.ToJSON();
                        break;
                    case ActiveMQBytesMessage m:
                        _text = System.Text.Encoding.Default.GetString(m.Content);
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"Not sure how to handle message of type '{_messageImplementation.GetType()}'.");
                }

                return _text;
            }
        }

        #endregion

        #region Constructors

        public ApacheMessageWrapper(Apache.NMS.IMessage message)
        {
            _messageImplementation = message;
        }

        #endregion

        #region Exposed Methods

        public void Acknowledge()
        {
            _messageImplementation.Acknowledge();
        }

        #endregion
    }
}
