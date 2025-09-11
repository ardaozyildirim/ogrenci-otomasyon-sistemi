const fetch = require('node-fetch');

const API_BASE_URL = 'http://localhost:5255/api';

// Sample data to be created
const sampleData = {
  grades: [
    {
      studentId: 1,
      courseId: 1,
      score: 95,
      gradeType: 'Midterm',
      semester: 'Güz 2024',
      date: '2024-10-15'
    },
    {
      studentId: 1,
      courseId: 1,
      score: 88,
      gradeType: 'Final', 
      semester: 'Güz 2024',
      date: '2024-12-15'
    },
    {
      studentId: 1,
      courseId: 2,
      score: 92,
      gradeType: 'Assignment',
      semester: 'Güz 2024',
      date: '2024-11-01'
    },
    {
      studentId: 1,
      courseId: 2,
      score: 85,
      gradeType: 'Quiz',
      semester: 'Güz 2024', 
      date: '2024-10-20'
    }
  ],
  attendance: [
    {
      studentId: 1,
      courseId: 1,
      date: '2024-09-01',
      status: 'Present',
      notes: 'First day of class'
    },
    {
      studentId: 1,
      courseId: 1,
      date: '2024-09-08',
      status: 'Present',
      notes: 'Active participation'
    },
    {
      studentId: 1,
      courseId: 1,
      date: '2024-09-15',
      status: 'Absent',
      notes: 'Medical excuse'
    },
    {
      studentId: 1,
      courseId: 2,
      date: '2024-09-02',
      status: 'Present',
      notes: 'Great engagement'
    },
    {
      studentId: 1,
      courseId: 2,
      date: '2024-09-09',
      status: 'Present',
      notes: ''
    }
  ]
};

async function makeApiCall(endpoint, data) {
  try {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`HTTP ${response.status}: ${errorText}`);
    }

    return await response.json();
  } catch (error) {
    console.error(`Error calling ${endpoint}:`, error.message);
    return null;
  }
}

async function seedGrades() {
  console.log('📊 Adding grade data...');
  
  for (const grade of sampleData.grades) {
    const result = await makeApiCall('/v1/Grades', grade);
    if (result) {
      console.log(`✅ Grade created: ${grade.gradeType} - Score: ${grade.score}`);
    } else {
      console.log(`❌ Failed to create grade: ${grade.gradeType}`);
    }
  }
}

async function seedAttendance() {
  console.log('\n📅 Adding attendance data...');
  
  for (const attendance of sampleData.attendance) {
    const result = await makeApiCall('/v1/Attendance', attendance);
    if (result) {
      console.log(`✅ Attendance created: ${attendance.date} - ${attendance.status}`);
    } else {
      console.log(`❌ Failed to create attendance: ${attendance.date}`);
    }
  }
}

async function main() {
  console.log('🌱 Starting data seeding with APIs...\n');
  
  await seedGrades();
  await seedAttendance();
  
  console.log('\n🎉 Data seeding completed!');
  console.log('📋 Check the Grade Management and Attendance pages to see the new data.');
}

// Run the script
main().catch(console.error);