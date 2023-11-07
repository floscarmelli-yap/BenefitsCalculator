namespace BenefitsCalculator.Data.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly BenefitsContext _context;

        /// <summary>
        /// Common Repository Constructor
        /// </summary>
        /// <param name="context"></param>
        public CommonRepository(BenefitsContext context)
        {
            _context = context;
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
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Updates entity data.
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(object entity)
        {
            _context.Update(entity);
        }
    }
}
