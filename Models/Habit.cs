namespace HabitHeroAPI.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g., Health, Productivity
        public int XP { get; set; } = 0; // Points gained
        public int Streak { get; set; } = 0; // Consecutive days
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
