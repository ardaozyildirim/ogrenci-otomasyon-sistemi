'use client';

import React, { useState } from 'react';
import { apiService } from '@/lib/api';

interface SeedDataStats {
  users: number;
  students: number;
  teachers: number;
  courses: number;
  grades: number;
  attendance: number;
  errors: number;
}

export default function DataSeederPage() {
  const [isSeeding, setIsSeeding] = useState(false);
  const [stats, setStats] = useState<SeedDataStats>({ 
    users: 0, students: 0, teachers: 0, courses: 0, grades: 0, attendance: 0, errors: 0 
  });
  const [logs, setLogs] = useState<string[]>([]);
  const [availableData, setAvailableData] = useState<{
    students: any[];
    courses: any[];
  }>({ students: [], courses: [] });

  const addLog = (message: string) => {
    setLogs(prev => [...prev.slice(-19), `${new Date().toLocaleTimeString()}: ${message}`]);
  };

  // Fetch existing data to understand what's available
  const checkAvailableData = async () => {
    addLog('🔍 Checking available data...');
    
    try {
      const [students, courses] = await Promise.all([
        apiService.getStudents(),
        apiService.getCourses()
      ]);
      
      // Ensure arrays are properly defined
      const safeStudents = Array.isArray(students) ? students : [];
      const safeCourses = Array.isArray(courses) ? courses : [];
      
      setAvailableData({ students: safeStudents, courses: safeCourses });
      addLog(`📊 Found: ${safeStudents.length} students, ${safeCourses.length} courses`);
      
      return { students: safeStudents, courses: safeCourses };
    } catch (error: any) {
      addLog(`❌ Failed to fetch data: ${error.message}`);
      return { students: [], courses: [] };
    }
  };

  // Create sample users if needed
  const createSampleUsers = async () => {
    addLog('👥 Creating sample users...');
    
    const sampleUsers = [
      {
        firstName: 'Ali',
        lastName: 'Yılmaz',
        email: 'ali.yilmaz@student.com',
        password: 'Student123',
        role: 'Student' as const,
        phoneNumber: '0532-123-4567'
      },
      {
        firstName: 'Ayşe',
        lastName: 'Demir',
        email: 'ayse.demir@student.com',
        password: 'Student123',
        role: 'Student' as const,
        phoneNumber: '0532-123-4568'
      },
      {
        firstName: 'Dr. Mehmet',
        lastName: 'Öztürk',
        email: 'mehmet.ozturk@teacher.com',
        password: 'Teacher123',
        role: 'Teacher' as const,
        phoneNumber: '0532-123-4569'
      },
      {
        firstName: 'Dr. Fatma',
        lastName: 'Kaya',
        email: 'fatma.kaya@teacher.com',
        password: 'Teacher123',
        role: 'Teacher' as const,
        phoneNumber: '0532-123-4570'
      }
    ];

    const createdUsers = [];
    let created = 0;
    let errors = 0;

    for (const user of sampleUsers) {
      try {
        const response = await apiService.register(user);
        createdUsers.push({ ...user, userId: response.userId });
        created++;
        addLog(`✅ User created: ${user.firstName} ${user.lastName} (${user.role})`);
      } catch (error: any) {
        errors++;
        addLog(`❌ Failed to create user ${user.firstName}: ${error.message}`);
      }
    }

    setStats(prev => ({ ...prev, users: prev.users + created, errors: prev.errors + errors }));
    addLog(`👥 User creation completed: ${created} created, ${errors} errors`);
    
    return createdUsers;
  };

  // Create sample students
  const createSampleStudents = async (users: any[]) => {
    addLog('🎓 Creating student records...');
    
    const studentUsers = users.filter(u => u.role === 'Student');
    const createdStudents = [];
    let created = 0;
    let errors = 0;

    for (let i = 0; i < studentUsers.length; i++) {
      const user = studentUsers[i];
      try {
        const studentData = {
          userId: user.userId,
          studentNumber: `2024${String(i + 1).padStart(4, '0')}`,
          department: 'Computer Engineering',
          grade: 85 + (i * 5)
        };
        
        const studentId = await apiService.createStudent(studentData);
        createdStudents.push({ ...studentData, id: studentId, user });
        created++;
        addLog(`✅ Student created: ${user.firstName} ${user.lastName} (${studentData.studentNumber})`);
      } catch (error: any) {
        errors++;
        addLog(`❌ Failed to create student ${user.firstName}: ${error.message}`);
      }
    }

    setStats(prev => ({ ...prev, students: prev.students + created, errors: prev.errors + errors }));
    addLog(`🎓 Student creation completed: ${created} created, ${errors} errors`);
    
    return createdStudents;
  };

  // Create sample teachers
  const createSampleTeachers = async (users: any[]) => {
    addLog('👨‍🏫 Creating teacher records...');
    
    const teacherUsers = users.filter(u => u.role === 'Teacher');
    const createdTeachers = [];
    let created = 0;
    let errors = 0;

    for (let i = 0; i < teacherUsers.length; i++) {
      const user = teacherUsers[i];
      try {
        const teacherData = {
          userId: user.userId,
          employeeNumber: `EMP2024${String(i + 1).padStart(3, '0')}`,
          department: 'Computer Engineering',
          specialization: i === 0 ? 'Software Engineering' : 'Data Science'
        };
        
        const teacherId = await apiService.createTeacher(teacherData);
        createdTeachers.push({ ...teacherData, id: teacherId, user });
        created++;
        addLog(`✅ Teacher created: ${user.firstName} ${user.lastName} (${teacherData.employeeNumber})`);
      } catch (error: any) {
        errors++;
        addLog(`❌ Failed to create teacher ${user.firstName}: ${error.message}`);
      }
    }

    setStats(prev => ({ ...prev, teachers: prev.teachers + created, errors: prev.errors + errors }));
    addLog(`👨‍🏫 Teacher creation completed: ${created} created, ${errors} errors`);
    
    return createdTeachers;
  };

  // Create sample courses
  const createSampleCourses = async (teachers: any[]) => {
    addLog('📚 Creating course records...');
    
    const sampleCourses = [
      {
        name: 'Introduction to Programming',
        code: 'CS101',
        description: 'Basic programming concepts and algorithms',
        credits: 3,
        semester: 'Fall 2024'
      },
      {
        name: 'Data Structures',
        code: 'CS201',
        description: 'Advanced data structures and algorithms',
        credits: 4,
        semester: 'Fall 2024'
      },
      {
        name: 'Database Systems',
        code: 'CS301',
        description: 'Database design and management',
        credits: 3,
        semester: 'Fall 2024'
      }
    ];

    const createdCourses = [];
    let created = 0;
    let errors = 0;

    for (let i = 0; i < sampleCourses.length; i++) {
      const course = sampleCourses[i];
      try {
        const teacherId = teachers[i % teachers.length]?.id || 1; // Use available teachers or fallback
        
        const courseData = {
          ...course,
          teacherId
        };
        
        const courseId = await apiService.createCourse(courseData);
        createdCourses.push({ ...courseData, id: courseId });
        created++;
        addLog(`✅ Course created: ${course.name} (${course.code})`);
      } catch (error: any) {
        errors++;
        addLog(`❌ Failed to create course ${course.name}: ${error.message}`);
      }
    }

    setStats(prev => ({ ...prev, courses: prev.courses + created, errors: prev.errors + errors }));
    addLog(`📚 Course creation completed: ${created} created, ${errors} errors`);
    
    return createdCourses;
  };

  const seedGrades = async () => {
    addLog('🌱 Starting grade seeding...');
    
    const { students, courses } = await checkAvailableData();
    
    if (students.length === 0 || courses.length === 0) {
      addLog('⚠️ No students or courses available. Please create basic data first.');
      setStats(prev => ({ ...prev, errors: prev.errors + 1 }));
      return;
    }
    
    const gradeTypes = ['Midterm', 'Final', 'Assignment', 'Quiz'];
    let created = 0;
    let errors = 0;

    // Create 2-3 grades per student per course
    for (const student of students.slice(0, 2)) { // Limit to first 2 students
      for (const course of courses.slice(0, 2)) { // Limit to first 2 courses
        for (let i = 0; i < 2; i++) {
          try {
            const gradeData = {
              studentId: student.id,
              courseId: course.id,
              score: Math.floor(Math.random() * 40) + 60, // Random score 60-100
              gradeType: gradeTypes[i % gradeTypes.length]
            };
            
            await apiService.createGrade(gradeData);
            created++;
            addLog(`✅ Grade created: ${student.user?.firstName || 'Student'} - ${course.name} (${gradeData.score})`);
          } catch (error: any) {
            errors++;
            addLog(`❌ Failed to create grade: ${error.message}`);
          }
        }
      }
    }

    setStats(prev => ({ ...prev, grades: prev.grades + created, errors: prev.errors + errors }));
    addLog(`📊 Grades seeding completed: ${created} created, ${errors} errors`);
  };

  const seedAttendance = async () => {
    addLog('📅 Starting attendance seeding...');
    
    const { students, courses } = await checkAvailableData();
    
    if (students.length === 0 || courses.length === 0) {
      addLog('⚠️ No students or courses available. Please create basic data first.');
      setStats(prev => ({ ...prev, errors: prev.errors + 1 }));
      return;
    }
    
    const statuses = ['Present', 'Absent'];
    const notes = ['Active participation', 'Good attention', 'Late arrival', 'Medical excuse', 'Family emergency'];
    let created = 0;
    let errors = 0;

    // Create attendance for last 5 days
    const today = new Date();
    const dates = [];
    for (let i = 4; i >= 0; i--) {
      const date = new Date(today);
      date.setDate(date.getDate() - i);
      dates.push(date.toISOString().split('T')[0]);
    }

    for (const student of students.slice(0, 2)) { // Limit to first 2 students
      for (const course of courses.slice(0, 2)) { // Limit to first 2 courses
        for (const date of dates) {
          try {
            const isPresent = Math.random() > 0.2; // 80% attendance rate
            const attendanceData = {
              studentId: student.id,
              courseId: course.id,
              date,
              status: isPresent ? 'Present' : 'Absent',
              notes: notes[Math.floor(Math.random() * notes.length)]
            };
            
            await apiService.createAttendance(attendanceData);
            created++;
            addLog(`✅ Attendance created: ${student.user?.firstName || 'Student'} - ${date} (${attendanceData.status})`);
          } catch (error: any) {
            errors++;
            addLog(`❌ Failed to create attendance: ${error.message}`);
          }
        }
      }
    }

    setStats(prev => ({ ...prev, attendance: prev.attendance + created, errors: prev.errors + errors }));
    addLog(`📅 Attendance seeding completed: ${created} created, ${errors} errors`);
  };

  // Create complete sample data
  const createCompleteData = async () => {
    addLog('🚀 Creating complete sample data...');
    
    try {
      // Step 1: Create users
      const users = await createSampleUsers();
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Step 2: Create students and teachers
      const [students, teachers] = await Promise.all([
        createSampleStudents(users),
        createSampleTeachers(users)
      ]);
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Step 3: Create courses
      const courses = await createSampleCourses(teachers);
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Step 4: Update available data
      await checkAvailableData();
      
      addLog('🎉 Complete data creation finished!');
    } catch (error: any) {
      addLog(`❌ Complete data creation failed: ${error.message}`);
    }
  };

  const seedAllData = async () => {
    setIsSeeding(true);
    setStats({ users: 0, students: 0, teachers: 0, courses: 0, grades: 0, attendance: 0, errors: 0 });
    setLogs([]);
    
    addLog('🚀 Starting complete data seeding process...');
    
    try {
      // Check if we have basic data first
      const { students, courses } = await checkAvailableData();
      
      if (students.length === 0 || courses.length === 0) {
        addLog('⚠️ No basic data found. Creating sample data first...');
        await createCompleteData();
        await new Promise(resolve => setTimeout(resolve, 1000));
      }
      
      // Now seed grades and attendance
      await seedGrades();
      await new Promise(resolve => setTimeout(resolve, 1000));
      await seedAttendance();
      
      addLog('🎉 All data seeding completed successfully!');
    } catch (error: any) {
      addLog(`❌ Seeding process failed: ${error.message}`);
    } finally {
      setIsSeeding(false);
    }
  };

  const clearLogs = () => {
    setLogs([]);
    setStats({ users: 0, students: 0, teachers: 0, courses: 0, grades: 0, attendance: 0, errors: 0 });
  };

  const quickCheck = async () => {
    setIsSeeding(true);
    await checkAvailableData();
    setIsSeeding(false);
  };

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="bg-white shadow rounded-lg p-6 mb-6">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">Data Seeder</h1>
          <p className="text-gray-600">
            API'leri kullanarak sisteme örnek veriler ekleyin
          </p>
        </div>

        {/* Stats */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
          <div className="bg-purple-50 border border-purple-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-purple-600">{stats.users}</div>
            <div className="text-sm text-purple-800">Users</div>
          </div>
          <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-blue-600">{stats.students}</div>
            <div className="text-sm text-blue-800">Students</div>
          </div>
          <div className="bg-indigo-50 border border-indigo-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-indigo-600">{stats.teachers}</div>
            <div className="text-sm text-indigo-800">Teachers</div>
          </div>
          <div className="bg-cyan-50 border border-cyan-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-cyan-600">{stats.courses}</div>
            <div className="text-sm text-cyan-800">Courses</div>
          </div>
          <div className="bg-green-50 border border-green-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-green-600">{stats.grades}</div>
            <div className="text-sm text-green-800">Grades</div>
          </div>
          <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-yellow-600">{stats.attendance}</div>
            <div className="text-sm text-yellow-800">Attendance</div>
          </div>
          <div className="bg-red-50 border border-red-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-red-600">{stats.errors}</div>
            <div className="text-sm text-red-800">Errors</div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-4">
            <div className="text-2xl font-bold text-gray-600">
              {stats.users + stats.students + stats.teachers + stats.courses + stats.grades + stats.attendance}
            </div>
            <div className="text-sm text-gray-800">Total Records</div>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="bg-white shadow rounded-lg p-6 mb-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Actions</h2>
          
          {/* Available Data Info */}
          <div className="mb-4 p-3 bg-gray-50 rounded-md">
            <p className="text-sm text-gray-600">
              <strong>Current Data:</strong> {availableData.students?.length || 0} students, {availableData.courses?.length || 0} courses
            </p>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <button
              onClick={quickCheck}
              disabled={isSeeding}
              className="bg-gray-600 text-white px-4 py-2 rounded-md hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              🔍 Check Available Data
            </button>
            <button
              onClick={createCompleteData}
              disabled={isSeeding}
              className="bg-purple-600 text-white px-4 py-2 rounded-md hover:bg-purple-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              👥 Create Sample Data
            </button>
            <button
              onClick={seedGrades}
              disabled={isSeeding}
              className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              📊 Add Grades
            </button>
            <button
              onClick={seedAttendance}
              disabled={isSeeding}
              className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              📅 Add Attendance
            </button>
            <button
              onClick={seedAllData}
              disabled={isSeeding}
              className="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              🚀 Create All Data
            </button>
            <button
              onClick={clearLogs}
              disabled={isSeeding}
              className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              🗑️ Clear Logs
            </button>
          </div>
        </div>

        {/* Logs */}
        <div className="bg-white shadow rounded-lg p-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">
            Operation Logs
            {isSeeding && (
              <span className="ml-2 inline-flex items-center">
                <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-blue-600"></div>
                <span className="ml-2 text-sm text-blue-600">Processing...</span>
              </span>
            )}
          </h2>
          <div className="bg-gray-50 rounded-md p-4 max-h-96 overflow-y-auto">
            {logs.length === 0 ? (
              <p className="text-gray-500 text-center">No logs yet. Click a button above to start.</p>
            ) : (
              <div className="space-y-1">
                {logs.map((log, index) => (
                  <div
                    key={index}
                    className={`text-sm font-mono ${
                      log.includes('✅') ? 'text-green-700' :
                      log.includes('❌') ? 'text-red-700' :
                      log.includes('🎉') ? 'text-purple-700 font-bold' :
                      log.includes('⚠️') ? 'text-yellow-700' :
                      'text-gray-700'
                    }`}
                  >
                    {log}
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

        {/* Usage Instructions */}
        <div className="bg-blue-50 border border-blue-200 rounded-lg p-6 mt-6">
          <h3 className="text-lg font-semibold text-blue-900 mb-2">💡 Usage Instructions</h3>
          <ul className="text-blue-800 space-y-1 text-sm">
            <li>• <strong>Check Available Data:</strong> See what students and courses already exist</li>
            <li>• <strong>Create Sample Data:</strong> Creates users, students, teachers, and courses</li>
            <li>• <strong>Add Grades:</strong> Creates sample grade records for existing students and courses</li>
            <li>• <strong>Add Attendance:</strong> Creates attendance records for the last 5 days</li>
            <li>• <strong>Create All Data:</strong> Performs a complete setup with all sample data</li>
            <li>• Data will appear in the respective management pages (Students, Courses, Grades, Attendance)</li>
          </ul>
        </div>
      </div>
    </div>
  );
}