using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170125101202551), Tags("Production")]
    public class Bug3532MeterChangeOuts : Migration
    {
        public override void Up()
        {
            Create.Column("PartialExcavation").OnTable("MeterChangeOuts")
                  .AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("PartialExcavation").FromTable("MeterChangeOuts");
        }
    }
}
