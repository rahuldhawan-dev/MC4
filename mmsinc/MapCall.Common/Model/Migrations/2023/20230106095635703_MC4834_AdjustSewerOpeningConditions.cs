using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230106095635703), Tags("Production")]
    public class MC4834_AdjustSewerOpeningConditions : Migration
    {
        public override void Up()
        {
            Alter.Table("OpeningConditions")
                 .AddColumn("IsActive")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(true);
            
            Update.Table("OpeningConditions")
                  .Set(new {IsActive = false})
                  .Where(new { Description = "Ladder Rung Repair" });

            Insert.IntoTable("OpeningConditions")
                  .Row(new { Description = "Inflow" });
        }

        public override void Down()
        {
            Delete.FromTable("OpeningConditions").Row(new { Description = "Inflow" });
            Delete.Column("IsActive").FromTable("OpeningConditions");
        }
    }
}

