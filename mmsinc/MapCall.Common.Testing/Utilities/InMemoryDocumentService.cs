using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MMSINC.Utilities.Documents;

namespace MapCall.Common.Testing.Utilities
{
    /// <summary>
    /// Use this instead of the regular DocumentService for testing. It includes
    /// additional methods for cleaning up files that were created between tests.
    /// </summary>
    public class InMemoryDocumentService : IDocumentService
    {
        #region Fields

        private readonly DocumentService _actualDocService = new DocumentService();
        private readonly Dictionary<string, byte[]> _filesByHash = new Dictionary<string, byte[]>();

        #endregion

        public string GetFileHash(byte[] binaryData)
        {
            // To keep this as close to the real implementation, use a
            // regular DocumentService to use the GetFileHash implementation.
            return _actualDocService.GetFileHash(binaryData);
        }

        public string Save(byte[] binaryData)
        {
            var hash = GetFileHash(binaryData);
            if (!_filesByHash.ContainsKey(hash))
            {
                _filesByHash[hash] = binaryData;
            }

            return hash;
        }

        public byte[] Open(string hash)
        {
            if (!_filesByHash.ContainsKey(hash))
            {
                throw new FileNotFoundException("Yeah that hash doesn't exist in this thing.");
            }

            // Again, to keep this as close to DocumentService as possible, this
            // needs to return a new copy of the byte array every time. That's
            // what'll happen when reading from the file system.

            return _filesByHash[hash].ToArray();
        }
    }
}
