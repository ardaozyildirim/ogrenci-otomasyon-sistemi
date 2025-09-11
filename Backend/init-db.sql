-- Initialize the database for Student Management System
-- This script will be run when the PostgreSQL container starts for the first time

-- Enable UUID extension (if needed in future)
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create initial admin user (password is 'admin123')
-- This will be created after Entity Framework migrations are applied

SELECT 'Database initialization script executed successfully.' as message;