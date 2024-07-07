/*
USE [master];
DROP LOGIN [test_user];
USE [test8];
DROP USER [test_user];

USE [master]
ALTER DATABASE test8 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [test8];
*/


-- 1. Создание БД
CREATE DATABASE test8;
GO

USE [test8];

-- 2. Создание таблиц employees, reasons, timesheet
CREATE TABLE [employees](
	[id] [int] not null IDENTITY primary key,
	[last_name] [nvarchar](256) not null,
	[first_name] [nvarchar](256) not null
);
GO

CREATE TABLE [reasons](
	[id] [int] not null IDENTITY primary key,
	[value] [nvarchar](256) not null,
);
GO

CREATE TABLE [timesheet](
	[id] [int] not null IDENTITY primary key,
	[employee] [int] not null,
	[reason] [int] not null,
	[start_date] [date] not null,
	[duration] [int] not null,
	[discounted] [bit] not null,
	[description] [nvarchar](256) not null,
	FOREIGN KEY (employee) REFERENCES [employees](id),
	FOREIGN KEY (reason) REFERENCES [reasons](id)
);
GO

-- 3. Добавление тестовых данных в таблицы 
INSERT INTO [employees] ([last_name], [first_name]) VALUES
('Иванов', 'Иван'),
('Петров', 'Петр'),
('Сидоров', 'Сидор'),
('Кузнецов', 'Алексей'),
('Смирнов', 'Александр'),
('Попов', 'Владимир'),
('Соколов', 'Михаил'),
('Морозов', 'Сергей'),
('Волков', 'Андрей');

INSERT INTO [reasons] ([value]) VALUES
('Отпуск'),
('Больничный'),
('Прогул');
GO

/*
INSERT INTO [timesheet] (employee, reason, start_date, duration, discounted, description) VALUES
(1, 1, '2024-04-07', 8, 1, 'Отпуск на неделю'),
(2, 1, '2024-07-06', 6, 0, 'Отпуск без сохранения зарплаты'),
(3, 1, '2024-03-03', 4, 1, 'Краткосрочный отпуск'),
(4, 2, '2024-05-04', 8, 1, 'Больничный, травма'),
(5, 2, '2024-06-01', 7, 1, 'Больничный, операция'),
(6, 1, '2024-01-06', 5, 1, 'Отпуск по семейным обстоятельствам'),
(7, 2, '2024-07-07', 9, 1, 'Больничный, лечение'),
(8, 2, '2024-03-01', 8, 1, 'Больничный, длительное лечение'),
(9, 3, '2024-06-03', 1, 0, 'Прогул, неявка на работу'),
(1, 2, '2024-04-10', 8, 1, 'Больничный, реабилитация');
*/

-- 4. Созданиу пользователя и выдача прав
USE [master];
CREATE LOGIN test_user WITH PASSWORD = 'Test123321';
GO

USE [test8];
CREATE USER test_user FOR LOGIN test_user;
GO

EXEC sp_addrolemember 'db_datareader', 'test_user';
EXEC sp_addrolemember 'db_datawriter', 'test_user';
