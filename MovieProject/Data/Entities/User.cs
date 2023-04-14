﻿using System.ComponentModel.DataAnnotations;

namespace MovieProject.Data.Entities
{
    public class User
    {
        public User()
        {
            this.UserId = Guid.NewGuid().ToString();
        }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
