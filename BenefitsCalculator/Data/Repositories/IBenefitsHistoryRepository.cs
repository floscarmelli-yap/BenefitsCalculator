using BenefitsCalculator.Data.Entities;

namespace BenefitsCalculator.Data.Repositories
{
    public interface IBenefitsHistoryRepository
    {
        Task<IEnumerable<BenefitsHistGroup>> GetAllBenefitsHistGroups();
        Task<IEnumerable<BenefitsHistGroup>> GetConsumerBenefitsHistGroups(int consumerId);
        Task<BenefitsHistGroup> GetBenefitsHistGroupById(int id);
        Task<BenefitsHistGroup> GetBenefitsHistGroupForDelete(int id);
        void DeleteHistory(BenefitsHistGroup historyGroup);
    }
}
