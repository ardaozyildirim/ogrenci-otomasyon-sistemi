// components/attendance/AttendanceList.tsx
import React, { useState, useEffect } from 'react';
import attendanceService, { Attendance } from '../../services/api/attendanceService';

interface AttendanceListProps {
  studentId?: number;
  courseId?: number;
}

const AttendanceList: React.FC<AttendanceListProps> = ({ studentId, courseId }) => {
  const [attendances, setAttendances] = useState<Attendance[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

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

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-md bg-red-50 p-4">
        <div className="text-sm text-red-700">
          {error}
        </div>
      </div>
    );
  }

  return (
    <div className="flex flex-col">
      <div className="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
        <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
          <div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Student
                  </th>
                  <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Course
                  </th>
                  <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Date
                  </th>
                  <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Status
                  </th>
                  <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Description
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {attendances.map((attendance) => (
                  <tr key={attendance.id}>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900">
                        {attendance.student.firstName} {attendance.student.lastName}
                      </div>
                      <div className="text-sm text-gray-500">
                        {attendance.student.studentNumber}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900">{attendance.course.name}</div>
                      <div className="text-sm text-gray-500">{attendance.course.code}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {new Date(attendance.attendanceDate).toLocaleDateString()}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${attendance.isPresent ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                        {attendance.isPresent ? 'Present' : 'Absent'}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {attendance.description}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            {attendances.length === 0 && (
              <div className="text-center py-4">
                <p className="text-sm text-gray-500">No attendance records found</p>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AttendanceList;