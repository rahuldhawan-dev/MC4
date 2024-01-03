using System.Text.RegularExpressions;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class ReadOnlyEmployeeLinkMap : ClassMap<ReadOnlyEmployeeLink>
    {
        public ReadOnlyEmployeeLinkMap()
        {
            Table(CreateEmployeeLinkView.VIEW_NAME);

            ReadOnly();

            Id(x => x.Id);
            Map(x => x.LinkedId);
            Map(x => x.LinkedOn);
            Map(x => x.LinkedBy);

            References(x => x.DataType);
            References(x => x.Employee).Not.LazyLoad();

            DiscriminateSubClassesOnColumn("DataTypeAndTableName").AlwaysSelectWithValue();

            // Need this so when SchemaExport doesn't create a table
            // for EmployeeLinkView
            SchemaAction.None();
        }
    }

    public class GrievanceEmployeeMap : SubclassMap<GrievanceEmployee>
    {
        public GrievanceEmployeeMap()
        {
            DiscriminatorValue(Grievance.DATA_TYPE_AND_TABLE_NAME);

            References(x => x.Grievance, "LinkedId").ReadOnly();
        }
    }

    public class EmployeeAccountabilityActionEmployeeMap : SubclassMap<EmployeeAccountabilityActionEmployee>
    {
        public EmployeeAccountabilityActionEmployeeMap()
        {
            DiscriminatorValue(EmployeeAccountabilityAction.DataTypeTableName.DATA_TYPE_AND_TABLE_NAME);

            References(x => x.EmployeeAccountabilityAction, "LinkedId").ReadOnly();
        }
    }

    public class JobObservationEmployeeMap : SubclassMap<JobObservationEmployee>
    {
        public JobObservationEmployeeMap()
        {
            DiscriminatorValue(JobObservation.DATA_TYPE_AND_TABLE_NAME);
            References(x => x.JobObservation, "LinkedId").ReadOnly();
        }
    }

    public class TailgateTalkEmployeeMap : SubclassMap<TailgateTalkEmployee>
    {
        public TailgateTalkEmployeeMap()
        {
            DiscriminatorValue(TailgateTalk.DATA_TYPE_AND_TABLE_NAME);

            References(x => x.TailgateTalk, "LinkedId").ReadOnly();
        }
    }

    public class TrainingRecordScheduledEmployeeMap : SubclassMap<TrainingRecordScheduledEmployee>
    {
        public TrainingRecordScheduledEmployeeMap()
        {
            DiscriminatorValue(TrainingRecordScheduledEmployee.DATA_TYPE_AND_TABLE_NAME);

            References(x => x.TrainingRecord, "LinkedId").ReadOnly();
        }
    }

    public class TrainingRecordAttendedEmployeeMap : SubclassMap<TrainingRecordAttendedEmployee>
    {
        public TrainingRecordAttendedEmployeeMap()
        {
            DiscriminatorValue(TrainingRecordAttendedEmployee.DATA_TYPE_AND_TABLE_NAME);

            References(x => x.TrainingRecord, "LinkedId").ReadOnly();
        }
    }

    public class EmployeeLinkViewMap : NHibernate.Mapping.AbstractAuxiliaryDatabaseObject
    {
        #region Exposed Methods

        public override string SqlCreateString(NHibernate.Dialect.Dialect dialect, NHibernate.Engine.IMapping p,
            string defaultCatalog, string defaultSchema)
        {
            return new Regex("^ALTER")
                  .Replace(AddFieldsToTrainingRecordsAndSuchForBug1738.ALTER_VIEW, "CREATE")
                  .Replace("+", "||");
        }

        public override string SqlDropString(NHibernate.Dialect.Dialect dialect, string defaultCatalog,
            string defaultSchema)
        {
            return CreateEmployeeLinkView.DROP_SQL;
        }

        #endregion
    }
}
