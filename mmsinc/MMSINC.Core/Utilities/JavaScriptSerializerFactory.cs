using System.Configuration;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace MMSINC.Utilities
{
    public class JavaScriptSerializerFactory
    {
        public const string CONFIG_SECTION =
            "system.web.extensions/scripting/webServices/jsonSerialization";

        private static int? _maxJsonLength;

        public static int MaxJsonLength => _maxJsonLength ?? (_maxJsonLength = GetMaxJsonLength()).Value;

        private static int GetMaxJsonLength()
        {
            var section = (ScriptingJsonSerializationSection)ConfigurationManager.GetSection(CONFIG_SECTION);
            return section.MaxJsonLength;
        }

        public static JavaScriptSerializer Build()
        {
            return new JavaScriptSerializer {MaxJsonLength = MaxJsonLength};
        }
    }
}
