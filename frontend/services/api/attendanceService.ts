// services/api/attendanceService.ts
import apiClient from '../../utils/apiClient';

export interface Attendance {
  id: number;
  studentId: number;
  courseId: number;
  attendanceDate: string;
  isPresent: boolean;
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

export interface CreateAttendanceRequest {
  studentId: number;
  courseId: number;
  attendanceDate: string;
  isPresent: boolean;
  description: string;
}

export interface UpdateAttendanceRequest {
  isPresent?: boolean;
  description?: string;
}

export interface AttendanceSummary {
  studentId: number;
  studentName: string;
  studentNumber: string;
  courseId: number;
  courseName: string;
  totalSessions: number;
  presentSessions: number;
  absentSessions: number;
  attendancePercentage: number;
}

class AttendanceService {
  async getAllAttendances(): Promise<Attendance[]> {
    return apiClient.get<Attendance[]>('/attendances');
  }

  async getAttendanceById(id: number): Promise<Attendance> {
    return apiClient.get<Attendance>(`/attendances/${id}`);
  }

  async getAttendancesByStudentId(studentId: number): Promise<Attendance[]> {
    return apiClient.get<Attendance[]>(`/attendances/student/${studentId}`);
  }

  async getAttendancesByCourseId(courseId: number): Promise<Attendance[]> {
    return apiClient.get<Attendance[]>(`/attendances/course/${courseId}`);
  }

  async getAttendanceByStudentAndCourse(studentId: number, courseId: number): Promise<Attendance[]> {
    return apiClient.get<Attendance[]>(`/attendances/student/${studentId}/course/${courseId}`);
  }

  async getAttendanceSummary(studentId?: number, courseId?: number): Promise<AttendanceSummary[]> {
    const params = new URLSearchParams();
    if (studentId) params.append('studentId', studentId.toString());
    if (courseId) params.append('courseId', courseId.toString());
    
    const queryString = params.toString() ? `?${params.toString()}` : '';
    return apiClient.get<AttendanceSummary[]>(`/attendances/summary${queryString}`);
  }

  async createAttendance(data: CreateAttendanceRequest): Promise<Attendance> {
    return apiClient.post<Attendance>('/attendances', data);
  }

  async updateAttendance(id: number, data: UpdateAttendanceRequest): Promise<Attendance> {
    return apiClient.put<Attendance>(`/attendances/${id}`, data);
  }

  async deleteAttendance(id: number): Promise<void> {
    return apiClient.delete<void>(`/attendances/${id}`);
  }

  // Soft delete endpoints
  async getDeletedAttendances(): Promise<Attendance[]> {
    return apiClient.get<Attendance[]>('/attendances/deleted');
  }

  async restoreAttendance(id: number): Promise<Attendance> {
    return apiClient.post<Attendance>(`/attendances/${id}/restore`);
  }
}

const attendanceService = new AttendanceService();
export default attendanceService;