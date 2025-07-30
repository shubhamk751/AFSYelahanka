namespace AirForceSchoolYelahanka.Web.ViewModel
{
    public class SectionContentViewModel
    {
        public int Id { get; set; }
        public string SelectedSectionName { get; set; }

        public List<string> AvailableSectionNames { get; set; } = new();
        public List<SectionItemViewModel> Items { get; set; } = new();
    }
}
