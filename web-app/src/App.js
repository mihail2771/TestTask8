import React, { useState, useEffect } from "react";
import axios from "axios";
import "./App.css";
import TimeSeetTable from "./TimeSeetTable";

function App() {
  const [timeSeet, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [limit] = useState(10);
  const [employees, setEmployees] = useState([]);
  const [form, setForm] = useState({
    id: 0,
    employee: 0,
    reason: 0,
    startDate: "",
    duration: 1,
    discounted: false,
    description: "",
  });

  const fetchEmployees = async () => {
    try {
      const res = await axios.get(`${process.env.REACT_APP_API_URL}/employee`);
      setEmployees(res.data);
    } catch (error) {
      console.error("Error fetching employees:", error);
    }
  };
  useEffect(() => {
    fetchEmployees();
  }, []);

  useEffect(() => {
    fetchData(pageNumber, limit);
  }, [pageNumber, limit]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm({
      ...form,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post(`${process.env.REACT_APP_API_URL}/`, form);

      fetchData(pageNumber, limit);
      setForm({
        id: 0,
        employee: 0,
        reason: 0,
        startDate: "",
        duration: 1,
        discounted: false,
        description: "",
      });
    } catch (error) {
      console.error("Error App handleSubmit: ", error);
    }
  };

  const fetchData = async (pageNumber, limit) => {
    try {
      const res = await axios.get(
        `${process.env.REACT_APP_API_URL}/TimeSheet`,
        {
          params: { pageNumber, limit },
        }
      );
      setData(res.data.timeSheets);
      setPageNumber(res.data.pageNumber);
      setTotalPages(res.data.totalPages);
    } catch (error) {
      console.error("Error App fetchData: ", error);
    }
  };

  const handlePreviousPage = () => {
    if (pageNumber > 1) {
      setPageNumber(pageNumber - 1);
    }
  };

  const handleNextPage = () => {
    if (pageNumber < totalPages) {
      setPageNumber(pageNumber + 1);
    }
  };

  const updateTimeSeet = async (editedTimeSeet) => {
    try {
      await axios.put(
        `${process.env.REACT_APP_API_URL}/${editedTimeSeet.id}`,
        editedTimeSeet
      );
      fetchData(pageNumber, limit);
    } catch (error) {
      console.error("Error updating timeSheet:", error);
    }
  };

  const deleteTimeSeet = async (editedTimeSeet) => {
    try {
      await axios.delete(
        `${process.env.REACT_APP_API_URL}/${editedTimeSeet.id}`
      );
      fetchData(pageNumber, limit);
    } catch (error) {
      console.error("Error updating timeSheet:", error);
    }
  };

  return (
    <div className="container">
      <div className="App">
        <h1>Учет отсутствия сотрудников на рабочем месте</h1>
        <form onSubmit={handleSubmit}>
          <div>
            <label>Сотрудник:</label>
            <select
              name="employee"
              value={form.employee}
              onChange={handleChange}
              required
            >
              <option value="">Выбор сотрудника</option>
              {employees.map((emp) => (
                <option
                  key={emp.id}
                  value={emp.id}
                >{`${emp.firstName} ${emp.lastName} (${emp.id})`}</option>
              ))}
            </select>
          </div>
          <div>
            <label>Причина отсутствия:</label>
            <select
              name="reason"
              value={form.reason}
              onChange={handleChange}
              required
            >
              <option value="">Выбор причины</option>
              <option value={1}>Отпуск</option>
              <option value={2}>Больничный</option>
              <option value={3}>Прогул</option>
            </select>
          </div>
          <div>
            <label>Дата начала:</label>
            <input
              type="date"
              name="startDate"
              value={form.startDate}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label>Продолжительность (раб. дней):</label>
            <input
              type="number"
              min="1"
              name="duration"
              value={form.duration}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label>Учтено при оплате:</label>
            <input
              type="checkbox"
              name="discounted"
              checked={form.discounted}
              onChange={(e) =>
                setForm({ ...form, discounted: e.target.checked })
              }
            />
          </div>
          <div>
            <label>Комментарий:</label>
            <textarea
              name="description"
              value={form.description}
              onChange={handleChange}
            />
          </div>
          <button type="submit">Добавить</button>
        </form>
        <TimeSeetTable
          timeSeet={timeSeet}
          employees={employees}
          updateTimeSeet={updateTimeSeet}
          deleteTimeSeet={deleteTimeSeet}
        />
        <div className="pagination">
          <button onClick={handlePreviousPage} disabled={pageNumber === 1}>
            Назад
          </button>
          <span>
            Страница {pageNumber} из {totalPages}
          </span>
          <button onClick={handleNextPage} disabled={pageNumber === totalPages}>
            Далее
          </button>
        </div>
      </div>
    </div>
  );
}

export default App;
