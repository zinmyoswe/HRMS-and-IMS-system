<template>
  <div class="payroll-container">
    <header class="page-header">
      <div class="header-content">
        <h1 class="page-title">Payroll Management</h1>
        <p class="page-subtitle">Process monthly salaries and track employee compensations.</p>
      </div>
      <el-button type="primary" size="large" class="add-btn" @click="showCreateDialog = true">
        <el-icon><Plus /></el-icon>
        <span>Generate Payroll</span>
      </el-button>
    </header>

    <div class="top-row">
      <el-card shadow="never" class="filter-card">
        <div class="filter-flex">
          <el-select v-model="filterYear" placeholder="Year" @change="handleFilter" class="filter-item-sm">
            <el-option v-for="year in availableYears" :key="year" :label="year" :value="year" />
          </el-select>
          <el-select v-model="filterMonth" placeholder="All Months" clearable @change="handleFilter" class="filter-item-md">
            <el-option v-for="(name, index) in months" :key="index" :label="name" :value="index + 1" />
          </el-select>
          <el-input v-model="searchStaff" placeholder="Search staff..." clearable @input="handleFilter" class="filter-item-lg">
            <template #prefix><el-icon><Search /></el-icon></template>
          </el-input>
        </div>
      </el-card>

      <div class="quick-stats">
        <div class="mini-stat">
          <span class="stat-label">Total Net Payout</span>
          <span class="stat-value">{{ formatCurrency(totalNetPayout) }}</span>
        </div>
      </div>
    </div>

    <el-card class="table-card" shadow="never">
      <el-table :data="displayedPayroll" v-loading="loading" style="width: 100%" class="custom-table">
        <el-table-column label="Employee" min-width="180">
          <template #default="{ row }">
            <div class="staff-cell">
              <el-avatar :size="32" class="staff-avatar">{{ getStaffName(row.staffId).charAt(0) }}</el-avatar>
              <span class="staff-name">{{ getStaffName(row.staffId) }}</span>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="Period" width="120">
          <template #default="{ row }">
            <span class="period-text">{{ months[row.month - 1].substring(0, 3) }} {{ row.year }}</span>
          </template>
        </el-table-column>

        <el-table-column label="Attendance" width="180">
          <template #default="{ row }">
            <div class="attendance-tags">
              <el-tooltip content="Present Days" placement="top">
                <span class="att-tag present">{{ row.presentDays }}P</span>
              </el-tooltip>
              <el-tooltip content="Absent Days" placement="top">
                <span class="att-tag absent">{{ row.absentDays }}A</span>
              </el-tooltip>
              <el-tooltip content="Leave Days" placement="top">
                <span class="att-tag leave">{{ row.leaveDays }}L</span>
              </el-tooltip>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="Gross Salary" width="140">
          <template #default="{ row }">
            <span class="amount gross">{{ formatCurrency(row.grossSalary) }}</span>
          </template>
        </el-table-column>

        <el-table-column label="Deductions" width="140">
          <template #default="{ row }">
            <span class="amount deduction">-{{ formatCurrency(row.totalDeduction) }}</span>
          </template>
        </el-table-column>

        <el-table-column label="Net Salary" width="150">
          <template #default="{ row }">
            <span class="amount net">{{ formatCurrency(row.netSalary) }}</span>
          </template>
        </el-table-column>

        <el-table-column label="Actions" width="120" align="right">
          <template #default="{ row }">
            <div class="action-buttons">
              <el-button circle :icon="View" @click="viewPayroll(row)" />
              <el-button circle type="danger" plain :icon="Delete" @click="deletePayroll(row.payrollId)" />
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="showCreateDialog" title="Generate New Payroll" width="550px" class="modern-dialog">
      <el-form ref="formRef" :model="formData" :rules="rules" label-position="top">
        <div class="form-grid-2">
          <el-form-item label="Staff Member" prop="staffId">
            <el-select v-model="formData.staffId" placeholder="Select staff" filterable>
              <el-option v-for="staff in staffList" :key="staff.staffId" :label="staff.staffName" :value="staff.staffId" />
            </el-select>
          </el-form-item>
          <div class="form-grid-inner">
            <el-form-item label="Month" prop="month">
              <el-select v-model="formData.month">
                <el-option v-for="(name, index) in months" :key="index" :label="name" :value="index + 1" />
              </el-select>
            </el-form-item>
            <el-form-item label="Year" prop="year">
              <el-input-number v-model="formData.year" :controls="false" style="width: 100%" />
            </el-form-item>
          </div>
        </div>

        <el-divider content-position="left">Attendance & Earnings</el-divider>
        
        <div class="form-grid-3">
          <el-form-item label="Working Days">
            <el-input-number v-model="formData.workingDays" :min="1" />
          </el-form-item>
          <el-form-item label="Present">
            <el-input-number v-model="formData.presentDays" :min="0" />
          </el-form-item>
          <el-form-item label="Absent">
            <el-input-number v-model="formData.absentDays" :min="0" />
          </el-form-item>
        </div>

        <div class="salary-input-group">
          <el-form-item label="Gross Base Salary" prop="grossSalary">
            <el-input v-model="formData.grossSalary" type="number"><template #prefix>$</template></el-input>
          </el-form-item>
          <el-form-item label="Deductions" prop="totalDeduction">
            <el-input v-model="formData.totalDeduction" type="number"><template #prefix>$</template></el-input>
          </el-form-item>
        </div>
      </el-form>
      <template #footer>
        <el-button @click="showCreateDialog = false">Cancel</el-button>
        <el-button type="primary" @click="submitForm" class="submit-btn">Confirm & Generate</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="showViewDialog" title="Payroll Statement" width="480px" custom-class="payslip-dialog">
      <div v-if="viewingPayroll" class="payslip-container">
        <div class="payslip-header">
          <h3>{{ getStaffName(viewingPayroll.staffId) }}</h3>
          <p>{{ months[viewingPayroll.month-1] }} {{ viewingPayroll.year }}</p>
        </div>
        
        <div class="payslip-body">
          <div class="payslip-row"><span>Working Days</span> <span>{{ viewingPayroll.workingDays }}</span></div>
          <div class="payslip-row"><span>Days Present</span> <span class="text-success">{{ viewingPayroll.presentDays }}</span></div>
          <div class="payslip-row mb-2"><span>Days Absent</span> <span class="text-danger">{{ viewingPayroll.absentDays }}</span></div>
          
          <el-divider />
          
          <div class="payslip-row"><span>Base Salary</span> <span>{{ formatCurrency(viewingPayroll.grossSalary) }}</span></div>
          <div class="payslip-row"><span>Allowances</span> <span>{{ formatCurrency(viewingPayroll.totalAllowance) }}</span></div>
          <div class="payslip-row text-danger"><span>Deductions</span> <span>-{{ formatCurrency(viewingPayroll.totalDeduction) }}</span></div>
          
          <div class="payslip-total">
            <span>Net Payout</span>
            <span>{{ formatCurrency(viewingPayroll.netSalary) }}</span>
          </div>
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import { Search, Plus, View, Delete } from '@element-plus/icons-vue';
import { payrollService } from '../services/payrollService';
import { staffService } from '../services/staffService';
import type { Payroll, Staff } from '../types';

