using BenefitsCalculator.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BenefitsCalculator.Models
{
    public class BenefitsHistDTO
    {
        public int Id { get; set; }

        public int HistGroupId { get; set; }

        public int Multiple { get; set; }

        [DataType(DataType.Currency)]
        public double AmountQuotation { get; set; }

        [DataType(DataType.Currency)]
        public double PendedAmount { get; set; }

        public Status BenefitsStatus { get; set; }
    }
}
