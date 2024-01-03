using NHibernate.Criterion;

namespace MMSINC.Data
{
    /// <summary>
    /// Represents a mappable property in an ISearchCriteria.
    /// </summary>
    public interface ISearchCriterion
    {
        ICriterion GetCriterion(ICriterion original, string propertyName);
    }
}
