namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionBeforeIncrease : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MarketingCampaigns", "QuestionAmountBeforeIncrease", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MarketingCampaigns", "QuestionAmountBeforeIncrease");
        }
    }
}
