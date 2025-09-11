// Data seeding script for Student Management System
// This script adds sample data using the API endpoints

const API_BASE_URL = 'http://localhost:5255/api';

// Sample data
const sampleUsers = [
  {
    firstName: 'Ali',
    lastName: 'Yılmaz', 
    email: 'ali.yilmaz@test.com',
    password: 'password123',
    role: 'Student',
    phoneNumber: '+905551234567',
    dateOfBirth: '2000-05-15',
    address: 'İstanbul, Türkiye'
  },
  {
    firstName: 'Ayşe',
    lastName: 'Kaya',
    email: 'ayse.kaya@test.com', 
    password: 'password123',
    role: 'Student',
    phoneNumber: '+905551234568',
    dateOfBirth: '2001-03-20',
    address: 'Ankara, Türkiye'
  },
  {
    firstName: 'Mehmet',
    lastName: 'Özkan',
    email: 'mehmet.ozkan@test.com',
    password: 'password123', 
    role: 'Student',
    phoneNumber: '+905551234569',
    dateOfBirth: '1999-11-10',
    address: 'İzmir, Türkiye'
  },
  {
    firstName: 'Dr. Fatma',
    lastName: 'Arslan',
    email: 'fatma.arslan@test.com',
    password: 'password123',
    role: 'Teacher',
    phoneNumber: '+905551234570',
    dateOfBirth: '1980-08-05',
    address: 'İstanbul, Türkiye'
  },
  {
    firstName: 'Prof. Ahmet',
    lastName: 'Demir', 
    email: 'ahmet.demir@test.com',
    password: 'password123',
    role: 'Teacher',
    phoneNumber: '+905551234571',
    dateOfBirth: '1975-12-12',
    address: 'Ankara, Türkiye'
  }
];

const sampleCourses = [
  {
    name: 'Bilgisayar Programlama I',
    code: 'CS101',
    description: 'Temel programlama kavramları ve algoritma geliştirme',
    credits: 4,
    semester: 'Güz 2024'
  },
  {
    name: 'Veri Yapıları ve Algoritmalar',
    code: 'CS201', 
    description: 'İleri veri yapıları ve algoritma analizi',
    credits: 4,
    semester: 'Güz 2024'
  },
  {
    name: 'Matematik I',
    code: 'MATH101',
    description: 'Diferansiyel ve integral hesap',
    credits: 3,
    semester: 'Güz 2024'
  },
  {
    name: 'Veritabanı Yönetim Sistemleri',
    code: 'CS301',
    description: 'İlişkisel veritabanları ve SQL',
    credits: 3,
    semester: 'Güz 2024'
  }
];

let authToken = null;
let createdUsers = [];
let createdStudents = [];
let createdTeachers = [];
let createdCourses = [];

// Helper function to make API requests
async function apiRequest(endpoint, options = {}) {
  const config = {
    headers: {
      'Content-Type': 'application/json',
      ...(authToken && { Authorization: `Bearer ${authToken}` }),
      ...options.headers,
    },
    ...options,
  };

  try {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
    
    if (!response.ok) {
      const errorText = await response.text();
      console.error(`API Error for ${endpoint}:`, errorText);
      throw new Error(`HTTP ${response.status}: ${errorText}`);
    }

    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return await response.json();
    }
    
    return await response.text();
  } catch (error) {
    console.error(`Error calling ${endpoint}:`, error);
    throw error;
  }
}

// Convert role string to enum value
function getRoleValue(role) {
  const roleMapping = {
    'Admin': 1,
    'Teacher': 2,
    'Student': 3
  };
  return roleMapping[role] || 3;
}

// Login as admin to get authentication token
async function loginAsAdmin() {
  console.log('🔑 Logging in as admin...');
  try {
    const response = await apiRequest('/Auth/login', {
      method: 'POST',
      body: JSON.stringify({
        email: 'admin@test.com',
        password: 'password123'
      })
    });
    
    authToken = response.token;
    console.log('✅ Admin login successful');
    return response;
  } catch (error) {
    console.error('❌ Admin login failed:', error);
    throw error;
  }
}

