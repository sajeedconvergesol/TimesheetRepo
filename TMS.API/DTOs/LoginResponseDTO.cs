﻿namespace TMS.API.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
    }
}
