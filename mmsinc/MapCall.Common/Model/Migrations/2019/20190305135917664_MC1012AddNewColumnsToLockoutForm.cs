using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190305135917664), Tags("Production")]
    public class MC1012AddNewColumnsToLockoutForm : Migration
    {
        public struct Questions
        {
            public const string
                AFFECTED_EMPLOYEES_NOTIFIED = "Have affected employees been notified prior to lock out?",
                AFFECTED_EQUIPMENT_SHUTDOWN = "Has the affected equipment been shut down?",
                ISOLATES_ENERGY_SOURCES = "Does this lock out isolate all energy sources to the equipment?",
                CLEARLY_INDICATES_PROHIBITED =
                    "Apply lockout devices to hold the machine or equipment in a “safe” position. Attach tag to the lockout device that clearly indicates that the operation of the equipment is prohibited?",
                RENDERED_SAFE_UNTIL_COMPLETE =
                    "Relieve, disconnect, restrain, or otherwise render safe and stored residual energy.  If re-accumulation of any stored energy is possible, continue to verify that it has been rendered “safe” until the job is complete.",
                CANNOT_BE_OPERATED =
                    "Verify that the machine or equipment cannot be operated before proceeding with the repair or maintenance work.  This is accomplished by trying to start-up the equipment.",
                PARKED_IN_HOME_SAFE =
                    "When returning equipment to service verify that non-essential items (tools, extra parts, etc.) are removed, components are intact and employees are safely positioned. Ensure that equipment is parked in the “Home” or safe “Start” position.",
                REMOVED_DEVICE_AND_NOTIFIED =
                    "Remove the lockout/tag out devices, and then notify all affected employees that the devices have been removed and the equipment is operational.",
                SAME_AS_INSTALLER = "Is the same person who installed the lockout device removing it?",
                CONFIRMED_BY_MANAGEMENT = "Authorized employee lock has been identified and confirmed by management?",
                REASONABLE_EFFORT_MADE =
                    "Authorized management person has made reasonable effort to contact employee who installed lock?",
                AUTHORIZED_MANAGEMENT_APPROVED =
                    "Authorized management person checked equipment and approved removal of lock?",
                SUPERVISOR_ENSURES_KNOWLEDGE =
                    "Supervisor will ensure that the authorized employee has this knowledge before he/she resumes work at that facility?",
                ENERGY_POTENTIAL =
                    "The machine or equipment has no potential for stored or residual energy or re-accumulation of stored energy after shut down which could endanger employees.",
                ENERGY_SINGLE =
                    "The machine or equipment has a single energy source which can be readily identified and isolated.",
                DE_ENERGIZED =
                    "The isolation and locking out of that energy source will completely de-energize and deactivate the machine or equipment. (If No is selected for this question, then add a note that states the following- “Please note that there must be a Lock Out form for each energy source)",
                ENERGY_ISOLATED =
                    "The machine or equipment is isolated from that energy source and locked out during servicing or maintenance.",
                SINGLE_LOCKOUT = "A single lockout device will achieve a locked-out condition",
                EXCLUSIVE =
                    "The lockout device is under the exclusive control of the authorized employee performing the servicing or maintenance.",
                NO_HAZARDS = "The servicing or maintenance does not create hazards for other employees.",
                NO_ACCIDENTS_INVOLVING =
                    "There have been no accidents involving the unexpected activation or re-energization of the machine or equipment during servicing or maintenance.";
        }

        public override void Up()
        {
            Alter.Table("LockoutForms")
                 .AddColumn("ContractorLockOutTagOut").AsBoolean().Nullable();

            this.CreateLookupTableWithValues("LockoutFormQuestionCategories", "Out Of Service", "Return To Service",
                "Management", "Lockout Conditions");

            Rename.Column("LockoutDeviceLocationId").OnTable("LockoutForms").To("IsolationPointId");
            Rename.Column("LockRemovedById").OnTable("LockoutForms").To("LockRemovalMethodId");

            // Create a table to store lockout form questions
            Create.Table("LockoutFormQuestions")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("CategoryId", "LockoutFormQuestionCategories")
                  .WithColumn("Question").AsCustom("text").NotNullable()
                  .WithColumn("IsActive").AsBoolean().WithDefaultValue(false)
                  .WithColumn("DisplayOrder").AsInt32().NotNullable();

            // Populate Questions
            Insert.IntoTable("LockoutFormQuestions").Rows(
                new {
                    IsActive = true, CategoryId = 1,
                    DisplayOrder = 1, Question = Questions.AFFECTED_EMPLOYEES_NOTIFIED
                },
                new {
                    IsActive = true, CategoryId = 1,
                    DisplayOrder = 2, Question = Questions.AFFECTED_EQUIPMENT_SHUTDOWN
                },
                new {
                    IsActive = true, CategoryId = 1,
                    DisplayOrder = 3, Question = Questions.ISOLATES_ENERGY_SOURCES
                },
                new {
                    IsActive = true, CategoryId = 1,
                    DisplayOrder = 4, Question = Questions.CLEARLY_INDICATES_PROHIBITED
                },
                new {
                    IsActive = true, CategoryId = 1,
                    DisplayOrder = 5, Question = Questions.RENDERED_SAFE_UNTIL_COMPLETE
                },
                new {
                    IsActive = true, CategoryId = 1,
                    DisplayOrder = 6, Question = Questions.CANNOT_BE_OPERATED
                },
                new {
                    IsActive = true, CategoryId = 2,
                    DisplayOrder = 7, Question = Questions.PARKED_IN_HOME_SAFE
                },
                new {
                    IsActive = true, CategoryId = 2,
                    DisplayOrder = 8, Question = Questions.REMOVED_DEVICE_AND_NOTIFIED
                },
                //SAME_AS_INSTALLER -- this is a static question that toggles management questions
                new {
                    IsActive = true, CategoryId = 3,
                    DisplayOrder = 9, Question = Questions.CONFIRMED_BY_MANAGEMENT
                },
                new {
                    IsActive = true, CategoryId = 3,
                    DisplayOrder = 10, Question = Questions.REASONABLE_EFFORT_MADE
                },
                new {
                    IsActive = true, CategoryId = 3,
                    DisplayOrder = 11, Question = Questions.AUTHORIZED_MANAGEMENT_APPROVED
                },
                new {
                    IsActive = true, CategoryId = 3,
                    DisplayOrder = 12, Question = Questions.SUPERVISOR_ENSURES_KNOWLEDGE
                }
            );

            // Create a table to store lockout form answers
            Create.Table("LockoutFormAnswers")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("LockoutFormId", "LockoutForms")
                  .WithForeignKeyColumn("LockoutFormQuestionId", "LockoutFormQuestions")
                  .WithColumn("Answer").AsBoolean().Nullable()
                  .WithColumn("Comments").AsCustom("text").Nullable();

            // Populate Answers from existing answers
            Execute.Sql("INSERT INTO LockoutFormAnswers(LockoutFormId, LockoutFormQuestionId, Answer) " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.AFFECTED_EMPLOYEES_NOTIFIED +
                        "'), isNull(AffectedEmployeesNotified,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.AFFECTED_EQUIPMENT_SHUTDOWN +
                        "'), isNull(AffectedEquipmentShutdown,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.ISOLATES_ENERGY_SOURCES +
                        "'), isNull(IsolatesEnergySources,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.CLEARLY_INDICATES_PROHIBITED +
                        "'), isNull(ClearlyIndicatesProhibited,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.RENDERED_SAFE_UNTIL_COMPLETE +
                        "'), isNull(RenderedSafeUntilComplete,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.CANNOT_BE_OPERATED + "'), isNull(CannotBeOperated,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.PARKED_IN_HOME_SAFE + "'), isNull(ParkedInHomeSafe,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.REMOVED_DEVICE_AND_NOTIFIED +
                        "'), isNull(RemovedDeviceAndNotified,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.CONFIRMED_BY_MANAGEMENT +
                        "'), isNull(ConfirmedByManagement,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.REASONABLE_EFFORT_MADE +
                        "'), isNull(ReasonableEffortMade,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.AUTHORIZED_MANAGEMENT_APPROVED +
                        "'), isNull(AuthorizedManagementApproved,0) FROM LockoutForms UNION ALL " +
                        "SELECT Id, (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                        Questions.SUPERVISOR_ENSURES_KNOWLEDGE +
                        "'), isNull(SupervisorEnsuresKnowledge,0) FROM LockoutForms");

            // Add new questions
            Insert.IntoTable("LockoutFormQuestions").Rows(
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 13, Question = Questions.ENERGY_POTENTIAL
                },
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 14, Question = Questions.ENERGY_SINGLE
                },
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 15, Question = Questions.DE_ENERGIZED
                },
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 16, Question = Questions.ENERGY_ISOLATED
                },
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 17, Question = Questions.SINGLE_LOCKOUT
                },
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 18, Question = Questions.EXCLUSIVE
                },
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 19, Question = Questions.NO_HAZARDS
                },
                new {
                    IsActive = true, CategoryId = 4,
                    DisplayOrder = 20, Question = Questions.NO_ACCIDENTS_INVOLVING
                });

            // Delete existing question columns
            Delete.Column("AffectedEmployeesNotified").FromTable("LockoutForms");
            Delete.Column("AffectedEquipmentShutdown").FromTable("LockoutForms");
            Delete.Column("IsolatesEnergySources").FromTable("LockoutForms");
            Delete.Column("ClearlyIndicatesProhibited").FromTable("LockoutForms");
            Delete.Column("RenderedSafeUntilComplete").FromTable("LockoutForms");
            Delete.Column("CannotBeOperated").FromTable("LockoutForms");
            Delete.Column("ParkedInHomeSafe").FromTable("LockoutForms");
            Delete.Column("RemovedDeviceAndNotified").FromTable("LockoutForms");
            Delete.Column("ConfirmedByManagement").FromTable("LockoutForms");
            Delete.Column("ReasonableEffortMade").FromTable("LockoutForms");
            Delete.Column("AuthorizedManagementApproved").FromTable("LockoutForms");
            Delete.Column("SupervisorEnsuresKnowledge").FromTable("LockoutForms");
        }

        public override void Down()
        {
            Delete.Column("ContractorLockOutTagOut").FromTable("LockoutForms");
            Rename.Column("LockRemovalMethodId").OnTable("LockoutForms").To("LockRemovedById");
            Rename.Column("IsolationPointId").OnTable("LockoutForms").To("LockoutDeviceLocationId");
            Alter.Table("LockoutForms")
                 .AddColumn("AffectedEmployeesNotified").AsBoolean().Nullable()
                 .AddColumn("AffectedEquipmentShutdown").AsBoolean().Nullable()
                 .AddColumn("IsolatesEnergySources").AsBoolean().Nullable()
                 .AddColumn("ClearlyIndicatesProhibited").AsBoolean().Nullable()
                 .AddColumn("RenderedSafeUntilComplete").AsBoolean().Nullable()
                 .AddColumn("CannotBeOperated").AsBoolean().Nullable()
                 .AddColumn("ParkedInHomeSafe").AsBoolean().Nullable()
                 .AddColumn("RemovedDeviceAndNotified").AsBoolean().Nullable()
                 .AddColumn("ConfirmedByManagement").AsBoolean().Nullable()
                 .AddColumn("ReasonableEffortMade").AsBoolean().Nullable()
                 .AddColumn("AuthorizedManagementApproved").AsBoolean().Nullable()
                 .AddColumn("SupervisorEnsuresKnowledge").AsBoolean().Nullable();

            Execute.Sql(
                "UPDATE LockoutForms SET AffectedEmployeesNotified = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.AFFECTED_EMPLOYEES_NOTIFIED + "')); " +
                "UPDATE LockoutForms SET AffectedEquipmentShutdown = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.AFFECTED_EQUIPMENT_SHUTDOWN + "')); " +
                "UPDATE LockoutForms SET IsolatesEnergySources = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.ISOLATES_ENERGY_SOURCES + "')); " +
                "UPDATE LockoutForms SET ClearlyIndicatesProhibited = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.CLEARLY_INDICATES_PROHIBITED + "')); " +
                "UPDATE LockoutForms SET RenderedSafeUntilComplete = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.RENDERED_SAFE_UNTIL_COMPLETE + "')); " +
                "UPDATE LockoutForms SET CannotBeOperated = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.CANNOT_BE_OPERATED + "')); " +
                "UPDATE LockoutForms SET ParkedInHomeSafe = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.PARKED_IN_HOME_SAFE + "')); " +
                "UPDATE LockoutForms SET RemovedDeviceAndNotified = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.REMOVED_DEVICE_AND_NOTIFIED + "')); " +
                "UPDATE LockoutForms SET ConfirmedByManagement = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.CONFIRMED_BY_MANAGEMENT + "')); " +
                "UPDATE LockoutForms SET ReasonableEffortMade = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.REASONABLE_EFFORT_MADE + "')); " +
                "UPDATE LockoutForms SET AuthorizedManagementApproved = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.AUTHORIZED_MANAGEMENT_APPROVED + "')); " +
                "UPDATE LockoutForms SET SupervisorEnsuresKnowledge = (SELECT Answer from LockOutFormAnswers WHERE LockoutFormId = LockoutForms.Id AND LockoutFormQuestionId = (SELECT Id from LockoutFormQuestions where cast(Question as varchar(max)) = '" +
                Questions.SUPERVISOR_ENSURES_KNOWLEDGE + "')); ");

            Delete.Table("LockoutFormAnswers");
            Delete.Table("LockoutFormQuestions");
            Delete.Table("LockoutFormQuestionCategories");
        }
    }
}
