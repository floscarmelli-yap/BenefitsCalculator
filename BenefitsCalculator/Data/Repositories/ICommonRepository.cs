namespace BenefitsCalculator.Data.Repositories
{
    public interface ICommonRepository
    {
        Task<bool> SaveAll();
        void UpdateEntity(object entity);
        void AddEntity(object entity);
    }
}
