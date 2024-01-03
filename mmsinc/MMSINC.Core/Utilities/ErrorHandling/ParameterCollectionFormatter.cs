using System.Linq;
using System.Text;
using MMSINC.Interface;

namespace MMSINC.Utilities.ErrorHandling
{
    public class ParameterCollectionFormatter : IParameterCollectionFormatter
    {
        #region Public Methods

        public string FormatParameters(IRequest request, string indentString, string lineSeparator,
            params string[] keysToSkip)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}Http Verb: {1}", indentString,
                request.HttpMethod);
            foreach (var key in request.Params.AllKeys)
            {
                if (!keysToSkip.Contains(key))
                {
                    sb.AppendFormat("{0}{1}{2}: {3}", lineSeparator,
                        indentString,
                        key, request.Params[key]);
                }
            }

            return sb.ToString();
        }

        #endregion
    }

    public interface IParameterCollectionFormatter
    {
        #region Methods

        string FormatParameters(IRequest request, string indentString, string lineSeparator,
            params string[] keysToSkip);

        #endregion
    }
}
