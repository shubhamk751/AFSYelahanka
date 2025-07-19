namespace AirForceSchoolYelahanka.Web.ViewModel.Home
{
    public class HomePageViewModel
    {
        public Dictionary<string, Dictionary<string, string>> SectionContent { get; set; } = new();
        public List<NewsItem> LatestNews { get; set; } = new();
        public List<NoticeItem> NoticeBoard { get; set; } = new();
        public List<ActivityItem> RecentActivities { get; set; } = new();
    }

    public class HomeInsertGeneric
    {
        public string Title { get; set; }
        
        public DateTime Date { get; set; }
    }
    public class NewsItem : HomeInsertGeneric
    {
        public string Link { get; set; }
    }

    public class NoticeItem : HomeInsertGeneric
    {
    }

    public class ActivityItem : HomeInsertGeneric
    {
    }
}


