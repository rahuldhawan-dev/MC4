using System;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.RestorationAccountingCodes
{
    public interface IRestorationAccountingCodeListView : IListView<RestorationAccountingCode>
    {
        event RestorationAccountingCodeEventHandler DeleteCommand;
        event RestorationAccountingCodeInsertEventHandler InsertCommand;

        String ErrorMessage { get; set; }
    }

    public delegate void RestorationAccountingCodeEventHandler(
        object sender, GridViewDeleteEventArgs e);

    public delegate void RestorationAccountingCodeInsertEventHandler(
        object sender, RestorationAccountingInsertEventArgs e);

    public class RestorationAccountingInsertEventArgs : EventArgs
    {
        public string Code { get; protected set; }
        public string SubCode { get; protected set; }

        public RestorationAccountingInsertEventArgs(string code, string subCode)
        {
            Code = code;
            SubCode = subCode;
        }
    }
}
