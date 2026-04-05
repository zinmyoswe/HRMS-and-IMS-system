import apiClient from './apiClient';
import type { Position } from '../types';

export const positionService = {
  // Get all positions
  async getAll(): Promise<Position[]> {
    const response = await apiClient.get('/positions');
    return response.data;
  },

  // Get single position
  async getById(id: number): Promise<Position> {
    const response = await apiClient.get(`/positions/${id}`);
    return response.data;
  },

  // Create position
  async create(position: Omit<Position, 'positionId'>): Promise<Position> {
    const response = await apiClient.post('/positions', position);
    return response.data;
  },

  // Update position
  async update(id: number, position: Position): Promise<Position> {
    const response = await apiClient.put(`/positions/${id}`, position);
    return response.data;
  },

  // Delete position
  async delete(id: number): Promise<void> {
    await apiClient.delete(`/positions/${id}`);
  },
};