<template>
  <div class="attendance-management">
    <!-- Header -->
    <div class="management-header">
      <h2>Attendance Management</h2>
      <el-button type="primary" @click="showCreateDialog = true">
        <el-icon style="margin-right: 5px"><Plus /></el-icon>
        Record Attendance
      </el-button>
    </div>

    <!-- Search & Filter -->
    <el-card style="margin-bottom: 20px">
      <el-row :gutter="10">
        <el-col :xs="24" :sm="12" :md="8">
          <el-date-picker
            v-model="filterDate"
            type="date"
            placeholder="Select date"
            @change="handleFilter"
          />
        </el-col>
        <el-col :xs="24" :sm="12" :md="8">
          <el-input
            v-model="searchStaff"
            placeholder="Search by staff name"
            clearable
            @input="handleFilter"
          >
            <template #prefix>
              <el-icon><Search /></el-icon>
            </template>
          </el-input>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8">
          <el-select
            v-model="filterSite"
            placeholder="Filter by site"
            clearable
            @change="handleFilter"
          >
            <el-option
              v-for="site in sites"
              :key="site.siteId"
              :label="site.siteName"
              :value="site.siteId"
            />
          </el-select>
        </el-col>
      </el-row>
    </el-card>

    <!-- Attendance Table -->
    <el-card>
      <el-table
        :data="displayedAttendance"
        stripe
        v-loading="loading"
        style="width: 100%"
      >
        <el-table-column label="Staff" width="150">
          <template #default="{ row }">
            {{ getStaffName(row.staffId) }}
          </template>
        </el-table-column>
        <el-table-column label="Work Date" width="120">
          <template #default="{ row }">
            {{ formatDate(row.workDate) }}
          </template>
        </el-table-column>
        <el-table-column label="Site" width="150">
          <template #default="{ row }">
            {{ getSiteName(row.siteId) }}
          </template>
        </el-table-column>
        <el-table-column label="Check-in" width="120">
          <template #default="{ row }">
            {{ getCheckInTime(row) }}
          </template>
        </el-table-column>
        <el-table-column label="Check-out" width="120">
          <template #default="{ row }">
            {{ getCheckOutTime(row) }}
          </template>
        </el-table-column>
        <el-table-column label="Logs Count" width="100">
          <template #default="{ row }">
            <el-tag type="info">{{ row.logs?.length || 0 }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="Actions" width="200">
          <template #default="{ row }">
            <el-button
              type="primary"
              size="small"
              @click="viewAttendance(row)"
            >
              Details
            </el-button>
            <el-button
              type="success"
              size="small"
              @click="addLog(row)"
            >
              Add Log
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- Create Attendance Dialog -->
    <el-dialog
      v-model="showCreateDialog"
      title="Record New Attendance"
      width="500px"
    >
      <el-form
        ref="formRef"
        :model="formData"
        :rules="rules"
        label-width="120px"
      >
        <el-form-item label="Staff" prop="staffId">
          <el-select v-model="formData.staffId" placeholder="Select staff">
            <el-option
              v-for="staff in staffList"
              :key="staff.staffId"
              :label="staff.staffName"
              :value="staff.staffId"
            />
          </el-select>
        </el-form-item>

        <el-form-item label="Work Date" prop="workDate">
          <el-date-picker
            v-model="formData.workDate"
            type="date"
            placeholder="Select date"
          />
        </el-form-item>

        <el-form-item label="Site" prop="siteId">
          <el-select v-model="formData.siteId" placeholder="Select site">
            <el-option
              v-for="site in sites"
              :key="site.siteId"
              :label="site.siteName"
              :value="site.siteId"
            />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="showCreateDialog = false">Cancel</el-button>
        <el-button type="primary" @click="submitForm">Create</el-button>
      </template>
    </el-dialog>

    <!-- Add Log Dialog -->
    <el-dialog
      v-model="showLogDialog"
      title="Add Attendance Log"
      width="500px"
    >
      <el-form
        ref="logFormRef"
        :model="logData"
        :rules="logRules"
        label-width="120px"
      >
        <el-form-item label="Log Type" prop="logType">
          <el-radio-group v-model="logData.logType">
            <el-radio label="IN">Check-in</el-radio>
            <el-radio label="OUT">Check-out</el-radio>
          </el-radio-group>
        </el-form-item>

        <el-form-item label="Log Time" prop="logTime">
          <el-date-picker
            v-model="logData.logTime"
            type="datetime"
            placeholder="Select time"
          />
        </el-form-item>

        <el-form-item label="Distance (m)" prop="distance">
          <el-input-number
            v-model="logData.distance"
            :min="0"
            controls-position="right"
            placeholder="Optional"
          />
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="showLogDialog = false">Cancel</el-button>
        <el-button type="primary" @click="submitLogForm">Add</el-button>
      </template>
    </el-dialog>

    <!-- View Attendance Details Dialog -->
    <el-dialog
      v-model="showViewDialog"
      title="Attendance Details"
      width="600px"
    >
      <div v-if="viewingAttendance" class="attendance-details">
        <el-row :gutter="20">
          <el-col :xs="24" :md="12">
            <div class="detail-item">
              <span class="detail-label">Staff:</span>
              <span class="detail-value">{{ getStaffName(viewingAttendance.staffId) }}</span>
            </div>
          </el-col>
          <el-col :xs="24" :md="12">
            <div class="detail-item">
              <span class="detail-label">Work Date:</span>
              <span class="detail-value">{{ formatDate(viewingAttendance.workDate) }}</span>
            </div>
          </el-col>
          <el-col :xs="24" :md="12">
            <div class="detail-item">
              <span class="detail-label">Site:</span>
              <span class="detail-value">{{ getSiteName(viewingAttendance.siteId) }}</span>
            </div>
          </el-col>
          <el-col :xs="24">
            <el-divider />
          </el-col>
          <el-col :xs="24">
            <h4>Attendance Logs</h4>
            <el-table
              :data="viewingAttendance.logs"
              style="width: 100%"
              v-if="viewingAttendance.logs && viewingAttendance.logs.length > 0"
            >
              <el-table-column label="Type" width="80">
                <template #default="{ row }">
                  <el-tag :type="row.logType === 'IN' ? 'success' : 'warning'">
                    {{ row.logType }}
                  </el-tag>
                </template>
              </el-table-column>
              <el-table-column label="Time">
                <template #default="{ row }">
                  {{ formatDateTime(row.logTime) }}
                </template>
              </el-table-column>
              <el-table-column label="Distance" width="100">
                <template #default="{ row }">
                  {{ row.distance ? row.distance + ' m' : '-' }}
                </template>
              </el-table-column>
            </el-table>
            <div v-else class="empty-logs">
              <p>No logs recorded yet</p>
            </div>
          </el-col>
        </el-row>
      </div>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { ElMessage } from 'element-plus';
import { attendanceService } from '../services/attendanceService';
import { staffService } from '../services/staffService';
import { siteService } from '../services/siteService';
import { Search, Plus } from '@element-plus/icons-vue';
import type { Attendance, Staff, Site, AttendanceLog } from '../types';

const attendanceList = ref<Attendance[]>([]);
const displayedAttendance = ref<Attendance[]>([]);
const staffList = ref<Staff[]>([]);
const sites = ref<Site[]>([]);
const loading = ref(false);
const showCreateDialog = ref(false);
const showLogDialog = ref(false);
const showViewDialog = ref(false);
const viewingAttendance = ref<Attendance | null>(null);
const currentAttendanceId = ref<number | null>(null);

const filterDate = ref<Date | null>(null);
const searchStaff = ref('');
const filterSite = ref<number | null>(null);

const formRef = ref();
const logFormRef = ref();
const formData = ref({
  staffId: 0,
  workDate: new Date(),
  siteId: 0,
});

const logData = ref({
  logType: 'IN',
  logTime: new Date(),
  distance: null as number | null,
});

const rules = {
  staffId: [{ required: true, message: 'Staff is required' }],
  workDate: [{ required: true, message: 'Work date is required' }],
  siteId: [{ required: true, message: 'Site is required' }],
};

const logRules = {
  logType: [{ required: true, message: 'Log type is required' }],
  logTime: [{ required: true, message: 'Log time is required' }],
};

onMounted(async () => {
  await loadAttendance();
  await loadStaff();
  await loadSites();
});

const loadAttendance = async () => {
  loading.value = true;
  try {
    attendanceList.value = await attendanceService.getAll();
    handleFilter();
  } catch (error) {
    ElMessage.error('Failed to load attendance data');
  } finally {
    loading.value = false;
  }
};

const loadStaff = async () => {
  try {
    staffList.value = await staffService.getAll();
  } catch (error) {
    ElMessage.error('Failed to load staff');
  }
};

const loadSites = async () => {
  try {
    sites.value = await siteService.getAll();
  } catch (error) {
    ElMessage.error('Failed to load sites');
  }
};

const getStaffName = (id: number) => {
  return staffList.value.find((s) => s.staffId === id)?.staffName || '-';
};

const getSiteName = (id: number | undefined) => {
  if (!id) return '-';
  return sites.value.find((s) => s.siteId === id)?.siteName || '-';
};

const formatDate = (date: any) => {
  if (!date) return '-';
  const d = new Date(date);
  return d.toLocaleDateString();
};

const formatDateTime = (date: any) => {
  if (!date) return '-';
  const d = new Date(date);
  return d.toLocaleString();
};

const getCheckInTime = (attendance: Attendance) => {
  const inLog = attendance.logs?.find((l) => l.logType === 'IN');
  return inLog ? formatDateTime(inLog.logTime) : '-';
};

const getCheckOutTime = (attendance: Attendance) => {
  const outLog = attendance.logs?.find((l) => l.logType === 'OUT');
  return outLog ? formatDateTime(outLog.logTime) : '-';
};

const handleFilter = () => {
  displayedAttendance.value = attendanceList.value.filter((attendance) => {
    const matchesDate =
      !filterDate.value ||
      new Date(attendance.workDate).toDateString() ===
        new Date(filterDate.value).toDateString();

    const matchesStaff =
      !searchStaff.value ||
      getStaffName(attendance.staffId)
        .toLowerCase()
        .includes(searchStaff.value.toLowerCase());

    const matchesSite =
      !filterSite.value || attendance.siteId === filterSite.value;

    return matchesDate && matchesStaff && matchesSite;
  });
};

const viewAttendance = (attendance: Attendance) => {
  viewingAttendance.value = attendance;
  showViewDialog.value = true;
};

const addLog = (attendance: Attendance) => {
  currentAttendanceId.value = attendance.attendanceId;
  logData.value = {
    logType: 'IN',
    logTime: new Date(),
    distance: null,
  };
  showLogDialog.value = true;
};

const submitForm = async () => {
  if (!formRef.value) return;

  await formRef.value.validate(async (valid: boolean) => {
    if (!valid) return;

    try {
      await attendanceService.create(formData.value);
      ElMessage.success('Attendance recorded successfully');
      showCreateDialog.value = false;
      resetForm();
      await loadAttendance();
    } catch (error: any) {
      ElMessage.error(error.response?.data || 'Operation failed');
    }
  });
};

const submitLogForm = async () => {
  if (!logFormRef.value || !currentAttendanceId.value) return;

  await logFormRef.value.validate(async (valid: boolean) => {
    if (!valid) return;

    try {
      await attendanceService.addLog(currentAttendanceId.value!, logData.value);
      ElMessage.success('Log added successfully');
      showLogDialog.value = false;
      await loadAttendance();
    } catch (error: any) {
      ElMessage.error(error.response?.data || 'Operation failed');
    }
  });
};

const resetForm = () => {
  formData.value = {
    staffId: 0,
    workDate: new Date(),
    siteId: 0,
  };
};
</script>

<style scoped>
.attendance-management {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
}

.management-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.management-header h2 {
  margin: 0;
}

:deep(.el-select) {
  width: 100%;
}

:deep(.el-input) {
  width: 100%;
}

:deep(.el-date-picker) {
  width: 100%;
}

.attendance-details {
  padding: 20px 0;
}

.detail-item {
  display: flex;
  justify-content: space-between;
  padding: 12px 0;
  border-bottom: 1px solid #eee;
}

.detail-label {
  font-weight: 600;
  color: #606266;
}

.detail-value {
  color: #303133;
}

.empty-logs {
  text-align: center;
  padding: 40px 20px;
  color: #999;
}
</style>
