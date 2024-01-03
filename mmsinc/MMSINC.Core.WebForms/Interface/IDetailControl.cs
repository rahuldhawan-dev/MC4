using System.Web.UI.WebControls;
using MMSINC.Controls;

namespace MMSINC.Interface
{
    public interface IDetailControl : IControl
    {
        #region Properties

        DetailViewMode CurrentMvpMode { get; }
        DataKey DataKey { get; }
        object DataItem { get; }

        #endregion

        #region Methods

        void UpdateItem(bool causesValidation);
        void InsertItem(bool causesValidation);
        void DeleteItem();
        void ChangeMvpMode(DetailViewMode newMode);

        #endregion
    }
}
