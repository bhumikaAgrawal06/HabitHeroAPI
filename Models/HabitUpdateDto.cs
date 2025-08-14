using System.ComponentModel.DataAnnotations;

namespace HabitHeroAPI.Models
{
    public class HabitUpdateDto
    {
        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(40)]
        public string Category { get; set; } = string.Empty;
    }
}
