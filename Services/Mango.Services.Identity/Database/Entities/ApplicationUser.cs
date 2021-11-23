using Microsoft.AspNetCore.Identity;

namespace Mango.Services.Identity.Database.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
