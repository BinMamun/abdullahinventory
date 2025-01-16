using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.UserModels
{
    public class UserUpdateModel
    {
        public Guid Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        public IList<ApplicationRole>? Roles { get; set; }
        public List<string> UserRoles { get; set; }
        public List<Permission> Permissions { get; set; }
        
        public List<Permission> GetUserPermisssions(IList<Claim> userClaims)
        {
            var permissions = new List<Permission>
            {
                new() { ClaimType = "Can View List?", 
                    ClaimValue = userClaims
                    .Any(c => c.Type.Equals("Can View List?") && c.Value.Equals("true")) },

                new() { ClaimType = "Can Create?", 
                    ClaimValue = userClaims
                    .Any(c => c.Type.Equals("Can Create?") && c.Value.Equals("true")) },

                new() { ClaimType = "Can Edit?",
                    ClaimValue = userClaims
                    .Any(c => c.Type.Equals("Can Edit?") && c.Value.Equals("true")) },
                
                new() { ClaimType = "Can Delete?",
                    ClaimValue = userClaims
                    .Any(c => c.Type.Equals("Can Delete?") && c.Value.Equals("true")) },

                new() { ClaimType = "Can DeleteAll?",
                    ClaimValue = userClaims
                    .Any(c => c.Type.Equals("Can DeleteAll?") && c.Value.Equals("true")) }
            };
            
            return permissions;
        }
    }
}
