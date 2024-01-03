using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentCharacteristicFieldType : IEntityLookup
    {
        #region Constants

        public struct DataTypes
        {
            public const string DROPDOWN = "DropDown";
        }
        
        #endregion
        
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [Required]
        public virtual string DataType { get; set; }

        public virtual string Regex { get; set; }

        #endregion

        #region Logical Properties

        public virtual string Description => DataType;

        public virtual Regex RegexObj => String.IsNullOrWhiteSpace(Regex) ? null : new Regex(Regex);

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return DataType;
        }

        #endregion
    }
}
