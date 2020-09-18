using Microsoft.Extensions.Configuration;
using TreeHouse.Database;

namespace TreeHouse.Services
{
    public class DbService
    {
        private readonly IConfiguration _configuration;

        public DbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TreeHouseContext CreateConnection()
        {
            return new TreeHouseContext(_configuration["db"]);
        }
    }
}
