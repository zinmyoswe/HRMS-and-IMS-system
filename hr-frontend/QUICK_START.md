# HR System - Quick Start Guide

## 🎯 What's Been Built

A **complete, production-ready HR Management System UI** with 5 main modules:

### ✅ Completed Components

1. **Dashboard** - Overview statistics and quick access
2. **Staff Management** - Full CRUD operations for employee records
3. **Department Management** - Manage organizational structure
4. **Payroll Management** - Process and track salary information
5. **Attendance Management** - Track employee attendance and logs

## 🚀 Getting Started

### Step 1: Ensure Backend is Running
Make sure your .NET Core API is running:
```
https://localhost:7192
```

Check the Swagger documentation at:
```
https://localhost:7192/swagger/index.html
```

### Step 2: Install Dependencies
```bash
cd C:\zinmyoswe\HRsystem\hr-frontend
npm install
```

### Step 3: Start Development Server
```bash
npm run dev
```

The application will start at `http://localhost:5173`

### Step 4: Open in Browser
Navigate to `http://localhost:5173` and you'll see:
- **Navigation sidebar** with all modules
- **Dashboard** with statistics
- Full HR management interface

## 📋 Feature Summary

### Staff Management
- ✅ View all staff members
- ✅ Search by name, code, email
- ✅ Filter by department and status
- ✅ Create new staff
- ✅ Edit existing staff
- ✅ Delete staff members
- ✅ Track all staff details (contact, department, manager, etc.)

### Department Management
- ✅ View all departments
- ✅ Create new departments
- ✅ Edit departments
- ✅ Delete departments
- ✅ View staff count per department

### Payroll Management
- ✅ View payroll records
- ✅ Filter by year and month
- ✅ Search by staff name
- ✅ Create new payroll entries
- ✅ View detailed payroll information
- ✅ Track attendance, deductions, and net salary

### Attendance Management
- ✅ Record attendance for staff
- ✅ View all attendance records
- ✅ Filter by date, staff, and site
- ✅ Add check-in/check-out logs
- ✅ Track distance (GPS data)
- ✅ View detailed attendance logs

### Dashboard
- ✅ Total staff count
- ✅ Department count
- ✅ Monthly payroll summary
- ✅ Daily attendance statistics
- ✅ Recent staff list

## 📁 File Structure

### Services Layer
```
src/services/
├── apiClient.ts              # Axios configuration
├── staffService.ts           # Staff API calls
├── departmentService.ts      # Department API calls
├── payrollService.ts         # Payroll API calls
├── attendanceService.ts      # Attendance API calls
└── siteService.ts            # Site API calls
```

### Components & Views
```
src/
├── views/
│   ├── Dashboard.vue                    # Overview page
│   ├── StaffManagement.vue              # Staff CRUD
│   ├── DepartmentManagement.vue         # Department CRUD
│   ├── PayrollManagement.vue            # Payroll management
│   └── AttendanceManagement.vue         # Attendance tracking
├── router/
│   └── index.ts                         # Route configuration
├── types/
│   └── index.ts                         # TypeScript interfaces
└── App.vue                              # Main layout
```

### Type Definitions
All data types are defined in `src/types/index.ts`:
- Staff, Department, Position, Site
- Attendance, AttendanceLog
- Payroll
- ApiResponse

## 🔌 API Endpoints Used

```
Staff:
  GET     /api/staffs          - Get all
  GET     /api/staffs/{id}     - Get single
  POST    /api/staffs          - Create
  PUT     /api/staffs/{id}     - Update
  DELETE  /api/staffs/{id}     - Delete

Departments:
  GET     /api/departments     - Get all
  POST    /api/departments     - Create
  PUT     /api/departments/{id} - Update
  DELETE  /api/departments/{id} - Delete

Payroll:
  GET     /api/payrolls        - Get all
  POST    /api/payrolls        - Create

Attendance:
  GET     /api/attendances     - Get all
  POST    /api/attendances     - Create
  POST    /api/attendances/{id}/logs - Add log

Sites:
  GET     /api/sites           - Get all
  POST    /api/sites           - Create
```

## 🎨 UI Features

✅ **Responsive Design**
- Desktop, tablet, and mobile support
- Flexible grid layout

✅ **Professional Styling**
- Purple gradient header
- Dark sidebar navigation
- Clean, modern interface
- Smooth transitions

✅ **Element Plus Components**
- Data tables with sorting
- Form validation
- Modal dialogs
- Date/time pickers
- Dropdowns and selects
- Status tags
- Icons

✅ **User Experience**
- Search and filter functionality
- Loading indicators
- Success/error notifications
- Confirmation dialogs
- Pagination-ready tables
- Empty states

## 🛠️ Development

### Add a New Module

1. Create a service in `src/services/`
2. Create a view in `src/views/`
3. Add route in `src/router/index.ts`
4. Add menu item in `src/App.vue`

### Customize Styling

Global styles in `src/style.css`
Component styles in `<style scoped>` blocks

### Handle API Errors

All services use try-catch with user-friendly error messages:
```typescript
try {
  const data = await staffService.getAll();
} catch (error) {
  ElMessage.error('Failed to load staff');
}
```

## 📦 Technology Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| Vue | 3.5.30 | UI Framework |
| TypeScript | 5.9 | Type safety |
| Vite | 8.0.1 | Build tool |
| Element Plus | 2.13.6 | UI Components |
| Axios | Latest | HTTP client |
| Vue Router | 4.x | Routing |

## 🚀 Production Build

Generate optimized build:
```bash
npm run build
```

Output in `dist/` folder, ready for deployment.

## 🔐 Security Checklist

- [ ] Enable certificate validation in production
- [ ] Add authentication/login page
- [ ] Implement JWT token handling
- [ ] Add role-based access control (RBAC)
- [ ] Validate inputs on backend
- [ ] Use HTTPS only
- [ ] Add CORS configuration
- [ ] Implement request rate limiting

## 🐛 Debugging

### Check API Connection
1. Open browser DevTools (F12)
2. Go to Network tab
3. Check requests to `https://localhost:7192/api`
4. Look for 200 status codes (success) or error codes

### Common Issues

**CORS Error**: Configure backend to allow frontend origin
**SSL Error**: Ensure backend runs on HTTPS
**Module Not Found**: Run `npm install` again
**Port in Use**: Kill process on port 5173 or change in vite.config.ts

## 📞 Support

For API issues: Check `https://localhost:7192/swagger/index.html`
For frontend issues: Check browser console (F12)

## ✨ What's Next?

### Enhance Features
- [ ] Add employee performance tracking
- [ ] Implement leave management
- [ ] Add expense tracking
- [ ] Create reports and exports
- [ ] Add email notifications
- [ ] Mobile app version

### Improve Security
- [ ] Implement authentication
- [ ] Add role-based access
- [ ] Enable audit logging
- [ ] Add data encryption

### Optimize Performance
- [ ] Implement data pagination
- [ ] Add caching
- [ ] Lazy load components
- [ ] Optimize bundle size

## 📄 Additional Resources

- Backend API: `https://localhost:7192/swagger/index.html`
- Full Documentation: `HR_SYSTEM_README.md`
- Vue 3: `https://vuejs.org/`
- Element Plus: `https://element-plus.org/`

---

**🎉 Your HR Management System is ready to use!**

Start the dev server and navigate to `http://localhost:5173` to begin.
