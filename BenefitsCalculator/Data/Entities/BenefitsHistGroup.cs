using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenefitsCalculator.Data.Entities
{
    public class BenefitsHistGroup
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Consumer
        /// </summary>
        [ForeignKey("ConsumerId")]
        public Consumer Consumer { get; set; }
        public int ConsumerId { get; set; }

        /// <summary>
        /// Date Created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        public AppUser CreatedBy { get; set; }

        /// <summary>
        /// Guaranteed Issue
        /// </summary>
        public double GuaranteedIssue { get; set; }

        /// <summary>
        /// Basic Salary
        /// </summary>
        public double BasicSalary { get; set; }

        /// <summary>
        /// List of Benefits Histories
        /// </summary>
        public ICollection<BenefitsHistory>? BenefitsHistories { get; set; }
    }
}
