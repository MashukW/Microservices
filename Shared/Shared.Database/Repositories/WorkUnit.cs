namespace Shared.Database.Repositories
{
    public class WorkUnit : IWorkUnit
    {
        private readonly BaseDbContext _baseContext;

        public WorkUnit(BaseDbContext baseContext)
        {
            _baseContext = baseContext;
        }

        public async Task SaveChanges()
        {
            await _baseContext.SaveChangesAsync();
        }
    }
}
