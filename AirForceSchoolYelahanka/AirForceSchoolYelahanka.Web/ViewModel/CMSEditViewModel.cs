using System.ComponentModel.DataAnnotations;

namespace AirForceSchoolYelahanka.Web.ViewModel
{
    public class CMSEditViewModel
    {
        public string Page { get; set; } = "Home";
        public List<SingleCmsSectionViewModel> Sections { get; set; } = new();
    }

    public class CmsListItem
    {
        public string Title { get; set; }
        public string? Link { get; set; }
        public string? Date { get; set; }
    }


    public class SingleCmsSectionViewModel
    {
        public string Key { get; set; } = string.Empty;
        public string ContentJson { get; set; } = "{}";
        // Either store as dictionary (for key-value sections)...
        public Dictionary<string, string>? ContentDict { get; set; }

        // ...or as list (for bulleted news/notice/activities)
        public List<CmsListItem>? ContentList { get; set; }
    }

}
