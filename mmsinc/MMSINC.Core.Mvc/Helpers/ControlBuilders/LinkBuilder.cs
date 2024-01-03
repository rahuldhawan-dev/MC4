using System;

namespace MMSINC.Helpers
{
    public class LinkBuilder : ControlBuilder<LinkBuilder>
    {
        #region Properties

        public string Text { get; protected set; }

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            var link = CreateTagBuilder("a", false);

            link.SetInnerText(Text);

            foreach (var key in HtmlAttributes.Keys)
            {
                link.Attributes[key] = HtmlAttributes[key].ToString();
            }

            return link.ToString();
        }

        #endregion

        #region Exposed Methods

        public LinkBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        public LinkBuilder WithHref(string url)
        {
            return With("href", url);
        }

        public override LinkBuilder WithName(string name)
        {
            throw new InvalidOperationException("The name attribute for anchor tags is unsupported.");
        }

        #endregion
    }
}
