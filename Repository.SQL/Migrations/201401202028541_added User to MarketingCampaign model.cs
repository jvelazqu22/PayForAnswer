namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUsertoMarketingCampaignmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MarketingCampaigns", "User_UserId", c => c.Int());
            AddForeignKey("dbo.MarketingCampaigns", "User_UserId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.MarketingCampaigns", "User_UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MarketingCampaigns", new[] { "User_UserId" });
            DropForeignKey("dbo.MarketingCampaigns", "User_UserId", "dbo.UserProfile");
            DropColumn("dbo.MarketingCampaigns", "User_UserId");
        }
    }
}
