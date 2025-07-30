namespace AirForceSchoolYelahanka.Web.ViewModel.Activities
{
    public class ActivityContentBlockViewModel
    {
        public string? Division { get; set; }
        public string? Type { get; set; }
        public string? Slug { get; set; }
        public string? Title { get; set; }
        public string? HtmlMainContent { get; set; }
        public string? HtmlSidebarContent { get; set; }
        public List<string>  SidebarImageUrls { get; set; } = new();
        public string? Link { get; set; }
        public DateTime? Date { get; set; }
    }
}
