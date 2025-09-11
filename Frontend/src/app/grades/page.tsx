'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';

interface Grade {
  id: number;
  studentId: number;
  studentName: string;
  courseId: number;
  courseName: string;
  grade: number;
  gradeType: 'Midterm' | 'Final' | 'Assignment' | 'Quiz';
  semester: string;
  date: string;
}

export default function GradesPage() {
  const [grades, setGrades] = useState<Grade[]>([]);
  const [loading, setLoading] = useState(true);
  const [showAddForm, setShowAddForm] = useState(false);
  const [newGrade, setNewGrade] = useState({
    studentId: 1,
    courseId: 1,
    grade: 0,
    gradeType: 'Midterm' as const,
    semester: 'Fall 2024',
    date: new Date().toISOString().split('T')[0]
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
    // TODO: Load grades from API
    setTimeout(() => {
      setGrades([
        {
          id: 1,
          studentId: 1,
          studentName: 'John Doe',
          courseId: 1,
          courseName: 'Introduction to Programming',
          grade: 85,
          gradeType: 'Midterm',
          semester: 'Fall 2024',
          date: '2024-10-15'
        },
        {
          id: 2,
          studentId: 2,
          studentName: 'Jane Smith',
          courseId: 2,
          courseName: 'Calculus I',
          grade: 92,
          gradeType: 'Final',
          semester: 'Fall 2024',
          date: '2024-12-10'
        }
      ]);
      setLoading(false);
    }, 1000);
  }, []);

  const handleAddGrade = (e: React.FormEvent) => {
    e.preventDefault();
    
    const selectedStudent = students.find(s => s.id === newGrade.studentId);
    const selectedCourse = courses.find(c => c.id === newGrade.courseId);
    
    const grade: Grade = {
      id: grades.length + 1,
      ...newGrade,
      studentName: selectedStudent?.name || '',
      courseName: selectedCourse?.name || ''
    };
    
    setGrades([...grades, grade]);
    setNewGrade({
      studentId: 1,
      courseId: 1,
      grade: 0,
      gradeType: 'Midterm',
      semester: 'Fall 2024',
      date: new Date().toISOString().split('T')[0]
    });
    setShowAddForm(false);
  };

  const handleDeleteGrade = (id: number) => {
    setGrades(grades.filter(grade => grade.id !== id));
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
        {loading ? (
          <div className="text-center py-12">
            <div className="text-gray-500">Loading grades...</div>
          </div>
        ) : (
          <div className="bg-white shadow overflow-hidden sm:rounded-md">
            <ul className="divide-y divide-gray-200">
              {grades.map((grade) => (
                <li key={grade.id}>
                  <div className="px-4 py-4 flex items-center justify-between">
                    <div className="flex items-center">
                      <div className="flex-shrink-0">
                        <div className="h-10 w-10 rounded-full bg-indigo-500 flex items-center justify-center">
                          <span className="text-white font-medium">
                            {grade.grade}
                          </span>
                        </div>
                      </div>
                      <div className="ml-4">
                        <div className="text-sm font-medium text-gray-900">
                          {grade.studentName}
                        </div>
                        <div className="text-sm text-gray-500">
                          {grade.courseName}
                        </div>
                        <div className="text-sm text-gray-500">
                          {grade.semester} • {grade.date}
                        </div>
                      </div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getGradeTypeColor(grade.gradeType)}`}>
                        {grade.gradeType}
                      </span>
                      <span className={`text-lg font-bold ${getGradeColor(grade.grade)}`}>
                        {grade.grade}
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
              ))}
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
                          {student.name}
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
                      value={newGrade.grade}
                      onChange={(e) => setNewGrade({...newGrade, grade: parseInt(e.target.value)})}
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
