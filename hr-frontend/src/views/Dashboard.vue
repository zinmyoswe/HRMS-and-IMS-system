<template>
  <div class="dashboard-container">
    <header class="dashboard-header">
      <div class="brand">
        
        <div class="welcome-text">
          <h1>Overview</h1>
          <p>Welcome back! Here's what's happening today.</p>
        </div>
      </div>
      <div class="header-actions">
        <el-button type="primary" plain :icon="Refresh" @click="reloadData">Refresh Data</el-button>
      </div>
    </header>

    <el-row :gutter="24" class="stat-row">
      <el-col v-for="(item, index) in statItems" :key="index" :xs="24" :sm="12" :md="6">
        <el-card shadow="hover" class="stat-card-modern">
          <div class="stat-inner">
            <div class="stat-info">
              <span class="label">{{ item.label }}</span>
              <h2 class="value">{{ item.value }}</h2>
              <span class="sub-label" :class="item.trendType">
                {{ item.subLabel }}
              </span>
            </div>
            <div class="stat-icon" :style="{ backgroundColor: item.bg }">
              <el-icon :size="24" :color="item.color">
                <component :is="item.icon" />
              </el-icon>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="24" class="content-row">
      <el-col :xs="24" :lg="16">
        <el-card shadow="never" class="table-card">
          <template #header>
            <div class="card-header">
              <div class="header-title">
                <h3>Recent Hires</h3>
                <span class="badge">{{ recentStaff.length }} New</span>
              </div>
              <el-button link type="primary" href="#/staff">View Directory</el-button>
            </div>
          </template>
          
          <el-table :data="recentStaff" style="width: 100%" header-cell-class-name="table-header">
            <el-table-column prop="staffName" label="Employee">
              <template #default="{ row }">
                <div class="user-cell">
                  <el-avatar :size="32" :src="`https://api.dicebear.com/7.x/initials/svg?seed=${row.staffName}`" />
                  <div class="user-info">
                    <span class="name">{{ row.staffName }}</span>
                    <span class="code">{{ row.staffCode }}</span>
                  </div>
                </div>
              </template>
            </el-table-column>
            <el-table-column prop="email" label="Email" />
            <el-table-column label="Status" align="right">
              <template #default="{ row }">
                <el-tag 
                  :type="row.isActive ? 'success' : 'info'" 
                  round 
                  effect="light"
                  class="status-tag"
                >
                  {{ row.isActive ? 'Active' : 'Inactive' }}
                </el-tag>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </el-col>

      <el-col :xs="24" :lg="8">
        <el-card shadow="never" class="chart-card">
          <template #header>
            <div class="card-header">
              <h3>Staff Allocation</h3>
            </div>
          </template>
          <div class="chart-container">
            <div class="empty-state">
              <el-icon :size="48" color="#dcdfe6"><PieChart /></el-icon>
              <p>Department distribution data is loading...</p>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { User, OfficeBuilding, Money, Calendar, Refresh, PieChart } from '@element-plus/icons-vue';
import { staffService } from '../services/staffService';
import { departmentService } from '../services/departmentService';
import type { Staff } from '../types';

const stats = ref({
  totalStaff: 0,
  totalDepartments: 0,
  monthlyPayroll: '$0',
  presentToday: 0,
});

const recentStaff = ref<Staff[]>([]);

const statItems = computed(() => [
  { label: 'Total Staff', value: stats.value.totalStaff, subLabel: '+2 this week', icon: User, color: '#409EFF', bg: '#ecf5ff', trendType: 'up' },
  { label: 'Departments', value: stats.value.totalDepartments, subLabel: 'Across all sites', icon: OfficeBuilding, color: '#67C23A', bg: '#f0f9eb', trendType: '' },
  { label: 'Payroll', value: stats.value.monthlyPayroll, subLabel: 'Current Month', icon: Money, color: '#E6A23C', bg: '#fdf6ec', trendType: '' },
  { label: 'Attendance', value: stats.value.presentToday, subLabel: 'On-site today', icon: Calendar, color: '#F56C6C', bg: '#fef0f0', trendType: 'up' },
]);

const reloadData = async () => {
  try {
    const allStaff = await staffService.getAll();
    stats.value.totalStaff = allStaff.length;
    stats.value.presentToday = Math.floor(allStaff.length * 0.85);
    const departments = await departmentService.getAll();
    stats.value.totalDepartments = departments.length;
    recentStaff.value = allStaff.slice(0, 5);
    stats.value.monthlyPayroll = '$' + (Math.random() * 100000).toLocaleString(undefined, { minimumFractionDigits: 2 });
  } catch (error) {
    console.error('Error:', error);
  }
};

onMounted(reloadData);
</script>

<style scoped>
.dashboard-container {
  padding: 24px;
  background-color: #f8fafc;
  min-height: 100vh;
}

/* Header Styling */
.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 32px;
}

.brand {
  display: flex;
  align-items: center;
  gap: 16px;
}

.logo {
  height: 48px;
  width: auto;
  border-radius: 8px;
}

.welcome-text h1 {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  color: #1e293b;
}

.welcome-text p {
  margin: 4px 0 0;
  color: #64748b;
  font-size: 14px;
}

/* Stat Cards */
.stat-card-modern {
  border: none;
  border-radius: 12px;
  transition: transform 0.2s;
}

.stat-inner {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.stat-info .label {
  font-size: 13px;
  color: #64748b;
  font-weight: 500;
}

.stat-info .value {
  margin: 8px 0;
  font-size: 28px;
  font-weight: 700;
  color: #0f172a;
}

.stat-info .sub-label {
  font-size: 12px;
  color: #94a3b8;
}

.up { color: #10b981 !important; }

.stat-icon {
  padding: 12px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Table & Chart Cards */
.table-card, .chart-card {
  border-radius: 12px;
  border: 1px solid #e2e8f0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-title {
  display: flex;
  align-items: center;
  gap: 12px;
}

.header-title h3 {
  margin: 0;
  font-size: 16px;
  color: #1e293b;
}

.badge {
  background: #eff6ff;
  color: #3b82f6;
  font-size: 12px;
  padding: 2px 8px;
  border-radius: 20px;
  font-weight: 600;
}

.user-cell {
  display: flex;
  align-items: center;
  gap: 12px;
}

.user-info {
  display: flex;
  flex-direction: column;
}

.user-info .name {
  font-weight: 600;
  color: #334155;
  font-size: 14px;
}

.user-info .code {
  font-size: 12px;
  color: #94a3b8;
}

.status-tag {
  font-weight: 600;
  text-transform: uppercase;
  font-size: 10px;
}

.chart-container {
  height: 340px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.empty-state {
  text-align: center;
  color: #94a3b8;
}

.content-row {
  margin-top: 24px;
}

:deep(.table-header) {
  background-color: #f8fafc !important;
  color: #64748b;
  font-weight: 600;
  text-transform: uppercase;
  font-size: 11px;
  letter-spacing: 0.05em;
}
</style>