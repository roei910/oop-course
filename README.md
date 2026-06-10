# running the project
run the backend server in c# .net, run the frontend project in angular node version 20 or higher

# using deployed version of the project
frontend: https://stock-collection.netlify.app/#/
frontend option2: https://oop-course-frontend.onrender.com/#/
backend swagger: https://oop-course.onrender.com/swagger

# running backend
set the StocksApi as startup project, 
add user secrets with CONNECTION_STRING for mongodb.
build and run the project

# running frontend
go to the FinanceGrid.WebClient folder, run npm install command and then run "npm start" / "ng serve" command
there is an option to use configurations like local, development and production which changes the backend server url.

# versions
npm @9.8.1 or higher
node @18.16.0 or higher
angular @16.1.0 or higher

# Database Configuration

The FinanceGrid microservices support both SQLite (default) and PostgreSQL databases.

## SQLite (Default)

```bash
# Docker Compose (default)
docker-compose up

# Aspire AppHost
# Uses SQLite by default (no additional configuration needed)
dotnet run --project FinanceGrid/FinanceGrid.AppHost
```

## PostgreSQL

```bash
# Docker Compose (requires PostgreSQL profile)
DATABASE_PROVIDER=PostgreSQL docker-compose --profile postgres up

# Aspire AppHost
DatabaseProvider=PostgreSQL dotnet run --project FinanceGrid/FinanceGrid.AppHost
```

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `DATABASE_PROVIDER` | Database provider (`SQLite` or `PostgreSQL`) | `SQLite` |
| `ConnectionStrings__FinancialData` | FinancialData connection string | `Data Source=FinancialData.db` |
| `ConnectionStrings__Users` | Users connection string | `Data Source=Users.db` |
| `ConnectionStrings__Webhook` | Webhook connection string | `Data Source=Webhook.db` |

## Configuration via .env File

The `.env` file in the repository root contains all database configuration options. 
To switch to PostgreSQL, edit the file and set:

```bash
DATABASE_PROVIDER=PostgreSQL
```

Then uncomment the PostgreSQL connection strings and configure credentials as needed.

## Important Notes

- **StocksApi** uses MongoDB and is not affected by this configuration
- **EF Core Migrations**: Each provider requires separate migration sets
- **Data Migration**: No automatic migration between SQLite and PostgreSQL; use a custom ETL script
- **Testing**: Integration tests should cover both SQLite and PostgreSQL providers