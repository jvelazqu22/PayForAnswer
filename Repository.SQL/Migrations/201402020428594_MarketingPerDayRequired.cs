namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarketingPerDayRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "MarketingBudgetPerDay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Questions", "NumberOfCampaignDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "NumberOfCampaignDays", c => c.Int());
            AlterColumn("dbo.Questions", "MarketingBudgetPerDay", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
