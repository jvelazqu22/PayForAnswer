SELECT * FROM Questions WHERE Id = '726D5E39-1283-48A9-8365-96EC6142847D'
UPDATE Questions SET StatusId = 7 WHERE Id = '726D5E39-1283-48A9-8365-96EC6142847D'

SELECT * FROM Payments WHERE Id >= 9
SELECT * FROM QuestionPaymentDetails WHERE QuestionPaymentDetailID >= 9
SELECT * FROM MarketingCampaigns
UPDATE Payments SET StatusId = 7 WHERE Id >= 9
UPDATE MarketingCampaigns SET StatusId = 101 WHERE QuestionPaymentDetailID >= 9