const payrollList = ref<Payroll[]>([]);
const displayedPayroll = ref<Payroll[]>([]);
const staffList = ref<Staff[]>([]);
const loading = ref(false);
const showCreateDialog = ref(false);
const showViewDialog = ref(false);
const viewingPayroll = ref<Payroll | null>(null);

const filterYear = ref(new Date().getFullYear());
const filterMonth = ref<number | null>(null);
const searchStaff = ref('');

const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

const availableYears = computed(() => {
  const current = new Date().getFullYear();
  return Array.from({length: 11}, (_, i) => current - 5 + i);
});

const totalNetPayout = computed(() => {
  return displayedPayroll.value.reduce((sum, item) => sum + (item.netSalary || 0), 0);
});

const formRef = ref();
const formData = ref({
  staffId: null as any,
  year: new Date().getFullYear(),
  month: new Date().getMonth() + 1,
  workingDays: 22,
  presentDays: 22,
  absentDays: 0,
  leaveDays: 0,
  totalAllowance: 0,
  totalDeduction: 0,
  grossSalary: 0,
  netSalary: 0,
});

const rules = {
  staffId: [{ required: true, message: 'Please select staff' }],
  grossSalary: [{ required: true, message: 'Gross salary required' }],
};

onMounted(async () => {
  await Promise.all([loadPayroll(), loadStaff()]);
});

