using System.Web;

namespace MMSINC.Controls
{
    public class PostedFileWrapper : IPostedFileWrapper
    {
        #region Private Members

        private HttpPostedFile _innerFile;

        #endregion

        #region Constructors

        public PostedFileWrapper(HttpPostedFile innerFile)
        {
            _innerFile = innerFile;
        }

        #endregion

        public int ContentLength
        {
            get { return _innerFile.ContentLength; }
        }

        public string FileName
        {
            get { return _innerFile.FileName; }
        }
    }
}
