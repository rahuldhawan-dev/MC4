using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReadOnlyEmployeeLink : IValidatableObject, IEmployeeLink
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public virtual int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual int LinkedId { get; set; }
        public virtual DateTime LinkedOn { get; set; }
        public virtual string LinkedBy { get; set; }
    }

    [Serializable]
    public class GrievanceEmployee : ReadOnlyEmployeeLink
    {
        #region Properties

        public virtual Grievance Grievance { get; set; }

        #endregion
    }

    [Serializable]
    public class EmployeeAccountabilityActionEmployee : ReadOnlyEmployeeLink
    {
        #region Properties

        public virtual EmployeeAccountabilityAction EmployeeAccountabilityAction { get; set; }

        #endregion
    }

    [Serializable]
    public class JobObservationEmployee : ReadOnlyEmployeeLink
    {
        #region Properties

        public virtual JobObservation JobObservation { get; set; }

        #endregion
    }

    [Serializable]
    public class TailgateTalkEmployee : ReadOnlyEmployeeLink
    {
        #region Properties

        public virtual TailgateTalk TailgateTalk { get; set; }

        #endregion
    }

    [Serializable]
    public class TrainingRecordEmployee : ReadOnlyEmployeeLink
    {
        #region Properties

        public virtual TrainingRecord TrainingRecord { get; set; }

        #endregion
    }

    [Serializable]
    public class TrainingRecordScheduledEmployee : TrainingRecordEmployee
    {
        public const string DATA_TYPE_AND_TABLE_NAME = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED +
                                                       AddFieldsToTrainingRecordsAndSuchForBug1738.DELIMITER +
                                                       TrainingRecordMap.TABLE_NAME;
    }

    [Serializable]
    public class TrainingRecordAttendedEmployee : TrainingRecordEmployee
    {
        public const string DATA_TYPE_AND_TABLE_NAME = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED +
                                                       AddFieldsToTrainingRecordsAndSuchForBug1738.DELIMITER +
                                                       TrainingRecordMap.TABLE_NAME;
    }
}
