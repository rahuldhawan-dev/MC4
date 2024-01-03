using System;
using System.Collections.Generic;

namespace MMSINC.Helpers
{
    internal static class ChartBuilderUtility
    {
        // This exists so a hundred copies of this dictionary aren't made for every single 
        // possible generic combination that ChartBuilder<X, Y> can be made.
        private static readonly Dictionary<TypeCode, string> _convertedValueTypeClientParameters;

        static ChartBuilderUtility()
        {
            const string INT = "int",
                         FLOAT = "float",
                         DATE = "date",
                         STRING = "string";

            _convertedValueTypeClientParameters = new Dictionary<TypeCode, string> {
                {TypeCode.Int16, INT},
                {TypeCode.Int32, INT},
                {TypeCode.Int64, INT},
                {TypeCode.Byte, INT},
                {TypeCode.SByte, INT},
                {TypeCode.UInt16, INT},
                {TypeCode.UInt32, INT},
                {TypeCode.UInt64, INT},
                {TypeCode.Decimal, FLOAT},
                {TypeCode.Single, FLOAT},
                {TypeCode.Double, FLOAT},
                {TypeCode.DateTime, DATE},
                {TypeCode.String, STRING}
            };
        }

        public static string GetJavascriptTypeCode(TypeCode typeCode)
        {
            return _convertedValueTypeClientParameters[typeCode];
        }
    }
}
