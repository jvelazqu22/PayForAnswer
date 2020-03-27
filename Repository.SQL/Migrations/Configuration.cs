using Domain.Constants;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;

namespace Repository.SQL.Migrations
{
    public class Configuration : DbMigrationsConfiguration<PfaDb> 
    {
        public Configuration() { AutomaticMigrationsEnabled = true; }

        protected override void Seed(Repository.SQL.PfaDb context)
        {
        }
    }
}
