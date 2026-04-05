<template>
  <div class="device-container">
    <header class="page-header">
      <div class="header-content">
        <h1 class="page-title">Device Fleet</h1>
        <p class="page-subtitle">Inventory management for all network assets and hardware.</p>
      </div>
      <el-button type="primary" size="large" class="gradient-btn" @click="openCreateDialog">
        <el-icon><Plus /></el-icon>
        <span>Add New Device</span>
      </el-button>
    </header>

    <el-card class="filter-card" shadow="never">
      <div class="filter-grid">
        <el-input
          v-model="searchText"
          placeholder="Search name, serial, or IP..."
          clearable
          class="modern-input"
          @input="onFilterChange"
        >
          <template #prefix>
            <el-icon><Search /></el-icon>
          </template>
        </el-input>

        <el-select
          v-model="filterDept"
          placeholder="All Departments"
          clearable
          class="modern-select"
          @change="onFilterChange"
        >
          <el-option v-for="dept in departmentOptions" :key="dept" :label="dept" :value="dept" />
        </el-select>

        <el-select
          v-model="filterType"
          placeholder="Device Types"
          clearable
          class="modern-select"
          @change="onFilterChange"
        >
          <el-option v-for="type in deviceTypeOptions" :key="type" :label="type" :value="type" />
        </el-select>
      </div>
    </el-card>

    <el-card class="table-card" shadow="never">
      <el-table
        :data="displayedDevices"
        v-loading="loading"
        style="width: 100%"
        class="modern-table"
        row-class-name="hover-row"
      >
        <el-table-column label="Asset Details" min-width="260">
          <template #default="{ row }">
            <div class="device-cell">
              <span class="device-name">{{ row.deviceName || 'Unnamed Asset' }}</span>
              <span class="device-meta">{{ row.brand }} {{ row.model }}</span>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="Category" width="160">
    <template #default="{ row }">
      <el-tag 
        :color="getCategoryStyle(row.deviceType).bg" 
        :style="{ color: getCategoryStyle(row.deviceType).text, border: 'none' }"
        effect="dark" 
        round 
        class="category-tag"
      >
        {{ row.deviceType }}
      </el-tag>
    </template>
  </el-table-column>

        <el-table-column label="Network Info" min-width="220">
          <template #default="{ row }">
            <div class="network-cell">
              <div class="ip-row">
                <span class="label">IP:</span>
                <code class="mono-text">{{ row.ipAddress || '—' }}</code>
              </div>
              <div class="mac-row">
                <span class="label">MAC:</span>
                <code class="mono-text small">{{ row.macAddress || '—' }}</code>
              </div>
            </div>
          </template>
        </el-table-column>

        <el-table-column prop="dept" label="Department" width="150">
          <template #default="{ row }">
            <span class="dept-text">{{ row.dept }}</span>
          </template>
        </el-table-column>

        <el-table-column label="Actions" width="120" align="right" fixed="right">
          <template #default="{ row }">
            <div class="action-buttons">
              <el-tooltip content="Edit Asset" placement="top">
                <el-button circle :icon="Edit" @click="editDevice(row)" class="edit-btn" />
              </el-tooltip>
              <el-tooltip content="Delete" placement="top">
                <el-button circle type="danger" plain :icon="Delete" @click="confirmDelete(row.deviceId)" />
              </el-tooltip>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      v-model="showDialog"
      :title="editingDevice ? 'Modify Asset' : 'Register Asset'"
      width="680px"
      class="modern-dialog"
      destroy-on-close
    >
      <el-form
        ref="formRef"
        :model="formData"
        :rules="rules"
        label-position="top"
        class="form-compact"
      >
        <div class="form-section-title">Identity</div>
        <div class="form-row">
          <el-form-item label="Device Name" prop="deviceName" class="flex-2">
            <el-input v-model="formData.deviceName" placeholder="e.g. TH-ASSET-01" />
          </el-form-item>
          <el-form-item label="Department" prop="dept" class="flex-1">
            <el-input v-model="formData.dept" placeholder="IT" />
          </el-form-item>
        </div>

        <div class="form-section-title">Hardware Specs</div>
        <div class="form-row">
          <el-form-item label="Brand" class="flex-1"><el-input v-model="formData.brand" /></el-form-item>
          <el-form-item label="Model" class="flex-1"><el-input v-model="formData.model" /></el-form-item>
          <el-form-item label="Type" prop="deviceType" class="flex-1"><el-input v-model="formData.deviceType" /></el-form-item>
        </div>

        <div class="form-section-title">Network & Tracking</div>
        <div class="form-row">
          <el-form-item label="IP Address" class="flex-1"><el-input v-model="formData.ipAddress" /></el-form-item>
          <el-form-item label="MAC Address" class="flex-1"><el-input v-model="formData.macAddress" /></el-form-item>
        </div>

        <el-form-item label="Remarks">
          <el-input v-model="formData.remark1" type="textarea" :rows="2" placeholder="Additional notes..." />
        </el-form-item>
      </el-form>

      <template #footer>
        <div class="dialog-footer">
          <el-button @click="showDialog = false" round>Cancel</el-button>
          <el-button type="primary" @click="submitForm" class="gradient-btn" round>
            {{ editingDevice ? 'Update Changes' : 'Register Device' }}
          </el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import { Search, Plus, Edit, Delete } from '@element-plus/icons-vue';
