'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/contexts/AuthContext';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const { login } = useAuth();
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
      await login({ email, password });
      router.push('/dashboard');
    } catch (err: any) {
      setError(err.message || 'Login failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleQuickLogin = async (userType: string) => {
    setLoading(true);
    setError('');
    
    let credentials;
    switch (userType) {
      case 'admin':
        credentials = { email: 'admin@test.com', password: 'Admin123' };
        break;
      case 'teacher':
        credentials = { email: 'teacher@test.com', password: 'Teacher123' };
        break;
      case 'student':
        credentials = { email: 'student@test.com', password: 'Student123' };
        break;
      default:
        return;
    }
    
    try {
      await login(credentials);
      router.push('/dashboard');
    } catch (err: any) {
      setError(err.message || 'Login failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full">
        {/* Header Section */}
        <div className="text-center mb-8">
          <div className="mx-auto bg-blue-600 rounded-lg p-3 w-16 h-16 flex items-center justify-center mb-4">
            <svg className="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
            </svg>
          </div>
          <h2 className="text-2xl font-bold text-gray-900 mb-1">
            Student Management System
          </h2>
          <p className="text-gray-600">Sign in to your account</p>
        </div>
        
        {/* Login Form */}
        <div className="bg-white rounded-lg shadow border border-gray-200">
          <div className="px-6 py-8">
            <form className="space-y-6" onSubmit={handleSubmit}>
              <div className="space-y-4">
                <div>
                  <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-1">
                    Email Address
                  </label>
                  <input
                    id="email"
                    name="email"
                    type="email"
                    required
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500"
                    placeholder="Enter your email"
                  />
                </div>
                
                <div>
                  <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">
                    Password
                  </label>
                  <input
                    id="password"
                    name="password"
                    type="password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500"
                    placeholder="Enter your password"
                  />
                </div>
              </div>

              {error && (
                <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-md text-sm">
                  {error}
                </div>
              )}

              <button
                type="submit"
                disabled={loading}
                className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-md disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {loading ? 'Signing in...' : 'Sign In'}
              </button>
              
              <div className="text-center">
                <a href="/register" className="text-blue-600 hover:text-blue-700 text-sm">
                  Don&apos;t have an account? Sign up
                </a>
              </div>
            </form>
          </div>
        </div>
        
        {/* Quick Login Section */}
        <div className="mt-6">
          <div className="relative mb-4">
            <div className="absolute inset-0 flex items-center">
              <div className="w-full border-t border-gray-300" />
            </div>
            <div className="relative flex justify-center text-sm">
              <span className="px-2 bg-gray-100 text-gray-500">
                Quick Login (Demo)
              </span>
            </div>
          </div>
          
          <div className="space-y-2">
            <button
              type="button"
              onClick={() => handleQuickLogin('admin')}
              disabled={loading}
              className="w-full bg-white border border-gray-300 rounded-md p-3 text-left hover:bg-gray-50 disabled:opacity-50"
            >
              <div className="flex items-center">
                <div className="bg-red-600 rounded p-2 mr-3">
                  <svg className="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
                  </svg>
                </div>
                <div className="flex-1">
                  <div className="text-sm font-medium text-gray-900">
                    Admin Access
                  </div>
                  <div className="text-xs text-gray-500">admin@test.com</div>
                </div>
              </div>
            </button>
            
            <button
              type="button"
              onClick={() => handleQuickLogin('teacher')}
              disabled={loading}
              className="w-full bg-white border border-gray-300 rounded-md p-3 text-left hover:bg-gray-50 disabled:opacity-50"
            >
              <div className="flex items-center">
                <div className="bg-blue-600 rounded p-2 mr-3">
                  <svg className="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                  </svg>
                </div>
                <div className="flex-1">
                  <div className="text-sm font-medium text-gray-900">
                    Teacher Access
                  </div>
                  <div className="text-xs text-gray-500">teacher@test.com</div>
                </div>
              </div>
            </button>
            
            <button
              type="button"
              onClick={() => handleQuickLogin('student')}
              disabled={loading}
              className="w-full bg-white border border-gray-300 rounded-md p-3 text-left hover:bg-gray-50 disabled:opacity-50"
            >
              <div className="flex items-center">
                <div className="bg-green-600 rounded p-2 mr-3">
                  <svg className="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 14l9-5-9-5-9 5 9 5z" />
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 14l6.16-3.422a12.083 12.083 0 01.665 6.479A11.952 11.952 0 0012 20.055a11.952 11.952 0 00-6.824-2.998 12.078 12.078 0 01.665-6.479L12 14z" />
                  </svg>
                </div>
                <div className="flex-1">
                  <div className="text-sm font-medium text-gray-900">
                    Student Access
                  </div>
                  <div className="text-xs text-gray-500">student@test.com</div>
                </div>
              </div>
            </button>
          </div>
          
          <div className="mt-4 bg-yellow-50 border border-yellow-200 rounded-md p-3">
            <p className="text-xs text-yellow-700">
              <strong>Demo Mode:</strong> Use test accounts above or any email with passwords: Admin123, Teacher123, Student123
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
