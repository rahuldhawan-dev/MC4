using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class State : IEntityLookup
    {
        #region Structs

        public struct MaxLengths
        {
            public const int NAME = 50,
                             ABBREVIATION = 50,
                             SCADA_TBL = 50;
        }

        public struct Indices
        {
            public const int NJ = 1,
                             NY = 2,
                             PA = 3,
                             IL = 4,
                             CA = 5,
                             FL = 6,
                             HI = 7,
                             IA = 8,
                             IN = 9,
                             KY = 10,
                             MD = 11,
                             MO = 12,
                             TN = 13,
                             VA = 14,
                             WV = 15,
                             WA = 20,
                             OT = 21,
                             GA = 22,
                             AL = 23,
                             AK = 24,
                             AZ = 25,
                             AR = 26,
                             CO = 27,
                             CT = 28,
                             DE = 29,
                             ID = 30,
                             KS = 31,
                             LA = 32,
                             ME = 33,
                             MA = 34,
                             MI = 35,
                             MN = 36,
                             MS = 37,
                             MT = 38,
                             NE = 39,
                             NV = 40,
                             NH = 41,
                             NM = 42,
                             NC = 43,
                             ND = 44,
                             OH = 45,
                             OK = 46,
                             OR = 47,
                             RI = 48,
                             SC = 49,
                             SD = 50,
                             TX = 51,
                             UT = 52,
                             VT = 53,
                             WI = 54,
                             WY = 55;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description => Abbreviation;

        public virtual string Name { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual string ScadaTable { get; set; }

        public virtual IList<County> Counties { get; set; }
        public virtual IList<Town> Towns { get; set; }
        public virtual IList<StateWorkDescription> WorkDescriptionOverrides { get; set; }

        public static int[] AllStateIndicesValues() => typeof(State).GetFields()
                                                                    .Select(x => (int)x.GetRawConstantValue())
                                                                    .ToArray();

        #endregion

        #region Constructors

        public State()
        {
            Counties = new List<County>();
            Towns = new List<Town>();
            WorkDescriptionOverrides = new List<StateWorkDescription>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Abbreviation;
        }

        #endregion
    }
}
