using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identitiy
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}