'use client';

import React, { useState } from 'react';
import { apiService } from '@/lib/api';

export default function ApiTestPage() {
  const [results, setResults] = useState<any[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const addResult = (result: any) => {
    setResults(prev => [result, ...prev.slice(0, 9)]); // Keep last 10 results
  };

  const testConnection = async () => {
    setIsLoading(true);
    const startTime = Date.now();
    
    try {
      const success = await apiService.testConnection();
      const duration = Date.now() - startTime;
      
      addResult({
        id: Date.now(),
        type: 'Connection Test',
        success,
        duration: `${duration}ms`,
        message: success ? 'API server is reachable' : 'API server is not reachable',
        timestamp: new Date().toLocaleTimeString()
      });
    } catch (error: any) {
      addResult({
        id: Date.now(),
        type: 'Connection Test',
        success: false,
        error: error.message,
        timestamp: new Date().toLocaleTimeString()
      });
    } finally {
      setIsLoading(false);
    }
  };

  const testLogin = async () => {
    setIsLoading(true);
    const startTime = Date.now();
    
    try {
      const response = await apiService.login({
        email: 'admin@test.com',
        password: 'Admin123'
      });
      
      const duration = Date.now() - startTime;
      
      addResult({
        id: Date.now(),
        type: 'Login Test',
        success: true,
        duration: `${duration}ms`,
        message: 'Login successful',
        data: {
          userId: response.userId,
          email: response.email,
          role: response.role,
          hasToken: !!response.token
        },
        timestamp: new Date().toLocaleTimeString()
      });
    } catch (error: any) {
      const duration = Date.now() - startTime;
      
      addResult({
        id: Date.now(),
        type: 'Login Test',
        success: false,
        duration: `${duration}ms`,
        error: error.message,
        timestamp: new Date().toLocaleTimeString()
      });
    } finally {
      setIsLoading(false);
    }
  };

  const testEndpoints = async () => {
    setIsLoading(true);
    
    const endpoints = [
      '/Auth/login',
      '/v1/Students',
      '/v1/Courses',
      '/v1/Grades',
      '/v1/Attendance'
    ];
    
    for (const endpoint of endpoints) {
      try {
        const result = await apiService.testEndpoint(endpoint);
        
        addResult({
          id: Date.now() + Math.random(),
          type: 'Endpoint Test',
          endpoint,
          success: result.success,
          status: result.status,
          error: result.error,
          timestamp: new Date().toLocaleTimeString()
        });
        
        // Small delay between tests
        await new Promise(resolve => setTimeout(resolve, 200));
      } catch (error: any) {
        addResult({
          id: Date.now() + Math.random(),
          type: 'Endpoint Test',
          endpoint,
          success: false,
          error: error.message,
          timestamp: new Date().toLocaleTimeString()
        });
      }
    }
    
    setIsLoading(false);
  };

  const clearResults = () => {
    setResults([]);
  };

  const currentToken = typeof window !== 'undefined' ? localStorage.getItem('authToken') : null;

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="bg-white shadow rounded-lg p-6 mb-6">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">API Connection Tester</h1>
          <p className="text-gray-600">
            Test API connectivity and troubleshoot authentication issues
          </p>
        </div>

        {/* Current Status */}
        <div className="bg-white shadow rounded-lg p-6 mb-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Current Status</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="bg-gray-50 rounded p-4">
              <div className="text-sm font-medium text-gray-700">API Base URL</div>
              <div className="text-lg font-mono text-gray-900">http://localhost:5255/api</div>
            </div>
            <div className="bg-gray-50 rounded p-4">
              <div className="text-sm font-medium text-gray-700">Authentication Token</div>
              <div className={`text-sm font-mono ${currentToken ? 'text-green-600' : 'text-red-600'}`}>
                {currentToken ? `${currentToken.substring(0, 20)}...` : 'No token stored'}
              </div>
            </div>
          </div>
        </div>

        {/* Test Controls */}
        <div className="bg-white shadow rounded-lg p-6 mb-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Tests</h2>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <button
              onClick={testConnection}
              disabled={isLoading}
              className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              🔗 Test Connection
            </button>
            <button
              onClick={testLogin}
              disabled={isLoading}
              className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              🔑 Test Login
            </button>
            <button
              onClick={testEndpoints}
              disabled={isLoading}
              className="bg-purple-600 text-white px-4 py-2 rounded-md hover:bg-purple-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              📡 Test Endpoints
            </button>
            <button
              onClick={clearResults}
              disabled={isLoading}
              className="bg-gray-600 text-white px-4 py-2 rounded-md hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              🗑️ Clear Results
            </button>
          </div>
        </div>

        {/* Test Results */}
        <div className="bg-white shadow rounded-lg p-6">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">
            Test Results
            {isLoading && (
              <span className="ml-2 inline-flex items-center">
                <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-blue-600"></div>
                <span className="ml-2 text-sm text-blue-600">Testing...</span>
              </span>
            )}
          </h2>
          
          <div className="space-y-4 max-h-96 overflow-y-auto">
            {results.length === 0 ? (
              <p className="text-gray-500 text-center py-8">
                No test results yet. Click a test button above to start.
              </p>
            ) : (
              results.map((result) => (
                <div
                  key={result.id}
                  className={`border rounded-lg p-4 ${
                    result.success ? 'border-green-200 bg-green-50' : 'border-red-200 bg-red-50'
                  }`}
                >
                  <div className="flex items-center justify-between mb-2">
                    <div className="flex items-center space-x-2">
                      <span className={`text-lg ${result.success ? 'text-green-600' : 'text-red-600'}`}>
                        {result.success ? '✅' : '❌'}
                      </span>
                      <span className="font-medium text-gray-900">{result.type}</span>
                      {result.endpoint && (
                        <span className="font-mono text-sm text-gray-600">{result.endpoint}</span>
                      )}
                    </div>
                    <div className="text-sm text-gray-500">{result.timestamp}</div>
                  </div>
                  
                  <div className="text-sm">
                    {result.message && (
                      <div className={result.success ? 'text-green-700' : 'text-red-700'}>
                        {result.message}
                      </div>
                    )}
                    
                    {result.error && (
                      <div className="text-red-700 font-mono bg-red-100 p-2 rounded mt-2">
                        {result.error}
                      </div>
                    )}
                    
                    {result.data && (
                      <div className="mt-2">
                        <pre className="text-xs bg-gray-100 p-2 rounded overflow-x-auto">
                          {JSON.stringify(result.data, null, 2)}
                        </pre>
                      </div>
                    )}
                    
                    {result.duration && (
                      <div className="text-gray-500 text-xs mt-1">
                        Duration: {result.duration}
                      </div>
                    )}
                    
                    {result.status && (
                      <div className={`text-xs mt-1 ${
                        result.status < 400 ? 'text-green-600' : 'text-red-600'
                      }`}>
                        HTTP {result.status}
                      </div>
                    )}
                  </div>
                </div>
              ))
            )}
          </div>
        </div>

        {/* Instructions */}
        <div className="bg-blue-50 border border-blue-200 rounded-lg p-6 mt-6">
          <h3 className="text-lg font-semibold text-blue-900 mb-2">🔧 Troubleshooting Steps</h3>
          <ol className="text-blue-800 space-y-1 text-sm list-decimal list-inside">
            <li>First, test the connection to see if the backend server is running</li>
            <li>If connection fails, make sure the backend is running on port 5255</li>
            <li>Test login with known credentials to verify authentication</li>
            <li>Test individual endpoints to check specific API functionality</li>
            <li>Check browser console (F12) for detailed error messages</li>
            <li>Verify CORS settings if getting cross-origin errors</li>
          </ol>
        </div>
      </div>
    </div>
  );
}