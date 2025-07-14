using System.ComponentModel.DataAnnotations;

namespace AirForceSchoolYelahanka.Web.ViewModel
{
    public class CMSEditViewModel
    {
        public string Page { get; set; } = "Home";
        public List<SingleCmsSectionViewModel> Sections { get; set; } = new();
    }


    public class SingleCmsSectionViewModel
    {
        public string Key { get; set; } = string.Empty;
        public string ContentJson { get; set; } = "{}";
    }

}
