using System;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140217134634), Tags("Production")]
    public class AddColumnsToTrainingRecordsForBug1704 : Migration
    {
        public const string TABLE_NAME = "tblTrainingRecords";
        public const string CLASS_LOCATIONS = "ClassLocations";

        public struct NewColumnNames
        {
            public const string FIRST_INSTRUCTOR = "InstructorId",
                                SECOND_INSTRUCTOR = "SecondInstructorId",
                                OUTSIDE_INSTRUCTOR = "OutsideInstructor",
                                CLASS_LOCATION = "ClassLocationId";
        }

        private string GetFKName(string table, string otherTable, string column)
        {
            return String.Format("FK_{0}_{1}_{2}", table, otherTable, column);
        }

        private string GetEmployeeFKName(string columnName)
        {
            return GetFKName(TABLE_NAME, "tblEmployee", columnName);
        }

        private void CreateClassLocation(string description, string opCode)
        {
            Execute.Sql(
                "INSERT INTO ClassLocations (Description, OperatingCenterId) SELECT TOP 1 '{0}', OperatingCenterId FROM OperatingCenters WHERE OperatingCenterCode = '{1}';",
                description, opCode);
        }

        public override void Up()
        {
            Create.Column(NewColumnNames.FIRST_INSTRUCTOR).OnTable(TABLE_NAME)
                  .AsInt32().Nullable().ForeignKey(GetEmployeeFKName(NewColumnNames.FIRST_INSTRUCTOR), "tblEmployee",
                       "tblEmployeeId");

            Create.Column(NewColumnNames.SECOND_INSTRUCTOR).OnTable(TABLE_NAME)
                  .AsInt32().Nullable().ForeignKey(GetEmployeeFKName(NewColumnNames.SECOND_INSTRUCTOR), "tblEmployee",
                       "tblEmployeeId");

            Create.Column(NewColumnNames.OUTSIDE_INSTRUCTOR).OnTable(TABLE_NAME).AsAnsiString(255).Nullable();

            Create.Table(CLASS_LOCATIONS)
                  .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn("Description").AsAnsiString(35).NotNullable()
                  .WithColumn("OperatingCenterId").AsInt32().NotNullable().ForeignKey(
                       GetFKName(CLASS_LOCATIONS, "OperatingCenters", "OperatingCenterId"), "OperatingCenters",
                       "OperatingCenterId");

            Create.Column(NewColumnNames.CLASS_LOCATION).OnTable(TABLE_NAME)
                  .AsInt32().Nullable()
                  .ForeignKey(GetFKName(TABLE_NAME, CLASS_LOCATIONS, NewColumnNames.CLASS_LOCATION), CLASS_LOCATIONS,
                       "Id");

            CreateClassLocation("Delran Operations", "NJ5");
            CreateClassLocation("Delran Production", "NJ5");
            CreateClassLocation("Fire Rd. - Atlantic/Cape May Ops", "NJ3");
            CreateClassLocation("Hillsborough - Belle Mead Ops", "EW2");
            CreateClassLocation("Lakewood Operations", "NJ4");
            CreateClassLocation("Mt Laurel Offices", "NJ1");
            CreateClassLocation("Netherwood - Plainfield Ops", "EW1");
            CreateClassLocation("Oak Glen Treatment Plant", "NJ4");
            CreateClassLocation("Short Hills Operations", "NJ6");
            CreateClassLocation("Shrewsbury Operations", "NJ7");
            CreateClassLocation("Swimming River Treatment Plnt", "NJ7");
            CreateClassLocation("Voorhees - Corporate Office", "NJ1");
            CreateClassLocation("Washington Operations", "NJ8");
            CreateClassLocation("Woodcrest -  Cherry Hill Offices", "NJ1");
        }

        public override void Down()
        {
            Delete.ForeignKey(GetFKName(TABLE_NAME, CLASS_LOCATIONS, NewColumnNames.CLASS_LOCATION))
                  .OnTable(TABLE_NAME);

            Delete.Column(NewColumnNames.CLASS_LOCATION).FromTable(TABLE_NAME);

            Delete.Table(CLASS_LOCATIONS);

            Delete.ForeignKey(GetEmployeeFKName(NewColumnNames.FIRST_INSTRUCTOR)).OnTable(TABLE_NAME);
            Delete.ForeignKey(GetEmployeeFKName(NewColumnNames.SECOND_INSTRUCTOR)).OnTable(TABLE_NAME);

            Delete.Column(NewColumnNames.FIRST_INSTRUCTOR).FromTable(TABLE_NAME);
            Delete.Column(NewColumnNames.SECOND_INSTRUCTOR).FromTable(TABLE_NAME);

            Delete.Column(NewColumnNames.OUTSIDE_INSTRUCTOR).FromTable(TABLE_NAME);
        }
    }
}
