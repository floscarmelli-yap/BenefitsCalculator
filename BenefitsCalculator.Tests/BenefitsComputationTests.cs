using BenefitsCalculator.ComputationLogic;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using Xunit;

namespace BenefitsCalculator.Tests
{
    public class BenefitsComputationTests
    {
        [Fact]
        public void MaxRangeIsOneAndMinRangeIsOne_BenefitsListCountShouldBeOne()
        {
            // Arrange
            var consumerSetup = new ConsumerSetupDTO
            {
                Setup = new SetupDTO
                {
                    Id = 1, // id value is only for testing
                    GuaranteedIssue = 15000,
                    MaxAgeLimit = 50,
                    MinAgeLimit = 49,
                    MaxRange = 1,
                    MinRange = 1,
                    Increments = 1
                },
                Consumer = new ConsumerDTO
                {
                    BasicSalary = 50000,
                    BirthDate = new DateTime(2020, 01, 01)
                }
            };
            var benComputation = new BenefitsComputation(consumerSetup);
            var result = new HistGroupDTO();

            // Act
            result = benComputation.ComputeBenefits();

            // Assert
            Assert.Single(result.BenefitsList);
        }

        [Fact]
        public void IncrementIsThreeMaxRangeIsFiveAndMinRangeIsOne_BenefitsListCountShouldBeTwo()
        {
            // Arrange
            var consumerSetup = new ConsumerSetupDTO
            {
                Setup = new SetupDTO
                {
                    Id = 1, // id value is only for testing
                    GuaranteedIssue = 15000,
                    MaxAgeLimit = 50,
                    MinAgeLimit = 49,
                    MaxRange = 5,
                    MinRange = 1,
                    Increments = 3
                },
                Consumer = new ConsumerDTO
                {
                    BasicSalary = 50000,
                    BirthDate = new DateTime(2020, 01, 01)
                }
            };
            var benComputation = new BenefitsComputation(consumerSetup);
            var result = new HistGroupDTO();

            // Act
            result = benComputation.ComputeBenefits();

            // Assert
            Assert.Equal(2, result.BenefitsList.Count);
        }

        [Fact]
        public void BenefitsComputation_AllPendedAmountForOutsideAgeRange_ReturnsZero()
        {
            // Arrange
            var consumerSetup = new ConsumerSetupDTO
            {
                Setup = new SetupDTO
                {
                    Id = 1, // id value is only for testing
                    GuaranteedIssue = 15000,
                    MaxAgeLimit = 50,
                    MinAgeLimit = 49,
                    MaxRange = 5,
                    MinRange = 1,
                    Increments = 3
                },
                Consumer = new ConsumerDTO
                {
                    BasicSalary = 50000,
                    BirthDate = new DateTime(2020, 01, 01)
                }
            };
            var benComputation = new BenefitsComputation(consumerSetup);
            var result = new HistGroupDTO();

            // Act
            result = benComputation.ComputeBenefits();

            // Assert
            Assert.Equal(0, result.BenefitsList.Sum(x => x.PendedAmount));
        }

        [Fact]
        public void BenefitsComputation_BenefitsStatusForWithinAgeRange_ContainsForApproval()
        {
            // Arrange
            var consumerSetup = new ConsumerSetupDTO
            {
                Setup = new SetupDTO
                {
                    Id = 1, // id value is only for testing
                    GuaranteedIssue = 15000,
                    MaxAgeLimit = 5,
                    MinAgeLimit = 1,
                    MaxRange = 1,
                    MinRange = 1,
                    Increments = 3
                },
                Consumer = new ConsumerDTO
                {
                    BasicSalary = 50000,
                    BirthDate = new DateTime(2020, 01, 01)
                }
            };
            var benComputation = new BenefitsComputation(consumerSetup);
            var result = new HistGroupDTO();

            // Act
            result = benComputation.ComputeBenefits();

            // Assert
            Assert.Equal(Status.ForApproval, result.BenefitsList.First().BenefitsStatus);
        }

    }
}
