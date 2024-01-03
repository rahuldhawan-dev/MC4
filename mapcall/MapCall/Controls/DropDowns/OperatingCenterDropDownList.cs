namespace MapCall.Controls.DropDowns
{
    public class OperatingCenterDropDownList : DataSourceDropDownList
    {
        public OperatingCenterDropDownList()
        {
            TableName = "OperatingCenters";
            ValueFieldName = "OperatingCenterID";
            TextFieldName = "OpCntr";
            EmptyItemText = "-- Select Operating Center --";
            EnableCaching = true;
            this.SelectCommand =
                "select OperatingCenterID, OperatingCenterCode + ' - ' + OperatingCenterName as OpCntr from OperatingCenters order by OperatingCenterCode";
        }
    }
}