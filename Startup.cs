using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SimplyAds.Models;

[assembly: OwinStartupAttribute(typeof(SimplyAds.Startup))]
namespace SimplyAds
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesAndUser();            
        }
        
        private void createRolesAndUser()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var role_ = new IdentityRole();
                role_.Name = "User";
                roleManager.Create(role_);

                var user = new ApplicationUser();
                user.UserName = "admin@ads.com";
                user.Email = "admin@ads.com";
                string pwd = "p@$$w0rd1";                                           
                //string pwd = "password";
                var chkUser = userManager.Create(user, pwd);
                if (chkUser.Succeeded)
                    userManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
