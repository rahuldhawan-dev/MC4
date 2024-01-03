using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapCall.Controls.DropDowns
{
    public class ContactTypesDropDownList: DataSourceDropDownList
    {
        public ContactTypesDropDownList()
        {
            TableName = "ContactTypes";
            ValueFieldName = "ContactTypeID";
            TextFieldName = "Name";
            OrderByTextField = true; // So NJ, PA, IL are at the top. 
            EmptyItemText = "-- Select Contact Type --";
            EnableCaching = false;
        }
    }
}