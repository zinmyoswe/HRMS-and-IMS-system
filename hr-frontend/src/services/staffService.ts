import apiClient from './apiClient';
import type { Staff } from '../types';

export const staffService = {
  // Get all staff
  async getAll(): Promise<Staff[]> {
    const response = await apiClient.get('/staffs');
    return response.data;
  },

  // Get single staff
  async getById(id: number): Promise<Staff> {
    const response = await apiClient.get(`/staffs/${id}`);
    return response.data;
  },

  // Create staff
  async create(staff: Omit<Staff, 'staffId'>): Promise<Staff> {
    const response = await apiClient.post('/staffs', staff);
    return response.data;
  },

  // Update staff
  async update(id: number, staff: Partial<Staff>): Promise<void> {
    await apiClient.put(`/staffs/${id}`, staff);
  },

  // Delete staff
  async delete(id: number): Promise<void> {
    await apiClient.delete(`/staffs/${id}`);
  },
};
