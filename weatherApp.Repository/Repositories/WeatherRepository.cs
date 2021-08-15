using weatherApp.DataAccess.Models;
using weatherApp.Repository.WeatherAppDbContext;

namespace weatherApp.Repository.Repositories
{
    public class WeatherRepository : RepositoryBase<WeatherTbl, WeatherDbContext>
    {
        private readonly WeatherDbContext _context;
        public WeatherRepository(WeatherDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
