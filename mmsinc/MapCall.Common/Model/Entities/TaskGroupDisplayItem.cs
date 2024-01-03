using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public class TaskGroupDisplayItem : DisplayItem<TaskGroup>
    {
        public string Caption { get; set; }

        public override string Display => Caption;
    }
}
