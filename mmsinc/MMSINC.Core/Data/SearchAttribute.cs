using System;

namespace MMSINC.Data
{
    // TODO: Move SearchAlias stuff to this attribute so everything can be contained in one spot.

    /// <summary>
    /// Attribute for describing things about a property on a search model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets/sets whether this property can be directly mapped from a search model 
        /// to an entity. ex: A logical property. Defaults to true.
        /// </summary>
        public bool CanMap { get; set; }

        /// <summary>
        /// Set to true if searching on this property is used for checking if 
        /// a collection does or does not have items.
        /// </summary>
        public bool ChecksExistenceOfChildCollection { get; set; }

        #endregion

        #region Constructor

        public SearchAttribute()
        {
            CanMap = true;
        }

        #endregion
    }
}
