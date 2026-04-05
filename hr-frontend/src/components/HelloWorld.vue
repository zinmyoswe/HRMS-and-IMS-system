<template>
  <div class="attendance-page">
    <div class="content-limit">
      <div class="page-header">
        <div class="title-section">
          <el-icon class="header-icon"><Calendar /></el-icon>
          <div>
            <h1>Attendance Management</h1>
            <p>Track and manage daily employee presence</p>
          </div>
        </div>
        <el-button type="primary" :icon="Plus">Clock In Staff</el-button>
      </div>

      <el-row :gutter="20" class="summary-cards">
        <el-col :span="8" v-for="item in summary" :key="item.label">
          <div class="summary-card">
            <span class="s-label">{{ item.label }}</span>
            <span class="s-value">{{ item.value }}</span>
          </div>
        </el-col>
      </el-row>

      <el-card shadow="never" class="attendance-card">
        <template #header>
          <div class="table-controls">
            <el-date-picker
              v-model="selectedDate"
              type="date"
              placeholder="Pick a day"
            />
            <el-input
              v-model="search"
              placeholder="Search employee..."
              :prefix-icon="Search"
              style="width: 250px"
            />
          </div>
        </template>

        <el-table :data="attendanceData" style="width: 100%">
          <el-table-column prop="name" label="Employee" min-width="180" />
          <el-table-column prop="checkIn" label="Check In" width="120" />
          <el-table-column prop="checkOut" label="Check Out" width="120" />
          <el-table-column label="Status" align="right">
            <template #default="{ row }">
              <el-tag :type="statusMap[row.status].type" effect="light" round>
                {{ statusMap[row.status].label }}
              </el-tag>
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { Calendar, Plus, Search } from '@element-plus/icons-vue';

const selectedDate = ref(new Date());
const search = ref('');

const summary = [
  { label: 'Present Today', value: '142' },
  { label: 'Late Arrival', value: '12' },
  { label: 'On Leave', value: '5' }
];

const statusMap: any = {
  present: { label: 'Present', type: 'success' },
  late: { label: 'Late', type: 'warning' },
  absent: { label: 'Absent', type: 'danger' }
};

const attendanceData = ref([
  { name: 'John Doe', checkIn: '08:45 AM', checkOut: '05:30 PM', status: 'present' },
  { name: 'Jane Smith', checkIn: '09:15 AM', checkOut: '06:00 PM', status: 'late' },
  { name: 'Michael Brown', checkIn: '08:50 AM', checkOut: '05:45 PM', status: 'present' },
]);
</script>

<style scoped>
.attendance-page {

  width: 100%;
  display: flex;
  justify-content: center; /* Centers the content-limit horizontally */
}

.content-limit {
  width: 100%;
  max-width: 1200px; /* Prevents content from becoming too wide on huge monitors */
  padding: 20px;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

.title-section {
  display: flex;
  align-items: center;
  gap: 15px;
}

.header-icon {
  font-size: 32px;
  color: #409eff;
  background: #ecf5ff;
  padding: 10px;
  border-radius: 12px;
}

.title-section h1 {
  margin: 0;
  font-size: 22px;
  color: #1f2937;
}

.title-section p {
  margin: 4px 0 0;
  color: #6b7280;
  font-size: 14px;
}

/* Summary Cards */
.summary-cards {
  margin-bottom: 24px;
}

.summary-card {
  background: white;
  padding: 20px;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.s-label {
  color: #6b7280;
  font-size: 13px;
  font-weight: 500;
}

.s-value {
  font-size: 24px;
  font-weight: 700;
  color: #111827;
}

/* Table Card */
.attendance-card {
  border-radius: 12px;
  border: 1px solid #e5e7eb;
}

.table-controls {
  display: flex;
  justify-content: space-between;
  gap: 15px;
}

:deep(.el-card__header) {
  background-color: #f9fafb;
}
</style>