namespace AirForceSchoolYelahanka.Web.ViewModel
{
    public class CmsContentFormViewModel
    {
        public string ItemKey { get; set; } = string.Empty;
        public IEnumerable<string> AvailableItemKeys { get; set; } = Enumerable.Empty<string>();

        public string Title { get; set; } = string.Empty;
        public string HtmlMainContent { get; set; } = string.Empty;
        public string HtmlSidebarContent { get; set; } = string.Empty;
        public List<string> SidebarImageUrls { get; set; } = new();
    }
}
