﻿using System.ComponentModel.DataAnnotations;

namespace AirForceSchoolYelahanka.Web.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
