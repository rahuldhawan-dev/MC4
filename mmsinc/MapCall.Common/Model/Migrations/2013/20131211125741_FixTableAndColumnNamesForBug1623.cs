using System;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131211125741), Tags("Production")]
    public class FixTableAndColumnNamesForBug1623 : Migration
    {
        public struct OldTableNames
        {
            public const string BARGAINING_UNITS = "tblBargaining_Unit",
                                GRIEVANCES = "tblBargaining_Unit_Grievances",
                                LOCALS = "tblBargaining_Unit_Local",
                                EMPLOYEES = "tblEmployee";
        }

        public struct NewTableNames
        {
            public const string BARGAINING_UNITS = "BargainingUnits",
                                DEPARTMENTS = "EmployeeDepartments",
                                GENDERS = "Genders",
                                GRIEVANCES = "UnionGrievances",
                                INSTITUTIONAL_KNOWLEDGE = "InstitutionalKnowledge",
                                LOCALS = "LocalBargainingUnits",
                                REASONS_FOR_DEPARTURE = "ReasonsForDeparture",
                                STATUSES = "EmployeeStatuses",
                                DPCC_STATUSES = "DPCCStatuses",
                                TCPA_STATUSES = "TCPAStatuses",
                                UNION_AFFILIATIONS = "UnionAffiliations";
        }

        public struct OldColumnNames
        {
            public struct BargainingUnits
            {
                public const string ID = "BargainingUnitId",
                                    NAME = "Bargaining_Unit";
            }

            public struct Employees
            {
                public const string STATUS = "Status",
                                    DEPARTMENT = "Department_Name",
                                    GENDER = "Gender",
                                    INSTITUTIONAL_KNOWLEDGE = "InstitutionalKnowledge",
                                    REASON_FOR_DEPARTURE = "Reason_For_Departure",
                                    REASON_FOR_DEPARTURE_ID = "Reason_For_DepartureId",
                                    DPCC_STATUS = "DPCC_Status",
                                    DPCC_STATUS_ID = "DPCC_StatusId",
                                    TCPA_STATUS = "TCPA_Status",
                                    TCPA_STATUS_ID = "TCPA_StatusId",
                                    UNION_AFFILIATION = "Union_Affiliation",
                                    UNION_AFFILIATION_ID = "Union_AffiliationId";
            }

            public struct Grievances
            {
                public const string ID = "Grievance_ID",
                                    CONTRACT_ID = "Contract_ID",
                                    DATE_RECEIVED = "Date Grievance Received",
                                    ESTIMATED_IMPACT_VALUE = "Estimated Impact Value",
                                    CATEGORIZATION_ID = "Grievance Categorization",
                                    STATUS_ID = "Grievance Status",
                                    NUMBER = "Grievance_Number",
                                    INCIDENT_DATE = "Incident Date",
                                    DESCRIPTION = "Step 1 Grievance Description",
                                    DESCRIPTION_OF_OUTCOME = "Description of Outcome",
                                    OP_CODE = "OpCode";
            }

            public struct Locals
            {
                public const string ID = "LocalId",
                                    NAME = "Local",
                                    OPERATING_CENTER_ID = "OpCenterId";
            }
        }

        public struct NewColumnNames
        {
            public struct Common
            {
                public const string ID = "Id",
                                    NAME = "Name",
                                    DESCRIPTION = "Description",
                                    OPERATING_CENTER_ID = "OperatingCenterId";
            }

            public struct Employees
            {
                public const string STATUS_ID = "StatusId",
                                    DEPARTMENT_ID = "DepartmentId",
                                    GENDER_ID = "GenderId",
                                    INSTITUTIONAL_KNOWLEDGE_ID = "InstitutionalKnowledgeId",
                                    REASON_FOR_DEPARTURE_ID = "ReasonForDepartureId",
                                    DPCC_STATUS_ID = "DPCCStatusId",
                                    TCPA_STATUS_ID = "TCPAStatusId",
                                    UNION_AFFILIATION_ID = "UnionAffiliationId";
            }

            public struct Grievances
            {
                public const string CONTRACT_ID = "ContractId",
                                    DATE_RECEIVED = "DateReceived",
                                    ESTIMATED_IMPACT_VALUE = "EstimatedImpactValue",
                                    CATEGORIZATION_ID = "CategorizationId",
                                    STATUS_ID = "StatusId",
                                    NUMBER = "Number",
                                    INCIDENT_DATE = "IncidentDate",
                                    DESCRIPTION_OF_OUTCOME = "DescriptionOfOutcome";
            }
        }

        public struct NewColumnSizes
        {
            public struct EmployeeStatuses
            {
                public const int DESCRIPTION = 10;
            }

            public struct Departments
            {
                public const int DESCRIPTION = 35;
            }

            public struct DPCCStatuses
            {
                public const int DESCRIPTION = 35;
            }

            public struct Genders
            {
                public const int DESCRIPTION = 6;
            }

            public struct InstitutionalKnowledge
            {
                public const int DESCRIPTION = 10;
            }

            public struct ReasonsForDeparture
            {
                public const int DESCRIPTION = 20;
            }

            public struct TCPAStatuses
            {
                public const int DESCRIPTION = 35;
            }

            public struct UnionAffiliations
            {
                public const int DESCRIPTION = 20;
            }
        }

        public override void Up()
        {
            // Unions
            Rename.Table(OldTableNames.BARGAINING_UNITS).To(NewTableNames.BARGAINING_UNITS);
            Rename.Table(OldTableNames.GRIEVANCES).To(NewTableNames.GRIEVANCES);
            Rename.Table(OldTableNames.LOCALS).To(NewTableNames.LOCALS);
            Rename.Column(OldColumnNames.BargainingUnits.ID)
                  .OnTable(NewTableNames.BARGAINING_UNITS)
                  .To(NewColumnNames.Common.ID);
            Rename.Column(OldColumnNames.BargainingUnits.NAME)
                  .OnTable(NewTableNames.BARGAINING_UNITS)
                  .To(NewColumnNames.Common.NAME);

            // Grievances
            Rename.Column(OldColumnNames.Grievances.ID)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Common.ID);
            Rename.Column(OldColumnNames.Grievances.CONTRACT_ID)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.CONTRACT_ID);
            Rename.Column(OldColumnNames.Grievances.DATE_RECEIVED)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.DATE_RECEIVED);
            Rename.Column(OldColumnNames.Grievances.ESTIMATED_IMPACT_VALUE)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.ESTIMATED_IMPACT_VALUE);
            Rename.Column(OldColumnNames.Grievances.CATEGORIZATION_ID)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.CATEGORIZATION_ID);
            Rename.Column(OldColumnNames.Grievances.STATUS_ID)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.STATUS_ID);
            Rename.Column(OldColumnNames.Grievances.NUMBER)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.NUMBER);
            Rename.Column(OldColumnNames.Grievances.INCIDENT_DATE)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.INCIDENT_DATE);
            Rename.Column(OldColumnNames.Grievances.DESCRIPTION)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Grievances.DESCRIPTION_OF_OUTCOME)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Grievances.DESCRIPTION_OF_OUTCOME);
            Execute.Sql(
                "UPDATE {0} SET {1} = oc.OperatingCenterId FROM OperatingCenters oc WHERE {1} = oc.OperatingCenterCode",
                NewTableNames.GRIEVANCES, OldColumnNames.Grievances.OP_CODE);
            Alter.Column(OldColumnNames.Grievances.OP_CODE).OnTable(NewTableNames.GRIEVANCES).AsInt32().Nullable();
            Rename.Column(OldColumnNames.Grievances.OP_CODE)
                  .OnTable(NewTableNames.GRIEVANCES)
                  .To(NewColumnNames.Common.OPERATING_CENTER_ID);

            // Locals
            Rename.Column(OldColumnNames.Locals.ID)
                  .OnTable(NewTableNames.LOCALS)
                  .To(NewColumnNames.Common.ID);
            Rename.Column(OldColumnNames.Locals.NAME)
                  .OnTable(NewTableNames.LOCALS)
                  .To(NewColumnNames.Common.NAME);
            Rename.Column(OldColumnNames.Locals.OPERATING_CENTER_ID)
                  .OnTable(NewTableNames.LOCALS)
                  .To(NewColumnNames.Common.OPERATING_CENTER_ID);
            Execute.Sql(
                "UPDATE {0} SET {1} = OperatingCenters.OperatingCenterId FROM OperatingCenters WHERE {0}.{1} IS NULL AND OperatingCenters.OperatingCenterCode = 'PA2';",
                NewTableNames.LOCALS, NewColumnNames.Common.OPERATING_CENTER_ID);
            Alter.Column(NewColumnNames.Common.OPERATING_CENTER_ID)
                 .OnTable(NewTableNames.LOCALS)
                 .AsInt32()
                 .NotNullable();
            Alter.Column("BargainingUnitId").OnTable(NewTableNames.LOCALS).AsInt32().NotNullable();

            // Fix DataTypes for documents and notes
            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';",
                NewTableNames.BARGAINING_UNITS, OldTableNames.BARGAINING_UNITS);
            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';", NewTableNames.GRIEVANCES,
                OldTableNames.GRIEVANCES);
            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';", NewTableNames.LOCALS,
                OldTableNames.LOCALS);

            // Employees
            // - Status
            Create.Table(NewTableNames.STATUSES)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION)
                  .AsAnsiString(NewColumnSizes.EmployeeStatuses.DESCRIPTION).NotNullable().Unique();
            Execute.Sql("INSERT INTO {0} ({1}) SELECT 'Active';INSERT INTO {0} ({1}) SELECT 'Inactive';",
                NewTableNames.STATUSES, NewColumnNames.Common.DESCRIPTION);
            Execute.Sql("UPDATE {0} SET {1} = s.{2} FROM {3} s WHERE {0}.{1} = s.{4};", OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.STATUS, NewColumnNames.Common.ID, NewTableNames.STATUSES,
                NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.STATUS)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.STATUS_ID);
            Alter.Column(NewColumnNames.Employees.STATUS_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(
                      String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.STATUSES,
                          NewColumnNames.Employees.STATUS_ID), NewTableNames.STATUSES, NewColumnNames.Common.ID);

            // Employees
            // - Department
            Create.Table(NewTableNames.DEPARTMENTS)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION).AsAnsiString(NewColumnSizes.Departments.DESCRIPTION)
                  .NotNullable().Unique();
            Execute.Sql("INSERT INTO {0} ({1}) SELECT DISTINCT {2} FROM {3} WHERE {2} IS NOT NULL AND {2} <> '';",
                NewTableNames.DEPARTMENTS, NewColumnNames.Common.DESCRIPTION, OldColumnNames.Employees.DEPARTMENT,
                OldTableNames.EMPLOYEES);
            Execute.Sql("UPDATE {0} SET {1} = d.{2} FROM {3} d WHERE {0}.{1} = d.{4};", OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.DEPARTMENT, NewColumnNames.Common.ID, NewTableNames.DEPARTMENTS,
                NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.DEPARTMENT)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.DEPARTMENT_ID);
            Alter.Column(NewColumnNames.Employees.DEPARTMENT_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(
                      String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.DEPARTMENTS,
                          NewColumnNames.Employees.DEPARTMENT_ID), NewTableNames.DEPARTMENTS, NewColumnNames.Common.ID);

            // Employees
            // - Gender
            Create.Table(NewTableNames.GENDERS)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION).AsAnsiString(NewColumnSizes.Genders.DESCRIPTION)
                  .NotNullable().Unique();
            Execute.Sql("INSERT INTO {0} ({1}) SELECT DISTINCT {2} FROM {3} WHERE {2} IS NOT NULL AND {2} <> '';",
                NewTableNames.GENDERS, NewColumnNames.Common.DESCRIPTION, OldColumnNames.Employees.GENDER,
                OldTableNames.EMPLOYEES);
            Execute.Sql("UPDATE {0} SET {1} = d.{2} FROM {3} d WHERE {0}.{1} = d.{4};", OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.GENDER, NewColumnNames.Common.ID, NewTableNames.GENDERS,
                NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.GENDER)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.GENDER_ID);
            Alter.Column(NewColumnNames.Employees.GENDER_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(
                      String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.GENDERS,
                          NewColumnNames.Employees.GENDER_ID), NewTableNames.GENDERS, NewColumnNames.Common.ID);

            // Employees
            // - ReasonForDepature
            Create.Table(NewTableNames.REASONS_FOR_DEPARTURE)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION)
                  .AsAnsiString(NewColumnSizes.ReasonsForDeparture.DESCRIPTION).NotNullable().Unique();
            Execute.Sql(
                "INSERT INTO {0} ({1}) SELECT DISTINCT LookupValue FROM Lookup WHERE EXISTS (SELECT 1 FROM {2} WHERE {3} IS NOT NULL AND Lookup.LookupId = {3});",
                NewTableNames.REASONS_FOR_DEPARTURE, NewColumnNames.Common.DESCRIPTION, OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.REASON_FOR_DEPARTURE_ID);
            Execute.Sql("UPDATE {0} SET {1} = r.{2} FROM {3} r INNER JOIN Lookup l ON l.LookupValue = r.{4};",
                OldTableNames.EMPLOYEES, OldColumnNames.Employees.REASON_FOR_DEPARTURE_ID, NewColumnNames.Common.ID,
                NewTableNames.REASONS_FOR_DEPARTURE, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.REASON_FOR_DEPARTURE_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.REASON_FOR_DEPARTURE_ID);
            Delete.Column(OldColumnNames.Employees.REASON_FOR_DEPARTURE).FromTable(OldTableNames.EMPLOYEES);
            Alter.Column(NewColumnNames.Employees.REASON_FOR_DEPARTURE_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES,
                          NewTableNames.REASONS_FOR_DEPARTURE,
                          NewColumnNames.Employees.REASON_FOR_DEPARTURE_ID), NewTableNames.REASONS_FOR_DEPARTURE,
                      NewColumnNames.Common.ID);

            // Employees
            // - UnionAffiliation
            Create.Table(NewTableNames.UNION_AFFILIATIONS)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION)
                  .AsAnsiString(NewColumnSizes.UnionAffiliations.DESCRIPTION).NotNullable().Unique();
            Execute.Sql(
                "INSERT INTO {0} ({1}) SELECT DISTINCT LookupValue FROM Lookup WHERE EXISTS (SELECT 1 FROM {2} WHERE {3} IS NOT NULL AND Lookup.LookupId = {3});",
                NewTableNames.UNION_AFFILIATIONS, NewColumnNames.Common.DESCRIPTION, OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.UNION_AFFILIATION_ID);
            Execute.Sql("UPDATE {0} SET {1} = r.{2} FROM {3} r INNER JOIN Lookup l ON l.LookupValue = r.{4};",
                OldTableNames.EMPLOYEES, OldColumnNames.Employees.UNION_AFFILIATION_ID, NewColumnNames.Common.ID,
                NewTableNames.UNION_AFFILIATIONS, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.UNION_AFFILIATION_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.UNION_AFFILIATION_ID);
            Delete.Column(OldColumnNames.Employees.UNION_AFFILIATION).FromTable(OldTableNames.EMPLOYEES);
            Alter.Column(NewColumnNames.Employees.UNION_AFFILIATION_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.UNION_AFFILIATIONS,
                          NewColumnNames.Employees.UNION_AFFILIATION_ID), NewTableNames.UNION_AFFILIATIONS,
                      NewColumnNames.Common.ID);

            // Employees
            // - TCPAStatus
            Create.Table(NewTableNames.TCPA_STATUSES)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION).AsAnsiString(NewColumnSizes.TCPAStatuses.DESCRIPTION)
                  .NotNullable().Unique();
            Execute.Sql(
                "INSERT INTO {0} ({1}) SELECT DISTINCT LookupValue FROM Lookup WHERE EXISTS (SELECT 1 FROM {2} WHERE {3} IS NOT NULL AND Lookup.LookupId = {3});",
                NewTableNames.TCPA_STATUSES, NewColumnNames.Common.DESCRIPTION, OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.TCPA_STATUS_ID);
            Execute.Sql("UPDATE {0} SET {1} = r.{2} FROM {3} r INNER JOIN Lookup l ON l.LookupValue = r.{4};",
                OldTableNames.EMPLOYEES, OldColumnNames.Employees.TCPA_STATUS_ID, NewColumnNames.Common.ID,
                NewTableNames.TCPA_STATUSES, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.TCPA_STATUS_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.TCPA_STATUS_ID);
            Delete.Column(OldColumnNames.Employees.TCPA_STATUS).FromTable(OldTableNames.EMPLOYEES);
            Alter.Column(NewColumnNames.Employees.TCPA_STATUS_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.TCPA_STATUSES,
                      NewColumnNames.Employees.TCPA_STATUS_ID), NewTableNames.TCPA_STATUSES, NewColumnNames.Common.ID);

            // Employees
            // - DPCCStatus
            Create.Table(NewTableNames.DPCC_STATUSES)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION).AsAnsiString(NewColumnSizes.DPCCStatuses.DESCRIPTION)
                  .NotNullable().Unique();
            Execute.Sql(
                "INSERT INTO {0} ({1}) SELECT DISTINCT LookupValue FROM Lookup WHERE EXISTS (SELECT 1 FROM {2} WHERE {3} IS NOT NULL AND Lookup.LookupId = {3});",
                NewTableNames.DPCC_STATUSES, NewColumnNames.Common.DESCRIPTION, OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.DPCC_STATUS_ID);
            Execute.Sql("UPDATE {0} SET {1} = r.{2} FROM {3} r INNER JOIN Lookup l ON l.LookupValue = r.{4};",
                OldTableNames.EMPLOYEES, OldColumnNames.Employees.DPCC_STATUS_ID, NewColumnNames.Common.ID,
                NewTableNames.DPCC_STATUSES, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.DPCC_STATUS_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.DPCC_STATUS_ID);
            Delete.Column(OldColumnNames.Employees.DPCC_STATUS).FromTable(OldTableNames.EMPLOYEES);
            Alter.Column(NewColumnNames.Employees.DPCC_STATUS_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.DPCC_STATUSES,
                      NewColumnNames.Employees.DPCC_STATUS_ID), NewTableNames.DPCC_STATUSES, NewColumnNames.Common.ID);

            // Employees
            // - InstitutionalKnowledge
            Create.Table(NewTableNames.INSTITUTIONAL_KNOWLEDGE)
                  .WithColumn(NewColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(NewColumnNames.Common.DESCRIPTION)
                  .AsAnsiString(NewColumnSizes.InstitutionalKnowledge.DESCRIPTION).NotNullable().Unique();
            Execute.Sql(
                "INSERT INTO {0} ({1}) SELECT DISTINCT LookupValue FROM Lookup WHERE EXISTS (SELECT 1 FROM {2} WHERE {3} IS NOT NULL AND Lookup.LookupId = {3});",
                NewTableNames.INSTITUTIONAL_KNOWLEDGE, NewColumnNames.Common.DESCRIPTION, OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE);
            Execute.Sql("UPDATE {0} SET {1} = r.{2} FROM {3} r INNER JOIN Lookup l ON l.LookupValue = r.{4};",
                OldTableNames.EMPLOYEES, OldColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE, NewColumnNames.Common.ID,
                NewTableNames.INSTITUTIONAL_KNOWLEDGE, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(OldColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(NewColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE_ID);
            Alter.Column(NewColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE_ID)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsInt32().Nullable()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES,
                          NewTableNames.INSTITUTIONAL_KNOWLEDGE,
                          NewColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE_ID), NewTableNames.INSTITUTIONAL_KNOWLEDGE,
                      NewColumnNames.Common.ID);
        }

        public override void Down()
        {
            // Unions
            Rename.Table(NewTableNames.BARGAINING_UNITS).To(OldTableNames.BARGAINING_UNITS);
            Rename.Table(NewTableNames.GRIEVANCES).To(OldTableNames.GRIEVANCES);
            Rename.Table(NewTableNames.LOCALS).To(OldTableNames.LOCALS);
            Rename.Column(NewColumnNames.Common.ID)
                  .OnTable(OldTableNames.BARGAINING_UNITS)
                  .To(OldColumnNames.BargainingUnits.ID);
            Rename.Column(NewColumnNames.Common.NAME)
                  .OnTable(OldTableNames.BARGAINING_UNITS)
                  .To(OldColumnNames.BargainingUnits.NAME);

            // Grievances
            Rename.Column(NewColumnNames.Common.ID)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.ID);
            Rename.Column(NewColumnNames.Grievances.CONTRACT_ID)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.CONTRACT_ID);
            Rename.Column(NewColumnNames.Grievances.DATE_RECEIVED)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.DATE_RECEIVED);
            Rename.Column(NewColumnNames.Grievances.ESTIMATED_IMPACT_VALUE)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.ESTIMATED_IMPACT_VALUE);
            Rename.Column(NewColumnNames.Grievances.CATEGORIZATION_ID)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.CATEGORIZATION_ID);
            Rename.Column(NewColumnNames.Grievances.STATUS_ID)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.STATUS_ID);
            Rename.Column(NewColumnNames.Grievances.NUMBER)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.NUMBER);
            Rename.Column(NewColumnNames.Grievances.INCIDENT_DATE)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.INCIDENT_DATE);
            Rename.Column(NewColumnNames.Common.DESCRIPTION)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.DESCRIPTION);
            Rename.Column(NewColumnNames.Grievances.DESCRIPTION_OF_OUTCOME)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.DESCRIPTION_OF_OUTCOME);
            Alter.Column(NewColumnNames.Common.OPERATING_CENTER_ID)
                 .OnTable(OldTableNames.GRIEVANCES)
                 .AsAnsiString(50)
                 .Nullable();
            Execute.Sql(
                "UPDATE {0} SET {1} = oc.OperatingCenterCode FROM OperatingCenters oc WHERE {0}.{1} = oc.OperatingCenterId;",
                OldTableNames.GRIEVANCES, NewColumnNames.Common.OPERATING_CENTER_ID);
            Rename.Column(NewColumnNames.Common.OPERATING_CENTER_ID)
                  .OnTable(OldTableNames.GRIEVANCES)
                  .To(OldColumnNames.Grievances.OP_CODE);

            // Locals
            Rename.Column(NewColumnNames.Common.ID)
                  .OnTable(OldTableNames.LOCALS)
                  .To(OldColumnNames.Locals.ID);
            Rename.Column(NewColumnNames.Common.NAME)
                  .OnTable(OldTableNames.LOCALS)
                  .To(OldColumnNames.Locals.NAME);
            Rename.Column(NewColumnNames.Common.OPERATING_CENTER_ID)
                  .OnTable(OldTableNames.LOCALS)
                  .To(OldColumnNames.Locals.OPERATING_CENTER_ID);

            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';",
                OldTableNames.BARGAINING_UNITS, NewTableNames.BARGAINING_UNITS);
            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';", OldTableNames.GRIEVANCES,
                NewTableNames.GRIEVANCES);
            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';", OldTableNames.LOCALS,
                NewTableNames.LOCALS);

            // Employees
            // - Status
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.STATUSES,
                NewColumnNames.Employees.STATUS_ID)).OnTable(OldTableNames.EMPLOYEES);
            Rename.Column(NewColumnNames.Employees.STATUS_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.STATUS);
            Alter.Column(OldColumnNames.Employees.STATUS)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsAnsiString(NewColumnSizes.EmployeeStatuses.DESCRIPTION)
                 .Nullable();
            Execute.Sql("UPDATE {0} SET {1} = s.{2} FROM {3} s WHERE {0}.{1} = s.{4};", OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.STATUS, NewColumnNames.Common.DESCRIPTION, NewTableNames.STATUSES,
                NewColumnNames.Common.ID);
            Delete.Table(NewTableNames.STATUSES);

            // Employees
            // - Department
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.DEPARTMENTS,
                NewColumnNames.Employees.DEPARTMENT_ID)).OnTable(OldTableNames.EMPLOYEES);
            Rename.Column(NewColumnNames.Employees.DEPARTMENT_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.DEPARTMENT);
            Alter.Column(OldColumnNames.Employees.DEPARTMENT)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsAnsiString(NewColumnSizes.Departments.DESCRIPTION)
                 .Nullable();
            Execute.Sql("UPDATE {0} SET {1} = s.{2} FROM {3} s WHERE {0}.{1} = s.{4};", OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.DEPARTMENT, NewColumnNames.Common.DESCRIPTION, NewTableNames.DEPARTMENTS,
                NewColumnNames.Common.ID);
            Delete.Table(NewTableNames.DEPARTMENTS);

            // Employees
            // - Gender
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES, NewTableNames.GENDERS,
                NewColumnNames.Employees.GENDER_ID)).OnTable(OldTableNames.EMPLOYEES);
            Rename.Column(NewColumnNames.Employees.GENDER_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.GENDER);
            Alter.Column(OldColumnNames.Employees.GENDER)
                 .OnTable(OldTableNames.EMPLOYEES)
                 .AsAnsiString(NewColumnSizes.Genders.DESCRIPTION)
                 .Nullable();
            Execute.Sql("UPDATE {0} SET {1} = s.{2} FROM {3} s WHERE {0}.{1} = s.{4};", OldTableNames.EMPLOYEES,
                OldColumnNames.Employees.GENDER, NewColumnNames.Common.DESCRIPTION, NewTableNames.GENDERS,
                NewColumnNames.Common.ID);
            Delete.Table(NewTableNames.GENDERS);

            // Employees
            // - ReasonForDeparture
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES,
                NewTableNames.REASONS_FOR_DEPARTURE,
                NewColumnNames.Employees.REASON_FOR_DEPARTURE_ID)).OnTable(OldTableNames.EMPLOYEES);
            Execute.Sql("INSERT INTO Lookup (LookupValue) SELECT {0} FROM {1}", NewColumnNames.Common.DESCRIPTION,
                NewTableNames.REASONS_FOR_DEPARTURE);
            Execute.Sql("UPDATE {0} SET {1} = l.LookupId FROM Lookup l INNER JOIN {2} r on r.{3} = l.LookupValue",
                OldTableNames.EMPLOYEES, NewColumnNames.Employees.REASON_FOR_DEPARTURE_ID,
                NewTableNames.REASONS_FOR_DEPARTURE, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(NewColumnNames.Employees.REASON_FOR_DEPARTURE_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.REASON_FOR_DEPARTURE_ID);
            Delete.Table(NewTableNames.REASONS_FOR_DEPARTURE);
            Alter.Table(OldTableNames.EMPLOYEES)
                 .AddColumn(OldColumnNames.Employees.REASON_FOR_DEPARTURE)
                 .AsAnsiString(NewColumnSizes.ReasonsForDeparture.DESCRIPTION)
                 .Nullable();

            // Employees
            // - UnionAffiliation
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES,
                NewTableNames.UNION_AFFILIATIONS,
                NewColumnNames.Employees.UNION_AFFILIATION_ID)).OnTable(OldTableNames.EMPLOYEES);
            Execute.Sql("INSERT INTO Lookup (LookupValue) SELECT {0} FROM {1}", NewColumnNames.Common.DESCRIPTION,
                NewTableNames.UNION_AFFILIATIONS);
            Execute.Sql("UPDATE {0} SET {1} = l.LookupId FROM Lookup l INNER JOIN {2} r on r.{3} = l.LookupValue",
                OldTableNames.EMPLOYEES, NewColumnNames.Employees.UNION_AFFILIATION_ID,
                NewTableNames.UNION_AFFILIATIONS, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(NewColumnNames.Employees.UNION_AFFILIATION_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.UNION_AFFILIATION_ID);
            Delete.Table(NewTableNames.UNION_AFFILIATIONS);
            Alter.Table(OldTableNames.EMPLOYEES)
                 .AddColumn(OldColumnNames.Employees.UNION_AFFILIATION)
                 .AsAnsiString(NewColumnSizes.UnionAffiliations.DESCRIPTION)
                 .Nullable();

            // Employees
            // - TCPAStatus
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES,
                NewTableNames.TCPA_STATUSES,
                NewColumnNames.Employees.TCPA_STATUS_ID)).OnTable(OldTableNames.EMPLOYEES);
            Execute.Sql("INSERT INTO Lookup (LookupValue) SELECT {0} FROM {1}", NewColumnNames.Common.DESCRIPTION,
                NewTableNames.TCPA_STATUSES);
            Execute.Sql("UPDATE {0} SET {1} = l.LookupId FROM Lookup l INNER JOIN {2} r on r.{3} = l.LookupValue",
                OldTableNames.EMPLOYEES, NewColumnNames.Employees.TCPA_STATUS_ID,
                NewTableNames.TCPA_STATUSES, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(NewColumnNames.Employees.TCPA_STATUS_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.TCPA_STATUS_ID);
            Delete.Table(NewTableNames.TCPA_STATUSES);
            Alter.Table(OldTableNames.EMPLOYEES)
                 .AddColumn(OldColumnNames.Employees.TCPA_STATUS)
                 .AsAnsiString(NewColumnSizes.TCPAStatuses.DESCRIPTION)
                 .Nullable();

            // Employees
            // - DPCCStatus
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES,
                NewTableNames.DPCC_STATUSES,
                NewColumnNames.Employees.DPCC_STATUS_ID)).OnTable(OldTableNames.EMPLOYEES);
            Execute.Sql("INSERT INTO Lookup (LookupValue) SELECT {0} FROM {1}", NewColumnNames.Common.DESCRIPTION,
                NewTableNames.DPCC_STATUSES);
            Execute.Sql("UPDATE {0} SET {1} = l.LookupId FROM Lookup l INNER JOIN {2} r on r.{3} = l.LookupValue",
                OldTableNames.EMPLOYEES, NewColumnNames.Employees.DPCC_STATUS_ID,
                NewTableNames.DPCC_STATUSES, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(NewColumnNames.Employees.DPCC_STATUS_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.DPCC_STATUS_ID);
            Delete.Table(NewTableNames.DPCC_STATUSES);
            Alter.Table(OldTableNames.EMPLOYEES)
                 .AddColumn(OldColumnNames.Employees.DPCC_STATUS)
                 .AsAnsiString(NewColumnSizes.DPCCStatuses.DESCRIPTION)
                 .Nullable();

            // Employees
            // - InstitutionalKnowledge
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", OldTableNames.EMPLOYEES,
                NewTableNames.INSTITUTIONAL_KNOWLEDGE,
                NewColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE_ID)).OnTable(OldTableNames.EMPLOYEES);
            Execute.Sql("INSERT INTO Lookup (LookupValue) SELECT {0} FROM {1}", NewColumnNames.Common.DESCRIPTION,
                NewTableNames.INSTITUTIONAL_KNOWLEDGE);
            Execute.Sql("UPDATE {0} SET {1} = l.LookupId FROM Lookup l INNER JOIN {2} r on r.{3} = l.LookupValue",
                OldTableNames.EMPLOYEES, NewColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE_ID,
                NewTableNames.INSTITUTIONAL_KNOWLEDGE, NewColumnNames.Common.DESCRIPTION);
            Rename.Column(NewColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE_ID)
                  .OnTable(OldTableNames.EMPLOYEES)
                  .To(OldColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE);
            Delete.Table(NewTableNames.INSTITUTIONAL_KNOWLEDGE);
        }
    }
}
