# ALIF_Project

Краткое описание СТЭКА проекта (
Библиотека (Application) которая выполняет всю бизнес логику которая разбита на слои Onion Architecturё.
Паттерн CQRS + MediatR который распределяет нагрузку на БД. Используется LINQ + EntityFrameWorkCore.
AutoMapper позоляющий удобно работать с таблицами.

Авторизация пользовтеля (JWT Tokens)

Сам проект под названием API который совершает все запросы и связывается с Библиотекой (Application). 
Паттерн CQRS + MediatR.
 
Сам проект Написан на WebAPI ASP.NET (.Net 5)
)

Команда для создание миграции = dotnet ef migrations add InitMigrations -p Persistence -s API

![1234](https://user-images.githubusercontent.com/69799846/236067586-4ad566a0-2631-4874-84a3-b7df1e59fe70.png)
![2345](https://user-images.githubusercontent.com/69799846/236067592-9501939d-fceb-4371-a331-fe3e7edc0294.png)

![image](https://user-images.githubusercontent.com/69799846/236067880-08efb0e6-ce9a-43b2-a4be-7f0813bd21c4.png)

