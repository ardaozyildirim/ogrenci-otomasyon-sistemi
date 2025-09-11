// components/Dashboard.tsx
import React, { useState, useEffect } from 'react';
import authService from '../services/api/authService';
import gradeService from '../services/api/gradeService';
import attendanceService from '../services/api/attendanceService';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';

interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
}

interface Grade {
  id: number;
  gradeValue: number;
  gradeDate: string;
  course: {
    name: string;
  };
  student: {
    firstName: string;
    lastName: string;
  };
}

interface Attendance {
  id: number;
  isPresent: boolean;
  attendanceDate: string;
  course: {
    name: string;
  };
  student: {
    firstName: string;
    lastName: string;
  };
}

const Dashboard: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [grades, setGrades] = useState<Grade[]>([]);
  const [attendances, setAttendances] = useState<Attendance[]>([]);
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    totalGrades: 0,
    averageGrade: 0,
    totalAttendance: 0,
    presentAttendance: 0,
    highestGrade: 0,
    lowestGrade: 100
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const userData = await authService.getCurrentUser();
        setUser(userData);
        
        // Fetch grades and attendance based on user role
        if (userData.role === 'Student') {
          const gradesData = await gradeService.getGradesByStudentId(userData.id);
          const attendanceData = await attendanceService.getAttendancesByStudentId(userData.id);
          setGrades(gradesData);
          setAttendances(attendanceData);
          
          // Calculate stats for student
          const totalGrades = gradesData.length;
          const averageGrade = totalGrades > 0 
            ? gradesData.reduce((sum, grade) => sum + grade.gradeValue, 0) / totalGrades 
            : 0;
          
          const totalAttendance = attendanceData.length;
          const presentAttendance = attendanceData.filter(a => a.isPresent).length;
          
          // Find highest and lowest grades
          const gradeValues = gradesData.map(g => g.gradeValue);
          const highestGrade = gradeValues.length > 0 ? Math.max(...gradeValues) : 0;
          const lowestGrade = gradeValues.length > 0 ? Math.min(...gradeValues) : 100;
          
          setStats({
            totalGrades,
            averageGrade: parseFloat(averageGrade.toFixed(2)),
            totalAttendance,
            presentAttendance,
            highestGrade,
            lowestGrade
          });
        } else if (userData.role === 'Teacher') {
          // For teachers, we might want to show all grades and attendance for their courses
          const gradesData = await gradeService.getAllGrades();
          const attendanceData = await attendanceService.getAllAttendances();
          setGrades(gradesData);
          setAttendances(attendanceData);
          
          // Calculate stats for teacher
          const totalGrades = gradesData.length;
          const averageGrade = totalGrades > 0 
            ? gradesData.reduce((sum, grade) => sum + grade.gradeValue, 0) / totalGrades 
            : 0;
          
          const totalAttendance = attendanceData.length;
          const presentAttendance = attendanceData.filter(a => a.isPresent).length;
          
          // Find highest and lowest grades
          const gradeValues = gradesData.map(g => g.gradeValue);
          const highestGrade = gradeValues.length > 0 ? Math.max(...gradeValues) : 0;
          const lowestGrade = gradeValues.length > 0 ? Math.min(...gradeValues) : 100;
          
          setStats({
            totalGrades,
            averageGrade: parseFloat(averageGrade.toFixed(2)),
            totalAttendance,
            presentAttendance,
            highestGrade,
            lowestGrade
          });
        } else {
          // For admin, show all data
          const gradesData = await gradeService.getAllGrades();
          const attendanceData = await attendanceService.getAllAttendances();
          setGrades(gradesData);
          setAttendances(attendanceData);
          
          // Calculate stats for admin
          const totalGrades = gradesData.length;
          const averageGrade = totalGrades > 0 
            ? gradesData.reduce((sum, grade) => sum + grade.gradeValue, 0) / totalGrades 
            : 0;
          
          const totalAttendance = attendanceData.length;
          const presentAttendance = attendanceData.filter(a => a.isPresent).length;
          
          // Find highest and lowest grades
          const gradeValues = gradesData.map(g => g.gradeValue);
          const highestGrade = gradeValues.length > 0 ? Math.max(...gradeValues) : 0;
          const lowestGrade = gradeValues.length > 0 ? Math.min(...gradeValues) : 100;
          
          setStats({
            totalGrades,
            averageGrade: parseFloat(averageGrade.toFixed(2)),
            totalAttendance,
            presentAttendance,
            highestGrade,
            lowestGrade
          });
        }
      } catch (error) {
        console.error('Error fetching dashboard data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  // Prepare data for charts
  const gradeDistributionData = [
    { name: '90-100', count: grades.filter(g => g.gradeValue >= 90).length },
    { name: '80-89', count: grades.filter(g => g.gradeValue >= 80 && g.gradeValue < 90).length },
    { name: '70-79', count: grades.filter(g => g.gradeValue >= 70 && g.gradeValue < 80).length },
    { name: '60-69', count: grades.filter(g => g.gradeValue >= 60 && g.gradeValue < 70).length },
    { name: '0-59', count: grades.filter(g => g.gradeValue < 60).length },
  ];

  const attendanceData = [
    { name: 'Present', value: stats.presentAttendance },
    { name: 'Absent', value: stats.totalAttendance - stats.presentAttendance },
  ];

  const COLORS = ['#10b981', '#ef4444'];

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Welcome Section */}
      <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)', color: 'var(--foreground)' }}>
        <div className="flex flex-col md:flex-row md:items-center md:justify-between">
          <div>
            <h1 className="text-2xl font-bold" style={{ color: 'var(--foreground)' }}>Welcome back, {user?.firstName}!</h1>
            <p className="mt-1" style={{ color: 'var(--foreground)' }}>
              Here's what's happening with your account today.
            </p>
          </div>
          <div className="mt-4 md:mt-0">
            <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium" style={{ backgroundColor: 'rgba(59, 130, 246, 0.1)', color: 'var(--primary)' }}>
              {user?.role}
            </span>
          </div>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <div className="flex items-center">
            <div className="p-3 rounded-lg" style={{ backgroundColor: 'rgba(59, 130, 246, 0.1)' }}>
              <svg className="w-6 h-6" style={{ color: 'var(--primary)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
              </svg>
            </div>
            <div className="ml-4">
              <h3 className="text-sm font-medium" style={{ color: 'var(--foreground)' }}>Total Grades</h3>
              <p className="text-2xl font-bold" style={{ color: 'var(--foreground)' }}>{stats.totalGrades}</p>
            </div>
          </div>
        </div>

        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <div className="flex items-center">
            <div className="p-3 rounded-lg" style={{ backgroundColor: 'rgba(16, 185, 129, 0.1)' }}>
              <svg className="w-6 h-6" style={{ color: 'var(--success)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
              </svg>
            </div>
            <div className="ml-4">
              <h3 className="text-sm font-medium" style={{ color: 'var(--foreground)' }}>Average Grade</h3>
              <p className="text-2xl font-bold" style={{ color: 'var(--foreground)' }}>{stats.averageGrade}</p>
            </div>
          </div>
        </div>

        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <div className="flex items-center">
            <div className="p-3 rounded-lg" style={{ backgroundColor: 'rgba(139, 92, 246, 0.1)' }}>
              <svg className="w-6 h-6" style={{ color: 'var(--info)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path>
              </svg>
            </div>
            <div className="ml-4">
              <h3 className="text-sm font-medium" style={{ color: 'var(--foreground)' }}>Total Attendance</h3>
              <p className="text-2xl font-bold" style={{ color: 'var(--foreground)' }}>{stats.totalAttendance}</p>
            </div>
          </div>
        </div>

        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <div className="flex items-center">
            <div className="p-3 rounded-lg" style={{ backgroundColor: 'rgba(245, 158, 11, 0.1)' }}>
              <svg className="w-6 h-6" style={{ color: 'var(--warning)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
              </svg>
            </div>
            <div className="ml-4">
              <h3 className="text-sm font-medium" style={{ color: 'var(--foreground)' }}>Attendance Rate</h3>
              <p className="text-2xl font-bold" style={{ color: 'var(--foreground)' }}>
                {stats.totalAttendance > 0 
                  ? `${Math.round((stats.presentAttendance / stats.totalAttendance) * 100)}%` 
                  : '0%'}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Additional Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <div className="flex items-center">
            <div className="p-3 rounded-lg" style={{ backgroundColor: 'rgba(59, 130, 246, 0.1)' }}>
              <svg className="w-6 h-6" style={{ color: 'var(--primary)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143L13 3z"></path>
              </svg>
            </div>
            <div className="ml-4">
              <h3 className="text-sm font-medium" style={{ color: 'var(--foreground)' }}>Highest Grade</h3>
              <p className="text-2xl font-bold" style={{ color: 'var(--foreground)' }}>{stats.highestGrade}</p>
            </div>
          </div>
        </div>

        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <div className="flex items-center">
            <div className="p-3 rounded-lg" style={{ backgroundColor: 'rgba(239, 68, 68, 0.1)' }}>
              <svg className="w-6 h-6" style={{ color: 'var(--danger)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M13 17h8m0 0V9m0 8l-8-8-4 4-6-6"></path>
              </svg>
            </div>
            <div className="ml-4">
              <h3 className="text-sm font-medium" style={{ color: 'var(--foreground)' }}>Lowest Grade</h3>
              <p className="text-2xl font-bold" style={{ color: 'var(--foreground)' }}>{stats.lowestGrade}</p>
            </div>
          </div>
        </div>
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <h3 className="text-lg font-medium mb-4" style={{ color: 'var(--foreground)' }}>Grade Distribution</h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart
                data={gradeDistributionData}
                margin={{
                  top: 5,
                  right: 30,
                  left: 20,
                  bottom: 5,
                }}
              >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar dataKey="count" name="Number of Grades" fill="#3b82f6" />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>
        <div className="rounded-lg shadow p-6" style={{ backgroundColor: 'var(--background)' }}>
          <h3 className="text-lg font-medium mb-4" style={{ color: 'var(--foreground)' }}>Attendance Overview</h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={attendanceData}
                  cx="50%"
                  cy="50%"
                  labelLine={true}
                  outerRadius={80}
                  fill="#8884d8"
                  dataKey="value"
                  label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                >
                  {attendanceData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>

      {/* Recent Activity */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Recent Grades */}
        <div className="rounded-lg shadow overflow-hidden" style={{ backgroundColor: 'var(--background)' }}>
          <div className="px-6 py-5 border-b" style={{ borderColor: 'var(--foreground)' }}>
            <h3 className="text-lg font-medium" style={{ color: 'var(--foreground)' }}>Recent Grades</h3>
          </div>
          <div className="p-6">
            {grades.length > 0 ? (
              <div className="flow-root">
                <ul className="divide-y" style={{ borderColor: 'var(--foreground)' }}>
                  {grades.slice(0, 5).map((grade) => (
                    <li key={grade.id} className="py-4">
                      <div className="flex items-center space-x-4">
                        <div className="flex-1 min-w-0">
                          <p className="text-sm font-medium truncate" style={{ color: 'var(--foreground)' }}>
                            {grade.course.name}
                          </p>
                          <p className="text-sm truncate" style={{ color: 'var(--foreground)' }}>
                            {user?.role === 'Student' 
                              ? `Grade: ${grade.gradeValue}` 
                              : `${grade.student.firstName} ${grade.student.lastName}`}
                          </p>
                        </div>
                        <div className={`inline-flex items-center text-base font-semibold px-2.5 py-0.5 rounded-full text-sm ${
                          grade.gradeValue >= 90 ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100' :
                          grade.gradeValue >= 80 ? 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-100' :
                          grade.gradeValue >= 70 ? 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-100' :
                          grade.gradeValue >= 60 ? 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-100' :
                          'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-100'
                        }`}>
                          {grade.gradeValue}
                        </div>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            ) : (
              <div className="text-center py-8">
                <svg className="mx-auto h-12 w-12" style={{ color: 'var(--foreground)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
                </svg>
                <h3 className="mt-2 text-sm font-medium" style={{ color: 'var(--foreground)' }}>No grades</h3>
                <p className="mt-1 text-sm" style={{ color: 'var(--foreground)' }}>No grades have been recorded yet.</p>
              </div>
            )}
          </div>
        </div>

        {/* Recent Attendance */}
        <div className="rounded-lg shadow overflow-hidden" style={{ backgroundColor: 'var(--background)' }}>
          <div className="px-6 py-5 border-b" style={{ borderColor: 'var(--foreground)' }}>
            <h3 className="text-lg font-medium" style={{ color: 'var(--foreground)' }}>Recent Attendance</h3>
          </div>
          <div className="p-6">
            {attendances.length > 0 ? (
              <div className="flow-root">
                <ul className="divide-y" style={{ borderColor: 'var(--foreground)' }}>
                  {attendances.slice(0, 5).map((attendance) => (
                    <li key={attendance.id} className="py-4">
                      <div className="flex items-center space-x-4">
                        <div className="flex-1 min-w-0">
                          <p className="text-sm font-medium truncate" style={{ color: 'var(--foreground)' }}>
                            {attendance.course.name}
                          </p>
                          <p className="text-sm truncate" style={{ color: 'var(--foreground)' }}>
                            {user?.role === 'Student' 
                              ? new Date(attendance.attendanceDate).toLocaleDateString() 
                              : `${attendance.student.firstName} ${attendance.student.lastName}`}
                          </p>
                        </div>
                        <div className="inline-flex items-center">
                          <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                            attendance.isPresent 
                              ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100' 
                              : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-100'
                          }`}>
                            {attendance.isPresent ? 'Present' : 'Absent'}
                          </span>
                        </div>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            ) : (
              <div className="text-center py-8">
                <svg className="mx-auto h-12 w-12" style={{ color: 'var(--foreground)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path>
                </svg>
                <h3 className="mt-2 text-sm font-medium" style={{ color: 'var(--foreground)' }}>No attendance records</h3>
                <p className="mt-1 text-sm" style={{ color: 'var(--foreground)' }}>No attendance records have been created yet.</p>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;