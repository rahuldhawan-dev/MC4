namespace MapCall.Controls.BoundFieldHelpers
{
    public enum BooleanBoundFieldControlTypes
    {
        CheckBox = 0, // Default, don't change.
        RadioButtonList
    }

    public class BooleanBoundFieldOptions
    {
        public BooleanBoundFieldControlTypes ControlType { get; set; }
        public string NullValueText { get; set; }
        public string FalseValueText { get; set; }
        public string TrueValueText { get; set; }

        public BooleanBoundFieldOptions()
        {
            NullValueText = "Not Answered";
            FalseValueText = "No";
            TrueValueText = "Yes";
        }
    }
}