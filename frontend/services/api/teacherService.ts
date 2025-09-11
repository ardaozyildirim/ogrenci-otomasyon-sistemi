// services/api/teacherService.ts
import apiClient from '../../utils/apiClient';

export interface Teacher {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  employeeId: string;
  phoneNumber: string | null;
  department: string;
  specialty: string | null;
  hireDate: string;
  isActive: boolean;
  fullName: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateTeacherRequest {
  firstName: string;
  lastName: string;
  email: string;
  employeeId: string;
  phoneNumber: string | null;
  department: string;
  specialty: string | null;
  hireDate: string;
}

export interface UpdateTeacherRequest {
  id: number;
  firstName?: string;
  lastName?: string;
  email?: string;
  employeeId?: string;
  phoneNumber?: string | null;
  department?: string;
  specialty?: string | null;
  hireDate?: string;
  isActive?: boolean;
}

class TeacherService {
  async getAllTeachers(): Promise<Teacher[]> {
    return apiClient.get<Teacher[]>('/teachers');
  }

  async getTeacherById(id: number): Promise<Teacher> {
    return apiClient.get<Teacher>(`/teachers/${id}`);
  }

  async getTeacherByEmail(email: string): Promise<Teacher> {
    return apiClient.get<Teacher>(`/teachers/search/email?email=${encodeURIComponent(email)}`);
  }

  async getTeacherByEmployeeId(employeeId: string): Promise<Teacher> {
    return apiClient.get<Teacher>(`/teachers/search/employee?employeeId=${encodeURIComponent(employeeId)}`);
  }

  async createTeacher(data: CreateTeacherRequest): Promise<Teacher> {
    return apiClient.post<Teacher>('/teachers', data);
  }

  async updateTeacher(id: number, data: UpdateTeacherRequest): Promise<Teacher> {
    return apiClient.put<Teacher>(`/teachers/${id}`, data);
  }

  async deleteTeacher(id: number): Promise<void> {
    return apiClient.delete<void>(`/teachers/${id}`);
  }
}

const teacherService = new TeacherService();
export default teacherService;