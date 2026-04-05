import apiClient from './apiClient';
import type { Department } from '../types';

export const departmentService = {
  // Get all departments
  async getAll(): Promise<Department[]> {
    const response = await apiClient.get('/departments');
    return response.data;
  },

  // Get single department
  async getById(id: number): Promise<Department> {
    const response = await apiClient.get(`/departments/${id}`);
    return response.data;
  },

  // Create department
  async create(department: Omit<Department, 'departmentId'>): Promise<Department> {
    const response = await apiClient.post('/departments', department);
    return response.data;
  },

  // Update department
  async update(id: number, department: Partial<Department>): Promise<void> {
    await apiClient.put(`/departments/${id}`, department);
  },

  // Delete department
  async delete(id: number): Promise<void> {
    await apiClient.delete(`/departments/${id}`);
  },
};
