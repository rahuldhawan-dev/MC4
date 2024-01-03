using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    /// <remarks>
    /// This is separate from IDetailsControl because of things specific to DetailsView controls that other 
    /// classes implementing IDetailsControl would not have access to.
    /// </remarks>
    public interface IDetailsView : IControl
    {
        #region Properties

        bool AutoGenerateRows { get; set; }
        DetailsViewMode CurrentMode { get; }
        int DataItemCount { get; }
        string[] DataKeyNames { get; set; }
        IDataSource DataSourceObject { get; }
        DataControlFieldCollection Fields { get; }
        object SelectedValue { get; }

        #endregion

        #region Events

        event EventHandler ModeChanged;
        event DetailsViewInsertEventHandler ItemInserting;
        event DetailsViewInsertedEventHandler ItemInserted;
        event DetailsViewUpdateEventHandler ItemUpdating;
        event DetailsViewDeleteEventHandler ItemDeleting;
        event DetailsViewCommandEventHandler ItemCommand;

        #endregion

        #region Methods

        void ChangeMode(DetailsViewMode mode);

        #endregion
    }
}
