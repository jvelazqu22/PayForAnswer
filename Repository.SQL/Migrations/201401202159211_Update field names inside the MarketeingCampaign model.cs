namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatefieldnamesinsidetheMarketeingCampaignmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MarketingCampaigns", "Status_Id", "dbo.Status");
            DropIndex("dbo.MarketingCampaigns", new[] { "Status_Id" });
            RenameColumn(table: "dbo.MarketingCampaigns", name: "Status_Id", newName: "StatusId");
            AddColumn("dbo.MarketingCampaigns", "CampaignManagerId", c => c.Int());
            AddForeignKey("dbo.MarketingCampaigns", "StatusId", "dbo.Status", "Id", cascadeDelete: true);
            CreateIndex("dbo.MarketingCampaigns", "StatusId");
            DropColumn("dbo.MarketingCampaigns", "StatudId");
            DropColumn("dbo.MarketingCampaigns", "UserIdThatStartedCampaign");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MarketingCampaigns", "UserIdThatStartedCampaign", c => c.Int());
            AddColumn("dbo.MarketingCampaigns", "StatudId", c => c.Int(nullable: false));
            DropIndex("dbo.MarketingCampaigns", new[] { "StatusId" });
            DropForeignKey("dbo.MarketingCampaigns", "StatusId", "dbo.Status");
            DropColumn("dbo.MarketingCampaigns", "CampaignManagerId");
            RenameColumn(table: "dbo.MarketingCampaigns", name: "StatusId", newName: "Status_Id");
            CreateIndex("dbo.MarketingCampaigns", "Status_Id");
            AddForeignKey("dbo.MarketingCampaigns", "Status_Id", "dbo.Status", "Id");
        }
    }
}
