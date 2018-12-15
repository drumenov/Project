using Microsoft.AspNetCore.Identity;

namespace Project.Models.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base () { }

        public AppRole(string name) : base(name) { }
    }
}
