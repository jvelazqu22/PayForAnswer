namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intNumberOfDaysToRun : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MarketingCampaigns", "NumberOfDaysToRun", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MarketingCampaigns", "NumberOfDaysToRun", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
