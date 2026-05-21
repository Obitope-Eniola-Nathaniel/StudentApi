HTTP Request arrives
↓
ASP.NET Core creates StudentsController
↓
StudentsController needs IStudentRepository
↓
ASP.NET Core creates StudentRepository
↓
StudentRepository needs AppDbContext
↓
ASP.NET Core creates AppDbContext
↓
Everything gets injected automatically
↓
Controller executes



Request hits controller
↓
Controller asks for repository
↓
ASP.NET Core injects repository
↓
Repository asks for DbContext
↓
ASP.NET Core injects DbContext
↓
Repository accesses PostgreSQL
↓
Data returned to controller
↓
Controller returns HTTP response