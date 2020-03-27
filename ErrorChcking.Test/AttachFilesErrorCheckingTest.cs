using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErrorChecking;
using Domain.Constants;

namespace ErrorChcking.Test
{
    [TestClass]
    public class AttachFilesErrorCheckingTest
    {
        // MethodName_StateUnderTest_ExpectedBehavior
        // Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()

        [TestMethod]
        public void GetStorageMaxSizeBytesForQuestion_10QAmountt_1MegabytesPerQuestionLevel()
        {
            // Arrange
            double questionAmount = 10;
            double maxSizeBytesLimit = 0;
            double expectedLimitResult = StorageSize.BytesInAMegabyte * General.MegabytesPerQuestionLevel * 1;
            // Act
            maxSizeBytesLimit = new AttachFilesErrorChecking().GetStorageMaxSizeBytesForQuestion(questionAmount);

            // Assert
            Assert.AreEqual(maxSizeBytesLimit, expectedLimitResult);
        }

        [TestMethod]
        public void GetStorageMaxSizeBytesForQuestion_19QAmountt_1MegabytesPerQuestionLevel()
        {
            // Arrange
            double questionAmount = 19;
            double maxSizeBytesLimit = 0;
            double expectedLimitResult = StorageSize.BytesInAMegabyte * General.MegabytesPerQuestionLevel * 1;
            // Act
            maxSizeBytesLimit = new AttachFilesErrorChecking().GetStorageMaxSizeBytesForQuestion(questionAmount);

            // Assert
            Assert.AreEqual(maxSizeBytesLimit, expectedLimitResult);
        }

        [TestMethod]
        public void GetStorageMaxSizeBytesForQuestion_20QAmountt_2MegabytesPerQuestionLevel()
        {
            // Arrange
            double questionAmount = 20;
            double maxSizeBytesLimit = 0;
            double expectedLimitResult = StorageSize.BytesInAMegabyte * General.MegabytesPerQuestionLevel * 2;
            // Act
            maxSizeBytesLimit = new AttachFilesErrorChecking().GetStorageMaxSizeBytesForQuestion(questionAmount);

            // Assert
            Assert.AreEqual(maxSizeBytesLimit, expectedLimitResult);
        }



        [TestMethod]
        public void GetStorageMaxSizeBytesForAnswer_10QAmountt_2MegabytesPerQuestionLevel()
        {
            // Arrange
            double questionAmount = 10;
            double maxSizeBytesLimit = 0;
            double expectedLimitResult = StorageSize.BytesInAMegabyte * General.MegabytesPerQuestionLevel * 1 * General.AnswerStorageMultiplier;
            // Act
            maxSizeBytesLimit = new AttachFilesErrorChecking().GetStorageMaxSizeBytesForAnswer(questionAmount);

            // Assert
            Assert.AreEqual(maxSizeBytesLimit, expectedLimitResult);
        }

        [TestMethod]
        public void GetStorageMaxSizeBytesForAnswer_19QAmountt_2MegabytesPerQuestionLevel()
        {
            // Arrange
            double questionAmount = 19;
            double maxSizeBytesLimit = 0;
            double expectedLimitResult = StorageSize.BytesInAMegabyte * General.MegabytesPerQuestionLevel * 1 * General.AnswerStorageMultiplier;
            // Act
            maxSizeBytesLimit = new AttachFilesErrorChecking().GetStorageMaxSizeBytesForAnswer(questionAmount);

            // Assert
            Assert.AreEqual(maxSizeBytesLimit, expectedLimitResult);
        }

        [TestMethod]
        public void GetStorageMaxSizeBytesForAnswer_20QAmountt_2MegabytesPerQuestionLevel()
        {
            // Arrange
            double questionAmount = 20;
            double maxSizeBytesLimit = 0;
            double expectedLimitResult = StorageSize.BytesInAMegabyte * General.MegabytesPerQuestionLevel * 2 * General.AnswerStorageMultiplier;
            // Act
            maxSizeBytesLimit = new AttachFilesErrorChecking().GetStorageMaxSizeBytesForAnswer(questionAmount);

            // Assert
            Assert.AreEqual(maxSizeBytesLimit, expectedLimitResult);
        }

    }
}
