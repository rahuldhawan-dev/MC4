using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Board : IEntity
    {
        #region Consts

        public const int MAX_NAME_LENGTH = 25;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Site Site { get; set; }
        public virtual IList<Sensor> Sensors { get; set; }

        #endregion

        #region Constructor

        public Board()
        {
            Sensors = new List<Sensor>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            // Need to trim Name because it's a char field and Nhibernate doesn't trim the whitespace off.
            return (Name ?? string.Empty).Trim();
        }

        #endregion
    }
}
