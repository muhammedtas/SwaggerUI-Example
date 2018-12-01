# SwaggerUI-Example
Fundamentals presentation of SwaggerUI over .net core web api.

Firstly restore and build your application. Then

1- Type your connection string into appSettings.json file to create your db.
2- Create your db according to domain classes with "dotnet ef migrations add "" and update command codes. 
3- Hit "dotnet watch run" 
4- Be sure the seed method is executed and fill some of the tables to see it in the swaggerUI layer
5- Open your browser and go to "https://localhost:5000/swagger/" 
