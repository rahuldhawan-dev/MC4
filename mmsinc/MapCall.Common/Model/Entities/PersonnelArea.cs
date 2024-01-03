using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PersonnelArea : IEntity
    {
        #region Consts

        public const int MAX_DESCRIPTION_LENGTH = CreatePersonnelAreaTableBug2028.MAX_DESCRIPTION_LENGTH;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }

        /// <summary>
        /// SAP value I guess.
        /// </summary>
        [DisplayName("Personnel Area Id")]
        public virtual int PersonnelAreaId { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
