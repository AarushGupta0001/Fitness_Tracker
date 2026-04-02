using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Workout>>> GetAll()
    {
        var workouts = await dbContext.Workouts
            .OrderByDescending(w => w.Date)
            .ToListAsync();

        return Ok(workouts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Workout>> GetById(int id)
    {
        var workout = await dbContext.Workouts.FindAsync(id);
        return workout is null ? NotFound() : Ok(workout);
    }

    [HttpPost]
    public async Task<ActionResult<Workout>> Create(Workout workout)
    {
        dbContext.Workouts.Add(workout);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = workout.Id }, workout);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Workout workout)
    {
        if (id != workout.Id)
        {
            return BadRequest("Route id must match payload id.");
        }

        var exists = await dbContext.Workouts.AnyAsync(w => w.Id == id);
        if (!exists)
        {
            return NotFound();
        }

        dbContext.Entry(workout).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var workout = await dbContext.Workouts.FindAsync(id);
        if (workout is null)
        {
            return NotFound();
        }

        dbContext.Workouts.Remove(workout);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}