import { deviceService } from '../services/deviceService';
import type { Device } from '../types';

const deviceList = ref<Device[]>([]);
const loading = ref(false);
const showDialog = ref(false);
const editingDevice = ref<Device | null>(null);
const searchText = ref('');
const filterDept = ref<string | null>(null);
const filterType = ref<string | null>(null);
const formRef = ref();
const formData = ref<Partial<Device>>({
  dept: '',
  deviceType: '',
  brand: '',
  model: '',
  fixedAssets: '',
  greenLabel: '',
  deviceName: '',
  serialNumber: '',
  macAddress: '',
  ipAddress: '',
  remark1: '',
  remark2: '',
});

const rules = {
  deviceName: [{ required: true, message: 'Device name is required' }],
  deviceType: [{ required: true, message: 'Device type is required' }],
};

const departmentOptions = computed(() =>
  Array.from(new Set(deviceList.value.map((item) => item.dept || 'Unassigned'))).sort(),
);

const deviceTypeOptions = computed(() =>
  Array.from(new Set(deviceList.value.map((item) => item.deviceType || 'Unknown'))).sort(),
);

const displayedDevices = computed(() =>
  deviceList.value.filter((item) => {
    const search = searchText.value.trim().toLowerCase();
    const matchesSearch =
      !search ||
      [
        item.deviceName,
        item.serialNumber,
        item.brand,
        item.model,
        item.deviceType,
        item.dept,
        item.ipAddress,
      ]
        .filter(Boolean)
        .some((value) => value!.toLowerCase().includes(search));

    const matchesDept = !filterDept.value || item.dept === filterDept.value;
    const matchesType = !filterType.value || item.deviceType === filterType.value;

    return matchesSearch && matchesDept && matchesType;
  }),
);

const loadDevices = async () => {
  loading.value = true;
  try {
    deviceList.value = await deviceService.getAll();
  } catch (error) {
    ElMessage.error('Failed to load devices');
  } finally {
    loading.value = false;
  }
};

const openCreateDialog = () => {
  resetForm();
  showDialog.value = true;
};

const editDevice = (device: Device) => {
  editingDevice.value = device;
  formData.value = { ...device };
  showDialog.value = true;
};

const confirmDelete = (id: number) => {
  ElMessageBox.confirm('Delete this device?', 'Warning', {
    confirmButtonText: 'Delete',
    cancelButtonText: 'Cancel',
    type: 'warning',
  })
    .then(async () => {
      try {
        await deviceService.delete(id);
        ElMessage.success('Device deleted');
        await loadDevices();
      } catch (error) {
        ElMessage.error('Failed to delete device');
      }
    })
    .catch(() => {});
};

const submitForm = async () => {
  if (!formRef.value) return;
  await formRef.value.validate(async (valid: boolean) => {
    if (!valid) return;

    try {
      const payload = {
        dept: formData.value.dept || '',
        deviceType: formData.value.deviceType || '',
        brand: formData.value.brand || '',
        model: formData.value.model || '',
        fixedAssets: formData.value.fixedAssets || '',
        greenLabel: formData.value.greenLabel || '',
        deviceName: formData.value.deviceName || '',
        serialNumber: formData.value.serialNumber || '',
        macAddress: formData.value.macAddress || '',
        ipAddress: formData.value.ipAddress || '',
        remark1: formData.value.remark1 || '',
        remark2: formData.value.remark2 || '',
      };

      if (editingDevice.value) {
        await deviceService.update(editingDevice.value.deviceId, payload);
        ElMessage.success('Device updated successfully');
      } else {
        await deviceService.create(payload);
        ElMessage.success('Device created successfully');
      }

      showDialog.value = false;
      await loadDevices();
    } catch (error: any) {
      ElMessage.error(error.response?.data || 'Failed to save device');
    }
  });
};

