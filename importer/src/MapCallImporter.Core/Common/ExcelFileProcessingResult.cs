namespace MapCallImporter.Common
{
    public enum ExcelFileProcessingResult
    {
        FileValid,
        FileAlreadyOpen,
        InvalidFileType,
        InvalidFileContents,
        CouldNotDetermineContentType,
        OtherError
    }
}