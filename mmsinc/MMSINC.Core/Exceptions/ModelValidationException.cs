using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MMSINC.Exceptions
{
    public class ModelValidationException : Exception
    {
        #region Private Members

        private readonly string _myMessage;

        #endregion

        #region Properties

        public override string Message
        {
            get { return _myMessage; }
        }

        public List<KeyValuePair<string, string>> ErrorLookup { get; private set; }

        #endregion

        #region Constructors

        public ModelValidationException(IEnumerable<KeyValuePair<string, ModelState>> modelState)
        {
            _myMessage = GenerateMessage(modelState);
        }

        #endregion

        #region Private Methods

        private string GenerateMessage(IEnumerable<KeyValuePair<string, ModelState>> modelState)
        {
            var sb = new StringBuilder("The following validation errors were found:" + Environment.NewLine);

            ErrorLookup = new List<KeyValuePair<string, string>>(
                from key in modelState
                from message in key.Value.Errors
                where key.Value.Errors.Any()
                select
                    new KeyValuePair<string, string>(key.Key,
                        message.ErrorMessage));

            ErrorLookup.Each(kv => sb.AppendLine(String.Format("{0}: {1}", kv.Key, kv.Value)));

            return sb.ToString();
        }

        #endregion
    }
}
