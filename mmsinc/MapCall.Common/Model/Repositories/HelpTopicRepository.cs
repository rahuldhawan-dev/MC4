using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class HelpTopicRepository :
        RepositoryBase<HelpTopic>, IHelpTopicRepository
    {
        #region Constructors

        public HelpTopicRepository(ISession session, IContainer container) :
            base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<HelpTopic> SearchHelpTopicsWithDocuments(ISearchHelpTopicWithDocument search)
        {
            HelpTopic helpTopic = null;
            var query = Session.QueryOver(() => helpTopic);

            if (!string.IsNullOrEmpty(search.DocumentTitle?.Value) || search.DocumentType != null ||
                search.Active != null ||
                (search.DocumentUpdated != null && search.DocumentUpdated.End != null) ||
                (search.DocumentNextReviewDate != null && search.DocumentNextReviewDate.End != null))
            {
                Document document = null;
                DataType dataType = null;
                var documents = QueryOver.Of<DocumentLink>()
                                         .JoinAlias(x => x.Document, () => document, JoinType.LeftOuterJoin)
                                         .JoinAlias(x => x.DataType, () => dataType, JoinType.LeftOuterJoin)
                                         .Where(dl =>
                                              dl.LinkedId == helpTopic.Id && dataType.TableName == "HelpTopics");

                if (!string.IsNullOrEmpty(search.DocumentTitle?.Value))
                {
                    const string documentFilenameProperty = "document.FileName";
                    if (search.DocumentTitle.MatchType == SearchStringMatchType.Wildcard)
                    {
                        documents = documents.Where(GetWildcardMatch(search.DocumentTitle.Value,
                            documentFilenameProperty));
                    }
                    else if (search.DocumentTitle.MatchType == SearchStringMatchType.Exact)
                    {
                        documents = documents
                           .Where(Restrictions.Eq(documentFilenameProperty, search.DocumentTitle.Value));
                    }
                }

                if (search.DocumentType != null && search.DocumentType.Any())
                {
                    documents = documents.Where(
                        Restrictions.In("DocumentType", search.DocumentType));
                }

                if (search.Active != null)
                {
                    documents = documents.Where(dl =>
                        dl.DocumentStatus.Id == search.Active.Value
                    );
                }

                if (search.DocumentUpdated != null && search.DocumentUpdated.End != null)
                {
                    string updatedAtPropertyName = "UpdatedAt";
                    documents = documents.Where(search.DocumentUpdated.GetCriterion(null, updatedAtPropertyName));
                }

                if (search.DocumentNextReviewDate != null && search.DocumentNextReviewDate.End != null)
                {
                    string nextReviewDateCreatedAt = "NextReviewDate";
                    documents = documents.Where(search.DocumentNextReviewDate.GetCriterion(null, nextReviewDateCreatedAt));
                }

                query.WithSubquery.WhereExists(documents.Select(Projections.Constant(1)));
            }

            return Search(search, query);
        }

        #endregion

        private ICriterion GetWildcardMatch(string value, string propName)
        {
            if (value.Contains("*"))
            {
                // MatchMode.Exact adheres to the wildcards exactly.
                value = value.Replace('*', '%');
                return Restrictions.Like(propName, value, MatchMode.Exact);
            }

            // Using MatchMode.Anywhere completely ignores wildcard characters and essentially
            // does a search of: LIKE '%prop.Value%'
            return Restrictions.Like(propName, value, MatchMode.Anywhere);
        }
    }

    public interface IHelpTopicRepository : IRepository<HelpTopic>
    {
        IEnumerable<HelpTopic> SearchHelpTopicsWithDocuments(ISearchHelpTopicWithDocument search);
    }
}
