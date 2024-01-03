using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class AuditLogEntryRepository : RepositoryBase<AuditLogEntry>, IAuditLogEntryRepository
    {
        #region Constants

        public static readonly string[] UNION_ENTITIES = {
            "Union",
            "UnionContract",
            "UnionContractProposal",
            "Local",
            "Grievance"
        };

        public enum UnionDataTypes
        {
            Grievance = 20,
            Contract = 21,
            ContractProposal = 23,
            Union = 25,
            Local = 27
        }

        #endregion

        #region Constructors

        public AuditLogEntryRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Private Methods

        private static ICriterion GetUnionEntries()
        {
            return
                Restrictions.In("EntityName", UNION_ENTITIES)
                | GetDocumentLinksForDataTypeId(UnionDataTypes.Grievance)
                | GetDocumentLinksForDataTypeId(UnionDataTypes.Contract)
                | GetDocumentLinksForDataTypeId(UnionDataTypes.ContractProposal)
                | GetDocumentLinksForDataTypeId(UnionDataTypes.Union)
                | GetDocumentLinksForDataTypeId(UnionDataTypes.Local)
                ;
        }

        /// <summary>
        /// Can't use IN because NewValue is a text field. 
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static AbstractCriterion GetDocumentLinksForDataTypeId(UnionDataTypes dataType)
        {
            return
                Restrictions.And(
                    Restrictions.And(
                        Restrictions.Eq("FieldName", "DataType"),
                        Restrictions.Eq("EntityName", "DocumentLink")
                    ),
                    Restrictions.Like("NewValue", ((int)dataType).ToString())
                );
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<AuditLogEntry> SearchLogsForSpecificEntityRecord(ISearchAuditLogEntryForSingleRecord search)
        {
            var query = Session.QueryOver<AuditLogEntry>();
            query.And(x => x.EntityId == search.EntityId);
            // The EntityName column has gotten fulled of non-entity name things over the years. 
            // It might be an entity name, it might be a controller name. Need to search for both
            // to ensure the logs are coming back when the EntityTypeName does not match the ControllerName.
            query.And(x => x.EntityName == search.EntityTypeName || x.EntityName == search.ControllerName);

            return Search(search, query);
        }

        public IEnumerable<AuditLogEntry> SearchUnionEntries(ISearchSet<AuditLogEntry> search)
        {
            var crit = Criteria;
            crit.Add(GetUnionEntries());
            return Search(search, crit);
        }

        public IEnumerable<string> GetUniqueAuditEntryTypes()
        {
            return (from e in Linq select e.AuditEntryType).Distinct();
        }

        public IEnumerable<string> GetUniqueEntityNames()
        {
            return (from e in Linq orderby e.EntityName select e.EntityName).Distinct();
        }

        #endregion
    }

    public interface IAuditLogEntryRepository : MMSINC.Data.NHibernate.IRepository<AuditLogEntry>
    {
        IEnumerable<AuditLogEntry> SearchLogsForSpecificEntityRecord(ISearchAuditLogEntryForSingleRecord search);
        IEnumerable<AuditLogEntry> SearchUnionEntries(ISearchSet<AuditLogEntry> search);
        IEnumerable<string> GetUniqueAuditEntryTypes();
        IEnumerable<string> GetUniqueEntityNames();
    }
}
