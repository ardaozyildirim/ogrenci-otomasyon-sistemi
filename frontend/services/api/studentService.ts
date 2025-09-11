// services/api/studentService.ts
import apiClient from '../../utils/apiClient';

export interface Student {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  studentNumber: string;
  dateOfBirth: string;
  gender: number;
  address: string;
  phoneNumber: string;
  enrollmentDate: string;
  status: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateStudentRequest {
  firstName: string;
  lastName: string;
  email: string;
  studentNumber: string;
  dateOfBirth: string;
  gender: number;
  address: string;
  phoneNumber: string;
  enrollmentDate: string;
}

export interface UpdateStudentRequest {
  firstName?: string;
  lastName?: string;
  email?: string;
  dateOfBirth?: string;
  gender?: number;
  address?: string;
  phoneNumber?: string;
  status?: number;
}

class StudentService {
  async getAllStudents(): Promise<Student[]> {
    return apiClient.get<Student[]>('/students');
  }

  async getStudentById(id: number): Promise<Student> {
    return apiClient.get<Student>(`/students/${id}`);
  }

  async getStudentByStudentNumber(studentNumber: string): Promise<Student> {
    return apiClient.get<Student>(`/students/number/${studentNumber}`);
  }

  async createStudent(data: CreateStudentRequest): Promise<Student> {
    return apiClient.post<Student>('/students', data);
  }

  async updateStudent(id: number, data: UpdateStudentRequest): Promise<Student> {
    return apiClient.put<Student>(`/students/${id}`, data);
  }

  async deleteStudent(id: number): Promise<void> {
    return apiClient.delete<void>(`/students/${id}`);
  }
}

const studentService = new StudentService();
export default studentService;