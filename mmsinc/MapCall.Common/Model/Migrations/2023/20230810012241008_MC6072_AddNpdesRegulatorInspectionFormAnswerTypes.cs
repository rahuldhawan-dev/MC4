using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230810012241008), Tags("Production")]
    public class Mc6072_AddGateStatusAnswerTypes : Migration
    {
        #region Constants

        private const string LEGACY_COLUMN_NAME = "IsGateMovingFreely",
                             NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE = "GateStatusAnswerTypes",
                             NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE_ID = "GateStatusAnswerTypeId",
                             NPDES_REGULATOR_INSPECTIONS = "NPDESRegulatorInspections";

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            this.CreateLookupTableWithValues(NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE, "Yes", "No", "N/A");
            Delete.Column(LEGACY_COLUMN_NAME).FromTable(NPDES_REGULATOR_INSPECTIONS);
            Alter.Table(NPDES_REGULATOR_INSPECTIONS).AddForeignKeyColumn(NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE_ID, NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(NPDES_REGULATOR_INSPECTIONS, NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE_ID, NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE);
            Delete.Table(NPDES_REGULATOR_INSPECTON_FORM_ANSWER_TYPE);
            Alter.Table(NPDES_REGULATOR_INSPECTIONS)
                 .AddColumn(LEGACY_COLUMN_NAME)
                 .AsBoolean()
                 .Nullable();
        }

        #endregion
    }
}
