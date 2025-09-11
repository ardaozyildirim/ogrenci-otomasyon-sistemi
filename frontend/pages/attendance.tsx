// pages/attendance.tsx
import React from 'react';
import Layout from '../components/Layout';
import AttendanceList from '../components/attendance/AttendanceList';

const AttendancePage: React.FC = () => {
  return (
    <Layout>
      <div className="pb-5 border-b border-gray-200">
        <h3 className="text-lg leading-6 font-medium text-gray-900">
          Attendance
        </h3>
        <p className="mt-2 max-w-4xl text-sm text-gray-500">
          View all attendance records
        </p>
      </div>
      <AttendanceList />
    </Layout>
  );
};

export default AttendancePage;