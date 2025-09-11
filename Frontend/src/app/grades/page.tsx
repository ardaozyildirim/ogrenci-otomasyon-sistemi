'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/contexts/AuthContext';
import { apiService, GradeDto, CreateGradeRequest } from '@/lib/api';

export default function GradesPage() {
  const [grades, setGrades] = useState<GradeDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [showAddForm, setShowAddForm] = useState(false);
  const [error, setError] = useState('');
  const [students, setStudents] = useState<any[]>([]);
  const [courses, setCourses] = useState<any[]>([]);
  const [newGrade, setNewGrade] = useState<CreateGradeRequest>({
    studentId: 1,
    courseId: 1,
    score: 0,
    gradeType: 'Midterm',
    semester: 'Fall 2024',
    date: new Date().toISOString().split('T')[0]
  });
  const { isAuthenticated, isLoading } = useAuth();
  const router = useRouter();

  useEffect(() => {
    // Wait for auth loading to complete
    if (isLoading) return;
    
    if (!isAuthenticated) {
      router.push('/login');
      return;
    }

    loadData();
  }, [isAuthenticated, isLoading, router]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [gradesData, studentsData, coursesData] = await Promise.all([
        apiService.getGrades(),
        apiService.getStudents(),
        apiService.getCourses()
      ]);
      // Ensure data is arrays
      setGrades(Array.isArray(gradesData) ? gradesData : []);
      setStudents(Array.isArray(studentsData) ? studentsData : []);
      setCourses(Array.isArray(coursesData) ? coursesData : []);
    } catch (err: any) {
      console.error('Error loading data:', err);
      setError(err.message || 'Failed to load data');
      // Set empty arrays on error
      setGrades([]);
      setStudents([]);
      setCourses([]);
    } finally {
      setLoading(false);
    }
  };

  const handleAddGrade = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      await apiService.createGrade(newGrade);
      
      // Reset form and reload data
      setNewGrade({
        studentId: students[0]?.id || 1,
        courseId: courses[0]?.id || 1,
        score: 0,
        gradeType: 'Midterm',
        semester: 'Fall 2024',
        date: new Date().toISOString().split('T')[0]
      });
      setShowAddForm(false);
      await loadData();
    } catch (err: any) {
      setError(err.message || 'Failed to create grade');
    }
  };

  const handleDeleteGrade = async (id: number) => {
    try {
      await apiService.deleteGrade(id);
      await loadData();
    } catch (err: any) {
      setError(err.message || 'Failed to delete grade');
    }
  };

  const getGradeColor = (grade: number) => {
    if (grade >= 90) return 'text-green-600';
    if (grade >= 80) return 'text-blue-600';
    if (grade >= 70) return 'text-yellow-600';
    if (grade >= 60) return 'text-orange-600';
    return 'text-red-600';
  };

  const getGradeTypeColor = (type: string) => {
    switch (type) {
      case 'Midterm':
        return 'bg-blue-100 text-blue-800';
      case 'Final':
        return 'bg-green-100 text-green-800';
      case 'Assignment':
        return 'bg-purple-100 text-purple-800';
      case 'Quiz':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-6">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Grades</h1>
              <p className="text-gray-600">Manage student grades</p>
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
                Add Grade
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        {error && (
          <div className="mb-4 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
            {error}
          </div>
        )}

        {loading ? (
          <div className="text-center py-12">
            <div className="text-gray-500">Loading grades...</div>
          </div>
        ) : (
          <div className="bg-white shadow overflow-hidden sm:rounded-md">
            <ul className="divide-y divide-gray-200">
              {grades && grades.length > 0 ? grades.map((grade) => (
                <li key={grade.id}>
                  <div className="px-4 py-4 flex items-center justify-between">
                    <div className="flex items-center">
                      <div className="flex-shrink-0">
                        <div className="h-10 w-10 rounded-full bg-indigo-500 flex items-center justify-center">
                          <span className="text-white font-medium">
                            {grade.score || 0}
                          </span>
                        </div>
                      </div>
                      <div className="ml-4">
                        <div className="text-sm font-medium text-gray-900">
                          {grade.studentName || 'Unknown Student'}
                        </div>
                        <div className="text-sm text-gray-500">
                          {grade.courseName || 'Unknown Course'}
                        </div>
                        <div className="text-sm text-gray-500">
                          {grade.semester || 'No semester'} • {grade.date || 'No date'}
                        </div>
                      </div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getGradeTypeColor(grade.gradeType || 'Other')}`}>
                        {grade.gradeType || 'Other'}
                      </span>
                      <span className={`text-lg font-bold ${getGradeColor(grade.score || 0)}`}>
                        {grade.score || 0}
                      </span>
                      <button
                        onClick={() => handleDeleteGrade(grade.id)}
                        className="text-red-600 hover:text-red-900"
                      >
                        Delete
                      </button>
                    </div>
                  </div>
                </li>
              )) : (
                <li className="px-4 py-8 text-center text-gray-500">
                  No grades found
                </li>
              )}
            </ul>
          </div>
        )}

        {/* Add Grade Modal */}
        {showAddForm && (
          <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
            <div className="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
              <div className="mt-3">
                <h3 className="text-lg font-medium text-gray-900 mb-4">Add New Grade</h3>
                <form onSubmit={handleAddGrade} className="space-y-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Student</label>
                    <select
                      value={newGrade.studentId}
                      onChange={(e) => setNewGrade({...newGrade, studentId: parseInt(e.target.value)})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    >
                      {students.map(student => (
                        <option key={student.id} value={student.id}>
                          {student.user.firstName} {student.user.lastName}
                        </option>
                      ))}
                    </select>
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Course</label>
                    <select
                      value={newGrade.courseId}
                      onChange={(e) => setNewGrade({...newGrade, courseId: parseInt(e.target.value)})}
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
                    <label className="block text-sm font-medium text-gray-700">Grade</label>
                    <input
                      type="number"
                      min="0"
                      max="100"
                      required
                      value={newGrade.score}
                      onChange={(e) => setNewGrade({...newGrade, score: parseInt(e.target.value)})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Grade Type</label>
                    <select
                      value={newGrade.gradeType}
                      onChange={(e) => setNewGrade({...newGrade, gradeType: e.target.value as any})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    >
                      <option value="Midterm">Midterm</option>
                      <option value="Final">Final</option>
                      <option value="Assignment">Assignment</option>
                      <option value="Quiz">Quiz</option>
                    </select>
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Semester</label>
                    <input
                      type="text"
                      required
                      value={newGrade.semester}
                      onChange={(e) => setNewGrade({...newGrade, semester: e.target.value})}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Date</label>
                    <input
                      type="date"
                      required
                      value={newGrade.date}
                      onChange={(e) => setNewGrade({...newGrade, date: e.target.value})}
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
                      Add Grade
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
