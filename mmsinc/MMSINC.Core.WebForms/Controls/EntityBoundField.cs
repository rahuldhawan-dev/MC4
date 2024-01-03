using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public interface IEntityBoundField
    {
        bool Required { get; set; }
        string TypeName { get; set; }
        string SelectMethod { get; set; }
        string DataValueField { get; set; }
        string DataTextField { get; set; }
        string SelectedValue { get; set; }
        string SelectedValueField { get; set; }
        string DataField { get; set; }
        ITemplate ItemTemplate { get; }
        ITemplate EditItemTemplate { get; }
    }

    public class EntityBoundField : TemplateField, IEntityBoundField
    {
        #region Private Members

        private DropDownListItemTemplate _itemTemplate;
        private DropDownListEditTemplate _editTemplate;

        #endregion

        #region Properties

        public string TypeName { get; set; }
        public string SelectMethod { get; set; }
        public string DataValueField { get; set; }
        public string DataTextField { get; set; }
        public string SelectedValue { get; set; }
        public string SelectedValueField { get; set; }
        public string DataField { get; set; }
        public bool Required { get; set; }

        public override ITemplate ItemTemplate
        {
            get
            {
                if (_itemTemplate == null)
                    _itemTemplate = new DropDownListItemTemplate(this);
                return _itemTemplate;
            }
        }

        public override ITemplate EditItemTemplate
        {
            get
            {
                if (_editTemplate == null)
                    _editTemplate = new DropDownListEditTemplate(this);
                return _editTemplate;
            }
        }

        #endregion

        #region Constructors

        public EntityBoundField()
        {
            base.ItemTemplate = ItemTemplate;
            base.EditItemTemplate = EditItemTemplate;
        }

        #endregion
    }
}
