using System.ComponentModel.DataAnnotations;

namespace AirForceSchoolYelahanka.Web.Models
{
    public class CmsSection
    {
        public int Id { get; set; }

        [Required]
        public string SectionName { get; set; } = "";

        [Required]
        public string ContentJson { get; set; } = "";
    }
}
