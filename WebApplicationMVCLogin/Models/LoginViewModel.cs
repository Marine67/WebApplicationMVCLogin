﻿using System.ComponentModel.DataAnnotations;

namespace WebApplicationMVCLogin.Models
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
