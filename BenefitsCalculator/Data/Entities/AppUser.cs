using Microsoft.AspNetCore.Identity;

namespace BenefitsCalculator.Data.Entities
{
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
    }
}
