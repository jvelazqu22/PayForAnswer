namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPayForAnswerFeefromquestionmodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Questions", "PayAnswerFee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "PayAnswerFee", c => c.Boolean(nullable: false));
        }
    }
}
