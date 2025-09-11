// pages/api-test.tsx
import React, { useState, useEffect } from 'react';
import Layout from '../components/Layout';
import teacherService from '../services/api/teacherService';
import studentService from '../services/api/studentService';
import courseService from '../services/api/courseService';
import gradeService from '../services/api/gradeService';
import attendanceService from '../services/api/attendanceService';

const ApiTestPage: React.FC = () => {
  const [teachers, setTeachers] = useState<any[]>([]);
  const [students, setStudents] = useState<any[]>([]);
  const [courses, setCourses] = useState<any[]>([]);
  const [grades, setGrades] = useState<any[]>([]);
  const [attendances, setAttendances] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const testApis = async () => {
    setLoading(true);
    setError(null);
    
    try {
      // Test teachers API
      const teachersData = await teacherService.getAllTeachers();
      setTeachers(teachersData);
      
      // Test students API
      const studentsData = await studentService.getAllStudents();
      setStudents(studentsData);
      
      // Test courses API
      const coursesData = await courseService.getAllCourses();
      setCourses(coursesData);
      
      // Test grades API
      const gradesData = await gradeService.getAllGrades();
      setGrades(gradesData);
      
      // Test attendances API
      const attendancesData = await attendanceService.getAllAttendances();
      setAttendances(attendancesData);
    } catch (err: any) {
      setError(err.message || 'An error occurred while testing APIs');
      console.error('API Test Error:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    testApis();
  }, []);

  return (
    <Layout>
      <div className="pb-5 border-b border-gray-200 dark:border-gray-700">
        <h1 className="text-2xl font-bold leading-7 text-gray-900 dark:text-white">
          API Connection Test
        </h1>
        <p className="mt-2 max-w-4xl text-sm text-gray-500 dark:text-gray-400">
          Testing connection to backend APIs
        </p>
      </div>
      
      {error && (
        <div className="mt-6 rounded-md bg-red-50 p-4 border-l-4 border-red-400 dark:bg-red-900 dark:border-red-700">
          <div className="flex">
            <div className="flex-shrink-0">
              <svg className="h-5 w-5 text-red-400 dark:text-red-300" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
              </svg>
            </div>
            <div className="ml-3">
              <h3 className="text-sm font-medium text-red-800 dark:text-red-200">
                {error}
              </h3>
            </div>
          </div>
        </div>
      )}
      
      <div className="mt-6 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div className="bg-white rounded-lg shadow p-6 dark:bg-gray-800">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white">Teachers</h3>
          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            {loading ? 'Loading...' : `${teachers.length} teachers found`}
          </p>
          {teachers.length > 0 && (
            <ul className="mt-4 space-y-2">
              {teachers.slice(0, 3).map(teacher => (
                <li key={teacher.id} className="text-sm">
                  <span className="font-medium">{teacher.firstName} {teacher.lastName}</span>
                  <span className="text-gray-500 dark:text-gray-400"> - {teacher.department}</span>
                </li>
              ))}
              {teachers.length > 3 && (
                <li className="text-sm text-gray-500 dark:text-gray-400">
                  ... and {teachers.length - 3} more
                </li>
              )}
            </ul>
          )}
        </div>
        
        <div className="bg-white rounded-lg shadow p-6 dark:bg-gray-800">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white">Students</h3>
          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            {loading ? 'Loading...' : `${students.length} students found`}
          </p>
          {students.length > 0 && (
            <ul className="mt-4 space-y-2">
              {students.slice(0, 3).map(student => (
                <li key={student.id} className="text-sm">
                  <span className="font-medium">{student.firstName} {student.lastName}</span>
                  <span className="text-gray-500 dark:text-gray-400"> - {student.studentNumber}</span>
                </li>
              ))}
              {students.length > 3 && (
                <li className="text-sm text-gray-500 dark:text-gray-400">
                  ... and {students.length - 3} more
                </li>
              )}
            </ul>
          )}
        </div>
        
        <div className="bg-white rounded-lg shadow p-6 dark:bg-gray-800">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white">Courses</h3>
          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            {loading ? 'Loading...' : `${courses.length} courses found`}
          </p>
          {courses.length > 0 && (
            <ul className="mt-4 space-y-2">
              {courses.slice(0, 3).map(course => (
                <li key={course.id} className="text-sm">
                  <span className="font-medium">{course.name}</span>
                  <span className="text-gray-500 dark:text-gray-400"> - {course.code}</span>
                </li>
              ))}
              {courses.length > 3 && (
                <li className="text-sm text-gray-500 dark:text-gray-400">
                  ... and {courses.length - 3} more
                </li>
              )}
            </ul>
          )}
        </div>
        
        <div className="bg-white rounded-lg shadow p-6 dark:bg-gray-800">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white">Grades</h3>
          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            {loading ? 'Loading...' : `${grades.length} grades found`}
          </p>
          {grades.length > 0 && (
            <ul className="mt-4 space-y-2">
              {grades.slice(0, 3).map(grade => (
                <li key={grade.id} className="text-sm">
                  <span className="font-medium">{grade.student.firstName} {grade.student.lastName}</span>
                  <span className="text-gray-500 dark:text-gray-400"> - {grade.gradeValue}</span>
                </li>
              ))}
              {grades.length > 3 && (
                <li className="text-sm text-gray-500 dark:text-gray-400">
                  ... and {grades.length - 3} more
                </li>
              )}
            </ul>
          )}
        </div>
        
        <div className="bg-white rounded-lg shadow p-6 dark:bg-gray-800">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white">Attendances</h3>
          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            {loading ? 'Loading...' : `${attendances.length} attendances found`}
          </p>
          {attendances.length > 0 && (
            <ul className="mt-4 space-y-2">
              {attendances.slice(0, 3).map(attendance => (
                <li key={attendance.id} className="text-sm">
                  <span className="font-medium">{attendance.student.firstName} {attendance.student.lastName}</span>
                  <span className="text-gray-500 dark:text-gray-400"> - {attendance.isPresent ? 'Present' : 'Absent'}</span>
                </li>
              ))}
              {attendances.length > 3 && (
                <li className="text-sm text-gray-500 dark:text-gray-400">
                  ... and {attendances.length - 3} more
                </li>
              )}
            </ul>
          )}
        </div>
      </div>
      
      <div className="mt-6">
        <button
          onClick={testApis}
          disabled={loading}
          className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50"
        >
          {loading ? (
            <>
              <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Testing...
            </>
          ) : 'Test APIs Again'}
        </button>
      </div>
    </Layout>
  );
};

export default ApiTestPage;