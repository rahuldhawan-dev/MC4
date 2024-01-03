namespace MMSINC.Metadata
{
    public class ComboBoxAttribute : SelectAttribute
    {
        public ComboBoxAttribute() : base(SelectType.ComboBox) { }
        public ComboBoxAttribute(string controllerViewDataKey) : base(SelectType.ComboBox, controllerViewDataKey) { }
    }
}
