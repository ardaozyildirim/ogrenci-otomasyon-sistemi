// pages/dashboard.tsx
import React from 'react';
import Layout from '../components/Layout';
import DashboardComponent from '../components/Dashboard';

const DashboardPage: React.FC = () => {
  return (
    <Layout>
      <DashboardComponent />
    </Layout>
  );
};

export default DashboardPage;