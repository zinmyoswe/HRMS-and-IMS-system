<template>
  <div class="staff-container">
    <header class="page-header">
      <div class="header-content">
        <h1 class="page-title">Staff Management</h1>
        <p class="page-subtitle">Manage your organization's workforce and departments.</p>
      </div>
      <el-button type="primary" size="large" class="add-btn" @click="handleOpenCreate">
        <el-icon><Plus /></el-icon>
        <span>Add New Staff</span>
      </el-button>
    </header>

    <el-card class="filter-card" shadow="never">
      <div class="filter-grid">
        <div class="search-input">
          <el-input
            v-model="searchText"
            placeholder="Search name, code, or email..."
            clearable
            @input="handleSearch"
          >
            <template #prefix>
              <el-icon><Search /></el-icon>
            </template>
          </el-input>
        </div>
        
        <div class="filter-controls">
          <el-select v-model="filterDepartment" placeholder="All Departments" clearable @change="handleFilter">
            <el-option
              v-for="dept in departments"
              :key="dept.departmentId"
              :label="dept.departmentName"
              :value="dept.departmentId"
            />
          </el-select>

          <el-select v-model="filterStatus" placeholder="All Status" clearable @change="handleFilter">
            <el-option label="Active" :value="true" />
            <el-option label="Inactive" :value="false" />
          </el-select>
        </div>
      </div>
    </el-card>

    <el-card class="table-card" shadow="never">
      <el-table
        :data="displayedStaff"
        v-loading="loading"
        style="width: 100%"
        class="custom-table"
      >
        <el-table-column label="Staff Info" min-width="250">
          <template #default="{ row }">
            <div class="staff-info-cell">
              <el-avatar :size="40" class="staff-avatar">{{ row.staffName.charAt(0) }}</el-avatar>
              <div class="info-text">
                <span class="staff-name">{{ row.staffName }}</span>
                <span class="staff-code">{{ row.staffCode }}</span>
              </div>
            </div>
          </template>
        </el-table-column>

        <el-table-column prop="email" label="Contact" min-width="200">
          <template #default="{ row }">
            <div class="contact-cell">
              <div class="contact-row"><el-icon><Message /></el-icon> {{ row.email }}</div>
              <div class="contact-row"><el-icon><Phone /></el-icon> {{ row.phone }}</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="Department" width="180">
          <template #default="{ row }">
            <el-tag effect="light" round class="dept-tag">
              {{ getDepartmentName(row.departmentId) }}
            </el-tag>
          </template>
        </el-table-column>

        <el-table-column label="Position" width="180">
          <template #default="{ row }">
            <el-tag effect="light" round class="position-tag">
              {{ getPositionName(row.positionId) }}
            </el-tag>
          </template>
        </el-table-column>

        <el-table-column label="Status" width="120">
          <template #default="{ row }">
            <div :class="['status-indicator', row.isActive ? 'active' : 'inactive']">
              <span class="dot"></span>
              {{ row.isActive ? 'Active' : 'Inactive' }}
            </div>
          </template>
        </el-table-column>

        <el-table-column label="Actions" width="140" align="right">
          <template #default="{ row }">
            <div class="action-buttons">
              <el-button circle :icon="Edit" @click="editStaff(row)" />
              <el-button circle type="danger" plain :icon="Delete" @click="deleteStaff(row.staffId)" />
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      v-model="showCreateDialog"
      :title="editingStaff ? 'Update Staff Member' : 'Create New Staff'"
      width="600px"
      class="modern-dialog"
      @close="resetForm"
    >
      <el-form
        ref="formRef"
        :model="formData"
        :rules="rules"
        label-position="top"
        class="modern-form"
      >
        <div class="form-grid">
          <el-form-item label="Staff Code" prop="staffCode">
            <el-input v-model="formData.staffCode" :disabled="!!editingStaff" placeholder="STF-001" />
          </el-form-item>
          
          <el-form-item label="Full Name" prop="staffName">
            <el-input v-model="formData.staffName" placeholder="Enter name" />
          </el-form-item>

          <el-form-item label="Email Address" prop="email">
            <el-input v-model="formData.email" placeholder="email@company.com" />
          </el-form-item>

          <el-form-item label="Phone Number" prop="phone">
            <el-input v-model="formData.phone" placeholder="+1..." />
          </el-form-item>

          <el-form-item label="Department" prop="departmentId">
            <el-select v-model="formData.departmentId" placeholder="Select" style="width: 100%">
              <el-option
                v-for="dept in departments"
                :key="dept.departmentId"
                :label="dept.departmentName"
                :value="dept.departmentId"
              />
            </el-select>
          </el-form-item>

          <el-form-item label="Position" prop="positionId">
            <el-select v-model="formData.positionId" placeholder="Select" style="width: 100%">
              <el-option
                v-for="pos in positions"
                :key="pos.positionId"
                :label="pos.positionName"
                :value="pos.positionId"
              />
            </el-select>
          </el-form-item>

          <el-form-item label="Joined Date" prop="joinedDate">
            <el-date-picker v-model="formData.joinedDate" type="date" style="width: 100%" />
          </el-form-item>
        </div>

        <el-form-item label="Residential Address" prop="address">
          <el-input v-model="formData.address" type="textarea" :rows="2" />
        </el-form-item>

        <div class="status-toggle">
          <span class="toggle-label">Active Status</span>
          <el-switch v-model="formData.isActive" />
        </div>
      </el-form>

      <template #footer>
        <div class="dialog-footer">
          <el-button @click="showCreateDialog = false">Cancel</el-button>
          <el-button type="primary" size="large" @click="submitForm" class="submit-btn">
            {{ editingStaff ? 'Save Changes' : 'Create Member' }}
          </el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import { 
  Search, 
  Plus, 
  Edit, 
  Delete, 
  Message, 
  Phone 
} from '@element-plus/icons-vue';

