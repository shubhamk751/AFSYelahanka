namespace AirForceSchoolYelahanka.Web.Config
{
    public static class CmsPages
    {
        public static readonly Dictionary<string, List<string>> PageSections = new()
        {
            { "Home", new List<string> { "HomePage_LatestNews", "HomePage_NoticeBoard", "HomePage_VideoGallery" } },
            { "CCA", new List<string> { "CCA.Plantation", "CCA.Diwali", "CCA.CCA","CCA.Career Counselling Orientation" } }
        };
    }
}
