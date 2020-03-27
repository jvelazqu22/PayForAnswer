namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PerDayMarketing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "MarketingBudgetPerDay", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Questions", "NumberOfCampaignDays", c => c.Int());
            AddColumn("dbo.MarketingCampaigns", "NumberOfDaysToRun", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MarketingCampaigns", "UsedBudget", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.MarketingCampaigns", "PerDayBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Questions", "MarketingBudget");
            DropColumn("dbo.MarketingCampaigns", "StartingBudget");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MarketingCampaigns", "StartingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Questions", "MarketingBudget", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.MarketingCampaigns", "PerDayBudget", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.MarketingCampaigns", "UsedBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MarketingCampaigns", "NumberOfDaysToRun");
            DropColumn("dbo.Questions", "NumberOfCampaignDays");
            DropColumn("dbo.Questions", "MarketingBudgetPerDay");
        }
    }
}
