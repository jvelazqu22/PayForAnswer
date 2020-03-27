namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedByUpdatedBy : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MarketingCampaigns", name: "UserId", newName: "CampaignManagerId");
            AddColumn("dbo.MarketingCampaigns", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.MarketingCampaigns", "UpdatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.MarketingCampaigns", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.UserProfile", "UserName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "UserName", c => c.String());
            DropColumn("dbo.MarketingCampaigns", "RowVersion");
            DropColumn("dbo.MarketingCampaigns", "UpdatedBy");
            DropColumn("dbo.MarketingCampaigns", "CreatedBy");
            RenameColumn(table: "dbo.MarketingCampaigns", name: "CampaignManagerId", newName: "UserId");
        }
    }
}
