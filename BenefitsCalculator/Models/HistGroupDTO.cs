using System.ComponentModel.DataAnnotations;

namespace BenefitsCalculator.Models
{
    public class HistGroupDTO
    {
        public int Id { get; set; } 

        public int ConsumerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public double GuaranteedIssue {  get; set; }

        public double BasicSalary { get; set; }

        public List<BenefitsHistDTO> BenefitsList { get; set; }
    }
}
