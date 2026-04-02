using FitnessTracker.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeightsController(AppDbContext context) : ControllerBase
{
    [HttpGet("catalog")]
    public async Task<ActionResult<WeightCatalogResponse>> GetCatalog()
    {
        var dumbel = await context.WeightDb
            .OrderBy(w => w.DisplayOrder)
            .Select(w => w.Value)
            .ToListAsync();

        var bar = await context.WeightBar
            .OrderBy(w => w.DisplayOrder)
            .Select(w => w.Value)
            .ToListAsync();

        var machine = await context.WeightMachine
            .OrderBy(w => w.DisplayOrder)
            .Select(w => w.Value)
            .ToListAsync();

        return Ok(new WeightCatalogResponse
        {
            Dumbel = dumbel,
            Bar = bar,
            Machine = machine
        });
    }
}

public class WeightCatalogResponse
{
    public List<decimal> Dumbel { get; set; } = [];
    public List<decimal> Bar { get; set; } = [];
    public List<decimal> Machine { get; set; } = [];
}
