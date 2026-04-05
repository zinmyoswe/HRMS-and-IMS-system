import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';

// Lazy load views
const Dashboard = () => import('../views/Dashboard.vue');
const StaffManagement = () => import('../views/StaffManagement.vue');
const DepartmentManagement = () => import('../views/DepartmentManagement.vue');
const DeviceManagement = () => import('../views/DeviceManagement.vue');
const PayrollManagement = () => import('../views/PayrollManagement.vue');
const AttendanceManagement = () => import('../views/AttendanceManagement.vue');

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'Dashboard',
    component: Dashboard,
  },
  {
    path: '/staff',
    name: 'StaffManagement',
    component: StaffManagement,
  },
  {
    path: '/departments',
    name: 'DepartmentManagement',
    component: DepartmentManagement,
  },
  {
    path: '/devices',
    name: 'DeviceManagement',
    component: DeviceManagement,
  },
  {
    path: '/payroll',
    name: 'PayrollManagement',
    component: PayrollManagement,
  },
  {
    path: '/attendance',
    name: 'AttendanceManagement',
    component: AttendanceManagement,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
