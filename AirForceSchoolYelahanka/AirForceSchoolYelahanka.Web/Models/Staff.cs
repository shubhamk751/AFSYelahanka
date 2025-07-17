namespace AirForceSchoolYelahanka.Web.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Qualification { get; set; }
        public string? RoleCategory { get; set; } // e.g., "PGT", "TGT", "Helpers", or null for Principal
    }
}