// Register users
async function registerUsers() {
  console.log('\n👥 Registering sample users...');
  
  for (const userData of sampleUsers) {
    try {
      const requestData = {
        ...userData,
        role: getRoleValue(userData.role)
      };
      
      const response = await apiRequest('/Auth/register', {
        method: 'POST',
        body: JSON.stringify(requestData)
      });
      
      createdUsers.push({
        ...userData,
        userId: response.userId || response.user?.id,
        response: response
      });
      
      console.log(`✅ Created user: ${userData.firstName} ${userData.lastName} (${userData.role})`);
    } catch (error) {
      console.error(`❌ Failed to create user ${userData.firstName} ${userData.lastName}:`, error.message);
    }
  }
}

// Create students
async function createStudents() {
  console.log('\n🎓 Creating student records...');
  
  const studentUsers = createdUsers.filter(user => user.role === 'Student');
  
  for (let i = 0; i < studentUsers.length; i++) {
    const user = studentUsers[i];
    try {
      const studentData = {
        userId: user.userId,
        studentNumber: `2024CS${String(i + 1).padStart(3, '0')}`,
        department: 'Bilgisayar Mühendisliği',
        grade: 85 + (i * 5),
        className: `BM-${i + 1}A`
      };
      
      const response = await apiRequest('/v1/Students', {
        method: 'POST',
        body: JSON.stringify(studentData)
      });
      
      createdStudents.push({
        ...studentData,
        studentId: response.data || response,
        user: user
      });
      
      console.log(`✅ Created student: ${user.firstName} ${user.lastName} (${studentData.studentNumber})`);
    } catch (error) {
      console.error(`❌ Failed to create student for ${user.firstName} ${user.lastName}:`, error.message);
    }
  }
}

// Create teachers
async function createTeachers() {
  console.log('\n👨‍🏫 Creating teacher records...');
  
  const teacherUsers = createdUsers.filter(user => user.role === 'Teacher');
  
  for (let i = 0; i < teacherUsers.length; i++) {
    const user = teacherUsers[i];
    try {
      const teacherData = {
        userId: user.userId,
        employeeNumber: `EMP2024${String(i + 1).padStart(3, '0')}`,
        department: 'Bilgisayar Mühendisliği',
        specialization: i === 0 ? 'Yazılım Mühendisliği' : 'Algoritma ve Veri Yapıları'
      };
      
      const response = await apiRequest('/v1/Teachers', {
        method: 'POST',
        body: JSON.stringify(teacherData)
      });
      
      createdTeachers.push({
        ...teacherData,
        teacherId: response.data || response,
        user: user
      });
      
      console.log(`✅ Created teacher: ${user.firstName} ${user.lastName} (${teacherData.employeeNumber})`);
    } catch (error) {
      console.error(`❌ Failed to create teacher for ${user.firstName} ${user.lastName}:`, error.message);
    }
  }
}

// Create courses
async function createCourses() {
  console.log('\n📚 Creating courses...');
  
  if (createdTeachers.length === 0) {
    console.log('⚠️ No teachers available, skipping course creation');
    return;
  }
  
  for (let i = 0; i < sampleCourses.length; i++) {
    const courseData = sampleCourses[i];
    try {
      const teacherId = createdTeachers[i % createdTeachers.length].teacherId;
      
      const requestData = {
        ...courseData,
        teacherId: teacherId
      };
      
      const response = await apiRequest('/v1/Courses', {
        method: 'POST',
        body: JSON.stringify(requestData)
      });
      
      createdCourses.push({
        ...requestData,
        courseId: response.data || response
      });
      
      console.log(`✅ Created course: ${courseData.name} (${courseData.code})`);
    } catch (error) {
      console.error(`❌ Failed to create course ${courseData.name}:`, error.message);
    }
  }
}

