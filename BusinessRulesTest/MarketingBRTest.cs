using BusinessRules;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Calculation;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessRulesTest
{
    [TestClass]
    public class MarketingBRTest
    {
        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        // Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()

        [TestMethod]
        public void AddFirstCampaignToPaymentModel_NormalData_NewMarketingModel()
        {
            // Arrange
            MarketingBR marketingBr = new MarketingBR();
            QuestionPaymentDetail questionPaymentModel = new QuestionPaymentDetail();
            CreateQuestionViewModel createQuestionViewModel = new CreateQuestionViewModel()
            {
                MarketingBudgetPerDay = 2,
                NumberOfCampaignDays = 2
            };

            // Act
            marketingBr.AddFirstCampaignToPaymentModel(createQuestionViewModel, questionPaymentModel);

            // Assert
            Assert.IsNotNull(questionPaymentModel.MarketingCampaign);
            Assert.AreEqual(0, questionPaymentModel.MarketingCampaign.UsedBudget);
            Assert.AreEqual(StatusValues.WaitingForPaymentNotification, questionPaymentModel.MarketingCampaign.StatusId);
            Assert.AreEqual(2, questionPaymentModel.MarketingCampaign.PerDayBudget);
            Assert.AreEqual(2, questionPaymentModel.MarketingCampaign.NumberOfDaysToRun);
        }

        [TestMethod]
        public void GetAllQuerstionActiveMarketingCampaigns_DateBoundaries_ReturnListOfActiveCampaigns()
        {
            // Arrange
            DateTime utcNow = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            List<MarketingCampaign> allMarketingCampaigns = new List<MarketingCampaign>()
            {
                new MarketingCampaign() { StatusId = StatusValues.WaitingForPaymentNotification },
                new MarketingCampaign() { StatusId = CampaignStatus.CampaignReadyToBeStarted },
                new MarketingCampaign() { StatusId = CampaignStatus.CampaignStarted, },
                new MarketingCampaign()
                {
                    StatusId = CampaignStatus.CampaignStarted,
                    StartDate = utcNow.Subtract(new TimeSpan(1,0,0,0)),
                    NumberOfDaysToRun = 2,
                },
                new MarketingCampaign()
                {
                    StatusId = CampaignStatus.CampaignManagerAssigned,
                    StartDate = utcNow.Subtract(new TimeSpan(2,0,0,0)),
                    NumberOfDaysToRun = 2,
                },
                new MarketingCampaign()
                {
                    StatusId = CampaignStatus.CampaignManagerAssigned,
                    StartDate = utcNow.Subtract(new TimeSpan(2,0,1,0)),
                    NumberOfDaysToRun = 2,
                },
            };
            MarketingBR marketingBr = new MarketingBR();
            List<MarketingCampaign> activeMarketingCampaigns;

            // Act
            activeMarketingCampaigns = marketingBr.GetAllQuestionActiveMarketingCampaigns(allMarketingCampaigns, utcNow);

            // Assert
            Assert.AreEqual(3, activeMarketingCampaigns.Count);

        }

        [TestMethod]
        public void GetAllQuerstionMarketingCampaigns_SimpleDataTest_ReturnListOfAllCampaigns()
        {
            // Arrange
            var questionPaymentDetails = new List<QuestionPaymentDetail>
            #region QuestinPaymentDetails
                {
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 1,
                        Payment = new Payment() { Id = 1, StatusId = StatusValues.WaitingForPaymentNotification, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 1,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = StatusValues.WaitingForPaymentNotification,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//1
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 2,
                        Payment = new Payment() { Id = 2, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 2,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//2
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 3,
                        Payment = new Payment() { Id = 3, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 3,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignManagerAssigned,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//3
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 4,
                        Payment = new Payment() { Id = 4, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 4,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow, StartDate = DateTime.UtcNow
                        }
                    },//4
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 5,
                        Payment = new Payment() { Id = 5, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 5,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow, StartDate = DateTime.UtcNow.Subtract(new TimeSpan(20,0,0,0))
                        }
                    },//5
                };
            #endregion

            var question = new Question()
            {
                Id = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Title = "What is the square root of -1?", 
                //Description = "What is the square root of -1?",
                Amount = 1, StatusId = StatusValues.PayPalIPNNotifyConfirmed, UserId = 1, CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(), 
                QuestionPaymentDetails = new List<QuestionPaymentDetail> ()
            };
            questionPaymentDetails.ForEach(r => question.QuestionPaymentDetails.Add(r));
            List<MarketingCampaign> allQuestionMarketingCampaigns;

            // Act
            allQuestionMarketingCampaigns = new MarketingBR().GetAllQuerstionMarketingCampaigns(question);

            // Assert
            Assert.AreEqual(5, questionPaymentDetails.Count);
        }

        [TestMethod]
        public void GetEndDate_SimpleDataTest_ReturnGoodEndDate()
        {
            // Arrange
            MarketingCampaign marketingCampaign = new MarketingCampaign() 
            {
                StartDate = new DateTime(2014, 1, 1, 0, 0, 0), NumberOfDaysToRun = 2, PerDayBudget = 2 
            };
            DateTime? endDate;

            // Act
            endDate = new MarketingBR().GetEndDate(marketingCampaign, DateTime.UtcNow);

            // Assert
            Assert.AreEqual(endDate, new DateTime(2014, 1, 3, 0, 0, 0));
        }

        [TestMethod]
        public void GetEndDate_InvalidCampaign_ReturnNullEndDate()
        {
            // Arrange
            MarketingCampaign marketingCampaign = new MarketingCampaign() { NumberOfDaysToRun = 2 };
            DateTime? endDate;

            // Act
            endDate = new MarketingBR().GetEndDate(marketingCampaign, DateTime.UtcNow);

            // Assert
            Assert.IsNull(endDate);
        }

        [TestMethod]
        public void DaysLeftBeforeCampaignEnds_SimpleDataTest_ReturnPositveNumber()
        {
            // Arrange
            MarketingCampaign marketingCampaign = new MarketingCampaign() { StartDate = DateTime.UtcNow, NumberOfDaysToRun = 2, PerDayBudget = 2 };
            int DaysLeftBeforeCampaignEnds;

            // Act
            DaysLeftBeforeCampaignEnds = new MarketingBR().DaysLeftBeforeCampaignEnds(marketingCampaign, DateTime.UtcNow);

            // Assert
            Assert.AreEqual(1, DaysLeftBeforeCampaignEnds);
        }

        [TestMethod]
        public void DaysLeftBeforeCampaignEnds_NullStartDate_ReturnsZero()
        {
            // Arrange
            MarketingCampaign marketingCampaign = new MarketingCampaign() { NumberOfDaysToRun = 2, PerDayBudget = 2 };
            double DaysLeftBeforeCampaignEnds;

            // Act
            DaysLeftBeforeCampaignEnds = new MarketingBR().DaysLeftBeforeCampaignEnds(marketingCampaign, DateTime.UtcNow);

            // Assert
            Assert.AreEqual(marketingCampaign.NumberOfDaysToRun, DaysLeftBeforeCampaignEnds);
        }

        [TestMethod]
        public void GetQuestionActiveCampaignsTotals_SimpleDataTest_ReturnsListOfMarketingCampaignTotals()
        {
            // Arrange
            DateTime utcNow = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            List<MarketingCampaign> allQuestionActiveMarketingCampaigns = new List<MarketingCampaign>()
            {
                new MarketingCampaign(),
                new MarketingCampaign() { StatusId = StatusValues.WaitingForPaymentNotification },
                new MarketingCampaign() { StatusId = CampaignStatus.CampaignReadyToBeStarted, PerDayBudget = 10, NumberOfDaysToRun = 0 },
                new MarketingCampaign() { StatusId = CampaignStatus.CampaignStarted, PerDayBudget = 10, NumberOfDaysToRun = 10},
                new MarketingCampaign()
                {
                    StatusId = CampaignStatus.CampaignStarted, StartDate = utcNow.Subtract(new TimeSpan(1,0,0,0)),
                    NumberOfDaysToRun = 2,
                },
                new MarketingCampaign()
                {
                    StatusId = CampaignStatus.CampaignManagerAssigned, StartDate = utcNow.Subtract(new TimeSpan(2,0,0,0)),
                    NumberOfDaysToRun = 10, PerDayBudget = 10
                },
                new MarketingCampaign()
                {
                    StatusId = CampaignStatus.CampaignManagerAssigned,
                    StartDate = utcNow.Subtract(new TimeSpan(2,0,1,0)),
                    NumberOfDaysToRun = 2, PerDayBudget = 10
                },
            };
            List<MarketingCampaignTotals> marketingCampaignsTotals = new List<MarketingCampaignTotals>();
            MarketingBR marketingBr = new MarketingBR();

            // Act
            marketingCampaignsTotals = marketingBr.GetQuestionActiveCampaignsTotals(allQuestionActiveMarketingCampaigns, utcNow);

            // Assert
            Assert.AreEqual(3, marketingCampaignsTotals.Count);
            Assert.AreEqual(180, marketingCampaignsTotals.Sum(r => r.TotalBudgetLeft));
        }

        [TestMethod]
        public void GetQuestionActiveMarketingCampaignSummary_NoActiveCampaigns_FillSummaryValues()
        {
            // Arrange
            QuestionDetailsViewModel questionDetailsViewModel = new QuestionDetailsViewModel();
            DateTime utcNow = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var questionPaymentDetails = new List<QuestionPaymentDetail>
            #region QuestinPaymentDetails
                {
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 1,
                        Payment = new Payment() { Id = 1, StatusId = StatusValues.WaitingForPaymentNotification, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 1,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = StatusValues.WaitingForPaymentNotification,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//1
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 2,
                        Payment = new Payment() { Id = 2, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 2,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow, StartDate = utcNow.Subtract(new TimeSpan(10,0,0,0))
                        }
                    },//2
                };
            #endregion

            Guid guid1 = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D");
            var question = new Question()
            {
                Id = guid1,
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                UserId = 1,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>()
            };
            questionPaymentDetails.ForEach(r => question.QuestionPaymentDetails.Add(r));

            // Act
            new MarketingBR().GetQuestionActiveMarketingCampaignSummary(question, questionDetailsViewModel, utcNow);

            // Assert
            Assert.AreEqual(0, questionDetailsViewModel.TotalActiveCampaigns);
            Assert.AreEqual(0, questionDetailsViewModel.MarketingBudgetLeft);
            Assert.IsNull(questionDetailsViewModel.LongestCampaignEndDate);
        }

        [TestMethod]
        public void GetQuestionActiveMarketingCampaignSummary_ActiveCampaigns1_FillSummaryValues()
        {
            // Arrange
            QuestionDetailsViewModel questionDetailsViewModel = new QuestionDetailsViewModel();
            DateTime utcNow = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var questionPaymentDetails = new List<QuestionPaymentDetail>
            #region QuestinPaymentDetails
                {
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 2,
                        Payment = new Payment() { Id = 2, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 2,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(9,0,0,0))
                        }
                    },//1
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 3,
                        Payment = new Payment() { Id = 3, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 3,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignManagerAssigned,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(8,0,0,0))
                        }
                    },//2
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 4,
                        Payment = new Payment() { Id = 4, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 4,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(7,0,0,0))
                        }
                    },//3
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 5,
                        Payment = new Payment() { Id = 5, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 5,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(7,0,0,0))
                        }
                    },//4
                };
            #endregion
            DateTime smallestDate = utcNow.AddDays(10).Subtract(new TimeSpan(7, 0, 0, 0));

            Guid guid1 = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D");
            var question = new Question()
            {
                Id = guid1,
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                UserId = 1,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>()
            };
            questionPaymentDetails.ForEach(r => question.QuestionPaymentDetails.Add(r));

            // Act
            new MarketingBR().GetQuestionActiveMarketingCampaignSummary(question, questionDetailsViewModel, utcNow);

            // Assert
            Assert.AreEqual(4, questionDetailsViewModel.TotalActiveCampaigns);
            Assert.AreEqual(9, questionDetailsViewModel.MarketingBudgetLeft);
            Assert.AreEqual(smallestDate, questionDetailsViewModel.LongestCampaignEndDate);
        }

        [TestMethod]
        public void GetQuestionActiveMarketingCampaignSummary_ActiveCampaigns2_FillSummaryValues()
        {
            // Arrange
            QuestionDetailsViewModel questionDetailsViewModel = new QuestionDetailsViewModel();
            DateTime utcNow = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var questionPaymentDetails = new List<QuestionPaymentDetail>
            #region QuestinPaymentDetails
                {
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 2,
                        Payment = new Payment() { Id = 2, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 2,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(9,0,0,0))
                        }
                    },//1
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 3,
                        Payment = new Payment() { Id = 3, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 3,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignManagerAssigned,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(8,0,0,0))
                        }
                    },//2
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 4,
                        Payment = new Payment() { Id = 4, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 4,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(7,0,0,0))
                        }
                    },//3
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 5,
                        Payment = new Payment() { Id = 5, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 5,
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = utcNow, StartDate = utcNow.Subtract(new TimeSpan(1,0,0,0))
                        }
                    },//4
                };
            #endregion
            DateTime smallestDate = utcNow.AddDays(10).Subtract(new TimeSpan(1, 0, 0, 0));

            Guid guid1 = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D");
            var question = new Question()
            {
                Id = guid1,
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                UserId = 1,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>()
            };
            questionPaymentDetails.ForEach(r => question.QuestionPaymentDetails.Add(r));

            // Act
            new MarketingBR().GetQuestionActiveMarketingCampaignSummary(question, questionDetailsViewModel, utcNow);

            // Assert
            Assert.AreEqual(4, questionDetailsViewModel.TotalActiveCampaigns);
            Assert.AreEqual(15, questionDetailsViewModel.MarketingBudgetLeft);
            Assert.AreEqual(smallestDate, questionDetailsViewModel.LongestCampaignEndDate);
        }

        [TestMethod]
        public void GetQuestionMarketingHistoryViewModelList_ActiveMarketingCampaigns_ReturnAllRecords()
        {
            // Arrange
            List<QuestionMarketingHistoryViewModel> questionMarketingHistoryList = new List<QuestionMarketingHistoryViewModel>();
            DateTime utcNow = DateTime.UtcNow;
            var paymentRepository = new Mock<IPaymentRepository>();
            var questionPaymentDetails = new List<QuestionPaymentDetail>
            #region QuestinPaymentDetails
                {
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 2,
                        Payment = new Payment() { Id = 2, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 2, PerDayBudget = 1, NumberOfDaysToRun = 10, 
                            StatusId = CampaignStatus.CampaignReadyToBeStarted, CreatedBy = Role.PayForAnswer, 
                            CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer, UpdatedOn = utcNow, 
                            StartDate = utcNow.Subtract(new TimeSpan(9,0,0,0)), EndDate = utcNow.AddDays(10-9)
                        }
                    },//1
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 3, 
                        Payment = new Payment() { Id = 3, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 3, PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignManagerAssigned,
                            CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, UpdatedBy = Role.PayForAnswer, UpdatedOn = utcNow, 
                            StartDate = utcNow.Subtract(new TimeSpan(8,0,0,0)), EndDate = utcNow.AddDays(10-8)
                        }
                    },//2
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 4,
                        Payment = new Payment() { Id = 4, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 4, PerDayBudget = 1, NumberOfDaysToRun = 10, 
                            StatusId = CampaignStatus.CampaignStarted, CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, 
                            UpdatedBy = Role.PayForAnswer, UpdatedOn = utcNow, 
                            StartDate = utcNow.Subtract(new TimeSpan(7,0,0,0)), EndDate = utcNow.AddDays(10-7)
                        }
                    },//3
                    new QuestionPaymentDetail
                    {
                        QuestionPaymentDetailID = 5, 
                        Payment = new Payment() { Id = 5, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                        QuestionAmountBeforeIncrease = 1, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = utcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = utcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            QuestionPaymentDetailID = 5, PerDayBudget = 1, NumberOfDaysToRun = 10, 
                            StatusId = CampaignStatus.CampaignStarted, CreatedBy = Role.PayForAnswer, CreatedOn = utcNow, 
                            UpdatedBy = Role.PayForAnswer, UpdatedOn = utcNow, 
                            StartDate = utcNow.Subtract(new TimeSpan(1,0,0,0)), EndDate = utcNow.AddDays(10-1)
                        }
                    },//4
                };
            #endregion

            Guid guid1 = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D");
            var question = new Question()
            {
                Id = guid1,
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                UserId = 1,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = questionPaymentDetails
            };
            questionPaymentDetails.ForEach(r => r.Question = question);
            paymentRepository.Setup(r => r.GetPaymentDetailListByQuestionID(question.Id)).Returns(questionPaymentDetails);

            // Act
            questionMarketingHistoryList = new MarketingBR().GetQuestionMarketingHistoryViewModelList(question.Id, paymentRepository.Object, question.UserId);

            // Assert
            Assert.AreEqual(4, questionMarketingHistoryList.Count);
        }
    }
}
