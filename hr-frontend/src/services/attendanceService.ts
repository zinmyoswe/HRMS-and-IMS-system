import apiClient from './apiClient';
import type { Attendance, AttendanceLog } from '../types';

export const attendanceService = {
  // Get all attendances
  async getAll(): Promise<Attendance[]> {
    const response = await apiClient.get('/attendances');
    return response.data;
  },

  // Get single attendance
  async getById(id: number): Promise<Attendance> {
    const response = await apiClient.get(`/attendances/${id}`);
    return response.data;
  },

  // Create attendance
  async create(attendance: Omit<Attendance, 'attendanceId'>): Promise<Attendance> {
    const response = await apiClient.post('/attendances', attendance);
    return response.data;
  },

  // Add attendance log
  async addLog(attendanceId: number, log: Omit<AttendanceLog, 'logId' | 'attendanceId'>): Promise<AttendanceLog> {
    const response = await apiClient.post(`/attendances/${attendanceId}/logs`, log);
    return response.data;
  },
};