// Replace these imports with your actual paths
import { staffService } from '../services/staffService';
import { departmentService } from '../services/departmentService';
import { positionService } from '../services/positionService';
import type { Staff, Department, Position } from '../types';

// --- State Management ---
const staffList = ref<Staff[]>([]);
const displayedStaff = ref<Staff[]>([]);
const departments = ref<Department[]>([]);
const positions = ref<Position[]>([]);
const loading = ref(false);
const showCreateDialog = ref(false);
const editingStaff = ref<Staff | null>(null);
const searchText = ref('');
const filterDepartment = ref<number | null>(null);
const filterStatus = ref<boolean | null>(null);

const formRef = ref();
const formData = ref({
  staffCode: '',
  staffName: '',
  email: '',
  phone: '',
  address: '',
  dateOfBirth: null as any,
  joinedDate: new Date(),
  departmentId: null as any,
  positionId: null as any,
  isActive: true,
});

const rules = {
  staffCode: [{ required: true, message: 'Required' }],
  staffName: [{ required: true, message: 'Required' }],
  email: [
    { required: true, message: 'Required' },
    { type: 'email', message: 'Invalid format' },
  ],
  joinedDate: [{ required: true, message: 'Required' }],
};

// --- Lifecycle ---
onMounted(async () => {
  await loadStaff();
  await loadDepartments();
  await loadPositions();
});

// --- Methods ---
const loadStaff = async () => {
  loading.value = true;
  try {
    staffList.value = await staffService.getAll();
    applyFilters();
  } catch (error) {
    ElMessage.error('Failed to load staff data');
  } finally {
    loading.value = false;
  }
};

const loadDepartments = async () => {
  try {
    departments.value = await departmentService.getAll();
  } catch (error) {
    console.error('Failed to load departments');
  }
};

const loadPositions = async () => {
  try {
    positions.value = await positionService.getAll();
  } catch (error) {
    console.error('Failed to load positions');
  }
};

const getDepartmentName = (id: number | undefined) => {
  if (!id) return 'Unassigned';
  return departments.value.find((d) => d.departmentId === id)?.departmentName || 'Unknown';
};

const getPositionName = (id: number | undefined) => {
  if (!id) return 'Unassigned';
  return positions.value.find((p) => p.positionId === id)?.positionName || 'Unknown';
};

const handleSearch = () => applyFilters();
const handleFilter = () => applyFilters();

