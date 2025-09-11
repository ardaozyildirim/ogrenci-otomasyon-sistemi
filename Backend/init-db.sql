-- Initialize the database for Student Management System
-- This script will be run when the PostgreSQL container starts for the first time

-- Enable UUID extension (if needed in future)
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Note: Admin user is created by the application during startup, not in this script
-- See SeedData method in Program.cs for admin user creation

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

-- Add soft delete columns to all tables if they don't exist
DO $$
BEGIN
    -- Add to Students table
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Students') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Students' AND column_name = 'IsDeleted') THEN
            ALTER TABLE "Students" ADD COLUMN "IsDeleted" boolean NOT NULL DEFAULT false;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Students' AND column_name = 'DeletedAt') THEN
            ALTER TABLE "Students" ADD COLUMN "DeletedAt" timestamp with time zone;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Students' AND column_name = 'DeletedBy') THEN
            ALTER TABLE "Students" ADD COLUMN "DeletedBy" text;
        END IF;
    END IF;

    -- Add to Teachers table
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Teachers') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Teachers' AND column_name = 'IsDeleted') THEN
            ALTER TABLE "Teachers" ADD COLUMN "IsDeleted" boolean NOT NULL DEFAULT false;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Teachers' AND column_name = 'DeletedAt') THEN
            ALTER TABLE "Teachers" ADD COLUMN "DeletedAt" timestamp with time zone;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Teachers' AND column_name = 'DeletedBy') THEN
            ALTER TABLE "Teachers" ADD COLUMN "DeletedBy" text;
        END IF;
    END IF;

    -- Add to Courses table
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Courses') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Courses' AND column_name = 'IsDeleted') THEN
            ALTER TABLE "Courses" ADD COLUMN "IsDeleted" boolean NOT NULL DEFAULT false;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Courses' AND column_name = 'DeletedAt') THEN
            ALTER TABLE "Courses" ADD COLUMN "DeletedAt" timestamp with time zone;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Courses' AND column_name = 'DeletedBy') THEN
            ALTER TABLE "Courses" ADD COLUMN "DeletedBy" text;
        END IF;
    END IF;

    -- Add to Users table
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Users') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Users' AND column_name = 'IsDeleted') THEN
            ALTER TABLE "Users" ADD COLUMN "IsDeleted" boolean NOT NULL DEFAULT false;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Users' AND column_name = 'DeletedAt') THEN
            ALTER TABLE "Users" ADD COLUMN "DeletedAt" timestamp with time zone;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Users' AND column_name = 'DeletedBy') THEN
            ALTER TABLE "Users" ADD COLUMN "DeletedBy" text;
        END IF;
    END IF;

    -- Add to Grades table
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Grades') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Grades' AND column_name = 'IsDeleted') THEN
            ALTER TABLE "Grades" ADD COLUMN "IsDeleted" boolean NOT NULL DEFAULT false;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Grades' AND column_name = 'DeletedAt') THEN
            ALTER TABLE "Grades" ADD COLUMN "DeletedAt" timestamp with time zone;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Grades' AND column_name = 'DeletedBy') THEN
            ALTER TABLE "Grades" ADD COLUMN "DeletedBy" text;
        END IF;
    END IF;

    -- Add to Attendances table
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Attendances') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Attendances' AND column_name = 'IsDeleted') THEN
            ALTER TABLE "Attendances" ADD COLUMN "IsDeleted" boolean NOT NULL DEFAULT false;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Attendances' AND column_name = 'DeletedAt') THEN
            ALTER TABLE "Attendances" ADD COLUMN "DeletedAt" timestamp with time zone;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'Attendances' AND column_name = 'DeletedBy') THEN
            ALTER TABLE "Attendances" ADD COLUMN "DeletedBy" text;
        END IF;
    END IF;

    -- Add to StudentCourses table
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'StudentCourses') THEN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'StudentCourses' AND column_name = 'IsDeleted') THEN
            ALTER TABLE "StudentCourses" ADD COLUMN "IsDeleted" boolean NOT NULL DEFAULT false;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'StudentCourses' AND column_name = 'DeletedAt') THEN
            ALTER TABLE "StudentCourses" ADD COLUMN "DeletedAt" timestamp with time zone;
        END IF;
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name = 'StudentCourses' AND column_name = 'DeletedBy') THEN
            ALTER TABLE "StudentCourses" ADD COLUMN "DeletedBy" text;
        END IF;
    END IF;
END $$;

SELECT 'Database initialization script executed successfully.' as message;