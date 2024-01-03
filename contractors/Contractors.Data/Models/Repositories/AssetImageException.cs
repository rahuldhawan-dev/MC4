using System;

namespace Contractors.Data.Models.Repositories {
    public class AssetImageException : Exception
    {
        public AssetImageExceptionType AssetExceptionType { get; private set; }

        public AssetImageException(AssetImageExceptionType type, string message, Exception innerException = null)
            : base(message, innerException)
        {
            AssetExceptionType = type;
        }
    }
}