import "./App.css";
import TimeSeetRow from "./TimeSeetRow";

function TimeSeetTable({
  timeSeet,
  employees,
  updateTimeSeet,
  deleteTimeSeet,
}) {
  return (
    <div className="table-wrapper">
      <table>
        <thead>
          <tr>
            <th>Сотрудник</th>
            <th>Причина отсутствия</th>
            <th>Дата начала</th>
            <th>Продолжительность (раб. дней)</th>
            <th>Учтено при оплате</th>
            <th>Комментарий</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {timeSeet.map((timeSeet) => (
            <TimeSeetRow
              timeSeet={timeSeet}
              employee={employees.find((emp) => timeSeet.employee === emp.id)}
              updateTimeSeet={updateTimeSeet}
              deleteTimeSeet={deleteTimeSeet}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default TimeSeetTable;
