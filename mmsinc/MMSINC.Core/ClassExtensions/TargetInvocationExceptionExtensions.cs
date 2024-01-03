using System;
using System.Reflection;

namespace MMSINC.ClassExtensions
{
    public static class TargetInvocationExceptionExtensions
    {
        public static Exception Unwind(this TargetInvocationException that)
        {
            while (that.InnerException as TargetInvocationException != null)
            {
                that = that.InnerException as TargetInvocationException;
            }

            return that.InnerException;
        }
    }
}
