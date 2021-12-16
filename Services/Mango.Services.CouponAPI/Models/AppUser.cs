namespace Mango.Services.CouponAPI.Models
{
    public class AppUser
    {
        public AppUser()
        {
            Email = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Roles = Array.Empty<string>();
        }

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string[] Roles { get; set; }
    }
}
