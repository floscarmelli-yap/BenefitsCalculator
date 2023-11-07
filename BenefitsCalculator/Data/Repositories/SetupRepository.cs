using BenefitsCalculator.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BenefitsCalculator.Data.Repositories
{
    public class SetupRepository : ISetupRepository
    {
        private readonly BenefitsContext _context;

        /// <summary>
        /// Setup Repository Constructor
        /// </summary>
        /// <param name="context"></param>
        public SetupRepository(BenefitsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes Setup data.
        /// </summary>
        /// <param name="setup"></param>
        public void DeleteSetup(Setup setup)
        {
            _context.Setups.Remove(setup);
        }

        /// <summary>
        /// Gets all Setup data.
        /// </summary>
        /// <returns>list of Setups</returns>
        public async Task<IEnumerable<Setup>> GetAllSetup()
        {
            return await _context.Setups.ToListAsync();
        }

        /// <summary>
        /// Gets Setup data by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Setup data or null</returns>
        public async Task<Setup?> GetSetupById(int id)
        {
            return await _context.Setups.FindAsync(id);
        }

        /// <summary>
        /// Checks if Setup data exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if exists, else false</returns>
        public async Task<bool> SetupExists(int id)
        {
            return await _context.Setups.AnyAsync(x => x.Id == id);
        }
    }
}
