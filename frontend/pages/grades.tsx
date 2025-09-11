// pages/grades.tsx
import React from 'react';
import Layout from '../components/Layout';

const GradesPage: React.FC = () => {
  return (
    <Layout>
      <div className="pb-5 border-b border-gray-200 dark:border-gray-700">
        <h1 className="text-2xl font-bold leading-7 text-gray-900 dark:text-white">
          Grades
        </h1>
        <p className="mt-2 max-w-4xl text-sm text-gray-500 dark:text-gray-400">
          View and manage student grades
        </p>
      </div>
      <div className="mt-6 bg-white rounded-lg shadow p-6 dark:bg-gray-800">
        <div className="text-center py-8">
          <div className="mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-blue-100 dark:bg-blue-900">
            <svg className="h-6 w-6 text-blue-600 dark:text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
            </svg>
          </div>
          <h3 className="mt-4 text-lg font-medium text-gray-900 dark:text-white">Grade Management</h3>
          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            Grade management features are coming soon.
          </p>
          <div className="mt-6">
            <button className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500">
              Get notified when ready
            </button>
          </div>
          <div className="mt-4">
            <p className="text-xs text-gray-400 dark:text-gray-500">
              Expected release: Q1 2026
            </p>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default GradesPage;