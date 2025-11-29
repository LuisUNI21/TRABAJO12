# Migrations for Task Manager API

This directory contains the Entity Framework Core migrations for the Task Manager API's database schema. Migrations are used to keep the database schema in sync with the application's data model.

## How to Apply Migrations

To apply the migrations to your database, use the following command in the Package Manager Console or the command line:

```
dotnet ef database update
```

## Creating a New Migration

To create a new migration after making changes to the data model, use the following command:

```
dotnet ef migrations add <MigrationName>
```

Replace `<MigrationName>` with a descriptive name for the migration.

## Important Notes

- Ensure that your database connection string is correctly configured in the `appsettings.json` file before applying migrations.
- Always backup your database before applying new migrations, especially in production environments.
- Review the generated migration files to understand the changes being applied to the database schema.