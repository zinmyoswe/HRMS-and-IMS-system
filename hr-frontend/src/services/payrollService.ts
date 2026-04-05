import apiClient from './apiClient';
import type { Payroll } from '../types';

export const payrollService = {
  // Get all payrolls
  async getAll(): Promise<Payroll[]> {
    const response = await apiClient.get('/payrolls');
    return response.data;
  },

  // Get single payroll
  async getById(id: number): Promise<Payroll> {
    const response = await apiClient.get(`/payrolls/${id}`);
    return response.data;
  },

  // Create payroll
  async create(payroll: Omit<Payroll, 'payrollId'>): Promise<Payroll> {
    const response = await apiClient.post('/payrolls', payroll);
    return response.data;
  },
};
