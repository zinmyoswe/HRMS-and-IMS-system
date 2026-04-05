<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import { useRoute } from 'vue-router';
import {
  House,
  User,
  OfficeBuilding,
  Money,
  Calendar,
  Bell,
  Search,
  ArrowDown,
  Switch,
  
} from '@element-plus/icons-vue';

const route = useRoute();
const activeMenu = ref(route.path);

// Sync menu with route changes
watch(() => route.path, (newPath) => {
  activeMenu.value = newPath;
});

const pageTitle = computed(() => {
  const titles: Record<string, string> = {
    '/': 'Dashboard Overview',
    '/staff': 'Staff Directory',
    '/departments': 'Organization Units',
    '/devices': 'Device Inventory',
    '/payroll': 'Payroll Management',
    '/attendance': 'Attendance Tracking',
  };
  return titles[route.path] || 'Management System';
});
</script>

<template>
  <el-container class="app-wrapper">
    <el-aside width="260px" class="main-sidebar">
      <div class="sidebar-brand">
        <img src="/src/assets/Inline-Logo-FC-Inverted.svg" alt="Logo" class="brand-logo" />
        <span class="brand-name"></span>
      </div>

      <el-scrollbar>
        <el-menu
          router
          :default-active="activeMenu"
          class="sidebar-menu"
          background-color="transparent"
          text-color="#94a3b8"
          active-text-color="#ffffff"
        >
          <div class="menu-label">Main Navigation</div>
          
          <el-menu-item index="/">
            <el-icon><House /></el-icon>
            <span>Dashboard</span>
          </el-menu-item>

          <el-menu-item index="/staff">
            <el-icon><User /></el-icon>
            <span>Staff Management</span>
          </el-menu-item>

          <el-menu-item index="/departments">
            <el-icon><OfficeBuilding /></el-icon>
            <span>Departments</span>
          </el-menu-item>


          <div class="menu-label">Financials & Operations</div>

          <el-menu-item index="/payroll">
            <el-icon><Money /></el-icon>
            <span>Payroll</span>
          </el-menu-item>

          <el-menu-item index="/attendance">
            <el-icon><Calendar /></el-icon>
            <span>Attendance</span>
          </el-menu-item>

          

          <div class="menu-label">IMS Management</div>

          <el-menu-item index="/devices">
            <el-icon><Switch /></el-icon>
            <span>Devices</span>
          </el-menu-item>
        </el-menu>
      </el-scrollbar>

      <div class="sidebar-footer">
        <div class="user-mini-card">
          <el-avatar :size="32" src="https://api.dicebear.com/7.x/avataaars/svg?seed=Admin" />
          <div class="user-meta">
            <p class="u-name">Admin User</p>
            <p class="u-role">Super Admin</p>
          </div>
        </div>
      </div>
    </el-aside>

    <el-container class="main-container">
      <el-header height="70px" class="main-header">
        <div class="header-left">
          <h2 class="page-current-title">{{ pageTitle }}</h2>
        </div>

        <div class="header-right">
          <el-input
            placeholder="Search..."
            :prefix-icon="Search"
            class="header-search"
          />
          
          <el-badge is-dot class="notice-item">
            <el-button :icon="Bell" circle variant="text" />
          </el-badge>

          <div class="v-divider"></div>

          <el-dropdown trigger="click">
            <div class="profile-trigger">
              <span>Admin</span>
              <el-icon><ArrowDown /></el-icon>
            </div>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item>My Profile</el-dropdown-item>
                <el-dropdown-item>Settings</el-dropdown-item>
                <el-dropdown-item divided style="color: #F56C6C">Logout</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </el-header>

      <el-main class="main-content-area">
        <router-view v-slot="{ Component }">
          <transition name="page-fade" mode="out-in">
            <component :is="Component" />
          </transition>
        </router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<style scoped>
/* Layout Structure */
.app-wrapper {
  height: 100vh;
  overflow: hidden;
}

/* Sidebar Styling */
.main-sidebar {
  background-color: #0f172a; /* Slate 900 */
  display: flex;
  flex-direction: column;
  transition: all 0.3s ease;
  border-right: 1px solid #1e293b;
}

.sidebar-brand {
  padding: 24px;
  display: flex;
  align-items: center;
  gap: 12px;
}

.brand-logo {
  height: 32px;
  width: auto;
}

.brand-name {
  color: white;
  font-weight: 700;
  font-size: 18px;
  letter-spacing: -0.5px;
}

.sidebar-menu {
  border-right: none;
  padding: 0 12px;
}

.menu-label {
  padding: 20px 12px 10px;
  font-size: 11px;
  font-weight: 700;
  color: #475569;
  text-transform: uppercase;
  letter-spacing: 1px;
}

:deep(.el-menu-item) {
  height: 48px;
  line-height: 48px;
  margin-bottom: 4px;
  border-radius: 8px;
}

:deep(.el-menu-item:hover) {
  background-color: #1e293b !important;
  color: #fff !important;
}

:deep(.el-menu-item.is-active) {

  color: #fff !important;
  /* box-shadow: 0 4px 12px rgba(59, 130, 246, 0.25); */
}

.sidebar-footer {
  padding: 20px;
  border-top: 1px solid #1e293b;
}

.user-mini-card {
  display: flex;
  align-items: center;
  gap: 10px;
  background: #1e293b;
  padding: 10px;
  border-radius: 12px;
}

.user-meta .u-name {
  color: white;
  font-size: 13px;
  font-weight: 600;
  margin: 0;
}

.user-meta .u-role {
  color: #64748b;
  font-size: 11px;
  margin: 0;
}

/* Header Styling */
.main-header {
  background: #ffffff;
  border-bottom: 1px solid #e2e8f0;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 32px;
}

.page-current-title {
  font-size: 18px;
  font-weight: 600;
  color: #1e293b;
  margin: 0;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 20px;
}

.header-search {
  width: 240px;
}

:deep(.header-search .el-input__wrapper) {
  background-color: #f1f5f9;
  box-shadow: none !important;
  border-radius: 8px;
}

.v-divider {
  width: 1px;
  height: 24px;
  background: #e2e8f0;
}

.profile-trigger {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  font-weight: 500;
  color: #475569;
}

/* Main Area */
.main-content-area {
  background-color: #f8fafc;
  padding: 0; /* Let dashboard component handle padding */
}

/* Page Transitions */
.page-fade-enter-active,
.page-fade-leave-active {
  transition: all 0.25s ease;
}

.page-fade-enter-from {
  opacity: 0;
  transform: translateY(10px);
}

.page-fade-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
</style>