using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HabitHeroAPI.Data;
using HabitHeroAPI.Models;

namespace HabitHeroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitContoller : ControllerBase
    {
        private readonly HabitContext _context;
        public HabitContoller(HabitContext context)
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

    }
    
    
}
