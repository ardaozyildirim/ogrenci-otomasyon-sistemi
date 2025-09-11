// components/attendance/AttendanceList.tsx
import React, { useState, useEffect } from 'react';
import attendanceService, { Attendance } from '../../services/api/attendanceService';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';

interface AttendanceListProps {
  studentId?: number;
  courseId?: number;
}

const AttendanceList: React.FC<AttendanceListProps> = ({ studentId, courseId }) => {
  const [attendances, setAttendances] = useState<Attendance[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [sortBy, setSortBy] = useState<'date' | 'status'>('date');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc');

  useEffect(() => {
    const fetchAttendances = async () => {
      try {
        setLoading(true);
        let data: Attendance[] = [];
        
        if (studentId) {
          data = await attendanceService.getAttendancesByStudentId(studentId);
        } else if (courseId) {
          data = await attendanceService.getAttendancesByCourseId(courseId);
        } else {
          data = await attendanceService.getAllAttendances();
        }
        
        setAttendances(data);
      } catch (err: any) {
        setError(err.message || 'Failed to fetch attendance records');
      } finally {
        setLoading(false);
      }
    };

    fetchAttendances();
  }, [studentId, courseId]);

  const sortedAttendances = [...attendances].sort((a, b) => {
    if (sortBy === 'date') {
      const dateA = new Date(a.attendanceDate).getTime();
      const dateB = new Date(b.attendanceDate).getTime();
      return sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
    } else {
      // Sort by status: present first, then absent
      if (a.isPresent === b.isPresent) return 0;
      if (sortOrder === 'asc') {
        return a.isPresent ? 1 : -1;
      } else {
        return a.isPresent ? -1 : 1;
      }
    }
  });

  const handleSort = (field: 'date' | 'status') => {
    if (sortBy === field) {
      setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
    } else {
      setSortBy(field);
      setSortOrder('desc');
    }
  };

  // Calculate attendance statistics
  const totalRecords = attendances.length;
  const presentRecords = attendances.filter(a => a.isPresent).length;
  const absentRecords = totalRecords - presentRecords;
  const attendanceRate = totalRecords > 0 ? Math.round((presentRecords / totalRecords) * 100) : 0;

  // Prepare data for charts
  const attendanceData = [
    { name: 'Present', value: presentRecords },
    { name: 'Absent', value: absentRecords },
  ];

  const COLORS = ['#10b981', '#ef4444'];

  // Prepare data for attendance trend over time
  const attendanceTrendData = sortedAttendances
    .slice(0, 10) // Take last 10 records
    .map(attendance => ({
      date: new Date(attendance.attendanceDate).toLocaleDateString(),
      status: attendance.isPresent ? 1 : 0,
      course: attendance.course.name
    }))
    .reverse(); // Reverse to show chronological order

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-md bg-red-50 p-4 border-l-4 border-red-400">
        <div className="flex">
          <div className="flex-shrink-0">
            <svg className="h-5 w-5 text-red-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
              <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
            </svg>
          </div>
          <div className="ml-3">
            <h3 className="text-sm font-medium text-red-800">
              {error}
            </h3>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Attendance Stats */}
      {totalRecords > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-blue-500 hover:shadow-xl transition-shadow duration-300">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-blue-100">
                <svg className="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
                </svg>
              </div>
              <div className="ml-4">
                <h3 className="text-sm font-medium text-gray-500">Total Records</h3>
                <p className="text-2xl font-bold text-gray-900">{totalRecords}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-green-500 hover:shadow-xl transition-shadow duration-300">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-green-100">
                <svg className="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                </svg>
              </div>
              <div className="ml-4">
                <h3 className="text-sm font-medium text-gray-500">Present</h3>
                <p className="text-2xl font-bold text-gray-900">{presentRecords}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-purple-500 hover:shadow-xl transition-shadow duration-300">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-purple-100">
                <svg className="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
                </svg>
              </div>
              <div className="ml-4">
                <h3 className="text-sm font-medium text-gray-500">Attendance Rate</h3>
                <p className="text-2xl font-bold text-gray-900">{attendanceRate}%</p>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Charts */}
      {totalRecords > 0 && (
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <div className="bg-white rounded-2xl shadow-lg p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-4">Attendance Overview</h3>
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
          <div className="bg-white rounded-2xl shadow-lg p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-4">Attendance Trend</h3>
            <div className="h-80">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart
                  data={attendanceTrendData}
                  margin={{
                    top: 5,
                    right: 30,
                    left: 20,
                    bottom: 5,
                  }}
                >
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="date" />
                  <YAxis domain={[0, 1]} tickFormatter={(value) => value === 1 ? 'Present' : 'Absent'} />
                  <Tooltip formatter={(value) => value === 1 ? 'Present' : 'Absent'} />
                  <Legend />
                  <Bar dataKey="status" name="Attendance Status" fill="#4f46e5">
                    {attendanceTrendData.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.status === 1 ? '#10b981' : '#ef4444'} />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>
        </div>
      )}

      {/* Attendance Records */}
      <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
        <div className="px-6 py-5 border-b border-gray-200 flex flex-col md:flex-row md:items-center md:justify-between">
          <h3 className="text-lg font-medium text-gray-900">
            {studentId ? 'My Attendance' : courseId ? 'Course Attendance' : 'All Attendance Records'}
          </h3>
          <div className="mt-2 md:mt-0">
            <div className="flex space-x-2">
              <button
                onClick={() => handleSort('date')}
                className={`px-3 py-1 text-sm rounded-md ${
                  sortBy === 'date' 
                    ? 'bg-indigo-100 text-indigo-700' 
                    : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                }`}
              >
                Sort by Date {sortBy === 'date' && (sortOrder === 'asc' ? '↑' : '↓')}
              </button>
              <button
                onClick={() => handleSort('status')}
                className={`px-3 py-1 text-sm rounded-md ${
                  sortBy === 'status' 
                    ? 'bg-indigo-100 text-indigo-700' 
                    : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                }`}
              >
                Sort by Status {sortBy === 'status' && (sortOrder === 'asc' ? '↑' : '↓')}
              </button>
            </div>
          </div>
        </div>
        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th className="px-6 py-3">Student</th>
                <th className="px-6 py-3">Course</th>
                <th className="px-6 py-3">Date</th>
                <th className="px-6 py-3">Status</th>
                <th className="px-6 py-3">Description</th>
              </tr>
            </thead>
            <tbody>
              {sortedAttendances.map((attendance) => (
                <tr key={attendance.id} className="hover:bg-gray-50 transition-colors duration-150">
                  <td className="px-6 py-4">
                    <div className="text-sm font-medium text-gray-900">
                      {attendance.student.firstName} {attendance.student.lastName}
                    </div>
                    <div className="text-sm text-gray-500">
                      {attendance.student.studentNumber}
                    </div>
                  </td>
                  <td className="px-6 py-4">
                    <div className="text-sm font-medium text-gray-900">{attendance.course.name}</div>
                    <div className="text-sm text-gray-500">{attendance.course.code}</div>
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    {new Date(attendance.attendanceDate).toLocaleDateString()}
                  </td>
                  <td className="px-6 py-4">
                    <span className={`px-3 py-1 inline-flex text-sm leading-5 font-semibold rounded-full ${
                      attendance.isPresent 
                        ? 'bg-green-100 text-green-800' 
                        : 'bg-red-100 text-red-800'
                    }`}>
                      {attendance.isPresent ? 'Present' : 'Absent'}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    {attendance.description || '-'}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {sortedAttendances.length === 0 && (
            <div className="text-center py-12">
              <svg className="mx-auto h-12 w-12 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path>
              </svg>
              <h3 className="mt-2 text-sm font-medium text-gray-900">No attendance records found</h3>
              <p className="mt-1 text-sm text-gray-500">No attendance records have been created yet.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AttendanceList;