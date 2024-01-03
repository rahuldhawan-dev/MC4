using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities;

namespace MMSINC.ClassExtensions
{
    // TODO: Move this to ControlBuilder and ControlHelper format.
    public static class ComboBoxExtensions
    {
        public const string COMBOBOX_PLUGIN_NAME = "multilist";

        public static IHtmlString ComboBox(this HtmlHelper helper, string expression,
            IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return helper.SelectInternal(null, optionLabel, expression, selectList, htmlAttributes);
        }

        private static IHtmlString SelectInternal(this HtmlHelper helper, ModelMetadata metadata, string optionLabel,
            string expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            selectList.ThrowIfNull("selectList");
            var fullName = helper.Name(expression).ToString();
            fullName.ThrowIfNullOrWhiteSpace("expression");
            var id = helper.Id(expression).ToString();
            metadata = metadata ?? ModelMetadata.FromStringExpression(expression, helper.ViewData);

            var targetDiv = new TagBuilder("div");
            targetDiv.MergeAttributes(htmlAttributes);
            targetDiv.MergeAttribute("name", fullName);
            targetDiv.GenerateId(fullName);

            var script = new StringBuilder("<script>");
            script.Append("$(document).ready(function() {");
            script.AppendFormat("var comboBox = $('#{0}').{1}({{", id, COMBOBOX_PLUGIN_NAME);
            script.AppendFormat("single: true, labelText: '{0}',", optionLabel);
            script.AppendFormat("datalist: {0}", JavaScriptSerializerFactory.Build().Serialize(selectList));
            script.Append("});\n");
            script.Append("$.validator.unobtrusive.parseDynamicContent(comboBox.get(0));\n");
            script.Append("});");
            script.Append("</script>");

            helper.InlineScript(fullName, script.ToString());

            targetDiv.MergeAttributes(helper.GetUnobtrusiveValidationAttributes(fullName, metadata));

            return new HtmlString(targetDiv.ToString(TagRenderMode.Normal));
        }
    }
}
