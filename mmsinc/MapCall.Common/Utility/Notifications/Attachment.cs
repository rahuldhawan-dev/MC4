namespace MapCall.Common.Utility.Notifications
{
    public class Attachment
    {
        #region Properties

        public string FileName { get; set; }
        public byte[] BinaryData { get; set; }

        #endregion

        #region Constructors

        public Attachment(string fileName, byte[] binaryData)
        {
            FileName = fileName;
            BinaryData = binaryData;
        }

        public Attachment() { }

        #endregion
    }
}
