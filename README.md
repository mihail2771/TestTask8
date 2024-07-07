# TestTask8
1. Выполнить скрипт в БД: [SQL.sql](https://github.com/mihail2771/TestTask8/blob/main/SQL.sql) для создания БД, таблиц и тестовых данных

2. Скачать билды проектов:
   - [webapi](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2Fmihail2771%2FTestTask8%2Ftree%2Fmain%2Fwebapi%2Fbin%2FRelease%2Fnet8.0%2Fpublish) 
   - [web-app ](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2Fmihail2771%2FTestTask8%2Ftree%2Fmain%2Fweb-app%2Fbuild)

   2.1 При необходимости отредактируйте строку подключения к БД  вписав свои параметры - файл appsettings.json :
      "Server=XADMY\\SQLEXPRESS;Database=test8;User Id=test_user;Password=Test123321;TrustServerCertificate=True;"

   2.2 Проверьте настройки сервера БД:
      
      2.2.1 В Object Explorer, щелкните правой кнопкой мыши на имя вашего сервера и выберите "Properties" (Свойства).
      
      2.2.2 Перейдите на вкладку "Connections" (Подключения).
      
      2.2.3 Убедитесь, что установлена галочка "Allow remote connections to this server" (Разрешить удаленные подключения к этому серверу).
      

4. Добавить веб-сайт в Диспечере IIS, указав пути до соответствуещего проекта:
   
Имя: TimeSheetsAPI

Порт: 5106

![image](https://github.com/mihail2771/TestTask8/assets/47285121/9fc27d1d-4853-449f-91ad-a43020e73167)

Имя: TimeSheetsWeb

Порт: 3001

![image](https://github.com/mihail2771/TestTask8/assets/47285121/82647d02-efaa-4b72-940c-647a9a0d3dc4)

