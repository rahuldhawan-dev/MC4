using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.ViewModels.SAPDeviceDetail
{
    public class SAPRegisterStructure
    {
        public virtual string RegisterId { get; set; }
        public virtual string Dials { get; set; }
        [JsonProperty(PropertyName = "UoM")]
        public virtual string UnitOfMeasure { get; set; }
        public virtual string ReadDate { get; set; }
        public virtual string Read { get; set; }
        public virtual string ReadType { get; set; }
        public virtual string MeterReadType { get; set; }
        public virtual string MeterReadReason { get; set; }

        #region Logical Properties

        public virtual string MapCallUnitOfMeasure
        {
            get
            {
                switch (UnitOfMeasure)
                {
                    case "Inch3":
                        return "Cubic inch";
                    case "1MLGl":
                        return "1 Million Gallon";
                    case "CCF":
                        return "100 cubit feet";
                    case "cm3":
                        return "Cubic centimeter";
                    case "dm3":
                        return "Cubic decimeter";
                    case "CGL":
                        return "100 gallons";
                    case "CI":
                        return "Centiliter";
                    case "10CGL":
                        return "10 CGL";
                    case "DFP":
                        return "Differential Pres (in H2O)";
                    case "DFT":
                        return "10 Cubic feet";
                    case "foz US":
                        return "Fluid Ounce US";
                    case "1CF":
                        return "Cubic foot";
                    case "1GL":
                        return "US gallon";
                    case "hl":
                        return "Hectoliter";
                    case "l":
                        return "Liter";
                    case "m3":
                        return "Cubic meter";
                    case "MG":
                        return "Millions of Gallons";
                    case "ml":
                        return "Milliliter";
                    case "mm3":
                        return "Cubic millimeter";
                    case "pt US":
                        return "Pint, US liquid";
                    case "qt US":
                        return "Quart, US liquid";
                    case "1000CF":
                        return "1000 Cubit Feet";
                    case "TG":
                        return "Thousand Gallons";
                    case "10GL":
                        return "10 Gallons";
                    case "10KGL":
                        return "10 Thousand Gallons";
                    case "yd3":
                        return "Cubic yard";
                    case "µl":
                        return "Microliter";
                    case "DCG":
                        return "10 CGL";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion
    }
}
