namespace AirForceSchoolYelahanka.Web.ViewModel.Home
{
    public class HomePageViewModel
    {
        public Dictionary<string, Dictionary<string, string>> SectionContent { get; set; } = new();
        public List<NewsItem> LatestNews { get; set; } = new();
        public List<NoticeItem> NoticeBoard { get; set; } = new();
        public List<ActivityItem> RecentActivities { get; set; } = new();
    }
    public class NewsItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
    }

    public class NoticeItem
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }

    public class ActivityItem
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}


