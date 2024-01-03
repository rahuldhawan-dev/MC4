using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;

namespace MapCall.Modules.Data.Contacts
{

    #region Structs

    struct Commands
    {
        public const string SEARCH_CONTACTS_BY_CONTACTID = @"SELECT * FROM [Contacts] WHERE [ContactId] = @ContactId";
        public const string SEARCH_CONTACTS_BY_NAME_AUTOCOMPLETE = @"SELECT DISTINCT
	                                                                    ContactID,
	                                                                    FirstName,
                                                                        LastName
                                                                    FROM 
	                                                                    Contacts c
                                                                    WHERE 
	                                                                    c.FirstName LIKE @Search + '%'
                                                                    OR
	                                                                    c.LastName LIKE @Search + '%'

                                                                    ORDER BY LastName ASC";

    }

    #endregion

    /// <summary>
    /// Summary description for ContactsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ContactsService : WebService
    {
        #region Private Methods

        private static string TryGetString(IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return ((reader[ordinal] != DBNull.Value) ? reader.GetString(ordinal) : string.Empty);
        }

        #endregion

        public IDictionary<string, object> SearchContactsById(int contactId)
        {

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = Commands.SEARCH_CONTACTS_BY_CONTACTID;
                comm.Parameters.AddWithValue("ContactID", contactId);

                connection.Open();

                using (var response = comm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (response.HasRows)
                    {
                        var result = new Dictionary<string, object>();
                        while (response.Read())
                        {
                            for (var i = 0; i < response.FieldCount; i++)
                            {
                                var col = response.GetName(i);
                                var value = response.GetValue(i);
                                result.Add(col, (value != DBNull.Value ? value : null));
                            }

                        }
                        return result;
                    }
                }
            }
            return null;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchContactsAutoComplete(string term)
        {
            var results = new Dictionary<int, string>();

            if (!String.IsNullOrWhiteSpace(term))
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
                using (var comm = connection.CreateCommand())
                {
                    comm.CommandText = Commands.SEARCH_CONTACTS_BY_NAME_AUTOCOMPLETE;

                    comm.Parameters.AddWithValue("Search", term);

                    connection.Open();

                    var response = comm.ExecuteReader(CommandBehavior.CloseConnection);

                    if (response.HasRows)
                    {
                        while (response.Read())
                        {
                            // Alex
                            var firstName = TryGetString(response, "FirstName");
                            var lastName = TryGetString(response, "LastName");
                            var contactId = response.GetInt32(response.GetOrdinal("ContactID"));

                            results.Add(contactId, lastName + ", " + firstName);

                        }
                    }
                }
            }

            // Asmx service won't return a dictionary. But it will return 
            // an array of KeyValuePairs. That's stupid. -Ross 1/10/11
            return results.ToArray();

        }

    }
}
