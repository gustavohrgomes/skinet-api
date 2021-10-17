using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Response
{
    public class UserResponse
    {
        public UserResponse(string email, string displayName, string token)
        {
            Email = email;
            DisplayName = displayName;
            Token = token;
        }

        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
    }
}
