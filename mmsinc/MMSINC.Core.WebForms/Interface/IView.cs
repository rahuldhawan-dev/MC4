using MMSINC.Controls;

namespace MMSINC.Interface
{
    public interface IView : IUserControl
    {
        #region Methods

        void DataBind();

        #endregion
    }
}
