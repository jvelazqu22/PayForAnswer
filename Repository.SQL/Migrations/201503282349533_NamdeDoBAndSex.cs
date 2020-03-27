namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NamdeDoBAndSex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "DoB", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "IsFemale", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsFemale");
            DropColumn("dbo.AspNetUsers", "DoB");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
    }
}
