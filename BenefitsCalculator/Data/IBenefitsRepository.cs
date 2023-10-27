using BenefitsCalculator.Data.Entities;

namespace BenefitsCalculator.Data
{
    public interface IBenefitsRepository
    {
        Task<IEnumerable<Consumer>> GetAllConsumers();
        Task<IEnumerable<Setup>> GetAllSetup();
        Task<IEnumerable<BenefitsHistGroup>> GetAllBenefitsHistGroups();
        Task<IEnumerable<BenefitsHistGroup>> GetConsumerBenefitsHistGroups(int consumerId);
        Task<Consumer?> GetConsumerById(int id);
        Task<Consumer> GetConsumersIncludingSetupById(int id);
        Task<Setup?> GetSetupById(int id);
        Task<BenefitsHistGroup> GetBenefitsHistGroupById(int id);
        Task<BenefitsHistGroup> GetBenefitsHistGroupForDelete(int id);
        Task<bool> ConsumerExists(int id);
        Task<bool> SetupExists(int id);
        Task<bool> SaveAll();
        void UpdateEntity(object entity);
        void AddEntity(object entity);
        void DeleteSetup(Setup setup);
        void DeleteConsumer(Consumer consumer);
        void DeleteHistory(BenefitsHistGroup historyGroup);
    }
}
