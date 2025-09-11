// components/Dashboard.tsx
import React, { useState, useEffect } from 'react';
import authService from '../services/api/authService';
import gradeService from '../services/api/gradeService';
import attendanceService from '../services/api/attendanceService';

interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
}

const Dashboard: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [grades, setGrades] = useState<any[]>([]);
  const [attendances, setAttendances] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

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
        } else if (userData.role === 'Teacher') {
          // For teachers, we might want to show all grades and attendance for their courses
          const gradesData = await gradeService.getAllGrades();
          const attendanceData = await attendanceService.getAllAttendances();
          setGrades(gradesData);
          setAttendances(attendanceData);
        }
      } catch (error) {
        console.error('Error fetching dashboard data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  return (
    <div>
      <div className="pb-5 border-b border-gray-200">
        <h3 className="text-lg leading-6 font-medium text-gray-900">
          Dashboard
        </h3>
        {user && (
          <p className="mt-2 max-w-4xl text-sm text-gray-500">
            Welcome, {user.firstName} {user.lastName} ({user.role})
          </p>
        )}
      </div>

      <div className="mt-6 grid grid-cols-1 gap-5 sm:grid-cols-2">
        {/* Grades Card */}
        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900">
              Recent Grades
            </h3>
            <div className="mt-5">
              {grades.length > 0 ? (
                <ul className="divide-y divide-gray-200">
                  {grades.slice(0, 5).map((grade) => (
                    <li key={grade.id} className="py-4">
                      <div className="flex items-center space-x-4">
                        <div className="min-w-0 flex-1">
                          <p className="text-sm font-medium text-gray-900 truncate">
                            {grade.course.name}
                          </p>
                          <p className="text-sm text-gray-500 truncate">
                            Grade: {grade.gradeValue}
                          </p>
                        </div>
                        <div>
                          <p className="text-sm text-gray-500">
                            {new Date(grade.gradeDate).toLocaleDateString()}
                          </p>
                        </div>
                      </div>
                    </li>
                  ))}
                </ul>
              ) : (
                <p className="text-sm text-gray-500">No grades found</p>
              )}
            </div>
          </div>
        </div>

        {/* Attendance Card */}
        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900">
              Recent Attendance
            </h3>
            <div className="mt-5">
              {attendances.length > 0 ? (
                <ul className="divide-y divide-gray-200">
                  {attendances.slice(0, 5).map((attendance) => (
                    <li key={attendance.id} className="py-4">
                      <div className="flex items-center space-x-4">
                        <div className="min-w-0 flex-1">
                          <p className="text-sm font-medium text-gray-900 truncate">
                            {attendance.course.name}
                          </p>
                          <p className="text-sm text-gray-500 truncate">
                            Status: {attendance.isPresent ? 'Present' : 'Absent'}
                          </p>
                        </div>
                        <div>
                          <p className="text-sm text-gray-500">
                            {new Date(attendance.attendanceDate).toLocaleDateString()}
                          </p>
                        </div>
                      </div>
                    </li>
                  ))}
                </ul>
              ) : (
                <p className="text-sm text-gray-500">No attendance records found</p>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;