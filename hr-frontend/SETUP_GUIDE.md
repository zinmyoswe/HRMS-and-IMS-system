# HR System Configuration & Setup Guide

## ✅ Installation Completed

Your HR Management System frontend has been successfully built and configured!

## 📋 What's Installed

### npm Dependencies Added
```json
{
  "dependencies": {
    "vue": "^3.5.30",
    "vue-router": "^4.x.x",
    "axios": "^1.x.x",
    "element-plus": "^2.13.6",
    "@element-plus/icons-vue": "^2.3.2"
  }
}
```

### Project Directories Created
```
src/
├── services/          # API service layer
├── views/            # Page components
├── types/            # TypeScript types
├── router/           # Vue Router config
└── components/       # Reusable components (ready for expansion)
```

## 🔗 Backend Connection

### API Base URL
```
https://localhost:7192/api
```

### Configuration Location
`src/services/apiClient.ts`

```typescript
const API_BASE_URL = 'https://localhost:7192/api';
```

### Verify Backend is Running
Visit: `https://localhost:7192/swagger/index.html`

## 🚀 Running the Application

### Development Mode
```bash
cd C:\zinmyoswe\HRsystem\hr-frontend
npm run dev
```

**Access at:** `http://localhost:5173`

### Build for Production
```bash
npm run build
```

### Preview Production Build
```bash
npm run preview
```

## 📚 Project Files Overview

### Configuration Files
- **vite.config.ts** - Build configuration with path aliases
- **tsconfig.json** - TypeScript configuration
- **tsconfig.app.json** - App-specific TS config
- **tsconfig.node.json** - Node config
- **package.json** - Dependencies and scripts
- **index.html** - HTML entry point

### Source Files
```
App.vue                 → Main layout with navigation
main.ts                 → Application entry point
style.css               → Global styles

src/services/
├── apiClient.ts       → Axios instance with interceptors
├── staffService.ts    → Staff API methods
├── departmentService.ts
├── payrollService.ts
├── attendanceService.ts
└── siteService.ts

src/views/
├── Dashboard.vue      → Statistics & overview
├── StaffManagement.vue
├── DepartmentManagement.vue
├── PayrollManagement.vue
└── AttendanceManagement.vue

src/types/
└── index.ts           → All TypeScript interfaces

src/router/
└── index.ts           → Route definitions
```

## 🎯 Features Built

### ✅ Dashboard Module
- Statistics cards (staff count, departments, payroll, attendance)
- Recent staff list
- Department distribution placeholder

### ✅ Staff Management Module
- Full CRUD operations
- Search functionality (name, code, email)
- Filter by department and status
- Form validation
- Dialog-based add/edit
- Confirmation on delete

### ✅ Department Management Module
- View all departments
- Create/Edit/Delete departments
- Staff count per department
- Form validation

### ✅ Payroll Management Module
- List payroll records with detailed info
- Filter by year, month, and staff
- Create new payroll entries
- View payroll details modal
- Currency formatting
- Attendance tracking integration

### ✅ Attendance Management Module
- Record daily attendance
- Check-in/Check-out logging
- Filter by date, staff, and site
- Add attendance logs
- View detailed attendance history
- Distance tracking

## 🔧 TypeScript Configuration

### Path Aliases
```typescript
import { staffService } from '@/services/staffService'
import type { Staff } from '@/types'
```

The `@` symbol resolves to `src/` directory.

## 🎨 Styling

### Global Styles
- `src/style.css` - Global CSS variables and reset

### Component Styles
- Scoped styles in each `.vue` file
- Element Plus theme integration
- Responsive grid system
- Dark mode ready (configurable)

