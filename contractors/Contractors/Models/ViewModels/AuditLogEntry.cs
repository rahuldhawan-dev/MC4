using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contractors.Models.ViewModels
{
    public class SecureSearchAuditLogEntryForSingleRecord : SearchSet<AuditLogEntry>
    {
        #region Public Methods

        [Required, Secured]
        public string EntityName { get; set; }
        [Required, Secured]
        public int? EntityId { get; set; }

        public override string DefaultSortBy => nameof(AuditLogEntry.Timestamp);

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            // SearchMapper automatically sets properties with "EntityId" to "Id"
            // because that's how we generally want to search for things. In this case,
            // AuditLogEntry has an actual column called EntityId that needs to be searched
            // so we want to set it back to EntityId.
            mapper.MappedProperties[nameof(EntityId)].ActualName = "EntityId";

            // The EntityName property should normally be a SearchString. However, due to the way
            // the ajax view is created and the way the urls are generated for the pagination links,
            // the EntityName value ends up being serialized to the url as "SearchString" rather than
            // the value itself. To bypass this, we do this little hack of changing the mapped value
            // to a SearchString.
            mapper.MappedProperties[nameof(EntityName)].Value = new SearchString
            {
                Value = EntityName,
                MatchType = SearchStringMatchType.Exact
            };
        }

        #endregion
    }
}