const onFilterChange = () => {
  // Trigger computed table refresh.
};

const resetForm = () => {
  editingDevice.value = null;
  formData.value = {
    dept: '',
    deviceType: '',
    brand: '',
    model: '',
    fixedAssets: '',
    greenLabel: '',
    deviceName: '',
    serialNumber: '',
    macAddress: '',
    ipAddress: '',
    remark1: '',
    remark2: '',
  };
};

const formatDate = (value?: string | Date | null) => {
  if (!value) return '—';
  const date = new Date(value);
  return date.toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
};

const getCategoryStyle = (type: string) => {
  const category = type?.toLowerCase() || '';
  
  const map: Record<string, { bg: string, text: string }> = {
    'router':      { bg: '#dbeafe', text: '#1e40af' }, // Blue
    'switch':      { bg: '#dcfce7', text: '#166534' }, // Green
    'ap':          { bg: '#f3e8ff', text: '#6b21a8' }, // Purple
    'cato':        { bg: '#ffedd5', text: '#9a3412' }, // Orange
    'printer':     { bg: '#e0f7fa', text: '#006064' }, // Cyan
    'finger scan': { bg: '#fce7f3', text: '#9d174d' }, // Pink
    'scanner':     { bg: '#fef3c7', text: '#92400e' }, // Amber
  };

  // Default fallback for "Other"
  return map[category] || { bg: '#f1f5f9', text: '#475569' };
};

onMounted(loadDevices);
</script>

<style scoped>
/* Layout & Container */
.device-container {
  padding: 32px;
  background-color: #f4f7fa;
  min-height: 100vh;
  color: #2d3748;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  margin-bottom: 32px;
}

.page-title {
  font-size: 1.75rem;
  font-weight: 800;
  letter-spacing: -0.025em;
  margin: 0;
  color: #1a202c;
}

.page-subtitle {
  color: #718096;
  margin: 4px 0 0;
}

/* Elements */
.gradient-btn {
  background: linear-gradient(135deg, #4f46e5 0%, #3b82f6 100%);
  border: none;
  font-weight: 600;
  transition: transform 0.2s, box-shadow 0.2s;
}

.gradient-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(79, 70, 229, 0.3);
}

.filter-card {
  border: none;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.05);
  margin-bottom: 24px;
}

.filter-grid {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr;
  gap: 16px;
}

.table-card {
  border: none;
  border-radius: 16px;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
  overflow: hidden;
}

/* Table styling */
.device-name {
  display: block;
  font-weight: 700;
  color: #1a202c;
  font-size: 1rem;
}

.device-meta {
  font-size: 0.85rem;
  color: #a0aec0;
}

.mono-text {
  font-family: 'JetBrains Mono', 'Fira Code', monospace;
  background: #edf2f7;
  padding: 2px 6px;
  border-radius: 4px;
  color: #4a5568;
}

.mono-text.small {
  font-size: 0.75rem;
  background: transparent;
  padding: 0;
  color: #718096;
}

.network-cell .label {
  font-size: 0.7rem;
  font-weight: 700;
  color: #cbd5e0;
  margin-right: 4px;
  text-transform: uppercase;
}

.action-buttons {
  display: flex;
  gap: 8px;
  justify-content: flex-end;
}

.edit-btn:hover {
  background-color: #ebf4ff;
  color: #3182ce;
  border-color: #3182ce;
}

/* Dialog & Form */
.form-section-title {
  font-size: 0.75rem;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  color: #a0aec0;
  margin: 16px 0 8px;
  border-bottom: 1px solid #edf2f7;
  padding-bottom: 4px;
}

.form-row {
  display: flex;
  gap: 16px;
}

.flex-1 { flex: 1; }
.flex-2 { flex: 2; }

:deep(.el-table__header) {
  background-color: #f8fafc !important;
  color: #718096;
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

:deep(.el-dialog) {
  border-radius: 16px;
}


</style>