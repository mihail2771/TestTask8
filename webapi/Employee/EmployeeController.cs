using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _employeeRepository;

    public EmployeeController(EmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Employee>> Get()
    {
        return await _employeeRepository.GetAllEmployeesAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> Get(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return employee;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Employee employee)
    {
        await _employeeRepository.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (existingEmployee == null)
        {
            return NotFound();
        }

        await _employeeRepository.UpdateEmployeeAsync(employee, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (existingEmployee == null)
        {
            return NotFound();
        }

        await _employeeRepository.DeleteEmployeeAsync(id);
        return NoContent();
    }
}
