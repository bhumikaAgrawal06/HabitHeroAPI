using HabitHeroAPI.Data;
using HabitHeroAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register EF Core with an in-memory database
builder.Services.AddDbContext<HabitContext>(options =>
    options.UseInMemoryDatabase("HabitList"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// (Optional) Seed a few habits at startup
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<HabitContext>();
    if (!ctx.Habits.Any())
    {
        ctx.Habits.AddRange(
            new Habit { Name = "Drink Water", Category = "Health", XP = 10, Streak = 1, LastUpdated = DateTime.UtcNow },
            new Habit { Name = "Read 10 pages", Category = "Learning", XP = 5, Streak = 2, LastUpdated = DateTime.UtcNow.AddDays(-1) }
        );
        ctx.SaveChanges();
    }
}

app.Run();
