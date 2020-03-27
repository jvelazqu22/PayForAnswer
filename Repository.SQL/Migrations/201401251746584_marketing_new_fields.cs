namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class marketing_new_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MarketingCampaigns", "StartingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MarketingCampaigns", "UsedBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MarketingCampaigns", "QuestionAmountIncrease", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MarketingCampaigns", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.MarketingCampaigns", "UpdatedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.MarketingCampaigns", "TotalBudget");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MarketingCampaigns", "TotalBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MarketingCampaigns", "UpdatedOn");
            DropColumn("dbo.MarketingCampaigns", "CreatedOn");
            DropColumn("dbo.MarketingCampaigns", "QuestionAmountIncrease");
            DropColumn("dbo.MarketingCampaigns", "UsedBudget");
            DropColumn("dbo.MarketingCampaigns", "StartingBudget");
        }
    }
}
