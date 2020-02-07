using Microsoft.AspNetCore.Identity;

namespace IdentityDemo
{
    public class Organization
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class PluralSightUser : IdentityUser
    {
        public string Locale { get; set; } = "en-GB";

        public string OrganizationId { get; set; }
    }
}
