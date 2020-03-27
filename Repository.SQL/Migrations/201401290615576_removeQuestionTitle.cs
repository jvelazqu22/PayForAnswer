namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeQuestionTitle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MarketingCampaigns", "QuestionTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MarketingCampaigns", "QuestionTitle", c => c.String());
        }
    }
}
