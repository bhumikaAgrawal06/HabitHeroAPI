using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HabitHeroAPI.Data;
using HabitHeroAPI.Models;

namespace HabitHeroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitController : ControllerBase
    {
        private readonly HabitContext _context;
        public HabitController(HabitContext context)
        {
            _context = context;
        }

        // Add methods for CRUD operations here

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Habit>>> GetHabits()
        {
            var items = await _context.Habits.ToListAsync();
            return Ok(items);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Habit>> GetHabit(int id)
        {
            var item = await _context.Habits.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Habit>> Create([FromBody] HabitCreateDto dto)
        {
            var habit = new Habit
            {
                Name = dto.Name,
                Category = dto.Category,
                XP = 0,
                Streak = 0,
                LastUpdated = DateTime.UtcNow
            };

            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habit);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Habit>> Update(int id, [FromBody] HabitUpdateDto dto)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit is null) return NotFound();

            habit.Name = dto.Name;
            habit.Category = dto.Category;
            await _context.SaveChangesAsync();

            return Ok(habit);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit is null) return NotFound();

            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id:int}/complete")]
        public async Task<ActionResult<Habit>> Complete(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit is null) return NotFound();

            var today = DateTime.UtcNow.Date;
            var last = habit.LastUpdated.Date;

            if (last == today)
            {
                // already completed today: keep streak
            }
            else if (last == today.AddDays(-1))
            {
                habit.Streak += 1;    // continued streak
            }
            else
            {
                habit.Streak = 1;     // reset streak
            }

            habit.XP += 10;            // award XP
            habit.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(habit);
        }

    }
    
    
}
