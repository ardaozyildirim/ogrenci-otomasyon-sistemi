// components/grades/GradesList.tsx
import React, { useState, useEffect } from 'react';
import gradeService, { Grade } from '../../services/api/gradeService';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, LineChart, Line } from 'recharts';

interface GradesListProps {
  studentId?: number;
  courseId?: number;
}

const GradesList: React.FC<GradesListProps> = ({ studentId, courseId }) => {
  const [grades, setGrades] = useState<Grade[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [sortBy, setSortBy] = useState<'date' | 'grade'>('date');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc');

  useEffect(() => {
    const fetchGrades = async () => {
      try {
        setLoading(true);
        let data: Grade[] = [];
        
        if (studentId) {
          data = await gradeService.getGradesByStudentId(studentId);
        } else if (courseId) {
          data = await gradeService.getGradesByCourseId(courseId);
        } else {
          data = await gradeService.getAllGrades();
        }
        
        setGrades(data);
      } catch (err: any) {
        setError(err.message || 'Failed to fetch grades');
      } finally {
        setLoading(false);
      }
    };

    fetchGrades();
  }, [studentId, courseId]);

  const sortedGrades = [...grades].sort((a, b) => {
    if (sortBy === 'date') {
      const dateA = new Date(a.gradeDate).getTime();
      const dateB = new Date(b.gradeDate).getTime();
      return sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
    } else {
      return sortOrder === 'asc' ? a.gradeValue - b.gradeValue : b.gradeValue - a.gradeValue;
    }
  });

  const handleSort = (field: 'date' | 'grade') => {
    if (sortBy === field) {
      setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
    } else {
      setSortBy(field);
      setSortOrder('desc');
    }
  };

  // Calculate statistics
  const totalGrades = grades.length;
  const averageGrade = totalGrades > 0 
    ? grades.reduce((sum, grade) => sum + grade.gradeValue, 0) / totalGrades 
    : 0;
  
  const highestGrade = totalGrades > 0 ? Math.max(...grades.map(g => g.gradeValue)) : 0;
  const lowestGrade = totalGrades > 0 ? Math.min(...grades.map(g => g.gradeValue)) : 0;

  // Prepare data for charts
  const gradeDistributionData = [
    { name: 'Excellent (90-100)', count: grades.filter(g => g.gradeValue >= 90).length },
    { name: 'Good (80-89)', count: grades.filter(g => g.gradeValue >= 80 && g.gradeValue < 90).length },
    { name: 'Average (70-79)', count: grades.filter(g => g.gradeValue >= 70 && g.gradeValue < 80).length },
    { name: 'Below Average (60-69)', count: grades.filter(g => g.gradeValue >= 60 && g.gradeValue < 70).length },
    { name: 'Poor (0-59)', count: grades.filter(g => g.gradeValue < 60).length },
  ];

  // Prepare data for grade trend over time
  const gradeTrendData = sortedGrades
    .slice(0, 10) // Take last 10 grades
    .map(grade => ({
      date: new Date(grade.gradeDate).toLocaleDateString(),
      grade: grade.gradeValue,
      course: grade.course.name
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
      {/* Grade Statistics */}
      {totalGrades > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-blue-500">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-blue-100">
                <svg className="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
                </svg>
              </div>
              <div className="ml-4">
                <h3 className="text-sm font-medium text-gray-500">Total Grades</h3>
                <p className="text-2xl font-bold text-gray-900">{totalGrades}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-green-500">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-green-100">
                <svg className="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
                </svg>
              </div>
              <div className="ml-4">
                <h3 className="text-sm font-medium text-gray-500">Average Grade</h3>
                <p className="text-2xl font-bold text-gray-900">{averageGrade.toFixed(2)}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-indigo-500">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-indigo-100">
                <svg className="w-6 h-6 text-indigo-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143L13 3z"></path>
                </svg>
              </div>
              <div className="ml-4">
                <h3 className="text-sm font-medium text-gray-500">Highest Grade</h3>
                <p className="text-2xl font-bold text-gray-900">{highestGrade}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-red-500">
            <div className="flex items-center">
              <div className="p-3 rounded-lg bg-red-100">
                <svg className="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M13 17h8m0 0V9m0 8l-8-8-4 4-6-6"></path>
                </svg>
              </div>
              <div className="ml-4">
                <h3 className="text-sm font-medium text-gray-500">Lowest Grade</h3>
                <p className="text-2xl font-bold text-gray-900">{lowestGrade}</p>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Charts */}
      {totalGrades > 0 && (
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <div className="bg-white rounded-2xl shadow-lg p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-4">Grade Distribution</h3>
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
                  <Bar dataKey="count" name="Number of Grades" fill="#4f46e5" />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>
          <div className="bg-white rounded-2xl shadow-lg p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-4">Grade Trend</h3>
            <div className="h-80">
              <ResponsiveContainer width="100%" height="100%">
                <LineChart
                  data={gradeTrendData}
                  margin={{
                    top: 5,
                    right: 30,
                    left: 20,
                    bottom: 5,
                  }}
                >
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="date" />
                  <YAxis domain={[0, 100]} />
                  <Tooltip />
                  <Legend />
                  <Line 
                    type="monotone" 
                    dataKey="grade" 
                    name="Grade Value" 
                    stroke="#4f46e5" 
                    activeDot={{ r: 8 }} 
                  />
                </LineChart>
              </ResponsiveContainer>
            </div>
          </div>
        </div>
      )}

      {/* Grades Table */}
      <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
        <div className="px-6 py-5 border-b border-gray-200 flex flex-col md:flex-row md:items-center md:justify-between">
          <h3 className="text-lg font-medium text-gray-900">
            {studentId ? 'My Grades' : courseId ? 'Course Grades' : 'All Grades'}
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
                onClick={() => handleSort('grade')}
                className={`px-3 py-1 text-sm rounded-md ${
                  sortBy === 'grade' 
                    ? 'bg-indigo-100 text-indigo-700' 
                    : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                }`}
              >
                Sort by Grade {sortBy === 'grade' && (sortOrder === 'asc' ? '↑' : '↓')}
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
                <th className="px-6 py-3">Grade</th>
                <th className="px-6 py-3">Date</th>
                <th className="px-6 py-3">Description</th>
              </tr>
            </thead>
            <tbody>
              {sortedGrades.map((grade) => (
                <tr key={grade.id} className="hover:bg-gray-50 transition-colors duration-150">
                  <td className="px-6 py-4">
                    <div className="text-sm font-medium text-gray-900">
                      {grade.student.firstName} {grade.student.lastName}
                    </div>
                    <div className="text-sm text-gray-500">
                      {grade.student.studentNumber}
                    </div>
                  </td>
                  <td className="px-6 py-4">
                    <div className="text-sm font-medium text-gray-900">{grade.course.name}</div>
                    <div className="text-sm text-gray-500">{grade.course.code}</div>
                  </td>
                  <td className="px-6 py-4">
                    <span className={`px-3 py-1 inline-flex text-sm leading-5 font-semibold rounded-full ${
                      grade.gradeValue >= 90 ? 'bg-green-100 text-green-800' :
                      grade.gradeValue >= 80 ? 'bg-blue-100 text-blue-800' :
                      grade.gradeValue >= 70 ? 'bg-yellow-100 text-yellow-800' :
                      grade.gradeValue >= 60 ? 'bg-orange-100 text-orange-800' :
                      'bg-red-100 text-red-800'
                    }`}>
                      {grade.gradeValue}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    {new Date(grade.gradeDate).toLocaleDateString()}
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    {grade.description || '-'}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {sortedGrades.length === 0 && (
            <div className="text-center py-12">
              <svg className="mx-auto h-12 w-12 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
              </svg>
              <h3 className="mt-2 text-sm font-medium text-gray-900">No grades found</h3>
              <p className="mt-1 text-sm text-gray-500">No grades have been recorded yet.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default GradesList;