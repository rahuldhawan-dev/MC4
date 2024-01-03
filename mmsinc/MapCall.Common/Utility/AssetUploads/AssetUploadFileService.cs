using System;
using System.Configuration;
using System.IO;

namespace MapCall.Common.Utility.AssetUploads
{
    public class AssetUploadFileService : IAssetUploadFileService
    {
        public const string PATH_CONFIG_KEY = "AssetUploadsDirectory";

        public string GetFilePath(Guid fileName)
        {
            return Path.Combine(ConfigurationManager.AppSettings[PATH_CONFIG_KEY], fileName.ToString());
        }

        public void SaveFile(Guid fileName, byte[] fileData)
        {
            File.WriteAllBytes(GetFilePath(fileName), fileData);
        }

        public byte[] LoadFile(Guid fileName)
        {
            return File.ReadAllBytes(GetFilePath(fileName));
        }
    }
}
