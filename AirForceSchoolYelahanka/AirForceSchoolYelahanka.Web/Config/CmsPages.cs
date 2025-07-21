namespace AirForceSchoolYelahanka.Web.Config
{
    public static class CmsPages
    {
        public static readonly Dictionary<string, List<string>> PageSections = new()
        {
            { "Home", new List<string> { "HomePage_LatestNews", "HomePage_NoticeBoard", "HomePage_RecentActivity" } },
            { "CCA-Junior-Wing", new List<string> { "CCA.Junior.Plantation", "CCA.Junior.Diwali", "CCA.Junior.CCA" } },
            { "CCA-Senior-Wing", new List<string> { "CCA.Senior.Career Counselling Orientation", "CCA.Senior.Diwali", "CCA.Senior.CCA" } },
            { "Contact", new List<string> { "ContactHeader", "ContactFormInfo" } },
        };
    }
}
