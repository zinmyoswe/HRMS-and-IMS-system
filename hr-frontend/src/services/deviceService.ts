import apiClient from './apiClient';
import type { Device } from '../types';

export const deviceService = {
  async getAll(): Promise<Device[]> {
    const response = await apiClient.get('/devices');
    return response.data;
  },

  async getById(id: number): Promise<Device> {
    const response = await apiClient.get(`/devices/${id}`);
    return response.data;
  },

  async create(device: Omit<Device, 'deviceId' | 'createdAt'>): Promise<Device> {
    const response = await apiClient.post('/devices', device);
    return response.data;
  },

  async update(id: number, device: Partial<Omit<Device, 'deviceId' | 'createdAt'>>): Promise<void> {
    await apiClient.put(`/devices/${id}`, device);
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`/devices/${id}`);
  },
};
