using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class VehicleUtilizationReportItem
    {
        public int VehicleId { get; set; }
        public string OperatingCenter { get; set; }
        public bool Flag { get; set; }
        public string VehicleStatus { get; set; }
        public string AssignmentStatus { get; set; }
        public string AssignmentCategory { get; set; }
        public string AssignmentJustification { get; set; }
        public string AccountingRequirement { get; set; }
        public string Facility { get; set; }
        public bool PoolUse { get; set; }
        public string VehicleType { get; set; }
        public string Model { get; set; }
        public string PlateNumber { get; set; }
        public string PrimaryVehicleUse { get; set; }
        public int ReplacementVehicleId { get; set; }

        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerMiddle { get; set; }
        public string Department { get; set; }

        public string Manager
        {
            get { return string.Format("{0}, {1} {2}.", ManagerLastName, ManagerFirstName, ManagerMiddle); }
        }

        public string PrimaryDriverFirstName { get; set; }
        public string PrimaryDriverLastName { get; set; }
        public string PrimaryDriverMiddle { get; set; }

        public string PrimaryDriver
        {
            get
            {
                return string.Format("{0}, {1} {2}.", PrimaryDriverLastName, PrimaryDriverFirstName,
                    PrimaryDriverMiddle);
            }
        }
    }

    public interface ISearchVehicleUtilizationReport : ISearchSet<VehicleUtilizationReportItem>
    {
        [SearchAlias("OperatingCenter", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("Department", "Id")]
        int? Department { get; set; }

        [SearchAlias("Status", "Id")]
        int? Status { get; set; }

        [SearchAlias("AssignmentStatus", "Id")]
        int? AssignmentStatus { get; set; }

        [SearchAlias("AssignmentCategory", "Id")]
        int? AssignmentCategory { get; set; }

        [SearchAlias("AssignmentJustification", "Id")]
        int? AssignmentJustification { get; set; }

        bool? PoolUse { get; set; }

        [SearchAlias("AccountingRequirement", "Id")]
        int? AccountingRequirement { get; set; }

        bool? Flag { get; set; }
    }
}