### Color Scheme
- Primary: Purple gradient (#667eea → #764ba2)
- Header: Gradient purple
- Sidebar: Dark blue (#233A4E)
- Accent: Green (#67C23A)
- Background: Light gray (#f0f2f5)

## 📱 Responsive Breakpoints

Element Plus grid system:
- `xs` - Extra small (<768px)
- `sm` - Small (768px+)
- `md` - Medium (992px+)
- `lg` - Large (1200px+)
- `xl` - Extra large (1400px+)

Example:
```vue
<el-col :xs="24" :sm="12" :md="8">
  <!-- Full width on mobile, half on tablet, third on desktop -->
</el-col>
```

## 🔌 API Integration Examples

### Using Staff Service
```typescript
import { staffService } from '@/services/staffService'

// Get all staff
const staff = await staffService.getAll()

// Get single staff
const staff = await staffService.getById(1)

// Create staff
await staffService.create({
  staffCode: 'STF001',
  staffName: 'John Doe',
  email: 'john@example.com',
  isActive: true
})

// Update staff
await staffService.update(1, { staffName: 'Jane Doe' })

// Delete staff
await staffService.delete(1)
```

## 🛠️ Development Workflow

### Adding a New Feature

1. **Create Service** (if new API endpoint)
   ```typescript
   // src/services/newService.ts
   import apiClient from './apiClient'
   export const newService = {
     async getAll() { /* ... */ }
   }
   ```

2. **Create View** (if new page)
   ```vue
   <!-- src/views/NewView.vue -->
   <template>...</template>
   <script setup>...</script>
   <style scoped>...</style>
   ```

3. **Add Route**
   ```typescript
   // src/router/index.ts
   {
     path: '/new',
     name: 'NewView',
     component: () => import('@/views/NewView.vue'),
   }
   ```

4. **Add Menu Item**
   ```vue
   <!-- src/App.vue -->
   <el-menu-item index="/new">
     <el-icon><IconName /></el-icon>
     <span>New Feature</span>
   </el-menu-item>
   ```

## 🚨 Common Setup Issues

### Issue: "Cannot find module '@/services/staffService'"
**Solution:** This is a VS Code intellisense issue. It resolves at runtime.

### Issue: "SSL certificate problem"
**Solution:** 
- Ensure backend runs on HTTPS
- Certificate validation is disabled in dev (see vite.config.ts)

### Issue: "CORS error from backend"
**Solution:** 
- Configure backend CORS to allow `http://localhost:5173`
- Check Program.cs for CORS configuration

### Issue: "Module not found errors"
**Solution:**
```bash
rm -r node_modules package-lock.json
npm install
```

## 📊 Performance Optimization Tips

1. **Lazy Load Routes**
   ```typescript
   component: () => import('@/views/PageName.vue')
   ```
   Already implemented for all routes!

2. **Use Keep-Alive for Components**
   ```vue
   <keep-alive>
     <router-view />
   </keep-alive>
   ```

3. **Implement Virtual Scrolling for Large Lists**
   ```vue
   <el-table-virtual-scroll :data="largeDataset">
   ```

4. **Optimize Images**
   - Use WebP format
   - Lazy load images
   - Compress SVGs

## 🔐 Security Reminders

1. **Enable HTTPS in Production**
   ```typescript
   // vite.config.ts - Change for production
   server: {
     https: true, // Enable HTTPS
   }
   ```

2. **Use Environment Variables**
   ```typescript
   // .env
   VITE_API_BASE_URL=https://api.yourdomain.com
   ```

3. **Add Authentication**
   - Implement login page
   - Store JWT tokens securely
   - Add token refresh logic
   - Implement logout

4. **Request Validation**
   - Validate all inputs
   - Sanitize user data
   - Use CSRF tokens

## 📦 Deployment

### Build for Production
```bash
npm run build
```

### Deploy to Server
1. Build the project (creates `dist/` folder)
2. Upload `dist/` contents to web server
3. Configure server to serve `index.html` for all routes
4. Ensure backend API is accessible from production domain

### Docker Deployment (Optional)
```dockerfile
FROM node:18-alpine
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build
FROM nginx:alpine
COPY --from=0 /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

## 📖 Documentation Generated

1. **HR_SYSTEM_README.md** - Comprehensive feature documentation
2. **QUICK_START.md** - Quick reference guide
3. **SETUP_GUIDE.md** - This file

## ✨ Next Steps

1. **Verify Backend**
   - Ensure .NET Core API is running
   - Check Swagger at `https://localhost:7192/swagger/index.html`

2. **Start Frontend**
   ```bash
   npm run dev
   ```

3. **Test Modules**
   - Navigate to each section
   - Verify data loads from backend
   - Test CRUD operations

4. **Add Features**
   - Authentication
   - Reports generation
   - Export to CSV/PDF
   - Email notifications
   - Mobile responsiveness enhancements

5. **Production Ready**
   - Add error tracking (Sentry)
   - Add analytics
   - Implement logging
   - Setup monitoring

## 🎉 You're All Set!

Your HR Management System is ready for development and testing.

**Quick Commands:**
```bash
# Install dependencies
npm install

# Start development
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

**Access Point:** `http://localhost:5173`

Happy coding! 🚀