// Create grades
async function createGrades() {
  console.log('\n📊 Creating grade records...');
  
  if (createdStudents.length === 0 || createdCourses.length === 0) {
    console.log('⚠️ No students or courses available, skipping grade creation');
    return;
  }
  
  const gradeTypes = ['Midterm', 'Final', 'Assignment', 'Quiz'];
  
  for (const student of createdStudents) {
    for (const course of createdCourses) {
      // Create 2 grades per student per course
      for (let i = 0; i < 2; i++) {
        try {
          const gradeData = {
            studentId: student.studentId,
            courseId: course.courseId,
            score: Math.floor(Math.random() * 40) + 60, // Random score between 60-100
            gradeType: gradeTypes[i % gradeTypes.length],
            semester: course.semester,
            date: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0] // Random date within last 30 days
          };
          
          const response = await apiRequest('/v1/Grades', {
            method: 'POST',
            body: JSON.stringify(gradeData)
          });
          
          console.log(`✅ Created grade: ${student.user.firstName} ${student.user.lastName} - ${course.name} (${gradeData.score})`);
        } catch (error) {
          console.error(`❌ Failed to create grade for ${student.user.firstName} ${student.user.lastName}:`, error.message);
        }
      }
    }
  }
}

// Create attendance records
async function createAttendance() {
  console.log('\n📅 Creating attendance records...');
  
  if (createdStudents.length === 0 || createdCourses.length === 0) {
    console.log('⚠️ No students or courses available, skipping attendance creation');
    return;
  }
  
  const statuses = ['Present', 'Absent'];
  
  for (const student of createdStudents) {
    for (const course of createdCourses) {
      // Create 5 attendance records per student per course
      for (let i = 0; i < 5; i++) {
        try {
          const attendanceData = {
            studentId: student.studentId,
            courseId: course.courseId,
            date: new Date(Date.now() - (i + 1) * 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0], // Weekly intervals
            status: Math.random() > 0.2 ? 'Present' : 'Absent', // 80% attendance rate
            notes: Math.random() > 0.5 ? 'Regular attendance' : ''
          };
          
          const response = await apiRequest('/v1/Attendance', {
            method: 'POST',
            body: JSON.stringify(attendanceData)
          });
          
          console.log(`✅ Created attendance: ${student.user.firstName} ${student.user.lastName} - ${course.name} (${attendanceData.status})`);
        } catch (error) {
          console.error(`❌ Failed to create attendance for ${student.user.firstName} ${student.user.lastName}:`, error.message);
        }
      }
    }
  }
}

// Main execution function
async function seedData() {
  console.log('🌱 Starting data seeding process...\n');
  
  try {
    // Step 1: Login as admin
    await loginAsAdmin();
    
    // Step 2: Register users  
    await registerUsers();
    
    // Step 3: Create students
    await createStudents();
    
    // Step 4: Create teachers
    await createTeachers();
    
    // Step 5: Create courses
    await createCourses();
    
    // Step 6: Create grades
    await createGrades();
    
    // Step 7: Create attendance records
    await createAttendance();
    
    console.log('\n🎉 Data seeding completed successfully!');
    console.log(`\n📊 Summary:`);
    console.log(`   👥 Users created: ${createdUsers.length}`);
    console.log(`   🎓 Students created: ${createdStudents.length}`);
    console.log(`   👨‍🏫 Teachers created: ${createdTeachers.length}`);
    console.log(`   📚 Courses created: ${createdCourses.length}`);
    console.log(`   📊 Grades created: ${createdStudents.length * createdCourses.length * 2}`);
    console.log(`   📅 Attendance records created: ${createdStudents.length * createdCourses.length * 5}`);
    
  } catch (error) {
    console.error('\n❌ Data seeding failed:', error);
  }
}

// Run the seeding process
if (typeof window === 'undefined') {
  // Running in Node.js
  const fetch = require('node-fetch');
  global.fetch = fetch;
  seedData();
} else {
  // Running in browser
  window.seedData = seedData;
  console.log('📋 Data seeding script loaded. Run seedData() to start the process.');
}