using System;

namespace MMSINC.Data
{
    /// <summary>
    /// This attribute will allow the ViewModelToSearchMapper to add the specified aliases 
    /// to the RepositoryBase Criteria, and the aliases to the searched property 
    /// <para>Note: It is case sensitive</para>
    /// <para>Note: You are joining an existing association/property, so you must provide 
    /// the name of that property as it is in your class.</para>
    /// <para>&#160;</para>
    /// <para>Using a Repository that has JoinAliases</para>
    /// If using a repository method with JoinAliases then these will need to match what you
    /// are using there. Also it's using what the property is named, not what the property's class name is
    /// <para>E.g. query.JoinAlias(_ => position.CommonName, () => commonName); ==> [SearchAlias("position.CommonName", "commonName", "Id")]</para>
    /// 
    /// <para>Using a Repository that has WithSubquery.WhereExists</para>
    /// <para>SearchAlias is NOT supported for properties for tables in SubQuery</para>
    /// <para>for one-to-many relationship, e.g. a HelpTopic has multiple documents, using a SearchAlias would result in duplicate main table (HelpTopics) records as it generates a JOIN between two tables</para>
    /// <para>You would need to use WithSubquery.WhereExists to create a subquery between the tables to achieve distinct records from main table e.g. get distinct HelpTopics even if there are multiple documents for a HelpTopic</para>
    /// </summary>
    /// <example>
    /// 
    /// Let's say you have an Address. Addresses usually have a Town. So let's say you
    /// wanted to create a search alias to find all Addresses with a Town.
    /// 
    /// [SearchAlias("Town", "Id")]
    /// 
    /// or perhaps by a Town's name
    /// 
    /// [SearchAlias("Town", "Name")]
    /// 
    /// You can also search for a property that is an association itself, like if you
    /// want to find a Town's County.
    /// 
    /// [SearchAlias("Town", "County.Id")] 
    /// 
    /// If you wanna search for an association on an association, like if you're trying to
    /// find a matching State through a County of a Town:
    /// 
    /// [SearchAlias("Town.County", "State.Id")]
    /// 
    /// 
    /// If you have already aliased the Town as town in a repository method
    /// [SearchAlias("Town", "town", "Name")]
    /// 
    /// If you have already aliased the Town as town in a repository method and operatingcenter as criteriaOperatingCenter in the repository
    /// e.g. [SearchAlias("town.OperatingCenter", "criteriaOperatingCenter", "PropertyYouWant")]
    ///
    /// If you have already aliased the Employee as "e" and operatingcenter as "oc" in the repository
    /// e.g. [SearchAlias("e.OperatingCenter", "oc", "IsContractedOperations")]
    ///
    /// If you have a field that depends on the alias from another field, that field may need to be marked as Required.
    /// 
    /// [SearchAlias("Facility", "fac", "Id", Required = true)]
    /// [SearchAlias("fac.OperatingCenter", "Id")]
    ///
    /// </example>
    public class SearchAliasAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// This is the association your entity has with another entity. This path
        /// should be a string representation of how you would get to that entity
        /// from your primary entity. See examples section for class.
        /// </summary>
        public string AssociationPath { get; set; }

        /// <summary>
        /// What you want to refer to this as
        /// e.g. JOIN Towns as T
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// The property on the other class you're searching for. You can't nest properties(ie "Blah.Blah")
        /// EXCEPT if you're calling something that's a foreign key reference(ie "Blah.Id"). 
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Set to true if the alias must always be included with the search, even if the property itself
        /// is not being searched on. This needs to be true if any other search properties depend on this alias.
        /// Defaults to false.
        /// </summary>
        public bool Required { get; set; }

        #endregion

        #region Constructors

        public SearchAliasAttribute(string associationPath, string alias, string property)
        {
            AssociationPath = associationPath;
            Alias = alias;
            Property = property;
        }

        /// <summary>
        /// Creates an association and generates the alias for you.
        /// </summary>
        public SearchAliasAttribute(string associationPath, string property)
        {
            AssociationPath = associationPath;
            Property = property;
            Alias = AssociationPath.Replace(".", "");
        }

        #endregion
    }
}
