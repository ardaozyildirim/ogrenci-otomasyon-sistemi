const API_BASE_URL = 'http://localhost:5255/api';

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
  errors?: string[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  userId: number;
  email: string;
  role: string;
  user: UserDto;
  expiresAt: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: 'Admin' | 'Teacher' | 'Student';
  phoneNumber?: string;
  dateOfBirth?: string;
  address?: string;
}

export interface UserDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  address?: string;
  fullName: string;
  createdAt: string;
}

export interface StudentDto {
  id: number;
  userId: number;
  studentNumber: string;
  department?: string;
  grade?: number;
  className?: string;
  user: UserDto;
}

export interface CreateStudentRequest {
  userId: number;
  studentNumber: string;
  department?: string;
  grade?: number;
  className?: string;
}

export interface TeacherDto {
  id: number;
  userId: number;
  employeeNumber: string;
  department?: string;
  specialization?: string;
  hireDate?: string; // Backend sends DateTime, frontend receives as string
  fullName: string;
  email: string;
  user: UserDto;
}

export interface CreateTeacherRequest {
  userId: number;
  employeeNumber: string;
  department?: string;
  specialization?: string;
}

export interface CourseDto {
  id: number;
  name: string;
  code: string;
  description: string;
  credits: number;
  teacherId: number;
  teacherName: string;
  status: string; // Backend sends CourseStatus enum as string
  startDate?: string;
  endDate?: string;
  schedule?: string;
  location?: string;
  teacher?: TeacherDto;
  // Keep semester for backward compatibility
  semester?: string;
}

export interface CreateCourseRequest {
  name: string;
  code: string;
  description: string;
  credits: number;
  teacherId: number;
  semester: string;
}

export interface GradeDto {
  id: number;
  studentId: number;
  studentName: string;
  courseId: number;
  courseName: string;
  score: number;
  gradeType: string;
  semester: string;
  date: string;
}

export interface CreateGradeRequest {
  studentId: number;
  courseId: number;
  score: number;
  gradeType: string;
  semester: string;
  date: string;
}

export interface AttendanceDto {
  id: number;
  studentId: number;
  studentName: string;
  courseId: number;
  courseName: string;
  date: string;
  status: string;
  notes?: string;
}

export interface CreateAttendanceRequest {
  studentId: number;
  courseId: number;
  date: string;
  status: string;
  notes?: string;
}

