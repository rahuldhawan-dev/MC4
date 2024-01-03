using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public interface IDropDownListItemTemplate : ITemplate
    {
        IEntityBoundField Parent { get; set; }
        new void InstantiateIn(Control container);
    }

    public class DropDownListItemTemplate : IDropDownListItemTemplate
    {
        #region Properties

        public IEntityBoundField Parent { get; set; }

        #endregion

        #region Constructors

        public DropDownListItemTemplate(IEntityBoundField parent)
        {
            Parent = parent;
        }

        #endregion

        #region Exposed Methods

        public void InstantiateIn(Control container)
        {
            var l = new Label {
                ID = string.Format("ddl{0}", Parent.DataField.Replace(".", "_"))
            };
            l.DataBinding += OnDataBinding;
            container.Controls.Add(l);
        }

        public void OnDataBinding(object sender, EventArgs e)
        {
            var l = (Label)sender;

            var dv = l.NamingContainer as IDataItemContainer;
            if (Parent.DataField != null && dv.DataItem != null)
            {
                var obj = DataBinder.Eval(dv.DataItem, Parent.DataField);
                if (obj != null)
                    l.Text = obj.ToString();
            }
        }

        #endregion
    }
}
