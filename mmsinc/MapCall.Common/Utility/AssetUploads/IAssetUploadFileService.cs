using System;

namespace MapCall.Common.Utility.AssetUploads
{
    public interface IAssetUploadFileService
    {
        string GetFilePath(Guid fileName);
        void SaveFile(Guid fileName, byte[] fileData);
        byte[] LoadFile(Guid fileName);
    }
}
