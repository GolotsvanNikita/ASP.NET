using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MusicPortal.Controllers;
using MusicPortal.Resources;

namespace MusicPortal.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;
        public PageViewModel? PageModel { get; set; }
        public string PageAction { get; set; } = "";

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PageModel == null) throw new Exception("PageModel is not set");

            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "Page navigation");

            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination justify-content-center");

            TagBuilder prevItem = CreateTag(PageModel.CurrentPage - 1, Resource.PreviousText, !PageModel.HasPreviousPage, urlHelper);
            ulTag.InnerHtml.AppendHtml(prevItem);

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder pageItem = CreateTag(i, i.ToString(), i == PageModel.CurrentPage, urlHelper);
                ulTag.InnerHtml.AppendHtml(pageItem);
            }

            TagBuilder nextItem = CreateTag(PageModel.CurrentPage + 1, Resource.NextText, !PageModel.HasNextPage, urlHelper);
            ulTag.InnerHtml.AppendHtml(nextItem);

            output.Content.AppendHtml(ulTag);
        }

        private TagBuilder CreateTag(int pageNumber, string linkText, bool isDisabled, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");

            item.AddCssClass("page-item");
            if (isDisabled)
            {
                item.AddCssClass("disabled");
            }

            link.AddCssClass("page-link");
            if (!isDisabled)
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }

            link.InnerHtml.Append(linkText);
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}