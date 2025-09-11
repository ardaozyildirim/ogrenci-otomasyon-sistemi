-- Initialize the database for Student Management System
-- This script will be run when the PostgreSQL container starts for the first time

-- Enable UUID extension (if needed in future)
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create initial admin user (password is 'admin123')
-- This will be created after Entity Framework migrations are applied

-- Add missing Status column to Courses table if it doesn't exist
-- This handles the case where the migration doesn't include the Status column
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Courses') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Courses' AND column_name = 'Status') THEN
            ALTER TABLE "Courses" ADD COLUMN "Status" integer NOT NULL DEFAULT 1;
        END IF;
    END IF;
END $$;

SELECT 'Database initialization script executed successfully.' as message;