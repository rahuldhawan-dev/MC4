using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230317145354901), Tags("Production")]
    public class MC4757_AddPitcherFilterColumnsToWorkOrders : AutoReversingMigration
    {
        public override void Up()
        {
            Create
               .Column("InitialServiceLineFlushTime")
               .OnTable("WorkOrders")
               .AsInt32()
               .Nullable();

            Create
               .Column("InitialFlushTimeEnteredById")
               .OnTable("WorkOrders")
               .AsInt32()
               .Nullable()
               .ForeignKey(
                    "FK_WorkOrders_tblPermissions_InitialFlushTimeEnteredById",
                    "tblPermissions",
                    "RecId");

            Create
               .Column("InitialFlushTimeEnteredAt")
               .OnTable("WorkOrders")
               .AsDateTime()
               .Nullable();

            Create
               .Column("HasPitcherFilterBeenProvidedToCustomer")
               .OnTable("WorkOrders")
               .AsBoolean()
               .Nullable();

            Create
               .Column("DatePitcherFilterDeliveredToCustomer")
               .OnTable("WorkOrders")
               .AsDateTime()
               .Nullable();

            Create.LookupTable("PitcherFilterCustomerDeliveryMethods");

            Create
               .Column("PitcherFilterCustomerDeliveryMethodId")
               .OnTable("WorkOrders")
               .AsInt32()
               .Nullable()
               .ForeignKey(
                    "FK_WorkOrders_PitcherFilterCustomerDeliveryMethods_PitcherFilterCustomerDeliveryMethodId",
                    "PitcherFilterCustomerDeliveryMethods",
                    "Id");

            Create
               .Column("PitcherFilterCustomerDeliveryOtherMethod")
               .OnTable("WorkOrders")
               .AsString(50)
               .Nullable();

            Create
               .Column("DateCustomerProvidedAWStateLeadInformation")
               .OnTable("WorkOrders")
               .AsDateTime()
               .Nullable();
        }
    }
}
