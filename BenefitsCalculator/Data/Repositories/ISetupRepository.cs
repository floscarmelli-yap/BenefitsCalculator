using BenefitsCalculator.Data.Entities;

namespace BenefitsCalculator.Data.Repositories
{
    public interface ISetupRepository
    {
        Task<IEnumerable<Setup>> GetAllSetup();
        Task<Setup?> GetSetupById(int id);
        Task<bool> SetupExists(int id);
        void DeleteSetup(Setup setup);
    }
}
