using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityFacilityAreaDisplayItem : DisplayItem<FacilityFacilityArea>
    {
        private string _display;

        #region Properties

        [SelectDynamic("Description", Field = "FacilityArea")]
        public string FacilityAreaDesc { get; set; }

        [SelectDynamic("Description", Field = "FacilitySubArea")]
        public string FacilitySubAreaDesc { get; set; }

        public override string Display
        {
            get
            {
                if (_display == null)
                {
                    _display = $"{FacilityAreaDesc} - {FacilitySubAreaDesc}";
                }

                return _display;
            }
        }

        #endregion
    }
}
