using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Project : IEntity
    {
        #region Consts

        public const int MAX_NAME_LENGTH = 25;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Site> Sites { get; protected set; }

        #endregion

        #region Constructor

        public Project()
        {
            Sites = new List<Site>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            // Name is a char field and NHibernate doesn't trim the whitespace off of it.
            return (Name ?? string.Empty).Trim();
        }

        #endregion
    }
}
