using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    public class CheckBoxListBuilder : ListBuilder<CheckBoxListBuilder>
    {
        protected override string CreateHtmlString()
        {
            // NOTE: EmptyText property is not used for CheckBoxLists.
            // NOTE: This doesn't(or shouldn't) work with nullable ints at the moment.

            var wrapperDiv = CreateTagBuilder("div", false);
            wrapperDiv.AddCssClass("checkbox-list");

            // CreateTagBuilder automatically adds the name attribute,
            // but we don't want that on the wrapper div, only the inputs.
            wrapperDiv.Attributes.Remove("name");

            var listItemBuilder = new StringBuilder();
            // Cache these so we're not hitting the dictionary a million times.
            var name = Name;

            // jQuery validation can't work with divs or anything besides typical
            // form inputs. So we need a hidden input with the unobtrusive validation
            // attributes in order to wire into that. We need to use a different name,
            // otherwise the empty value will get posted back to the server and cause 
            // issues during model binding. The client-side aspect is handled by
            // a hacky override of the elementValue function in jquery.validate.unobtrusive.fix.
            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("name", name + "_CheckBoxList");
            hidden.AddCssClass("dummy-check-box-list-input");
            hidden.MergeAttributes(GetUnobtrusiveHtmlAttributes());
            listItemBuilder.Append(hidden.ToString(TagRenderMode.SelfClosing));

            foreach (var item in GetItems())
            {
                var cbli = new CheckBoxListItemBuilder();
                cbli.Checked = item.Selected;
                cbli.Name = name;
                cbli.Text = item.Text;
                cbli.Value = item.Value;

                listItemBuilder.Append(cbli.ToHtmlString());
            }

            wrapperDiv.InnerHtml = listItemBuilder.ToString();
            return wrapperDiv.ToString(TagRenderMode.Normal);
        }
    }
}
