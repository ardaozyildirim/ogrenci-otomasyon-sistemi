// pages/grades.tsx
import React from 'react';
import Layout from '../components/Layout';
import GradesList from '../components/grades/GradesList';

const GradesPage: React.FC = () => {
  return (
    <Layout>
      <div className="pb-5 border-b border-gray-200">
        <h3 className="text-lg leading-6 font-medium text-gray-900">
          Grades
        </h3>
        <p className="mt-2 max-w-4xl text-sm text-gray-500">
          View all grades
        </p>
      </div>
      <GradesList />
    </Layout>
  );
};

export default GradesPage;