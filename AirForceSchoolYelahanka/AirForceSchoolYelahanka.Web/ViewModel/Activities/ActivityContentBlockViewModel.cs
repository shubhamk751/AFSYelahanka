namespace AirForceSchoolYelahanka.Web.ViewModel.Activities
{
    public class ActivityContentBlockViewModel
    {
        public string Title { get; set; }
        public string HtmlMainContent { get; set; }
        public string HtmlSidebarContent { get; set; }
        public List<string> SidebarImageUrls { get; set; } = new();
    }
}
