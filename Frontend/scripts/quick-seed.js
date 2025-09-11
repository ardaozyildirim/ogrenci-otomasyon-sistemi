// Simple data seeding script for browser console
// Copy and paste this into the browser console while on the frontend app

async function quickSeedData() {
  const API_BASE_URL = 'http://localhost:5255/api';
  
  // Get auth token from localStorage (if user is logged in)
  const authToken = localStorage.getItem('authToken');
  
  if (!authToken) {
    console.log('❌ Please login first to get authentication token');
    return;
  }

  async function apiCall(endpoint, data, method = 'POST') {
    try {
      const response = await fetch(`${API_BASE_URL}${endpoint}`, {
        method: method,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${authToken}`
        },
        body: JSON.stringify(data)
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      return await response.json();
    } catch (error) {
      console.error(`Error calling ${endpoint}:`, error);
      return null;
    }
  }

  console.log('🌱 Starting quick data seeding...');

  // Create a few grades
  const gradeData = [
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
    }
  ];

  for (const grade of gradeData) {
    const result = await apiCall('/v1/Grades', grade);
    if (result) {
      console.log(`✅ Created grade: Score ${grade.score} for student ${grade.studentId}`);
    }
  }

  // Create attendance records
  const attendanceData = [
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
  ];

  for (const attendance of attendanceData) {
    const result = await apiCall('/v1/Attendance', attendance);
    if (result) {
      console.log(`✅ Created attendance: ${attendance.status} for student ${attendance.studentId} on ${attendance.date}`);
    }
  }

  console.log('🎉 Quick data seeding completed!');
  console.log('📊 Go to the Grades or Attendance pages to see the new data');
}

// Auto-run if possible, otherwise provide instructions
if (typeof localStorage !== 'undefined' && localStorage.getItem('authToken')) {
  quickSeedData();
} else {
  console.log('📋 Quick seed script loaded. Make sure you are logged in, then run: quickSeedData()');
}