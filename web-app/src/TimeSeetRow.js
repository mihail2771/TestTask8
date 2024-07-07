import React, { useState, useEffect } from "react";
import "./App.css";

function TimeSeetRow({ timeSeet, employee, updateTimeSeet, deleteTimeSeet }) {
  const [isEditing, setIsEditing] = useState(false);
  const [editedTimeSeet, setEditedTimeSeet] = useState({ ...timeSeet });

  useEffect(() => {
    setEditedTimeSeet({ ...timeSeet });
  }, [timeSeet]);

  const handleEditChange = (e) => {
    const { name, checked, type, value } = e.target;
    setEditedTimeSeet({
      ...editedTimeSeet,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSave = async () => {
    try {
      await updateTimeSeet(editedTimeSeet);
      setIsEditing(false);
    } catch (error) {
      console.error("Error saving record:", error);
    }
  };

  const handleDelete = async () => {
    try {
      await deleteTimeSeet(editedTimeSeet);
      setIsEditing(false);
    } catch (error) {
      console.error("Error saving record:", error);
    }
  };
  
  return (
    <tr id={isEditing ? editedTimeSeet.id : timeSeet.id} value={isEditing ? editedTimeSeet.id : timeSeet.id}>
      <td>{`${employee.firstName} ${employee.lastName} (${employee.id})`}</td>
      <td>
        {
          <select
            name="reason"
            value={editedTimeSeet.reason}
            onChange={handleEditChange}
            disabled={!isEditing}
          >
            <option value="">Выбор причины</option>
            <option value={1}>Отпуск</option>
            <option value={2}>Больничный</option>
            <option value={3}>Прогул</option>
          </select>
        }
      </td>
      <td>
        {
          <input
            type="date"
            name="startDate"
            value={
              isEditing
                ? editedTimeSeet.startDate.split("T")[0]
                : timeSeet.startDate.split("T")[0]
            }
            onChange={handleEditChange}
            disabled={!isEditing}
          />
        }
      </td>
      <td>
        {
          <input
            type="number"
            min="1"
            name="duration"
            value={isEditing ? editedTimeSeet.duration : timeSeet.duration}
            onChange={handleEditChange}
            disabled={!isEditing}
          />
        }
      </td>
      <td>
        {
          <input
            type="checkbox"
            name="discounted"
            checked={
              isEditing ? editedTimeSeet.discounted : timeSeet.discounted
            }
            onChange={handleEditChange}
            disabled={!isEditing}
          />
        }
      </td>
      <td>
        {
          <textarea
            name="description"
            value={
              isEditing ? editedTimeSeet.description : timeSeet.description
            }
            onChange={handleEditChange}
            disabled={!isEditing}
          />
        }
      </td>
      <td className="actions">
        {isEditing ? (
          <>
            <button className="save" onClick={handleSave}>
              Сохранить
            </button>
            <button className="cancel" onClick={() => setIsEditing(false)}>
              Отмена
            </button>
          </>
        ) : (
          <>
            <button className="edit" onClick={() => setIsEditing(true)}>
              Изменить
            </button>
            <button className="delete" onClick={handleDelete}>
              Удалить
            </button>
          </>
        )}
      </td>
    </tr>
  );
}

export default TimeSeetRow;
