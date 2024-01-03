using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210916102050921), Tags("Production")]
    public class MC3749AdditionalChangesForNearMiss : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn("NearMisses", "EmployeeId", "tblEmployee", "tblEmployeeID");
            Delete.ForeignKeyColumn("NearMisses", "EmployeeTypeId", "EmployeeTypes");
            Delete.Column("EmployeeName").FromTable("NearMisses");
            Delete.Column("EmailAddress").FromTable("NearMisses");
            Delete.Column("PhoneCellular").FromTable("NearMisses");
            Delete.Column("ContractorName").FromTable("NearMisses");
            Delete.Column("ReviewedAt").FromTable("NearMisses");
            Delete.Column("ReviewedBy").FromTable("NearMisses");

            Alter.Table("NearMisses")
                 .AddColumn("RelatedToContractor").AsBoolean().Nullable()
                 .AddColumn("ReviewedDate").AsDateTime().Nullable()
                 .AddColumn("HaveReviewedNearMiss").AsBoolean().Nullable()
                 .AddColumn("IsContractedOperations").AsBoolean().Nullable()
                 .AddForeignKeyColumn("ReviewedBy", "tblPermissions", "RecID");
        }

        public override void Down()
        {
            Delete.Column("RelatedToContractor").FromTable("NearMisses");
            Delete.Column("ReviewedDate").FromTable("NearMisses");
            Delete.Column("HaveReviewedNearMiss").FromTable("NearMisses");
            Delete.Column("IsContractedOperations").FromTable("NearMisses");
            Delete.ForeignKeyColumn("NearMisses", "ReviewedBy", "tblPermissions", "RecID");
            Alter.Table("NearMisses")
                 .AddForeignKeyColumn("EmployeeTypeId", "EmployeeTypes")
                 .AddForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeID")
                 .AddColumn("EmployeeName").AsString().Nullable()
                 .AddColumn("EmailAddress").AsString().Nullable()
                 .AddColumn("PhoneCellular").AsString().Nullable()
                 .AddColumn("ContractorName").AsString().Nullable()
                 .AddColumn("ReviewedAt").AsString().Nullable()
                 .AddColumn("ReviewedBy").AsString().Nullable();
        }
    }
}

