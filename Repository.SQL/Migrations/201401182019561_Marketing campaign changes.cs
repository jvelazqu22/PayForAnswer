namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Marketingcampaignchanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarketingCampaigns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QuestionId = c.Long(nullable: false),
                        TotalBudget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatudId = c.Int(nullable: false),
                        PerDayBudget = c.Decimal(precision: 18, scale: 2),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        UserIdThatStartedCampaign = c.Int(),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.Status_Id)
                .Index(t => t.QuestionId)
                .Index(t => t.Status_Id);
            
            AddColumn("dbo.Questions", "MarketingBudget", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Questions", "AnswerFee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "AnswerFee", c => c.Decimal(precision: 18, scale: 2));
            DropIndex("dbo.MarketingCampaigns", new[] { "Status_Id" });
            DropIndex("dbo.MarketingCampaigns", new[] { "QuestionId" });
            DropForeignKey("dbo.MarketingCampaigns", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.MarketingCampaigns", "QuestionId", "dbo.Questions");
            DropColumn("dbo.Questions", "MarketingBudget");
            DropTable("dbo.MarketingCampaigns");
        }
    }
}
