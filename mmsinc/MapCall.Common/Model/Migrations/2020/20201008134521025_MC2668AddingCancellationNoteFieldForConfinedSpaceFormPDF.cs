using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201008134521025), Tags("Production")]
    public class MC2668AddingCancellationNoteFieldForConfinedSpaceFormPDF : Migration
    {
        public override void Up()
        {
            Alter.Table("ConfinedSpaceForms").AddColumn("PermitCancellationNote").AsAnsiString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("PermitCancellationNote").FromTable("ConfinedSpaceForms");
        }
    }
}
