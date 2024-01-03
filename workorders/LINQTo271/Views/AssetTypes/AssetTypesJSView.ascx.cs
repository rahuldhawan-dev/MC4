using System.Text;
using WorkOrders.Library.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.AssetTypes
{
    public partial class AssetTypesJSView : WorkOrdersMvpUserControl
    {
        #region Properties

        protected string IDList
        {
            get { return GetIDList(); }
        }

        #endregion

        #region Private Methods

        private static string GetIDList()
        {
            var sb = new StringBuilder();

            foreach (var type in AssetTypeRepository.SelectAllAsList())
                sb.AppendFormat("{0}:{1},",
                    type.Description.Replace(" ", "_").Replace("/", "_").ToUpperInvariant(),
                    type.AssetTypeID);
            sb.Length--;
            return sb.ToString();
        }

        #endregion
    }
}
