using Domain.Models.Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Repository.SQL;

namespace Utilities
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole, int>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, int> roleStore) : base(roleStore) { }

        public static ApplicationRoleManager Create( IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager( new ApplicationRoleStore(context.Get<PfaDb>()) );
        }
    }
}
