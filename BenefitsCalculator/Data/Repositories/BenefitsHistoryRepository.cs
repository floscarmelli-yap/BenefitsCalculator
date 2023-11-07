using BenefitsCalculator.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BenefitsCalculator.Data.Repositories
{
    public class BenefitsHistoryRepository : IBenefitsHistoryRepository
    {
        private readonly BenefitsContext _context;

        /// <summary>
        /// Benefits History Repository Constructor
        /// </summary>
        /// <param name="context"></param>
        public BenefitsHistoryRepository(BenefitsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes Benefits History Group data.
        /// </summary>
        /// <param name="historyGroup"></param>
        public void DeleteHistory(BenefitsHistGroup historyGroup)
        {
            _context.BenefitsHistGroups.Remove(historyGroup);
        }

        /// <summary>
        /// Gets all Benefits History Group data.
        /// </summary>
        /// <returns>List of BenefitsHistGroup</returns>
        public async Task<IEnumerable<BenefitsHistGroup>> GetAllBenefitsHistGroups()
        {
            return await _context.BenefitsHistGroups
                .Include(x => x.Consumer)
                .ToListAsync();
        }

        /// <summary>
        /// Gets BenefitsHistGroup data by id.
        /// Includes Benefits History and Consumer data
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BenefitsHistGroup data</returns>
        public async Task<BenefitsHistGroup> GetBenefitsHistGroupById(int id)
        {
            return await _context.BenefitsHistGroups
                .Include(x => x.BenefitsHistories)
                .Include(x => x.Consumer)
                .Where(x => x.Id == id)
                .FirstAsync();
        }

        /// <summary>
        /// Gets Benefits History Group including its related data for delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BenefitsHistGroup data.</returns>
        public async Task<BenefitsHistGroup> GetBenefitsHistGroupForDelete(int id)
        {
            // check if remove is needed
            return await _context.BenefitsHistGroups
                .Include(x => x.BenefitsHistories)
                .Where(x => x.Id == id)
                .FirstAsync();
        }

        /// <summary>
        /// Gets all Consumer BenefitsHistGroup data.
        /// </summary>
        /// <returns>list of BenefitsHistGroup</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<BenefitsHistGroup>> GetConsumerBenefitsHistGroups(int consumerId)
        {
            return await _context.BenefitsHistGroups
                .Include(x => x.Consumer)
                .Where(x => x.ConsumerId == consumerId)
                .ToListAsync();
        }
    }
}
