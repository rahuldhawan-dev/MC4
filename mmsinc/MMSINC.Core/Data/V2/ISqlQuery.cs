using System;
using System.Collections;
using System.Collections.Generic;

namespace MMSINC.Data.V2
{
    public interface ISqlQuery
    {
        int ExecuteUpdate();
        T UniqueResult<T>();
        int? SafeUniqueIntResult();
        IEnumerable<T> Enumerable<T>();

        ISqlQuery SetString(string name, string value);
        ISqlQuery SetInt32(string name, int value);
        ISqlQuery SetParameterList(string name, IEnumerable vals);
        ISqlQuery AddScalar(string columnAlias, Type type);
    }
}
