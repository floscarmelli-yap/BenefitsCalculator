using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenefitsCalculator.Data.Entities
{
    public class Setup
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Guaranteed Issue
        /// </summary>
        public double GuaranteedIssue { get; set; }

        /// <summary>
        /// Max Age Limit
        /// </summary>
        public int MaxAgeLimit { get; set; }

        /// <summary>
        /// Min Age Limit
        /// </summary>
        public int MinAgeLimit { get; set; }

        /// <summary>
        /// Min Range
        /// </summary>
        public int MinRange { get; set; }

        /// <summary>
        /// Max Range
        /// </summary>
        public int MaxRange { get; set; }

        /// <summary>
        /// Increments
        /// </summary>
        public int Increments { get; set; }
    }
}
