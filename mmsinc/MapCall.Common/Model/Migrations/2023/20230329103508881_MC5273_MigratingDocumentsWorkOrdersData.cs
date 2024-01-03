using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230329103508881), Tags("Production")]
    public class MC5273_MigratingDocumentsWorkOrdersData : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"INSERT INTO DocumentLink(DocumentID, DataTypeID, DataLinkID, DocumentTypeID, UpdatedById)
SELECT 
DWO.DocumentID, 
DT.DataTypeID, 
DWO.WorkOrderID, 
D.documentTypeID,
COALESCE(D.UpdatedById, U.RecId)
FROM DocumentsWorkOrders DWO 
JOIN Document D on DWO.DocumentID = D.documentID
JOIN DataType DT on DT.Table_Name = 'WorkOrders'
JOIN tblPermissions U on U.UserName = 'mcadmin'");
        }

        public override void Down()
        {
            Execute.Sql(@"Delete from DocumentLink where DataTypeID = 127");
        }
    }
}

