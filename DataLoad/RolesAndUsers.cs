using Domain.Models.Entities.Identity;
using Repository.SQL;
using Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Domain.Constants;
using System.Collections.Generic;
using System.Linq;
using Domain.Models.Entities;

namespace DataLoad
{
    public class RolesAndUsers
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public RolesAndUsers(PfaDb context)
        {
            _userManager = new ApplicationUserManager(new ApplicationUserStore(context));
            _roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

        }

        public void AddDefaultData()
        {
            AddRoles();
            AddTestUsers();
            ApplicationUser user = _userManager.FindByName("jvelazquez22g");
            ApplicationRole role = _roleManager.FindByName(Role.Admin);
            AddUsersToRoles(user, role);
            List<string> userRoles = _userManager.GetRoles(user.Id).ToList();
        }

        public void AddRoles()
        {
            foreach(var roleName in Role.UserRoleList)
            {
                //Create Role Admin if it does not exist
                var role = _roleManager.FindByName<ApplicationRole, int>(roleName);
                if (role == null)
                {
                    role = new ApplicationRole(roleName);
                    var roleresult = _roleManager.Create(role);
                }
            }
        }   

        public List<ApplicationUser> GetUsersToBeAdded()
        {
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    UserName = "jvelazquez22g", Email = "jvelazqu22@gmail.com", NewEmail = "",  EmailConfirmed = true,
                    AcceptedTermsConditionsAndPrivacyPolicy = true, Notifications = new Notifications()
                },
                new ApplicationUser()
                {
                    UserName = "jvelazquez22h", Email = "jvelazqu22@hotmail.com", NewEmail = "",  EmailConfirmed = true,
                    AcceptedTermsConditionsAndPrivacyPolicy = true, Notifications = new Notifications()
                },
                new ApplicationUser()
                {
                    UserName = "nalimj", Email = "nalimj@hotmail.com", NewEmail = "",  EmailConfirmed = true,
                    AcceptedTermsConditionsAndPrivacyPolicy = true, Notifications = new Notifications()
                },
                new ApplicationUser()
                {
                    UserName = "rvelazq1", Email = "rvelazq1@hotmail.com", NewEmail = "", EmailConfirmed = true,
                    AcceptedTermsConditionsAndPrivacyPolicy = true, Notifications = new Notifications()
                },
                new ApplicationUser()
                {
                    UserName = "mvelazq1", Email = "pfamartinez1@gmail.com", NewEmail = "", EmailConfirmed = true,
                    AcceptedTermsConditionsAndPrivacyPolicy = true, Notifications = new Notifications()
                },
            };
            return users;
        }

        public void AddTestUsers()
        {
            List<ApplicationUser> usersToBeAdded = GetUsersToBeAdded();
            foreach(var userToAdd in usersToBeAdded)
            {
                var user = _userManager.FindByName<ApplicationUser, int>(userToAdd.UserName);
                if (user == null)
                {
                    var result = _userManager.Create(userToAdd, "P@ssword1");
                    result = _userManager.SetLockoutEnabled(userToAdd.Id, true);
                }
            }
        }

        public void AddUsersToRoles(ApplicationUser user, ApplicationRole role)
        {
            var rolesForUser = _userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = _userManager.AddToRole<ApplicationUser,int>(user.Id, role.Name);
            }
        }
    }
}
