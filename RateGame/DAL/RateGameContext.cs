using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RateGame.DAL;

namespace RateGame
{
    public class RateGameContext : DbContext
    {
        public RateGameContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Game> Games {get;set;}
    }
}