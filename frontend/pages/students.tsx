// pages/students.tsx
import React from 'react';
import Layout from '../components/Layout';

const StudentsPage: React.FC = () => {
  return (
    <Layout>
      <div className="pb-5 border-b border-gray-200">
        <h3 className="text-lg leading-6 font-medium text-gray-900">
          Students
        </h3>
        <p className="mt-2 max-w-4xl text-sm text-gray-500">
          Manage students
        </p>
      </div>
      <div className="mt-6">
        <p>Student management features will be implemented here.</p>
      </div>
    </Layout>
  );
};

export default StudentsPage;