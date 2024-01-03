using System.Data;

namespace MMSINC.ClassExtensions
{
    public static class IDbCommandExtensions
    {
        public static void AddParameter(this IDbCommand comm, string paramName, object value)
        {
            var p = comm.CreateParameter();
            p.ParameterName = paramName;
            p.Value = value;
            comm.Parameters.Add(p);
        }

        public static void ExecuteNonQuery(this IDbCommand that, string command)
        {
            that.CommandText = command;
            that.ExecuteNonQuery();
        }
    }
}
