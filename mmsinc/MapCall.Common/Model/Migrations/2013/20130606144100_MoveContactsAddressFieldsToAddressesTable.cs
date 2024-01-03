using System;
using System.Collections.Generic;
using System.Data;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130606144100), Tags("Production")]
    public class MoveContactsAddressFieldsToAddressesTable : Migration
    {
        public const string CONTACTS_TABLE_NAME = "Contacts",
                            CONTACTS_ADDRESSID = "AddressId",
                            FOREIGN_KEY = "FK_Contacts_Addresses_AddressId",
                            CONTACTS_STATE_FOREIGN_KEY = "FK_Contacts_States";

        #region Private Methods

        private void ConvertToAddress()
        {
            const string commandText = @"    
                SET NOCOUNT ON
                DECLARE @contactId int
      
                DECLARE tableCursor
                CURSOR FOR
                    SELECT [ContactId] FROM [Contacts]

                OPEN tableCursor
                    FETCH NEXT FROM tableCursor INTO @contactid;
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        insert into [Addresses] ([Address1], [Address2], [TownId], [ZipCode])
                            select c.Address1, c.Address2, t.TownID, c.Zip as [ZipCode]
                            from [Contacts] c
                            inner join [Towns] t on t.StateID = c.StateID and t.Town = c.City
                            where c.ContactID = @contactId
                            and c.Address1 is not null
                            and c.Zip is not null;

                        update [Contacts] set AddressId = @@IDENTITY where [ContactId] = @contactId

                        FETCH NEXT FROM tableCursor INTO @contactId;
                    END
                CLOSE tableCursor;
                DEALLOCATE tableCursor;";

            Execute.Sql(commandText);
        }

        #endregion

        public override void Up()
        {
            Alter.Table(CONTACTS_TABLE_NAME)
                 .AddColumn(CONTACTS_ADDRESSID)
                 .AsInt32()
                 .Nullable()
                 .ForeignKey(FOREIGN_KEY, "Addresses", "Id");

            ConvertToAddress();
        }

        public override void Down()
        {
            Delete.ForeignKey(FOREIGN_KEY).OnTable(CONTACTS_TABLE_NAME);
            Delete.Column(CONTACTS_ADDRESSID).FromTable(CONTACTS_TABLE_NAME);
        }
    }
}
