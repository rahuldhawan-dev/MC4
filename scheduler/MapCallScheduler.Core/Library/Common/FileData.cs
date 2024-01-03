namespace MapCallScheduler.Library.Common
{
    public class FileData
    {
        #region Properties

        public string Filename { get; protected set; }
        public string Content { get; protected set; }
        public byte[] Bytes { get; protected set; }

        #endregion

        #region Constructors

        public FileData(string filename, string content, byte[] bytes = null)
        {
            Filename = filename;
            Content = content;
            Bytes = bytes;
        }

        #endregion
    }
}
