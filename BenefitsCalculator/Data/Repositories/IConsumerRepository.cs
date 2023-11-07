using BenefitsCalculator.Data.Entities;

namespace BenefitsCalculator.Data.Repositories
{
    public interface IConsumerRepository
    {
        Task<IEnumerable<Consumer>> GetAllConsumers();
        Task<Consumer?> GetConsumerById(int id);
        Task<Consumer> GetConsumersIncludingSetupById(int id);
        Task<bool> ConsumerExists(int id);
        void DeleteConsumer(Consumer consumer);
    }
}
