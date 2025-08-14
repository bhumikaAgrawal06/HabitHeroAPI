using System.ComponentModel.DataAnnotations;

namespace HabitHeroAPI.Models
{
    public class Habit
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(40)]
        public string Category { get; set; } = string.Empty; // e.g., Health, Productivity

        public int XP { get; set; } = 0; // Points gained

        public int Streak { get; set; } = 0; // Consecutive days

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    }
}
