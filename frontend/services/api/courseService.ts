// services/api/courseService.ts
import apiClient from '../../utils/apiClient';

export interface Course {
  id: number;
  name: string;
  code: string;
  description: string;
  credits: number;
  status: number;
  createdAt: string;
  updatedAt: string;
  teacherId: number;
  teacher: {
    id: number;
    firstName: string;
    lastName: string;
  };
}

export interface CreateCourseRequest {
  name: string;
  code: string;
  description: string;
  credits: number;
  teacherId: number;
}

export interface UpdateCourseRequest {
  name?: string;
  code?: string;
  description?: string;
  credits?: number;
  teacherId?: number;
  status?: number;
}

class CourseService {
  async getAllCourses(): Promise<Course[]> {
    return apiClient.get<Course[]>('/courses');
  }

  async getCourseById(id: number): Promise<Course> {
    return apiClient.get<Course>(`/courses/${id}`);
  }

  async getCoursesByTeacherId(teacherId: number): Promise<Course[]> {
    return apiClient.get<Course[]>(`/courses/teacher/${teacherId}`);
  }

  async createCourse(data: CreateCourseRequest): Promise<Course> {
    return apiClient.post<Course>('/courses', data);
  }

  async updateCourse(id: number, data: UpdateCourseRequest): Promise<Course> {
    return apiClient.put<Course>(`/courses/${id}`, data);
  }

  async deleteCourse(id: number): Promise<void> {
    return apiClient.delete<void>(`/courses/${id}`);
  }
}

const courseService = new CourseService();
export default courseService;