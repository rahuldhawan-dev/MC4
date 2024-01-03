using System;
using System.IO;

namespace MMSINC.Utilities
{
    public class ClosureProofConsoleOutputWrapper : StreamWriter
    {
        #region Constructors

        public ClosureProofConsoleOutputWrapper(Stream stream) : base(stream)
        {
            if (!stream.CanWrite)
                throw new InvalidOperationException("Can't write to standard output!");
        }

        public ClosureProofConsoleOutputWrapper() : this(Console.OpenStandardOutput()) { }

        #endregion

        #region Exposed Methods

        public override void Close()
        {
            // noop
        }

        protected override void Dispose(bool disposing)
        {
            // noop
        }

        public override void Flush()
        {
            // noop
        }

        #endregion
    }
}
