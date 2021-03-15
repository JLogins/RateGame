using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RateGame.DAL
{
    public interface IGameRepository
    {
        Task<List<Game>> FindByTitle(string queryString);
        Task<List<Game>> Get(int page = 0);
    }

    public class GameRepository : IGameRepository
    {
        private readonly RateGameContext _context;
        const int DefaultPageSize = 20;

        public GameRepository(RateGameContext context)
        {
            _context = context;
        }
        public Task<List<Game>> FindByTitle(string queryString)
        {
            return _context.Games.Where(x => x.Title.Contains(queryString)).ToListAsync();
        }

         public Task<List<Game>> Get(int page = 0)
        {
            return _context.Games.Take(DefaultPageSize).ToListAsync();
        }
    }
}
