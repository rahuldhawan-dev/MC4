using System;
using System.Collections.Generic;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CorrectiveOrderProblemCode : IEntity
    {
        #region Constants

        public struct Indices
        {
            public const int OTHER = 24;
        }

        public struct StringLengths
        {
            public const int CODE = 200;
            public const int DESCRIPTION = 200;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }

        public virtual string Display => (new CorrectiveOrderProblemCodeDisplayItem
            {Code = Code, Description = Description}).Display;

        public virtual IList<EquipmentType> EquipmentTypes { get; set; }

        #endregion

        #region Constructors

        public CorrectiveOrderProblemCode()
        {
            EquipmentTypes = new List<EquipmentType>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    [Serializable]
    public class CorrectiveOrderProblemCodeDisplayItem : DisplayItem<CorrectiveOrderProblemCode>
    {
        #region Properties

        public string Code { get; set; }
        public string Description { get; set; }

        public override string Display
        {
            get
            {
                var ret = new StringBuilder(Code);

                if (!string.IsNullOrWhiteSpace(Description))
                {
                    ret.Append(" - ");
                    ret.Append(Description);
                }

                return ret.ToString();
            }
        }

        #endregion
    }
}
