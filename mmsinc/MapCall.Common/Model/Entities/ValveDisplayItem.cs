using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ValveDisplayItem : DisplayItem<Valve>
    {
        private string _display;

        public string ValveNumber { get; set; }

        [SelectDynamic("Description")]
        public string Status { get; set; }

        public override string Display
        {
            get
            {
                if (_display == null)
                {
                    _display = $"{ValveNumber} - {Status}";
                }

                return _display;
            }
        }
    }
}