const loadPayroll = async () => {
  loading.value = true;
  try {
    payrollList.value = await payrollService.getAll();
    handleFilter();
  } catch (e) { ElMessage.error('Load failed'); }
  finally { loading.value = false; }
};

const loadStaff = async () => {
  try { staffList.value = await staffService.getAll(); } catch (e) {}
};

const getStaffName = (id: number) => staffList.value.find(s => s.staffId === id)?.staffName || 'Unknown';

const formatCurrency = (val: number) => new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(val);

const handleFilter = () => {
  displayedPayroll.value = payrollList.value.filter(p => {
    const y = !filterYear.value || p.year === filterYear.value;
    const m = !filterMonth.value || p.month === filterMonth.value;
    const s = !searchStaff.value || getStaffName(p.staffId).toLowerCase().includes(searchStaff.value.toLowerCase());
    return y && m && s;
  });
};

const viewPayroll = (p: Payroll) => {
  viewingPayroll.value = p;
  showViewDialog.value = true;
};

const submitForm = async () => {
  if (!formRef.value) return;
  await formRef.value.validate(async (valid: boolean) => {
    if (!valid) return;
    try {
      // Calculate net before sending if simple logic applies
      formData.value.netSalary = Number(formData.value.grossSalary) + Number(formData.value.totalAllowance) - Number(formData.value.totalDeduction);
      await payrollService.create(formData.value);
      ElMessage.success('Payroll generated');
      showCreateDialog.value = false;
      loadPayroll();
    } catch (e) { ElMessage.error('Failed to create'); }
  });
};

const deletePayroll = (id: number) => {
  ElMessageBox.confirm('Delete this record?', 'Warning', { type: 'warning' }).then(() => {
    ElMessage.info('Feature pending API update');
  });
};
</script>

<style scoped>
.payroll-container { padding: 24px; background-color: #f8fafc; min-height: 100vh; }

.page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
.page-title { font-size: 1.5rem; font-weight: 700; color: #1e293b; margin: 0; }
.page-subtitle { color: #64748b; margin: 4px 0 0; }

.top-row { display: flex; gap: 16px; margin-bottom: 20px; align-items: stretch; }
.filter-card { flex: 1; border-radius: 12px; }
.filter-flex { display: flex; gap: 12px; }
.filter-item-sm { width: 100px; }
.filter-item-md { width: 150px; }
.filter-item-lg { flex: 1; }

.quick-stats { background: #fff; border: 1px solid #e2e8f0; border-radius: 12px; padding: 0 24px; display: flex; align-items: center; }
.stat-label { font-size: 0.75rem; color: #64748b; text-transform: uppercase; font-weight: 600; display: block; }
.stat-value { font-size: 1.25rem; font-weight: 700; color: #10b981; }

.table-card { border-radius: 12px; }
.staff-cell { display: flex; align-items: center; gap: 10px; }
.staff-name { font-weight: 600; color: #1e293b; }
.period-text { color: #64748b; font-weight: 500; }

.attendance-tags { display: flex; gap: 4px; }
.att-tag { padding: 2px 8px; border-radius: 4px; font-size: 11px; font-weight: 700; }
.present { background: #f0fdf4; color: #16a34a; }
.absent { background: #fef2f2; color: #dc2626; }
.leave { background: #fff7ed; color: #ea580c; }

.amount { font-family: monospace; font-weight: 600; }
.amount.gross { color: #475569; }
.amount.deduction { color: #dc2626; }
.amount.net { color: #0f172a; font-size: 1.05rem; }

/* Dialog Styles */
.form-grid-2 { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.form-grid-inner { display: flex; gap: 8px; }
.form-grid-3 { display: grid; grid-template-columns: repeat(3, 1fr); gap: 12px; }
.salary-input-group { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin-top: 10px; }

.payslip-header { text-align: center; margin-bottom: 24px; }
.payslip-header h3 { margin: 0; color: #1e293b; }
.payslip-header p { color: #64748b; margin: 4px 0; }
.payslip-row { display: flex; justify-content: space-between; padding: 8px 0; color: #475569; }
.payslip-total { margin-top: 16px; padding: 16px; background: #f8fafc; border-radius: 8px; display: flex; justify-content: space-between; font-weight: 800; font-size: 1.2rem; color: #10b981; }

.submit-btn { border-radius: 8px; font-weight: 600; padding: 0 24px; }
</style>