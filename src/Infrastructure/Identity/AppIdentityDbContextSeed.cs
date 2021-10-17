using Core.Entities.Identitiy;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "John",
                    Email = "johndoe@test.com",
                    UserName = "johndoe@test.com",
                    Address = new Address
                    {
                        FirstName = "John",
                        LastName = "doe",
                        Street = "10 the street",
                        City = "New York",
                        State = "NY",
                        ZipCode = "90210"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$$w0rd");
            }
        }
    }
}