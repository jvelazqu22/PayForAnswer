namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignManager : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MarketingCampaigns", "User_UserId", "dbo.UserProfile");
            DropIndex("dbo.MarketingCampaigns", new[] { "User_UserId" });
            AddColumn("dbo.MarketingCampaigns", "CampaignManager_UserId", c => c.Int());
            AddForeignKey("dbo.MarketingCampaigns", "CampaignManager_UserId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.MarketingCampaigns", "CampaignManager_UserId");
            DropColumn("dbo.MarketingCampaigns", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MarketingCampaigns", "User_UserId", c => c.Int());
            DropIndex("dbo.MarketingCampaigns", new[] { "CampaignManager_UserId" });
            DropForeignKey("dbo.MarketingCampaigns", "CampaignManager_UserId", "dbo.UserProfile");
            DropColumn("dbo.MarketingCampaigns", "CampaignManager_UserId");
            CreateIndex("dbo.MarketingCampaigns", "User_UserId");
            AddForeignKey("dbo.MarketingCampaigns", "User_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
