using BenefitsCalculator.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BenefitsCalculator.Data
{
    public class BenefitsRepository : IBenefitsRepository
    {
        private readonly BenefitsContext _context;
        private readonly ILogger<BenefitsRepository> _logger;

        /// <summary>
        /// Benefits repository constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public BenefitsRepository(BenefitsContext context, ILogger<BenefitsRepository> logger)
        {
            _context = context;
            _logger = logger;
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
        /// Gets all Setup data.
        /// </summary>
        /// <returns>list of Setups</returns>
        public async Task<IEnumerable<Setup>> GetAllSetup()
        {
            return await _context.Setups.ToListAsync();
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
        /// Gets Setup data by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Setup data or null</returns>
        public async Task<Setup?> GetSetupById(int id)
        {
            return await _context.Setups.FindAsync(id);
        }

        /// <summary>
        /// Creates new entity data.
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(object entity)
        {
            _context.Add(entity);
        }

        /// <summary>
        /// Saves entity changes.
        /// </summary>
        /// <returns>true if saving is successful, else false</returns>
        public async Task<bool> SaveAll()
        {
            return (await _context.SaveChangesAsync() > 0);
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

        /// <summary>
        /// Updates entity data.
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(object entity)
        {
            _context.Update(entity);
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
        /// Deletes Setup data.
        /// </summary>
        /// <param name="setup"></param>
        public void DeleteSetup(Setup setup)
        {
            _context.Setups.Remove(setup);
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
        /// Deletes Benefits History Group data.
        /// </summary>
        /// <param name="historyGroup"></param>
        public void DeleteHistory(BenefitsHistGroup historyGroup)
        {
            _context.BenefitsHistGroups.Remove(historyGroup);
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
        /// Gets all Benefits History Group data.
        /// </summary>
        /// <returns>List of BenefitsHistGroup</returns>
        public async Task<IEnumerable<BenefitsHistGroup>> GetAllBenefitsHistGroups()
        {
            return await _context.BenefitsHistGroups
                .Include(x => x.Consumer)
                .ToListAsync();
        }
    }
}
