// pages/dashboard.tsx
import React from 'react';
import DashboardComponent from '../components/Dashboard';
import Layout from '../components/Layout';

const DashboardPage: React.FC = () => {
  return (
    <Layout>
      <div className="pb-5 border-b border-gray-200">
        <h1 className="text-2xl font-bold leading-7 text-gray-900 sm:text-3xl sm:truncate">
          Dashboard
        </h1>
        <p className="mt-2 max-w-4xl text-sm text-gray-500">
          Welcome to your student management dashboard. Here you can view all your academic information at a glance.
        </p>
      </div>
      <div className="mt-6">
        <DashboardComponent />
      </div>
    </Layout>
  );
};

export default DashboardPage;