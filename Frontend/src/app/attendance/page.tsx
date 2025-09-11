'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';

interface Attendance {
  id: number;
  studentId: number;
  studentName: string;
  courseId: number;
  courseName: string;
  date: string;
  status: 'Present' | 'Absent' | 'Late' | 'Excused';
  notes?: string;
}

export default function AttendancePage() {
  const [attendance, setAttendance] = useState<Attendance[]>([]);
  const [loading, setLoading] = useState(true);
  const [showAddForm, setShowAddForm] = useState(false);
  const [newAttendance, setNewAttendance] = useState({
    studentId: 1,
    courseId: 1,
    date: new Date().toISOString().split('T')[0],
    status: 'Present' as const,
    notes: ''
  });

  const students = [
    { id: 1, name: 'John Doe' },
    { id: 2, name: 'Jane Smith' }
  ];

  const courses = [
    { id: 1, name: 'Introduction to Programming' },
    { id: 2, name: 'Calculus I' }
  ];

  useEffect(() => {
    // TODO: Load attendance from API
    setTimeout(() => {
      setAttendance([
        {
          id: 1,
          studentId: 1,
          studentName: 'John Doe',
          courseId: 1,
          courseName: 'Introduction to Programming',
          date: '2024-10-15',
          status: 'Present',
          notes: ''
        },
        {
          id: 2,
          studentId: 2,
          studentName: 'Jane Smith',
          courseId: 2,
          courseName: 'Calculus I',
          date: '2024-10-15',
          status: 'Late',
          notes: 'Traffic delay'
        },
        {
          id: 3,
          studentId: 1,
          studentName: 'John Doe',
          courseId: 1,
          courseName: 'Introduction to Programming',
          date: '2024-10-16',
          status: 'Absent',
          notes: 'Sick leave'
        }
      ]);
      setLoading(false);
    }, 1000);
  }, []);

  const handleAddAttendance = (e: React.FormEvent) => {
    e.preventDefault();
    
    const selectedStudent = students.find(s => s.id === newAttendance.studentId);
    const selectedCourse = courses.find(c => c.id === newAttendance.courseId);
    
    const attendanceRecord: Attendance = {
      id: attendance.length + 1,
      ...newAttendance,
      studentName: selectedStudent?.name || '',
      courseName: selectedCourse?.name || ''
    };
    
    setAttendance([...attendance, attendanceRecord]);
    setNewAttendance({
      studentId: 1,
      courseId: 1,
      date: new Date().toISOString().split('T')[0],
      status: 'Present',
      notes: ''
    });
    setShowAddForm(false);
  };

  const handleDeleteAttendance = (id: number) => {
    setAttendance(attendance.filter(record => record.id !== id));
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Present':
        return 'bg-green-100 text-green-800';
      case 'Absent':
        return 'bg-red-100 text-red-800';
      case 'Late':
        return 'bg-yellow-100 text-yellow-800';
      case 'Excused':
        return 'bg-blue-100 text-blue-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Present':
        return '✓';
      case 'Absent':
        return '✗';
      case 'Late':
        return '⏰';
      case 'Excused':
        return '📝';
      default:
        return '?';
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-6">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Attendance</h1>
              <p className="text-gray-600">Manage student attendance</p>
            </div>
            <div className="flex items-center space-x-4">
              <Link
                href="/dashboard"
                className="text-gray-600 hover:text-gray-900"
              >
                Back to Dashboard
              </Link>
              <button
                onClick={() => setShowAddForm(true)}
                className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
              >
                Add Attendance
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        {loading ? (
          <div className="text-center py-12">
            <div className="text-gray-500">Loading attendance...</div>
          </div>
        ) : (
          <div className="bg-white shadow overflow-hidden sm:rounded-md">
            <ul className="divide-y divide-gray-200">
              {attendance.map((record) => (
                <li key={record.id}>
                  <div className="px-4 py-4 flex items-center justify-between">
                    <div className="flex items-center">
                      <div className="flex-shrink-0">
                        <div className="h-10 w-10 rounded-full bg-orange-500 flex items-center justify-center">
                          <span className="text-white font-medium text-lg">
                            {getStatusIcon(record.status)}
                          </span>
                        </div>
                      </div>
                      <div className="ml-4">
                        <div className="text-sm font-medium text-gray-900">
                          {record.studentName}
                        </div>
                        <div className="text-sm text-gray-500">
                          {record.courseName}
                        </div>
                        <div className="text-sm text-gray-500">
                          {record.date} {record.notes && `• ${record.notes}`}
                        </div>
                      </div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(record.status)}`}>
                        {record.status}
                      </span>
                      <button
                        onClick={() => handleDeleteAttendance(record.id)}
                        className="text-red-600 hover:text-red-900"
                      >
                        Delete
                      </button>
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          </div>
        )}

        {/* Add Attendance Modal */}
        {showAddForm && (
          <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
            <div className="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
              <div className="mt-3">
                <h3 className="text-lg font-medium text-gray-900 mb-4">Add Attendance Record</h3>
                <form onSubmit={handleAddAttendance} className="space-y-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Student</label>
                    <select
                      value={newAttendance.studentId}
                      onChange={(e) => setNewAttendance({...newAttendance, studentId: parseInt(e.target.value)})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    >
                      {students.map(student => (
                        <option key={student.id} value={student.id}>
                          {student.name}
                        </option>
                      ))}
                    </select>
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Course</label>
                    <select
                      value={newAttendance.courseId}
                      onChange={(e) => setNewAttendance({...newAttendance, courseId: parseInt(e.target.value)})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    >
                      {courses.map(course => (
                        <option key={course.id} value={course.id}>
                          {course.name}
                        </option>
                      ))}
                    </select>
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Date</label>
                    <input
                      type="date"
                      required
                      value={newAttendance.date}
                      onChange={(e) => setNewAttendance({...newAttendance, date: e.target.value})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Status</label>
                    <select
                      value={newAttendance.status}
                      onChange={(e) => setNewAttendance({...newAttendance, status: e.target.value as any})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    >
                      <option value="Present">Present</option>
                      <option value="Absent">Absent</option>
                      <option value="Late">Late</option>
                      <option value="Excused">Excused</option>
                    </select>
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Notes (Optional)</label>
                    <textarea
                      value={newAttendance.notes}
                      onChange={(e) => setNewAttendance({...newAttendance, notes: e.target.value})}
                      rows={2}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  
                  <div className="flex justify-end space-x-3 pt-4">
                    <button
                      type="button"
                      onClick={() => setShowAddForm(false)}
                      className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
                    >
                      Cancel
                    </button>
                    <button
                      type="submit"
                      className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                    >
                      Add Attendance
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}
