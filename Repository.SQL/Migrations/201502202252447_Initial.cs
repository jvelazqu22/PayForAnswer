namespace Repository.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QuestionId = c.Guid(nullable: false),
                        StatusId = c.Int(),
                        UserId = c.Int(nullable: false),
                        PaymentId = c.Long(),
                        CreatedOn = c.DateTime(nullable: false),
                        CommentsUrl = c.String(),
                        DescriptionUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payments", t => t.PaymentId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.QuestionId)
                .Index(t => t.StatusId)
                .Index(t => t.UserId)
                .Index(t => t.PaymentId);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        Container = c.String(),
                        PrimaryUri = c.String(),
                        SecondaryUri = c.String(),
                        ContentType = c.String(),
                        SizeInBytes = c.Long(nullable: false),
                        Answer_Id = c.Long(),
                        Question_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Answers", t => t.Answer_Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Answer_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StatusId = c.Int(),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 35),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatusId = c.Int(),
                        UserId = c.Int(nullable: false),
                        CommentsUrl = c.String(),
                        DescriptionUrl = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.StatusId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.QuestionPaymentDetails",
                c => new
                    {
                        QuestionPaymentDetailID = c.Long(nullable: false, identity: true),
                        QuestionId = c.Guid(nullable: false),
                        PaymentId = c.Long(nullable: false),
                        Type = c.Int(),
                        QuestionAmountBeforeIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalMarketingBudget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuestionAmountIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 50),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.QuestionPaymentDetailID)
                .ForeignKey("dbo.Payments", t => t.PaymentId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.Type)
                .Index(t => t.QuestionId)
                .Index(t => t.PaymentId)
                .Index(t => t.Type);
            
            CreateTable(
                "dbo.MarketingCampaigns",
                c => new
                    {
                        QuestionPaymentDetailID = c.Long(nullable: false),
                        PerDayBudget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfDaysToRun = c.Int(nullable: false),
                        UsedBudget = c.Decimal(precision: 18, scale: 2),
                        StatusId = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 50),
                        CampaignManagerId = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.QuestionPaymentDetailID)
                .ForeignKey("dbo.QuestionPaymentDetails", t => t.QuestionPaymentDetailID)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CampaignManagerId)
                .Index(t => t.QuestionPaymentDetailID)
                .Index(t => t.StatusId)
                .Index(t => t.CampaignManagerId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewEmail = c.String(maxLength: 100),
                        AcceptedTermsConditionsAndPrivacyPolicy = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        NewAnswerToMyQuestion = c.Boolean(nullable: false),
                        NewQuestionRelatedToMySubjects = c.Boolean(nullable: false),
                        NewComment = c.Boolean(nullable: false),
                        QuestionStatusChangeWithMyAnswers = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubjectName = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SubjectName);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        FeedbackUrl = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Thread = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        Message = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SubjectQuestions",
                c => new
                    {
                        Subject_Id = c.Long(nullable: false),
                        Question_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.Question_Id })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.UserSubject",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        SubjectID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.SubjectID })
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.SubjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Answers", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Questions", "StatusId", "dbo.Status");
            DropForeignKey("dbo.QuestionPaymentDetails", "Type", "dbo.Status");
            DropForeignKey("dbo.QuestionPaymentDetails", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuestionPaymentDetails", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.MarketingCampaigns", "CampaignManagerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSubject", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.UserSubject", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubjectQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.SubjectQuestions", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Questions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MarketingCampaigns", "StatusId", "dbo.Status");
            DropForeignKey("dbo.MarketingCampaigns", "QuestionPaymentDetailID", "dbo.QuestionPaymentDetails");
            DropForeignKey("dbo.Attachments", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Attachments", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.UserSubject", new[] { "SubjectID" });
            DropIndex("dbo.UserSubject", new[] { "UserID" });
            DropIndex("dbo.SubjectQuestions", new[] { "Question_Id" });
            DropIndex("dbo.SubjectQuestions", new[] { "Subject_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Subjects", new[] { "SubjectName" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "UserID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.MarketingCampaigns", new[] { "CampaignManagerId" });
            DropIndex("dbo.MarketingCampaigns", new[] { "StatusId" });
            DropIndex("dbo.MarketingCampaigns", new[] { "QuestionPaymentDetailID" });
            DropIndex("dbo.QuestionPaymentDetails", new[] { "Type" });
            DropIndex("dbo.QuestionPaymentDetails", new[] { "PaymentId" });
            DropIndex("dbo.QuestionPaymentDetails", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "UserId" });
            DropIndex("dbo.Questions", new[] { "StatusId" });
            DropIndex("dbo.Payments", new[] { "StatusId" });
            DropIndex("dbo.Attachments", new[] { "Question_Id" });
            DropIndex("dbo.Attachments", new[] { "Answer_Id" });
            DropIndex("dbo.Answers", new[] { "PaymentId" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropIndex("dbo.Answers", new[] { "StatusId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.UserSubject");
            DropTable("dbo.SubjectQuestions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Logs");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Subjects");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.MarketingCampaigns");
            DropTable("dbo.QuestionPaymentDetails");
            DropTable("dbo.Questions");
            DropTable("dbo.Status");
            DropTable("dbo.Payments");
            DropTable("dbo.Attachments");
            DropTable("dbo.Answers");
        }
    }
}
