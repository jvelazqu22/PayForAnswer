namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarketingQuestionTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MarketingCampaigns", "QuestionTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MarketingCampaigns", "QuestionTitle");
        }
    }
}
