using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using System.Runtime.CompilerServices;

namespace BenefitsCalculator.ComputationLogic
{
    public class BenefitsComputation
    {
        private readonly ConsumerSetupDTO _consumerSetup;

        public BenefitsComputation(ConsumerSetupDTO consumerSetup)
        {
            _consumerSetup = consumerSetup;
        }

        /// <summary>
        /// Computes the benefits of consumer using its setup
        /// </summary>
        /// <returns>Benefits History Group data</returns>
        public HistGroupDTO ComputeBenefits()
        {
            List<BenefitsHistDTO> benefitsList = new List<BenefitsHistDTO>();
            HistGroupDTO benefitsResult = new HistGroupDTO();

            var setup = _consumerSetup.Setup;
            var consumer = _consumerSetup.Consumer;

            // if setup id is not 0, continue execution,
            // else proceed to the return statement
            if(setup.Id != 0)
            {
                // loop through the specified range
                for (int i = setup.MinRange; i <= setup.MaxRange; i += setup.Increments)
                {
                    // benefits quotation amount * increments inside range
                    var benefitsQuotation = consumer.BasicSalary * i;
                    double pendedAmount = 0;

                    // compute age of consumer
                    int age = GetAge(consumer.BirthDate);
                    Status status = Status.Approved;

                    // check if age is within range
                    if (IsAgeWithinRange(age))
                    {
                        pendedAmount = benefitsQuotation - setup.GuaranteedIssue;

                        // check if pended amount is greater than 0;
                        // if benefits quotation amount is greater than guaranteed issue,
                        // status should be for approval
                        if (pendedAmount > 0)
                        {
                            status = Status.ForApproval;
                        }
                    }

                    // add results to the model
                    benefitsList.Add(new BenefitsHistDTO
                    {
                        Multiple = i,
                        AmountQuotation = benefitsQuotation,
                        PendedAmount = pendedAmount,
                        BenefitsStatus = status,
                    });
                }

                benefitsResult = new HistGroupDTO
                {
                    ConsumerId = consumer.Id,
                    CreatedDate = DateTime.Now,
                    GuaranteedIssue = setup.GuaranteedIssue,
                    BasicSalary = consumer.BasicSalary,
                    BenefitsList = benefitsList
                };
            }

            return benefitsResult;
        }

        /// <summary>
        /// Computes Age using Birth Date
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns>Age</returns>
        private int GetAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;

            if (birthDate.AddYears(age) > DateTime.Now.Date )
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Checks if age is within the minimum and maximum age limit.
        /// </summary>
        /// <param name="age"></param>
        /// <returns>true if within limit, else false</returns>
        private bool IsAgeWithinRange(int age)
        {
            return ((age >= _consumerSetup.Setup.MinAgeLimit) &&
                   (age <= _consumerSetup.Setup.MaxAgeLimit));
        }
    }
}
