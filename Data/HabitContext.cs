using Microsoft.EntityFrameworkCore;
using HabitHeroAPI.Models;

namespace HabitHeroAPI.Data
{
    public class HabitContext : DbContext
    {
        public HabitContext(DbContextOptions<HabitContext> options) : base(options) { }

        public DbSet<Habit> Habits { get; set; }
    }
}
