using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BacterialWaterSampleAnalyst : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual IList<OperatingCenter> OperatingCenters { get; set; }

        #endregion

        #region Constructor

        public BacterialWaterSampleAnalyst()
        {
            OperatingCenters = new List<OperatingCenter>();
        }

        #endregion

        public override string ToString()
        {
            return new BacterialWaterSampleAnalystDisplayItem {
                FirstName = Employee.FirstName,
                LastName = Employee.LastName,
                MiddleName = Employee.MiddleName,
                EmployeeId = Employee.EmployeeId
            }.Display;
        }
    }

    [Serializable]
    public class BacterialWaterSampleAnalystDisplayItem : DisplayItem<BacterialWaterSampleAnalyst>
    {
        [SelectDynamic("FirstName", Field = "Employee")]
        public string FirstName { get; set; }

        [SelectDynamic("MiddleName", Field = "Employee")]
        public string MiddleName { get; set; }

        [SelectDynamic("LastName", Field = "Employee")]
        public string LastName { get; set; }

        [SelectDynamic("EmployeeId", Field = "Employee")]
        public string EmployeeId { get; set; }

        public override string Display =>
            $"{LastName}, {FirstName} {MiddleName} - {EmployeeId}".Replace("  ", " ");
    }
}
