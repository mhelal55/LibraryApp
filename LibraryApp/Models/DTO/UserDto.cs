﻿namespace LibraryApp.Models.DTO
{
    public class UserDto
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
