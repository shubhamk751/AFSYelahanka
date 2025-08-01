using System.ComponentModel.DataAnnotations;

namespace AirForceSchoolYelahanka.Web.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;

    }
}
