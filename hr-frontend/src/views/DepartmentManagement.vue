<template>
  <div class="dept-container">
    <header class="page-header">
      <div class="header-content">
        <h1 class="page-title">Departments</h1>
        <p class="page-subtitle">Organize your organizational structure and team distribution.</p>
      </div>
      <el-button type="primary" size="large" class="add-btn" @click="handleOpenCreate">
        <el-icon><Plus /></el-icon>
        <span>Add Department</span>
      </el-button>
    </header>

    <div class="stats-overview">
      <el-card shadow="never" class="stat-card">
        <template #default>
          <div class="stat-inner">
            <el-icon class="stat-icon-bg depts"><OfficeBuilding /></el-icon>
            <div class="stat-data">
              <span class="label">Total Departments</span>
              <span class="value">{{ departments.length }}</span>
            </div>
          </div>
        </template>
      </el-card>
      
      <el-card shadow="never" class="stat-card">
        <template #default>
          <div class="stat-inner">
            <el-icon class="stat-icon-bg staff"><User /></el-icon>
            <div class="stat-data">
              <span class="label">Total Staff Assigned</span>
              <span class="value">{{ staffList.length }}</span>
            </div>
          </div>
        </template>
      </el-card>
    </div>

    <el-card class="table-card" shadow="never">
      <el-table
        :data="departments"
        v-loading="loading"
        style="width: 100%"
        class="custom-table"
      >
        <el-table-column prop="departmentId" label="ID" width="100">
          <template #default="{ row }">
            <span class="id-tag">#{{ row.departmentId }}</span>
          </template>
        </el-table-column>

        <el-table-column prop="departmentName" label="Department Name" min-width="200">
          <template #default="{ row }">
            <span class="dept-name-text">{{ row.departmentName }}</span>
          </template>
        </el-table-column>

        <el-table-column label="Headcount" width="180">
          <template #default="{ row }">
            <div class="headcount-pill" :class="{ 'is-empty': getStaffCount(row.departmentId) === 0 }">
              <el-icon><User /></el-icon>
              <span>{{ getStaffCount(row.departmentId) }} Members</span>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="Actions" width="140" align="right">
          <template #default="{ row }">
            <div class="action-group">
              <el-tooltip content="Edit Department" placement="top">
                <el-button circle :icon="Edit" @click="editDepartment(row)" />
              </el-tooltip>
              <el-tooltip content="Delete" placement="top">
                <el-button circle type="danger" plain :icon="Delete" @click="deleteDepartment(row.departmentId)" />
              </el-tooltip>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      v-model="showCreateDialog"
      :title="editingDepartment ? 'Update Department' : 'Create Department'"
      width="420px"
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
        <el-form-item label="Department Name" prop="departmentName">
          <el-input 
            v-model="formData.departmentName" 
            placeholder="e.g., Engineering, Human Resources"
            size="large"
          />
        </el-form-item>
        
        <p class="form-helper" v-if="!editingDepartment">
          <el-icon><InfoFilled /></el-icon>
          New departments will appear immediately in staff selection.
        </p>
      </el-form>

      <template #footer>
        <div class="dialog-footer">
          <el-button @click="showCreateDialog = false">Cancel</el-button>
          <el-button type="primary" size="large" @click="submitForm" class="submit-btn">
            {{ editingDepartment ? 'Save Changes' : 'Create Department' }}
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
  Plus, 
  Edit, 
  Delete, 
  OfficeBuilding, 
  User, 
  InfoFilled 
} from '@element-plus/icons-vue';

// Service/Type imports (KEEP YOUR EXISTING ONES)
import { departmentService } from '../services/departmentService';
import { staffService } from '../services/staffService';
import type { Department, Staff } from '../types';

const departments = ref<Department[]>([]);
const staffList = ref<Staff[]>([]);
const loading = ref(false);
const showCreateDialog = ref(false);
const editingDepartment = ref<Department | null>(null);

const formRef = ref();
const formData = ref({ departmentName: '' });

const rules = {
  departmentName: [{ required: true, message: 'Please provide a name', trigger: 'blur' }],
};

onMounted(async () => {
  await Promise.all([loadDepartments(), loadStaff()]);
});

