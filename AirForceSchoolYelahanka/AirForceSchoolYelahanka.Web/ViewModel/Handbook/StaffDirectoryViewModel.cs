using AirForceSchoolYelahanka.Web.Models;

namespace AirForceSchoolYelahanka.Web.ViewModel.Handbook
{
    public class StaffDirectoryViewModel
    {
        public Dictionary<string, List<Staff>> StaffGroupedByRole { get; set; }
        public Dictionary<string, int> StaffCountByRole { get; set; }
    }
}
