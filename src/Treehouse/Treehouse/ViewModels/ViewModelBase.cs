using TreeHouse.Database;
using TreeHouse.Services;

namespace TreeHouse.ViewModels
{
    public abstract class ViewModelBase
    {
        private readonly DbService _dbService;

        protected ViewModelBase(DbService dbService)
        {
            _dbService = dbService;
        }

        protected TreeHouseContext CreateConnection()
        {
            return _dbService.CreateConnection();
        }
    }
}