class ApiService {
  private getAuthToken(): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('authToken');
    }
    return null;
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    const token = this.getAuthToken();
    
    const config: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
        ...options.headers,
      },
      ...options,
    };

    try {
      const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const contentType = response.headers.get('content-type');
      if (!contentType || !contentType.includes('application/json')) {
        const text = await response.text();
        console.error('Non-JSON response:', text);
        throw new Error('Server returned non-JSON response');
      }

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('API Error:', error);
      throw error;
    }
  }

  // Auth endpoints
  async login(credentials: LoginRequest): Promise<LoginResponse> {
    const token = this.getAuthToken();
    
    const config: RequestInit = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
      },
      body: JSON.stringify(credentials),
    };

    try {
      const response = await fetch(`${API_BASE_URL}/Auth/login`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Login API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const contentType = response.headers.get('content-type');
      if (!contentType || !contentType.includes('application/json')) {
        const text = await response.text();
        console.error('Non-JSON response:', text);
        throw new Error('Server returned non-JSON response');
      }

      const data = await response.json();
      return data; // Auth controller returns the data directly, not wrapped in ApiResponse
    } catch (error) {
      console.error('Login API Error:', error);
      throw error;
    }
  }

  async register(userData: RegisterRequest): Promise<LoginResponse> {
    const token = this.getAuthToken();
    
    // Convert role string to enum value for backend compatibility
    const roleMapping: { [key: string]: number } = {
      'Admin': 1,
      'Teacher': 2,
      'Student': 3
    };
    
    const requestData = {
      ...userData,
      role: roleMapping[userData.role] || 3 // Default to Student if role not found
    };
    
    const config: RequestInit = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
      },
      body: JSON.stringify(requestData),
    };

    try {
      const response = await fetch(`${API_BASE_URL}/Auth/register`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Register API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const contentType = response.headers.get('content-type');
      if (!contentType || !contentType.includes('application/json')) {
        const text = await response.text();
        console.error('Non-JSON response:', text);
        throw new Error('Server returned non-JSON response');
      }

      const data = await response.json();
      return data; // Auth controller returns the data directly, not wrapped in ApiResponse
    } catch (error) {
      console.error('Register API Error:', error);
      throw error;
    }
  }

  // Student endpoints
  async getStudents(): Promise<StudentDto[]> {
    try {
      const response = await this.request<StudentDto[]>('/v1/Students');
      return response.data;
    } catch (error) {
      // If wrapped response fails, try direct response format
      const token = this.getAuthToken();
      
      const config: RequestInit = {
        headers: {
          'Content-Type': 'application/json',
          ...(token && { Authorization: `Bearer ${token}` }),
        },
      };

      const response = await fetch(`${API_BASE_URL}/v1/Students`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Students API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const data = await response.json();
      return data; // Direct response from backend
    }
  }

  async getStudent(id: number): Promise<StudentDto> {
    const response = await this.request<StudentDto>(`/v1/Students/${id}`);
    return response.data;
  }

  async createStudent(studentData: CreateStudentRequest): Promise<number> {
    const response = await this.request<number>('/v1/Students', {
      method: 'POST',
      body: JSON.stringify(studentData),
    });
    return response.data;
  }

  async updateStudent(id: number, studentData: Partial<CreateStudentRequest>): Promise<void> {
    await this.request(`/v1/Students/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ id, ...studentData }),
    });
  }

  async deleteStudent(id: number): Promise<void> {
    await this.request(`/v1/Students/${id}`, {
      method: 'DELETE',
    });
  }

  // Teacher endpoints
  async getTeachers(): Promise<TeacherDto[]> {
    try {
      const response = await this.request<TeacherDto[]>('/v1/Teachers');
      return response.data;
    } catch (error) {
      // If wrapped response fails, try direct response format
      const token = this.getAuthToken();
      
      const config: RequestInit = {
        headers: {
          'Content-Type': 'application/json',
          ...(token && { Authorization: `Bearer ${token}` }),
        },
      };

      const response = await fetch(`${API_BASE_URL}/v1/Teachers`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Teachers API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const data = await response.json();
      return data; // Direct response from backend
    }
  }

  async getTeacher(id: number): Promise<TeacherDto> {
    const response = await this.request<TeacherDto>(`/v1/Teachers/${id}`);
    return response.data;
  }

  async createTeacher(teacherData: CreateTeacherRequest): Promise<number> {
    const response = await this.request<number>('/v1/Teachers', {
      method: 'POST',
      body: JSON.stringify(teacherData),
    });
    return response.data;
  }

  async updateTeacher(id: number, teacherData: Partial<CreateTeacherRequest>): Promise<void> {
    await this.request(`/v1/Teachers/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ id, ...teacherData }),
    });
  }

  async deleteTeacher(id: number): Promise<void> {
    await this.request(`/v1/Teachers/${id}`, {
      method: 'DELETE',
    });
  }

  // Course endpoints
  async getCourses(): Promise<CourseDto[]> {
    try {
      const response = await this.request<CourseDto[]>('/v1/Courses');
      return response.data;
    } catch (error) {
      // If wrapped response fails, try direct response format
      const token = this.getAuthToken();
      
      const config: RequestInit = {
        headers: {
          'Content-Type': 'application/json',
          ...(token && { Authorization: `Bearer ${token}` }),
        },
      };

      const response = await fetch(`${API_BASE_URL}/v1/Courses`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Courses API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const data = await response.json();
      return data; // Direct response from backend
    }
  }

  async getCourse(id: number): Promise<CourseDto> {
    const response = await this.request<CourseDto>(`/v1/Courses/${id}`);
    return response.data;
  }

  async createCourse(courseData: CreateCourseRequest): Promise<number> {
    const response = await this.request<number>('/v1/Courses', {
      method: 'POST',
      body: JSON.stringify(courseData),
    });
    return response.data;
  }

  async updateCourse(id: number, courseData: Partial<CreateCourseRequest>): Promise<void> {
    await this.request(`/v1/Courses/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ id, ...courseData }),
    });
  }

  async deleteCourse(id: number): Promise<void> {
    await this.request(`/v1/Courses/${id}`, {
      method: 'DELETE',
    });
  }

  // Grade endpoints
  async getGrades(): Promise<GradeDto[]> {
    try {
      const response = await this.request<GradeDto[]>('/v1/Grades');
      return response.data;
    } catch (error) {
      // If wrapped response fails, try direct response format
      const token = this.getAuthToken();
      
      const config: RequestInit = {
        headers: {
          'Content-Type': 'application/json',
          ...(token && { Authorization: `Bearer ${token}` }),
        },
      };

      const response = await fetch(`${API_BASE_URL}/v1/Grades`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Grades API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const data = await response.json();
      return data; // Direct response from backend
    }
  }

  async getGrade(id: number): Promise<GradeDto> {
    const response = await this.request<GradeDto>(`/v1/Grades/${id}`);
    return response.data;
  }

  async createGrade(gradeData: CreateGradeRequest): Promise<number> {
    const response = await this.request<number>('/v1/Grades', {
      method: 'POST',
      body: JSON.stringify(gradeData),
    });
    return response.data;
  }

  async updateGrade(id: number, gradeData: Partial<CreateGradeRequest>): Promise<void> {
    await this.request(`/v1/Grades/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ id, ...gradeData }),
    });
  }

  async deleteGrade(id: number): Promise<void> {
    await this.request(`/v1/Grades/${id}`, {
      method: 'DELETE',
    });
  }

  // Attendance endpoints
  async getAttendance(): Promise<AttendanceDto[]> {
    try {
      const response = await this.request<AttendanceDto[]>('/v1/Attendance');
      return response.data;
    } catch (error) {
      // If wrapped response fails, try direct response format
      const token = this.getAuthToken();
      
      const config: RequestInit = {
        headers: {
          'Content-Type': 'application/json',
          ...(token && { Authorization: `Bearer ${token}` }),
        },
      };

      const response = await fetch(`${API_BASE_URL}/v1/Attendance`, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Attendance API Error Response:', errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const data = await response.json();
      return data; // Direct response from backend
    }
  }

  async getAttendanceRecord(id: number): Promise<AttendanceDto> {
    const response = await this.request<AttendanceDto>(`/v1/Attendance/${id}`);
    return response.data;
  }

  async createAttendance(attendanceData: CreateAttendanceRequest): Promise<number> {
    const response = await this.request<number>('/v1/Attendance', {
      method: 'POST',
      body: JSON.stringify(attendanceData),
    });
    return response.data;
  }

  async updateAttendance(id: number, attendanceData: Partial<CreateAttendanceRequest>): Promise<void> {
    await this.request(`/v1/Attendance/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ id, ...attendanceData }),
    });
  }

  async deleteAttendance(id: number): Promise<void> {
    await this.request(`/v1/Attendance/${id}`, {
      method: 'DELETE',
    });
  }
}

export const apiService = new ApiService();
