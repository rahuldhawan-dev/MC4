using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    public class MvpFileUpload : FileUpload, IFileUpload
    {
        #region Private Members

        private IPostedFileWrapper _wrappedPostedFile;

        #endregion

        #region Properties

        public IPostedFileWrapper WrappedPostedFile
        {
            get
            {
                if (_wrappedPostedFile == null)
                    _wrappedPostedFile = new PostedFileWrapper(PostedFile);
                return _wrappedPostedFile;
            }
        }

        #endregion

        #region Exposed Methods

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return
                ControlExtensions.FindIControl
                    <TIControl>((Control)this, id);
        }

        #endregion
    }
}
