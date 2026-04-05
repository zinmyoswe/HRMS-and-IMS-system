import apiClient from './apiClient';
import type { Site } from '../types';

export const siteService = {
  // Get all sites
  async getAll(): Promise<Site[]> {
    const response = await apiClient.get('/sites');
    return response.data;
  },

  // Get single site
  async getById(id: number): Promise<Site> {
    const response = await apiClient.get(`/sites/${id}`);
    return response.data;
  },

  // Create site
  async create(site: Omit<Site, 'siteId'>): Promise<Site> {
    const response = await apiClient.post('/sites', site);
    return response.data;
  },
};
