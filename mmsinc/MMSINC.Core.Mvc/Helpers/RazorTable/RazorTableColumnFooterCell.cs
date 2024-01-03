using System.Web.Mvc;

namespace MMSINC.Helpers.RazorTable
{
    // It would be nice to come back and add a way for the footer cell
    // to automatically calculate and display the value along with formatting
    // the value. However, trying to make that work in a consistently implemented
    // way that allows us to do both a) just set a static value like we're doing now and 
    // b) allow us to do computations is really complicated and I'd need to spend
    // all day figuring out how to make that work.

    public interface IRazorTableColumnFooterCell
    {
        object Value { get; set; }

        TagBuilder RenderCell();
    }

    public class RazorTableColumnFooterCell : IRazorTableColumnFooterCell
    {
        public object Value { get; set; }

        public TagBuilder RenderCell()
        {
            var tagBuilder = new TagBuilder("td");
            // This might not make sense and it might make more sense to have a GetRenderedValue override or something.
            // That would return a string and handle any formatting.
            tagBuilder.InnerHtml = Value?.ToString();
            return tagBuilder;
        }
    }
}
