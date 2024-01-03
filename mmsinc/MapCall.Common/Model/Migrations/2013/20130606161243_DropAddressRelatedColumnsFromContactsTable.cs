using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130606161243), Tags("Production")]
    public class DropAddressRelatedColumnsFromContactsTable : Migration
    {
        public const string CONTACTS_TABLE_NAME = "Contacts",
                            CONTACTS_STATE_FOREIGN_KEY = "FK_Contacts_States";

        public override void Up()
        {
            Delete.ForeignKey(CONTACTS_STATE_FOREIGN_KEY).OnTable(CONTACTS_TABLE_NAME);
            Delete.Column("StateId").FromTable(CONTACTS_TABLE_NAME);
            Delete.Column("Address1").FromTable(CONTACTS_TABLE_NAME);
            Delete.Column("Address2").FromTable(CONTACTS_TABLE_NAME);
            Delete.Column("City").FromTable(CONTACTS_TABLE_NAME);
            Delete.Column("Zip").FromTable(CONTACTS_TABLE_NAME);
        }

        public override void Down()
        {
            Alter.Table(CONTACTS_TABLE_NAME).AddColumn("StateId").AsInt32().Nullable();
            Alter.Table(CONTACTS_TABLE_NAME).AddColumn("City").AsString(255).Nullable();
            Alter.Table(CONTACTS_TABLE_NAME).AddColumn("Address1").AsString(255).Nullable();
            Alter.Table(CONTACTS_TABLE_NAME).AddColumn("Address2").AsString(255).Nullable();
            Alter.Table(CONTACTS_TABLE_NAME).AddColumn("Zip").AsString(10).Nullable();

            Create.ForeignKey(CONTACTS_STATE_FOREIGN_KEY)
                  .FromTable("Contacts").ForeignColumn("StateID")
                  .ToTable("States").PrimaryColumn("StateID");

            // To be nice, repopulate the old data from the Addresses table if it's possible.
            Execute.Sql(@"update [Contacts] 
                            set [City] = t.Town,
                                [StateId] = s.StateId,
                                [Address1] = a.Address1,
                                [Address2] = a.Address2,
                                [Zip] = a.ZipCode                  
                          from [Contacts] c
                          inner join [Addresses] a on a.Id = c.AddressId
                          inner join [Towns] t on t.TownId = a.TownId
                          inner join [Counties] cnt on cnt.CountyId = t.CountyId
                          inner join [States] s on s.StateId = cnt.StateId");
        }
    }
}
