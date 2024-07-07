using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("/")]
public class TimeSheetController : ControllerBase
{
    private readonly TimeSheetRepository _timeSheetRepository;

    public TimeSheetController(TimeSheetRepository timeSheetRepository)
    {
        _timeSheetRepository = timeSheetRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<TimeSheet>> Get()
    {
        return await _timeSheetRepository.GetAllTimeSheetsAsync();
    }

    [HttpGet]
    [Route("TimeSheet")]
    public async Task<ActionResult> GetTimeSheetsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var (timeSheets, totalPages) = await _timeSheetRepository.GetAllTimeSheetsPagedAsync(pageNumber, pageSize);
        return Ok(new
        {
            PageNumber = pageNumber,
            TotalPages = totalPages,
            TimeSheets = timeSheets
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeSheet>> Get(int id)
    {
        var TimeSheet = await _timeSheetRepository.GetTimeSheetByIdAsync(id);
        if (TimeSheet == null)
        {
            return NotFound();
        }
        return TimeSheet;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TimeSheet TimeSheet)
    {
        await _timeSheetRepository.AddTimeSheetAsync(TimeSheet);
        return CreatedAtAction(nameof(Get), new { id = TimeSheet.Id }, TimeSheet);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] TimeSheet TimeSheet)
    {
        if (id != TimeSheet.Id)
        {
            return BadRequest();
        }

        var existingTimeSheet = await _timeSheetRepository.GetTimeSheetByIdAsync(id);
        if (existingTimeSheet == null)
        {
            return NotFound();
        }

        await _timeSheetRepository.UpdateTimeSheetAsync(TimeSheet, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existingTimeSheet = await _timeSheetRepository.GetTimeSheetByIdAsync(id);
        if (existingTimeSheet == null)
        {
            return NotFound();
        }

        await _timeSheetRepository.DeleteTimeSheetAsync(id);
        return NoContent();
    }
}
