using System.ComponentModel.DataAnnotations;

namespace BenefitsCalculator.Models
{
    public class SetupDTO
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public double GuaranteedIssue { get; set; }

        [Required]
        [Range(0, 120)]
        public int MaxAgeLimit { get; set; }

        [Required]
        [Range(0, 100)]
        public int MinAgeLimit { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MinRange { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxRange { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Increments { get; set; }
    }
}
