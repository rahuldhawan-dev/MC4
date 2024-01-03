using MapCallImporter.Common;

namespace MapCallScheduler.JobHelpers.AssetUploadProcessor
{
    public interface IAssetUploadFileHandler
    {
        TimedExcelFileMappingResult Handle(string fileName);
    }
}