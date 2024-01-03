namespace MapCall.Controls.DropDowns
{
    public class StatesDropDownList : DataSourceDropDownList
    {
        public StatesDropDownList()
        {
            TableName = "States";
            ValueFieldName = "StateID";
            TextFieldName = "Abbreviation";
            OrderByTextField = false; // So NJ, PA, IL are at the top. 
            EmptyItemText = "-- Select State --";
            EnableCaching = true;
            CacheDuration = 60000; // Ten minutes should be long enough of a gap before we need
        }
    }

}