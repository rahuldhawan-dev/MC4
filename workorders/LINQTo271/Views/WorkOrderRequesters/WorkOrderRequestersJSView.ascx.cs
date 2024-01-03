using System.Text;
using WorkOrders.Library.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrderRequesters
{
    public partial class WorkOrderRequestersJSView : WorkOrdersMvpUserControl
    {
        #region Properties

        protected string IDList
        {
            get { return GetIDList(); }
        }

        #endregion

        #region Private Static Methods

        private static string GetIDList()
        {
            var sb = new StringBuilder();

            foreach (var requester in WorkOrderRequesterRepository.SelectAllAsList())
                sb.AppendFormat("{0}:{1},",
                    requester.Description.Replace(" ", "_").ToUpperInvariant(),
                    requester.WorkOrderRequesterID);
            sb.Length--;
            return sb.ToString();
        }

        #endregion
    }
}