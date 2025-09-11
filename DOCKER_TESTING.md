# Docker Testing Commands for Student Management System

## Check Docker Container Status
```bash
docker ps
```

## Check API Logs
```bash
docker logs student_management_api
```

## Check Database Logs
```bash
docker logs student_management_db
```

## Test API Health (using curl or your preferred tool)
```bash
# Test if API is responding
curl http://localhost:8080/swagger/index.html

# Test Registration Endpoint
curl -X POST "http://localhost:8080/api/auth/register" \
     -H "Content-Type: application/json" \
     -d '{
       "firstName": "John",
       "lastName": "Doe",
       "email": "john.doe@example.com",
       "password": "password123",
       "role": 1
     }'

# Test Login Endpoint  
curl -X POST "http://localhost:8080/api/auth/login" \
     -H "Content-Type: application/json" \
     -d '{
       "email": "john.doe@example.com",
       "password": "password123"
     }'
```

## Connect to PostgreSQL Database
```bash
# Connect to database container
docker exec -it student_management_db psql -U postgres -d student_management_system

# List tables
\dt

# Check users table
SELECT * FROM "Users";

# Exit
\q
```

## Docker Management Commands
```bash
# Stop containers
docker-compose down

# Start containers
docker-compose up -d

# Rebuild and start
docker-compose up --build -d

# View logs
docker-compose logs -f api
docker-compose logs -f postgres
```

## Environment Variables
- **Database**: PostgreSQL 16 Alpine
- **Database Name**: student_management_system
- **Database User**: postgres
- **Database Password**: Student123!
- **API Port**: 8080
- **Database Port**: 5432

## API Access
- **Swagger UI**: http://localhost:8080
- **Base API URL**: http://localhost:8080/api
- **Authentication**: JWT Bearer Token