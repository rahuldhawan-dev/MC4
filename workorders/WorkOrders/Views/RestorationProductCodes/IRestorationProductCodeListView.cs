using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.RestorationProductCodes
{
    public interface IRestorationProductCodeListView : IListView<RestorationProductCode>
    {
        event RestorationProductCodeEventHandler DeleteCommand;
        event RestorationProductCodeInsertEventHandler InsertCommand;

        String ErrorMessage { get; set; }
    }

    public delegate void RestorationProductCodeInsertEventHandler(
        object sender, RestorationProductInsertEventArgs args);

    public delegate void RestorationProductCodeEventHandler(
        object sender, GridViewDeleteEventArgs e);

    public class RestorationProductInsertEventArgs : EventArgs
    {
        public string Code { get; protected set; }

        public RestorationProductInsertEventArgs(string code)
        {
            Code = code;
        }
    }
    
}
