using System.IO;

namespace MapCall.Common.Utility
{
    public class StringOrTextWriter
    {
        #region Private Members

        private readonly dynamic _innerWriter;

        #endregion

        #region Constructors

        public StringOrTextWriter(TextWriter textWriter)
        {
            _innerWriter = textWriter;
        }

        public StringOrTextWriter(StringWriter stringWriter)
        {
            _innerWriter = stringWriter;
        }

        #endregion

        #region Exposed Methods

        public void Flush()
        {
            _innerWriter.Flush();
        }

        public void Write(string format, params object[] args)
        {
            _innerWriter.Write(format, args);
        }

        public void WriteLine(string format, params object[] args)
        {
            _innerWriter.WriteLine(format, args);
        }

        #endregion
    }
}
