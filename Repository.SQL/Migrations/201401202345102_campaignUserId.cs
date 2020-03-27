namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campaignUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MarketingCampaigns", "CampaignManager_UserId", "dbo.UserProfile");
            DropIndex("dbo.MarketingCampaigns", new[] { "CampaignManager_UserId" });
            AddColumn("dbo.MarketingCampaigns", "UserId", c => c.Int());
            AddForeignKey("dbo.MarketingCampaigns", "UserId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.MarketingCampaigns", "UserId");
            DropColumn("dbo.MarketingCampaigns", "CampaignManagerId");
            DropColumn("dbo.MarketingCampaigns", "CampaignManager_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MarketingCampaigns", "CampaignManager_UserId", c => c.Int());
            AddColumn("dbo.MarketingCampaigns", "CampaignManagerId", c => c.Int());
            DropIndex("dbo.MarketingCampaigns", new[] { "UserId" });
            DropForeignKey("dbo.MarketingCampaigns", "UserId", "dbo.UserProfile");
            DropColumn("dbo.MarketingCampaigns", "UserId");
            CreateIndex("dbo.MarketingCampaigns", "CampaignManager_UserId");
            AddForeignKey("dbo.MarketingCampaigns", "CampaignManager_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
