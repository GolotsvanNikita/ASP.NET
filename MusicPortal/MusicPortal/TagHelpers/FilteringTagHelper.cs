using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MusicPortal.Controllers;
using MusicPortal.Models;

namespace MusicPortal.TagHelpers
{
    [HtmlTargetElement("filtering")]
    public class FilteringTagHelper : TagHelper
    {
        [HtmlAttributeName("property")]
        public SortState Property { get; set; }

        [HtmlAttributeName("current")]
        public SortState Current { get; set; }

        [HtmlAttributeName("action")]
        public string? Action { get; set; } = "Index";

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        private readonly IUrlHelperFactory _urlHelperFactory;

        public FilteringTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext == null)
            {
                return;
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            int currentPage = 1;
            string searchString = string.Empty;
            if (ViewContext.ViewData.Model is IndexViewModel indexModel)
            {
                if (indexModel.PageViewModel != null)
                {
                    currentPage = indexModel.PageViewModel.CurrentPage;
                }
                searchString = indexModel.SearchString ?? string.Empty;
            }

            SortState desc = GetDesc(Property);
            bool isCurrentColumn = Current == Property || Current == desc;
            bool isUp = Current == Property;

            SortState nextSortOrder = isCurrentColumn ? GetOpposite(Current) : Property;

            var routeValues = new { page = currentPage, sortOrder = nextSortOrder, searchString = searchString };
            var url = urlHelper.Action(Action, routeValues);

            output.TagName = "a";
            output.Attributes.SetAttribute("href", url ?? "#");

            if (isCurrentColumn)
            {
                output.Attributes.SetAttribute("class", "sort-active");

                var icon = new TagBuilder("i")
                {
                    TagRenderMode = TagRenderMode.EndTag
                };
                icon.AddCssClass("glyphicon");
                icon.AddCssClass(isUp ? "glyphicon-chevron-up" : "glyphicon-chevron-down");
                output.PostContent.AppendHtml(" ");
                output.PostContent.AppendHtml(icon);
            }
        }

        private SortState GetDesc(SortState asc)
        {
            return asc switch
            {
                SortState.NameAsc => SortState.NameDesc,
                SortState.DurationAsc => SortState.DurationDesc,
                _ => asc
            };
        }

        private SortState GetOpposite(SortState state)
        {
            return state switch
            {
                SortState.NameAsc => SortState.NameDesc,
                SortState.NameDesc => SortState.NameAsc,
                SortState.DurationAsc => SortState.DurationDesc,
                SortState.DurationDesc => SortState.DurationAsc,
                _ => state
            };
        }
    }
}