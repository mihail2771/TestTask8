using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class TimeSheetRepository
{
    private readonly string _connectionString;

    public TimeSheetRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<TimeSheet>> GetAllTimeSheetsAsync()
    {
        var TimeSheets = new List<TimeSheet>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT * FROM timesheet", connection);
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    TimeSheets.Add(new TimeSheet(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetInt32(reader.GetOrdinal("employee")),
                        reader.GetInt32(reader.GetOrdinal("Reason")),
                        reader.GetDateTime(reader.GetOrdinal("Start_Date")),
                        reader.GetInt32(reader.GetOrdinal("Duration")),
                        reader.GetBoolean(reader.GetOrdinal("Discounted")),
                        reader.GetString(reader.GetOrdinal("Description"))
                    ));
                }
            }
        }

        return TimeSheets;
    }

    public async Task<(IEnumerable<TimeSheet>, int TotalPages)> GetAllTimeSheetsPagedAsync(int pageNumber, int pageSize)
    {
        var TimeSheets = new List<TimeSheet>();
        int totalRecords = 0;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var countCommand = new SqlCommand("SELECT COUNT(*) FROM timesheet", connection);
            totalRecords = (int)await countCommand.ExecuteScalarAsync();

            // Получаем записи для текущей страницы
            var offset = (pageNumber - 1) * pageSize;

            var command = new SqlCommand(@"
                    SELECT * FROM timesheet
                    ORDER BY id
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;", connection);

            command.Parameters.AddWithValue("@Offset", offset);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    TimeSheets.Add(new TimeSheet(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetInt32(reader.GetOrdinal("employee")),
                        reader.GetInt32(reader.GetOrdinal("Reason")),
                        reader.GetDateTime(reader.GetOrdinal("Start_Date")),
                        reader.GetInt32(reader.GetOrdinal("Duration")),
                        reader.GetBoolean(reader.GetOrdinal("Discounted")),
                        reader.GetString(reader.GetOrdinal("Description"))
                    ));
                }
            }
        }

        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        return (TimeSheets, totalPages);
    }

    public async Task<TimeSheet> GetTimeSheetByIdAsync(int id)
    {
        TimeSheet TimeSheet = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT * FROM timesheet WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    TimeSheet = new TimeSheet(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetInt32(reader.GetOrdinal("employee")),
                        reader.GetInt32(reader.GetOrdinal("Reason")),
                        reader.GetDateTime(reader.GetOrdinal("Start_Date")),
                        reader.GetInt32(reader.GetOrdinal("Duration")),
                        reader.GetBoolean(reader.GetOrdinal("Discounted")),
                        reader.GetString(reader.GetOrdinal("Description"))
                    );
                }
            }
        }

        return TimeSheet;
    }

    public async Task AddTimeSheetAsync(TimeSheet TimeSheet)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(@"
                    INSERT INTO timesheet (employee, Reason, Start_Date, Duration, Discounted, Description) 
                    VALUES (@Employee, @Reason, @StartDate, @Duration, @Discounted, @Description)", connection);

            command.Parameters.AddWithValue("@Employee", TimeSheet.Employee);
            command.Parameters.AddWithValue("@Reason", TimeSheet.Reason);
            command.Parameters.AddWithValue("@StartDate", TimeSheet.StartDate);
            command.Parameters.AddWithValue("@Duration", TimeSheet.Duration);
            command.Parameters.AddWithValue("@Discounted", TimeSheet.Discounted);
            command.Parameters.AddWithValue("@Description", TimeSheet.Description);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateTimeSheetAsync(TimeSheet TimeSheet, int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(@"
                    UPDATE timesheet SET 
                    employee = @Employee, 
                    Reason = @Reason, 
                    Start_Date = @StartDate, 
                    Duration = @Duration, 
                    Discounted = @Discounted, 
                    Description = @Description 
                    WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Employee", TimeSheet.Employee);
            command.Parameters.AddWithValue("@Reason", TimeSheet.Reason);
            command.Parameters.AddWithValue("@StartDate", TimeSheet.StartDate);
            command.Parameters.AddWithValue("@Duration", TimeSheet.Duration);
            command.Parameters.AddWithValue("@Discounted", TimeSheet.Discounted);
            command.Parameters.AddWithValue("@Description", TimeSheet.Description);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteTimeSheetAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("DELETE FROM timesheet WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