const applyFilters = () => {
  displayedStaff.value = staffList.value.filter((staff) => {
    const matchesSearch =
      !searchText.value ||
      staff.staffName.toLowerCase().includes(searchText.value.toLowerCase()) ||
      staff.staffCode.toLowerCase().includes(searchText.value.toLowerCase()) ||
      staff.email.toLowerCase().includes(searchText.value.toLowerCase());

    const matchesDept =
      filterDepartment.value === null || staff.departmentId === filterDepartment.value;

    const matchesStatus =
      filterStatus.value === null || staff.isActive === filterStatus.value;

    return matchesSearch && matchesDept && matchesStatus;
  });
};

const handleOpenCreate = () => {
  resetForm();
  showCreateDialog.value = true;
};

const editStaff = (staff: Staff) => {
  editingStaff.value = staff;
  formData.value = { ...staff };
  showCreateDialog.value = true;
};

const submitForm = async () => {
  if (!formRef.value) return;
  await formRef.value.validate(async (valid: boolean) => {
    if (!valid) return;
    try {
      if (editingStaff.value) {
        await staffService.update(editingStaff.value.staffId, formData.value);
        ElMessage.success('Staff updated');
      } else {
        await staffService.create(formData.value);
        ElMessage.success('Staff created');
      }
      showCreateDialog.value = false;
      await loadStaff();
    } catch (error: any) {
      ElMessage.error(error.response?.data || 'Operation failed');
    }
  });
};

const deleteStaff = (id: number) => {
  ElMessageBox.confirm('Delete this staff member?', 'Warning', {
    confirmButtonText: 'Delete',
    cancelButtonText: 'Cancel',
    type: 'warning',
    buttonSize: 'default'
  }).then(async () => {
    try {
      await staffService.delete(id);
      ElMessage.success('Deleted successfully');
      await loadStaff();
    } catch (error) {
      ElMessage.error('Delete failed');
    }
  }).catch(() => {});
};

const resetForm = () => {
  editingStaff.value = null;
  formData.value = {
    staffCode: '',
    staffName: '',
    email: '',
    phone: '',
    address: '',
    dateOfBirth: null,
    joinedDate: new Date(),
    departmentId: null,
    positionId: null,
    isActive: true,
  };
};
</script>

<style scoped>
.staff-container {
  padding: 24px;
  background-color: #f8fafc;
  min-height: 100vh;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.page-title {
  font-size: 1.5rem;
  font-weight: 700;
  margin: 0;
  color: #1e293b;
}

.page-subtitle {
  color: #64748b;
  margin: 4px 0 0;
}

.filter-card {
  margin-bottom: 24px;
  border-radius: 8px;
}

.filter-grid {
  display: flex;
  gap: 16px;
  flex-wrap: wrap;
}

.search-input { flex: 1; min-width: 250px; }
.filter-controls { display: flex; gap: 12px; }

.table-card { border-radius: 8px; }

.staff-info-cell {
  display: flex;
  align-items: center;
  gap: 12px;
}

.staff-avatar { background-color: #409eff; font-weight: bold; }
.info-text { display: flex; flex-direction: column; }
.staff-name { font-weight: 600; color: #1e293b; }
.staff-code { font-size: 0.75rem; color: #94a3b8; }

.contact-row {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 0.85rem;
  color: #64748b;
  margin-bottom: 2px;
}

.status-indicator {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-weight: 500;
  font-size: 0.85rem;
}

.dot { width: 8px; height: 8px; border-radius: 50%; }
.active { color: #10b981; }
.active .dot { background-color: #10b981; }
.inactive { color: #ef4444; }
.inactive .dot { background-color: #ef4444; }

.dept-tag { background-color: #e0f2fe; color: #0369a1; border-color: #bae6fd; }
.position-tag { background-color: #f0fdf4; color: #166534; border-color: #bbf7d0; }

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.status-toggle {
  display: flex;
  justify-content: space-between;
  padding: 12px;
  background: #f1f5f9;
  border-radius: 6px;
  margin-top: 12px;
}

.toggle-label { font-weight: 500; color: #475569; }

:deep(.el-table__header) { color: #475569; font-weight: 600; }
</style>