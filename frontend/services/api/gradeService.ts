// services/api/gradeService.ts
import apiClient from '../../utils/apiClient';

export interface Grade {
  id: number;
  studentId: number;
  courseId: number;
  gradeValue: number;
  gradeDate: string;
  description: string;
  createdAt: string;
  updatedAt: string;
  student: {
    id: number;
    firstName: string;
    lastName: string;
    studentNumber: string;
  };
  course: {
    id: number;
    name: string;
    code: string;
    teacher: {
      id: number;
      firstName: string;
      lastName: string;
    };
  };
}

export interface CreateGradeRequest {
  studentId: number;
  courseId: number;
  gradeValue: number;
  description: string;
}

export interface UpdateGradeRequest {
  gradeValue?: number;
  description?: string;
}

class GradeService {
  async getAllGrades(): Promise<Grade[]> {
    return apiClient.get<Grade[]>('/grades');
  }

  async getGradeById(id: number): Promise<Grade> {
    return apiClient.get<Grade>(`/grades/${id}`);
  }

  async getGradesByStudentId(studentId: number): Promise<Grade[]> {
    return apiClient.get<Grade[]>(`/grades/student/${studentId}`);
  }

  async getGradesByCourseId(courseId: number): Promise<Grade[]> {
    return apiClient.get<Grade[]>(`/grades/course/${courseId}`);
  }

  async createGrade(data: CreateGradeRequest): Promise<Grade> {
    return apiClient.post<Grade>('/grades', data);
  }

  async updateGrade(id: number, data: UpdateGradeRequest): Promise<Grade> {
    return apiClient.put<Grade>(`/grades/${id}`, data);
  }

  async deleteGrade(id: number): Promise<void> {
    return apiClient.delete<void>(`/grades/${id}`);
  }

  // Soft delete endpoints
  async getDeletedGrades(): Promise<Grade[]> {
    return apiClient.get<Grade[]>('/grades/deleted');
  }

  async restoreGrade(id: number): Promise<Grade> {
    return apiClient.post<Grade>(`/grades/${id}/restore`);
  }
}

const gradeService = new GradeService();
export default gradeService;