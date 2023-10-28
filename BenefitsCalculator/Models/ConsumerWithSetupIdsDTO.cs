using System.ComponentModel.DataAnnotations;

namespace BenefitsCalculator.Models
{
    public class ConsumerWithSetupIdsDTO
    {
        public int Id { get; set; }

        public int? SetupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public double BasicSalary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public int[] SetupIds { get; set; } = new int[0];
    }
}