const loadDepartments = async () => {
  loading.value = true;
  try {
    departments.value = await departmentService.getAll();
  } catch (error) {
    ElMessage.error('Failed to load departments');
  } finally {
    loading.value = false;
  }
};

const loadStaff = async () => {
  try {
    staffList.value = await staffService.getAll();
  } catch (error) {
    console.error('Count load failed');
  }
};

const getStaffCount = (departmentId: number) => {
  return staffList.value.filter((s) => s.departmentId === departmentId).length;
};

const handleOpenCreate = () => {
  resetForm();
  showCreateDialog.value = true;
};

const editDepartment = (department: Department) => {
  editingDepartment.value = department;
  formData.value = { ...department };
  showCreateDialog.value = true;
};

const submitForm = async () => {
  if (!formRef.value) return;
  await formRef.value.validate(async (valid: boolean) => {
    if (!valid) return;
    try {
      if (editingDepartment.value) {
        await departmentService.update(editingDepartment.value.departmentId, formData.value);
        ElMessage.success('Department updated');
      } else {
        await departmentService.create(formData.value);
        ElMessage.success('Department created');
      }
      showCreateDialog.value = false;
      await loadDepartments();
    } catch (error: any) {
      ElMessage.error(error.response?.data || 'Request failed');
    }
  });
};

const deleteDepartment = (id: number) => {
  ElMessageBox.confirm('Remove this department? This might affect staff assignments.', 'Warning', {
    confirmButtonText: 'Remove',
    cancelButtonText: 'Keep',
    type: 'warning'
  }).then(async () => {
    try {
      await departmentService.delete(id);
      ElMessage.success('Removed successfully');
      await loadDepartments();
    } catch (error) {
      ElMessage.error('Delete failed');
    }
  }).catch(() => {});
};

const resetForm = () => {
  editingDepartment.value = null;
  formData.value = { departmentName: '' };
};
</script>

<style scoped>
.dept-container {
  padding: 30px;
  background-color: #f8fafc;
  min-height: 100vh;
}

/* Header */
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  margin-bottom: 32px;
}

.page-title {
  font-size: 1.75rem;
  font-weight: 700;
  color: #0f172a;
  margin: 0;
}

.page-subtitle {
  color: #64748b;
  margin-top: 4px;
}

.add-btn {
  height: 48px;
  border-radius: 12px;
  font-weight: 600;
  padding: 0 24px;
}

/* Stats Overview */
.stats-overview {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 24px;
  margin-bottom: 24px;
}

.stat-card {
  border-radius: 16px;
  border: 1px solid #e2e8f0;
}

.stat-inner {
  display: flex;
  align-items: center;
  gap: 16px;
}

.stat-icon-bg {
  font-size: 1.5rem;
  padding: 12px;
  border-radius: 12px;
}

.stat-icon-bg.depts { background: #eff6ff; color: #3b82f6; }
.stat-icon-bg.staff { background: #f0fdf4; color: #22c55e; }

.stat-data { display: flex; flex-direction: column; }
.stat-data .label { font-size: 0.85rem; color: #64748b; font-weight: 500; }
.stat-data .value { font-size: 1.5rem; font-weight: 800; color: #1e293b; }

/* Table Section */
.table-card {
  border-radius: 16px;
  border: 1px solid #e2e8f0;
}

.id-tag {
  font-family: 'JetBrains Mono', monospace;
  background: #f1f5f9;
  color: #475569;
  padding: 4px 8px;
  border-radius: 6px;
  font-weight: 600;
  font-size: 0.85rem;
}

.dept-name-text {
  font-weight: 600;
  color: #1e293b;
  font-size: 1rem;
}

.headcount-pill {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 4px 12px;
  background: #f0fdf4;
  color: #166534;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
}

.headcount-pill.is-empty {
  background: #f1f5f9;
  color: #64748b;
}

.action-group {
  display: flex;
  gap: 8px;
  justify-content: flex-end;
}

/* Dialog Styles */
.form-helper {
  display: flex;
  align-items: center;
  gap: 8px;
  color: #64748b;
  font-size: 0.8rem;
  margin-top: 12px;
  background: #f8fafc;
  padding: 10px;
  border-radius: 8px;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

.submit-btn {
  min-width: 140px;
  border-radius: 10px;
}
</style>