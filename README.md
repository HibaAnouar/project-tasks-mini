# Project Tasks Mini

A full-stack mini-project to manage **projects** and **tasks** .

---

## Tools Used

- **Backend**: .NET 10, EF Core, SQLite, JWT Authentication  
- **Frontend**: React 18 + Vite, Plain CSS  
- **Database**: SQLite

---

## Clone Project

```bash 
# Clone the repository
git clone https://github.com/HibaAnouar/project-tasks-mini.git

# Navigate into the project folder
cd project-tasks-mini
```
# Backend Setup

## Navigate to backend folder
```bash
cd backend
```

## Restore dependencies
```bash
dotnet restore
```

## Add initial migration (if not already added)
```bash
dotnet ef migrations add Init
```

## Apply migration to create database
```bash
dotnet ef database update
```

## Run the backend
```bash
dotnet run
```
# Frontend Setup

## Navigate to frontend folder
```bash
cd frontend
```

## Install dependencies
```bash
npm install
```

## Run the frontend in development mode
```bash
npm run dev
```
Frontend will run at: http://localhost:5173

# Database Setup

## Tables:

Users: id, email, passwordHash

Projects: id, title, description (optional), userId

Tasks: id, title, description (optional), dueDate (optional), isCompleted, projectId

## Use EF Core migrations to create or update the database:
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update 
```

# Usage

## Login:

Navigate to the frontend /login page

Enter your email and password to get access

## Projects:

Create a new project with a title (description optional)

View all projects in the list

Delete a project using the Delete button

## Project Details / Tasks:

Click a project to view its tasks

Add a new task with title (description and due date optional)

Mark tasks as completed

Delete tasks if needed

Progress bar shows the percentage of completed tasks

