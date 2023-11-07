using BenefitsCalculator.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BenefitsCalculator.Data.Repositories
{
    public class ConsumerRepository : IConsumerRepository
    {
        private readonly BenefitsContext _context;

        /// <summary>
        /// Consumer Repository Constructor
        /// </summary>
        /// <param name="context"></param>
        public ConsumerRepository(BenefitsContext context) 
        { 
            _context = context;
        }

        /// <summary>
        /// Checks if Consumer data exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if exists, else false</returns>
        public async Task<bool> ConsumerExists(int id)
        {
            return await _context.Consumers.AnyAsync(x => x.Id == id);
        }

        /// <summary>
        /// Deletes Consumer data.
        /// </summary>
        /// <param name="consumer"></param>
        public void DeleteConsumer(Consumer consumer)
        {
            _context.Consumers.Remove(consumer);
        }

        /// <summary>
        /// Gets all Consumer data.
        /// </summary>
        /// <returns>list of Consumers</returns>
        public async Task<IEnumerable<Consumer>> GetAllConsumers()
        {
            return await _context.Consumers.ToListAsync();
        }

        /// <summary>
        /// Gets Consumer data by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Consumer data or null</returns>
        public async Task<Consumer?> GetConsumerById(int id)
        {
            return await _context.Consumers.FindAsync(id);
        }

        /// <summary>
        /// Gets a Consumer and its Setup data by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Consumer data</returns>
        public async Task<Consumer> GetConsumersIncludingSetupById(int id)
        {
            return await _context.Consumers
                .Include(x => x.Setup)
                .Where(x => x.Id == id)
                .FirstAsync();
        }
    }
}
