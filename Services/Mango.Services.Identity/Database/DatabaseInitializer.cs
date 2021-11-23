using IdentityModel;
using Mango.Services.Identity.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using System.Security.Claims;

namespace Mango.Services.Identity.Initializer
{
    public class DatabaseInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DatabaseInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Initialize()
        {
            var adminRole = _roleManager.FindByNameAsync(RoleConstants.Admin);
            if (adminRole.Result == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Admin));
                await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Customer));
            }
            else
            {
                return;
            }

            var adminUser = new ApplicationUser
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111111",
                FirstName = "Ben",
                LastName = "Admin"
            };

            await _userManager.CreateAsync(adminUser, "Asd123!1");
            await _userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            var adminClaims = await _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName + " " + adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, RoleConstants.Admin),
            });

            var customerUser = new ApplicationUser
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111111",
                FirstName = "Ben",
                LastName = "Customer"
            };

            await _userManager.CreateAsync(customerUser, "Asd123!1");
            await _userManager.AddToRoleAsync(customerUser, RoleConstants.Customer);
            var customerClaims = await _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName + " " + adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, RoleConstants.Customer),
            });
        }
    }
}
