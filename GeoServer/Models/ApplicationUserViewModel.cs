using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoServer.Models
{
    public class ApplicationUserViewModel : ApplicationUser
    {
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Email")]
        public override string Email { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Roles")]
        public IList<string> RoleNames { get; set; }
        public static ApplicationUserViewModel CopyToApplicationUserViewModel(ApplicationUser ApplicationUser)
        {
            return new ApplicationUserViewModel
            {
                Id = ApplicationUser.Id,
                Email = ApplicationUser.Email,
                RoleNames = new List<string>()
            };
        }
    }

    public class ApplicationUserIndexPageViewModel
    {
        public IEnumerable<ApplicationUserViewModel> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
