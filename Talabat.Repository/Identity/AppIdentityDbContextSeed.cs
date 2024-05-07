using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Mohamed Ahmed",
                    Email = "mohamed.ahmed@gmail.com",
                    UserName = "mohamed.ahmed",
                    PhoneNumber = "01004877992"
                };


                await _userManager.CreateAsync(user, "Pa$$w0rd");
            }
            
        }
    }
}
