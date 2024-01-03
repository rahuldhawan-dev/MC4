using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPEquipmentsClassENG
    {
        public string ENG_TYP { get; set; }
        public string OWNED_BY { get; set; }
        public string APPLICATION_ENG { get; set; }
        public string HP_RATING { get; set; }
        public string SELF_STARTING { get; set; }
        public string ENG_MAX_FUEL { get; set; }
        public string ENG_FUEL_UOM { get; set; }
        public string ENG_CYLINDERS { get; set; }
        public string TNK_VOLUME { get; set; }
        public string SPECIAL_MAINT_NOTES_DETAILS { get; set; }
        public string SPECIAL_MAINT_NOTES_DIST { get; set; }
    }
}
