namespace AirForceSchoolYelahanka.Web.Config
{
    public static class CmsPages
    {
        public static readonly Dictionary<string, List<string>> PageSections = new()
        {
            { "Home", new List<string> { "HomePageSection1", "HomePageSection2" } },
            { "About", new List<string> { "AboutPageIntro", "AboutPageTeam" } },
            { "Contact", new List<string> { "ContactHeader", "ContactFormInfo" } },
        };
    }
}
