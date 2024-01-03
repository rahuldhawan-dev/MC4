using System;
using System.Text;
using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171025110548554), Tags("Production")]
    public class WO0000000195491 : Migration
    {
        #region Private methods

        private void CleanupCollectedByColumn()
        {
            // This was a text field that had usernames/not usernames in it. 
            // This is going to lose values for "Beth Coma" who entered samples between 2005-2007.
            // No one knows who this refers to user-wise. 
            // Similarly, "265" will also not have an attached user. This is only a single row.
            // Henderson Laboratory will be converted to Ann Marie Lewis as that's the value for Analysis_Performed_By. Also only a single row.

            // Remove whitespace from a few values.
            Execute.Sql("update BacterialWaterSamples set [Collected_By] = LTRIM(RTRIM(Collected_By))");
            Execute.Sql(
                "update BacterialWaterSamples set [Collected_By] = 'Ann Marie Kelly' where Collected_By = 'Henderson Laboratory'");

            Action<string, string> updateUserName = (oldName, newName) => {
                Update.Table("BacterialWaterSamples").Set(new {Collected_By = newName})
                      .Where(new {Collected_By = oldName});
            };

            updateUserName("Ann Marie Kelly", "kellya");
            updateUserName("Anthonymarcucci", "marcuca");
            updateUserName("Colenczuc", "olenczc");
            updateUserName("Colenczuk", "olenczc");
            updateUserName("D.Derwid", "DERWIDDS");
            updateUserName("garciatj", "GARCIAT1");
            updateUserName("Ila Patel", "patelid");
            updateUserName("Ilapatel", "patelid");
            updateUserName("Ipatel", "patelid");
            updateUserName("Jameswalker", "walkerjd");
            updateUserName("johnsea1", "johnsea");
            updateUserName("jordanspitzerlondon", "spitzej2");
            updateUserName("jramsden", "jessramsden");
            updateUserName("jramsedn", "jessramsden");
            updateUserName("Maureen Kelly", "kellym2");
            updateUserName("Mkelly", "kellym2");
            updateUserName("Olenczuc", "olenczc");
            updateUserName("Ratzlae", "ratzlae1");
            updateUserName("Sunilpatil", "patilsr");
            updateUserName("Tamburello", "cairatamburello");
            updateUserName("Thomasiverson", "iversota");
            updateUserName("Vishalmodi", "modivm");

            Execute.Sql(
                "update BacterialWaterSamples set CollectedByUserId = (select top 1 RecId from tblPermissions where Username = Collected_By)");
        }

        #endregion

        public override void Up()
        {
            // Rename existing columns so they match the model.
            Rename.Column("DTM_Incubator_In").OnTable("BacterialWaterSamples").To("ColiformSetupDTM");
            Rename.Column("DTM_Incubator_Out").OnTable("BacterialWaterSamples").To("ColiformReadDTM");
            Rename.Column("SampleId").OnTable("BacterialWaterSamples").To("SampleNumber");
            Rename.Column("Value_Ortho").OnTable("BacterialWaterSamples").To("OrthophosphateAsP");
            Rename.Column("HPC").OnTable("BacterialWaterSamples").To("FinalHPC");

            Create.Table("BacterialWaterSampleAnalysts")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("EmployeeId").AsInt32().NotNullable()
                  .ForeignKey("FK_BacterialWaterSampleAnalysts_tblEmployee_EmployeeId", "tblEmployee", "tblEmployeeId")
                  .WithColumn("IsActive").AsBoolean().NotNullable();

            Create.Table("BacterialWaterSampleAnalystsOperatingCenters")
                  .WithColumn("BacterialWaterSampleAnalystId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_BacterialWaterSampleAnalystsOperatingCenters_BacterialWaterSampleAnalysts_BacterialWaterSampleAnalystId",
                       "BacterialWaterSampleAnalysts", "Id")
                  .WithColumn("OperatingCenterId").AsInt32().NotNullable()
                  .ForeignKey("FK_BacterialWaterSampleAnalystsOperatingCenters_OperatingCenters_OperatingCenterId",
                       "OperatingCenters", "OperatingCenterId");

            // There aren't any default values given for this table.
            this.CreateLookupTableWithValues("BacterialWaterSampleConfirmMethods");

            Alter.Table("BacterialWaterSamples")
                 .AddColumn("IsSpreader").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("OrthophosphateAsPO4").AsDecimal(6, 3).Nullable()
                 .AddColumn("ColiformSetupAnalystId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_ColiformSetupAnalystId",
                      "BacterialWaterSampleAnalysts", "Id")
                 .AddColumn("ColiformReadAnalystId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_ColiformReadAnalystId",
                      "BacterialWaterSampleAnalysts", "Id")
                 .AddColumn("HPCSetupAnalystId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_HPCSetupAnalystId",
                      "BacterialWaterSampleAnalysts", "Id")
                 .AddColumn("HPCReadAnalystId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_HPCReadAnalystId",
                      "BacterialWaterSampleAnalysts", "Id")
                 .AddColumn("HPCSetupDTM").AsDateTime().Nullable()
                 .AddColumn("HPCReadDTM").AsDateTime().Nullable()
                 .AddColumn("ColiformConfirmMethodId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleConfirmMethods_ColiformConfirmMethodId",
                      "BacterialWaterSampleConfirmMethods", "Id")
                 .AddColumn("EColiConfirmMethodId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleConfirmMethods_EColiConfirmMethodId",
                      "BacterialWaterSampleConfirmMethods", "Id")
                 .AddColumn("HPCConfirmMethodId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleConfirmMethods_HPCConfirmMethodId",
                      "BacterialWaterSampleConfirmMethods", "Id")
                 .AddColumn("CollectedByUserId").AsInt32().Nullable()
                 .ForeignKey("FK_BacterialWaterSamples_tblPermissions_CollectedByUserId", "tblPermissions", "RecId");

            Update.Table("BacterialWaterSamples").Set(new {Coliform_Confirm = false})
                  .Where(new {Coliform_Confirm = (bool?)null});

            Alter.Column("Coliform_Confirm").OnTable("BacterialWaterSamples").AsBoolean().NotNullable();

            this.EnableIdentityInsert("BacterialSampleTypes");
            Execute.Sql(@"IF NOT EXISTS (SELECT * FROM BacterialSampleTypes WHERE Id = 16)
   BEGIN
       INSERT INTO BacterialSampleTypes (Id, Description) VALUES (16, 'Emergency Response')
   END");
            this.DisableIdentityInsert("BacterialSampleTypes");

            // 3. Take existing Read/SetupAnalyst values, convert them to new format, and update new BWS columns.
            //      This will need to add operating centers too.

            //    NAME                 | EMPLOYEE ID    | OPERATING CENTER IDS
            //    garciatj/T Garcia    | 7860           | 10, 14
            //    MT/M. Tisch          | 2004           | 15, 16
            //    K.Shaffer/K. Shaffer | 7885           | 15, 16
            //    EH                   | 7869           | 11
            //    ER                   | 16886          | 11
            //    C Olenczuk           | 2319           | 14
            //    A M Kelly            | 17019          | 14
            //    SY                   | 2024           | 15

            System.Action<string[], int, int[]> fixAnalyst = (names, empId, opCenterId) => {
                // Insert into analysts table for the employee id. Save this value
                // Insert into analysts for each op center
                // Update BWS for columns for each name, use the same saved value for each name. 

                var sb = new StringBuilder();
                sb.AppendLine($@"
    declare @analystId int
    insert into [BacterialWaterSampleAnalysts] (EmployeeId, IsActive) VALUES({empId}, 1)
    set @analystId = @@IDENTITY");

                foreach (var opc in opCenterId)
                {
                    sb.AppendLine(
                        $"insert into [BacterialWaterSampleAnalystsOperatingCenters] (BacterialWaterSampleAnalystId, OperatingCenterId) VALUES (@analystId, {opc})");
                }

                foreach (var name in names)
                {
                    sb.AppendLine(
                        $"update [BacterialWaterSamples] set [ColiformSetupAnalystId] = @analystId where [SetupAnalyst] = '{name}'");
                    sb.AppendLine(
                        $"update [BacterialWaterSamples] set [ColiformReadAnalystId] = @analystId where [ReadAnalyst] = '{name}'");
                }

                Execute.Sql(sb.ToString());
            };

            fixAnalyst(new[] {"garciatj", "T Garcia"}, 7860, new[] {10, 14});
            fixAnalyst(new[] {"MT", "M. Tisch", "M.Tisch"}, 2004, new[] {15, 16});
            fixAnalyst(new[] {"K.Shaffer", "K. Shaffer"}, 7885, new[] {15, 16});
            fixAnalyst(new[] {"EH"}, 7869, new[] {11});
            fixAnalyst(new[] {"ER"}, 16886, new[] {11});
            fixAnalyst(new[] {"C Olenczuk"}, 2319, new[] {14});
            fixAnalyst(new[] {"A M Kelly"}, 17019, new[] {14});
            fixAnalyst(new[] {"SY"}, 2024, new[] {15});

            Delete.Column("SetupAnalyst").FromTable("BacterialWaterSamples");
            Delete.Column("ReadAnalyst").FromTable("BacterialWaterSamples");

            CleanupCollectedByColumn();
        }

        public override void Down()
        {
            Create.Column("SetupAnalyst").OnTable("BacterialWaterSamples").AsString(50).Nullable();
            Create.Column("ReadAnalyst").OnTable("BacterialWaterSamples").AsString(50).Nullable();

            Delete.ForeignKey("FK_BacterialWaterSamples_tblPermissions_CollectedByUserId")
                  .OnTable("BacterialWaterSamples");
            Delete.ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleConfirmMethods_ColiformConfirmMethodId")
                  .OnTable("BacterialWaterSamples");
            Delete.ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleConfirmMethods_EColiConfirmMethodId")
                  .OnTable("BacterialWaterSamples");
            Delete.ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleConfirmMethods_HPCConfirmMethodId")
                  .OnTable("BacterialWaterSamples");
            Delete.ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_ColiformSetupAnalystId")
                  .OnTable("BacterialWaterSamples");
            Delete.ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_ColiformReadAnalystId")
                  .OnTable("BacterialWaterSamples");
            Delete.ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_HPCSetupAnalystId")
                  .OnTable("BacterialWaterSamples");
            Delete.ForeignKey("FK_BacterialWaterSamples_BacterialWaterSampleAnalysts_HPCReadAnalystId")
                  .OnTable("BacterialWaterSamples");

            Delete.Table("BacterialWaterSampleConfirmMethods");

            Delete.Column("CollectedByUserId").FromTable("BacterialWaterSamples");
            Delete.Column("ColiformSetupAnalystId").FromTable("BacterialWaterSamples");
            Delete.Column("ColiformReadAnalystId").FromTable("BacterialWaterSamples");
            Delete.Column("HPCSetupAnalystId").FromTable("BacterialWaterSamples");
            Delete.Column("HPCReadAnalystId").FromTable("BacterialWaterSamples");
            Delete.Column("ColiformConfirmMethodId").FromTable("BacterialWaterSamples");
            Delete.Column("EColiConfirmMethodId").FromTable("BacterialWaterSamples");
            Delete.Column("HPCConfirmMethodId").FromTable("BacterialWaterSamples");
            Delete.Column("IsSpreader").FromTable("BacterialWaterSamples");
            Delete.Column("HPCSetupDTM").FromTable("BacterialWaterSamples");
            Delete.Column("HPCReadDTM").FromTable("BacterialWaterSamples");
            Delete.Column("OrthophosphateAsPO4").FromTable("BacterialWaterSamples");

            // Rename existing columns back to their weird confusing names.
            Rename.Column("ColiformSetupDTM").OnTable("BacterialWaterSamples").To("DTM_Incubator_In");
            Rename.Column("ColiformReadDTM").OnTable("BacterialWaterSamples").To("DTM_Incubator_Out");
            //  Rename.Column("ColiformSetupAnalyst").OnTable("BacterialWaterSamples").To("SetupAnalyst");
            //  Rename.Column("ColiformReadAnalyst").OnTable("BacterialWaterSamples").To("ReadAnalyst");
            Rename.Column("SampleNumber").OnTable("BacterialWaterSamples").To("SampleId");
            Rename.Column("OrthophosphateAsP").OnTable("BacterialWaterSamples").To("Value_Ortho");
            Rename.Column("FinalHPC").OnTable("BacterialWaterSamples").To("HPC");

            Delete
               .ForeignKey(
                    "FK_BacterialWaterSampleAnalystsOperatingCenters_BacterialWaterSampleAnalysts_BacterialWaterSampleAnalystId")
               .OnTable("BacterialWaterSampleAnalystsOperatingCenters");
            Delete.ForeignKey("FK_BacterialWaterSampleAnalystsOperatingCenters_OperatingCenters_OperatingCenterId")
                  .OnTable("BacterialWaterSampleAnalystsOperatingCenters");
            Delete.Table("BacterialWaterSampleAnalystsOperatingCenters");

            Delete.ForeignKey("FK_BacterialWaterSampleAnalysts_tblEmployee_EmployeeId")
                  .OnTable("BacterialWaterSampleAnalysts");
            Delete.Table("BacterialWaterSampleAnalysts");
        }
    }
}
