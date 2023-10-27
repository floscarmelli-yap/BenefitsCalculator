using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenefitsCalculator.Data.Entities
{
    /// <summary>
    /// Status Types:
    /// Approved = 1
    /// For Approval = 2
    /// </summary>
    public enum Status
    {
        Approved = 1,
        ForApproval = 2
    }

    public class BenefitsHistory
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Multiple
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// Benefits Amount Quotation
        /// </summary>
        public double AmountQuotation { get; set; }

        /// <summary>
        /// Pending Amount For Approval
        /// </summary>
        public double PendedAmount { get; set; }

        /// <summary>
        /// Benefits Status
        /// </summary>
        public int BenefitsStatus { get; set; }

        /// <summary>
        /// Benefits History Group
        /// </summary>
        [ForeignKey("BenefitsHistGroupId")]
        public BenefitsHistGroup BenefitsHistGroup { get; set; }
        public int BenefitsHistGroupId { get; set; }
    }
}
