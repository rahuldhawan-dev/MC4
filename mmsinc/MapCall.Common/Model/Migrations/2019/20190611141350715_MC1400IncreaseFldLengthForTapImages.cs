using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190611141350715), Tags("Production")]
    public class MC1400IncreaseFldLengthForTapImages : Migration
    {
        public const int FLD = 255;

        public override void Up()
        {
            Alter.Table("TapImages").AlterColumn("fld").AsAnsiString(FLD).NotNullable();
        }

        public override void Down()
        {
            //noop, can't make smaller
        }
    }
}
