using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        var employees = new List<Employee>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(@"
                    SELECT * FROM employees", connection);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    employees.Add(new Employee(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("last_name")),
                        reader.GetString(reader.GetOrdinal("first_name")))
                    );
                }
            }
        }

        return employees;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        Employee? employee = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(@"
                    SELECT * FROM employees 
                    WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    employee = new Employee(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("last_name")),
                        reader.GetString(reader.GetOrdinal("first_name")));
                }
            }
        }

        return employee;
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(@"
                    INSERT INTO employees (last_name, first_name) 
                    VALUES (@LastName, @FirstName)", connection);

            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateEmployeeAsync(Employee employee, int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(@"
                    UPDATE employees SET 
                    last_name = @LastName,
                    first_name = @FirstName
                    WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(@"
                    DELETE FROM employees 
                    WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
