using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenefitsCalculator.Data.Entities
{
    public class Consumer
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Basic Salary
        /// </summary>
        public double BasicSalary { get; set; }

        /// <summary>
        /// Birth Date
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Setup
        /// </summary>
        [ForeignKey("SetupId")]
        public Setup? Setup { get; set; }
        public int? SetupId { get; set; }

        /// <summary>
        /// Benefits History Groups List
        /// </summary>
        public ICollection<BenefitsHistGroup>? BenefitsHistGroups { get; set; }
    }
}
