using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using MapCall.Common.Model.Entities;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPEntity
    {
        #region Constants
        
        public const string SAP_DATE_FORMAT = "yyyyMMdd",
                            SAP_TIME_FORMAT = "HHmmss",
                            MAPCALL_SOURCE_IDENTIFIER = "MAPCALL";

        #endregion

        #region Private Members

        private Dictionary<string, string> _purposes;

        #endregion

        #region Private Methods

        private string GetPermitsMapping(string permitDescription)
        {
            switch (permitDescription)
            {
                case "Air Permit":
                    return "AIRPERMIT";
                case "Is Confined Space":
                    return "CONFSPACE";
                case "Job Safety Checklist":
                    return "JOBSFYCHK";
                case "Has Lockout Requirement":
                    return "LOCKOUT";
                case "Hot Work":
                    return "HOTWORK";
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region Constructors

        public SAPEntity()
        {
            _purposes = new Dictionary<string, string> {
                {"Customer", "I01"},
                {"Equip Reliability", "I02"},
                {"Safety", "I03"},
                {"Compliance", "I04"},
                {"Regulatory", "I05"},
                {"Seasonal", "I06"},
                {"Leak Detection", "I07"},
                {"Revenue 150-500", "I08"},
                {"Revenue 500-1000", "I09"},
                {"Revenue >1000", "I10"},
                {"Damaged/Billable", "I11"},
                {"Estimates", "I12"},
                {"Water Quality", "I13"},
                {"Asset Record Control", "I14"},
                {"Demolition", "I15"},
                {"Locate", "I16"},
                {"Clean Out", "I17"},
                {"Construction Project", "DV02"}
            };
        }

        #endregion

        #region Exposed Methods

        public string GetShowUrl(string entityType, int id)
        {
            return entityType != "Equipment" ?
                $"{ConfigurationManager.AppSettings["BaseUrl"]}Modules/mvc/FieldOperations/{entityType}/Show/{id}" :
                $"{ConfigurationManager.AppSettings["BaseUrl"]}Modules/mvc/{entityType}/Show/{id}";
        }

        public string GetPurposeCode(string mapCallPurpose)
        {
            return (mapCallPurpose != null && _purposes.TryGetValue(mapCallPurpose, out var purpose)) ? purpose : string.Empty;
        }

        //TODO: This should be moved to a more standard way of doing this. Likely an SAPCode field on the table.
        public string SAPPriority(string Priority)
        {
            switch (Priority)
            {
                case "Emergency":
                    return "2";
                case "High Priority":
                case "High":
                    return "3";
                case "Routine":
                    return "4";
                case "Medium":
                    return "6";
                case "Low":
                    return "7";
                default:
                    return string.Empty;
            }
        }

        //public string GetPermitsForProductionWorkOrder(ProductionWorkOrder productionWorkOrder)
        //{
        //    string permits = "";
        //    if (productionWorkOrder.ProductionPrerequisites != null && productionWorkOrder.ProductionPrerequisites.Count > 0)
        //        for (int i = 0; i < productionWorkOrder.ProductionPrerequisites.Count; i++)
        //            permits = permits != "" ? permits + ";" + GetPermitsMapping(productionWorkOrder.ProductionPrerequisites[i]?.Description) : GetPermitsMapping(productionWorkOrder.ProductionPrerequisites[i]?.Description);

        //    return permits;
        //}

        public string GetPermits(Equipment equipment)
        {
            string permits = "";
            if (equipment.ProductionPrerequisites != null && equipment.ProductionPrerequisites.Count > 0)
                for (int i = 0; i < equipment.ProductionPrerequisites.Count; i++)
                    permits = permits != ""
                        ? permits + ";" + GetPermitsMapping(equipment.ProductionPrerequisites[i]?.Description)
                        : GetPermitsMapping(equipment.ProductionPrerequisites[i]?.Description);

            return permits;
        }

        public string ConvertSapDate(string SapDate)
        {
            if (SapDate != null && !string.IsNullOrWhiteSpace(SapDate) && SapDate != "00000000")
                return DateTime.ParseExact(SapDate, "yyyyMMdd", CultureInfo.InvariantCulture).Date.ToShortDateString();
            return null;
        }

        #endregion
    }

    public interface IHasStatus
    {
        string SAPStatus { get; }
    }
}
