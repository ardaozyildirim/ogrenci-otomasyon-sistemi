// pages/courses.tsx
import React from 'react';
import Layout from '../components/Layout';

const CoursesPage: React.FC = () => {
  return (
    <Layout>
      <div className="pb-5 border-b border-gray-200">
        <h3 className="text-lg leading-6 font-medium text-gray-900">
          Courses
        </h3>
        <p className="mt-2 max-w-4xl text-sm text-gray-500">
          Manage courses
        </p>
      </div>
      <div className="mt-6">
        <p>Course management features will be implemented here.</p>
      </div>
    </Layout>
  );
};

export default CoursesPage;