namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SearchKeywords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SearchKeywords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Keywords = c.String(),
                        MarketingCampaign_QuestionPaymentDetailID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MarketingCampaigns", t => t.MarketingCampaign_QuestionPaymentDetailID)
                .Index(t => t.MarketingCampaign_QuestionPaymentDetailID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SearchKeywords", "MarketingCampaign_QuestionPaymentDetailID", "dbo.MarketingCampaigns");
            DropIndex("dbo.SearchKeywords", new[] { "MarketingCampaign_QuestionPaymentDetailID" });
            DropTable("dbo.SearchKeywords");
        }
    }
}
