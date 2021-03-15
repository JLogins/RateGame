using System.Linq;
using System.Threading.Tasks;

namespace RateGame
{
    internal class DBSeeder
    {
        //TODO interfaces
        internal static async Task InitAsync(RateGameContext context, RawgIOService service)
        {
            context.Database.EnsureCreated();

            //No games found
            if (!context.Games.Any())
            {
                var games = await service.Download();

                if (games != null)
                {
                    context.Games.AddRange(games);
                }

                await context.SaveChangesAsync();
            }
        }

        internal static async Task RemoveAll(RateGameContext context)
        {
            var games = context.Games;
            if (games.Any())
            {
                context.Games.RemoveRange(games);
                await context.SaveChangesAsync();
            }
        }
    }
}