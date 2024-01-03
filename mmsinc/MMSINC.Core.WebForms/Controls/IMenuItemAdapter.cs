namespace MMSINC.Controls
{
    public interface IMenuItemAdapter
    {
        #region Methods

        System.Web.UI.WebControls.MenuItem ToDotNetMenuItem(MenuItem item);

        #endregion
    }
}
