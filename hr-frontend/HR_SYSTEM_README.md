# HR Management System UI

A comprehensive Human Resources Management System built with **Vue 3**, **TypeScript**, **Vite**, and **Element Plus** UI Library.

## 📋 Features

### 1. **Dashboard**
   - Quick statistics overview
   - Total staff count
   - Department distribution
   - Monthly payroll summary
   - Attendance statistics
   - Recent staff list

### 2. **Staff Management**
   - View all staff members
   - Create new staff records
   - Edit existing staff information
   - Delete staff members
   - Search by name, code, or email
   - Filter by department and status
   - Track staff details:
     - Staff code and name
     - Email and phone
     - Address and date of birth
     - Joined date
     - Department assignment
     - Manager assignment
     - Active/Inactive status

### 3. **Department Management**
   - View all departments
   - Create new departments
   - Edit department information
   - Delete departments
   - View staff count per department

### 4. **Payroll Management**
   - View payroll records by month/year
   - Create payroll entries
   - Filter by year, month, and staff
   - Track payroll details:
     - Staff information
     - Working days, present days, absent days, leave days
     - Gross salary and allowances
     - Deductions
     - Net salary calculation
   - View detailed payroll information

### 5. **Attendance Management**
   - Record attendance for staff
   - View attendance records
   - Filter by date, staff, and site
   - Add attendance logs (check-in/check-out)
   - Track:
     - Check-in and check-out times
     - Work sites
     - Distance traveled (optional GPS data)
   - View detailed attendance logs

## 🛠️ Technology Stack

- **Frontend Framework**: Vue 3 (Composition API)
- **Language**: TypeScript
- **Build Tool**: Vite
- **UI Library**: Element Plus
- **HTTP Client**: Axios
- **Routing**: Vue Router
- **Icons**: @element-plus/icons-vue

## 📁 Project Structure

```
src/
├── components/          # Reusable Vue components
├── views/              # Page components
│   ├── Dashboard.vue
│   ├── StaffManagement.vue
│   ├── DepartmentManagement.vue
│   ├── PayrollManagement.vue
│   └── AttendanceManagement.vue
├── services/           # API service layer
│   ├── apiClient.ts
│   ├── staffService.ts
│   ├── departmentService.ts
│   ├── payrollService.ts
│   ├── siteService.ts
│   └── attendanceService.ts
├── types/              # TypeScript type definitions
│   └── index.ts
├── router/             # Vue Router configuration
│   └── index.ts
├── App.vue             # Root application component
├── main.ts             # Application entry point
└── style.css           # Global styles
```

## 🚀 Getting Started

### Prerequisites
- Node.js 16+
- npm or yarn

### Installation

1. Navigate to the frontend directory:
```bash
cd C:\zinmyoswe\HRsystem\hr-frontend
```

2. Install dependencies:
```bash
npm install
```

### Development

Start the development server:
```bash
npm run dev
```

The application will be available at `http://localhost:5173` (or the port shown in terminal)

### Build for Production

```bash
npm run build
```

### Preview Production Build

```bash
npm run preview
```

## 🔌 API Integration

### Backend Configuration

The frontend connects to the backend API at:
```
https://localhost:7192/api
```

The API client is configured in `src/services/apiClient.ts` with:
- Base URL: `https://localhost:7192/api`
- Automatic request/response logging
- Error handling and interceptors
- SSL certificate validation disabled (development only)

### API Endpoints

#### Staff
- `GET /api/staffs` - Get all staff
- `GET /api/staffs/{id}` - Get single staff
- `POST /api/staffs` - Create staff
- `PUT /api/staffs/{id}` - Update staff
- `DELETE /api/staffs/{id}` - Delete staff

#### Departments
- `GET /api/departments` - Get all departments
- `GET /api/departments/{id}` - Get single department
- `POST /api/departments` - Create department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department

#### Payroll
- `GET /api/payrolls` - Get all payroll records
- `GET /api/payrolls/{id}` - Get single payroll
- `POST /api/payrolls` - Create payroll

#### Attendance
- `GET /api/attendances` - Get all attendance
- `GET /api/attendances/{id}` - Get single attendance
- `POST /api/attendances` - Create attendance
- `POST /api/attendances/{attendanceId}/logs` - Add attendance log

#### Sites
- `GET /api/sites` - Get all sites
- `GET /api/sites/{id}` - Get single site
- `POST /api/sites` - Create site

## 📝 Data Types

### Staff
```typescript
interface Staff {
  staffId: number;
  staffCode: string;
  staffName: string;
  email: string;
  phone?: string;
  address?: string;
  dateOfBirth?: Date | string;
  joinedDate: Date | string;
  departmentId?: number;
  positionId?: number;
  managerId?: number;
  isActive: boolean;
}
```

### Department
```typescript
interface Department {
  departmentId: number;
  departmentName: string;
}
```

### Site
```typescript
interface Site {
  siteId: number;
  siteName: string;
  siteType: string; // Office, Customer, Factory
  latitude?: number;
  longitude?: number;
}
```

### Attendance
```typescript
interface Attendance {
  attendanceId: number;
  staffId: number;
  workDate: Date | string;
  siteId?: number;
  logs?: AttendanceLog[];
}

interface AttendanceLog {
  logId: number;
  attendanceId: number;
  logType: string; // IN / OUT
  logTime: Date | string;
  distance?: number;
}
```

### Payroll
```typescript
interface Payroll {
  payrollId: number;
  staffId: number;
  year: number;
  month: number;
  workingDays: number;
  presentDays: number;
  absentDays: number;
  leaveDays: number;
  totalAllowance: number;
  totalDeduction: number;
  grossSalary: number;
  netSalary: number;
}
```

## 🎨 UI Design Features

- **Responsive Design**: Works on desktop, tablet, and mobile
- **Modern Layout**: Professional header, sidebar, and footer
- **Color Scheme**: Purple gradient header with dark sidebar
- **Element Plus Components**: 
  - Tables with sorting and filtering
  - Forms with validation
  - Dialogs for CRUD operations
  - Dropdowns and date pickers
  - Tags and status indicators
- **User Experience**:
  - Loading states
  - Success/error notifications
  - Confirmation dialogs
  - Search and filter functionality
  - Transition animations

## 🔧 Configuration

### Vite Config
The `vite.config.ts` includes:
- Vue plugin support
- Path alias for `@` imports
- HTTPS configuration for development

### TypeScript
- Strict mode enabled
- ES modules support
- Path aliases configured

## 📚 Service Layer

Each service module provides:
- Type-safe API calls
- Error handling
- Request/response logging
- Consistent interface

Example:
```typescript
import { staffService } from '@/services/staffService';

// Get all staff
const staff = await staffService.getAll();

// Create new staff
const newStaff = await staffService.create(staffData);

// Update staff
await staffService.update(staffId, updateData);

// Delete staff
await staffService.delete(staffId);
```

## 🚨 Error Handling

The application includes:
- Try-catch blocks for API calls
- User-friendly error messages
- API response error handling
- Console logging for debugging
- Loading states during async operations

## 🔐 Security Notes

- SSL certificate validation is disabled in development (`vite.config.ts`)
- For production, enable proper certificate validation
- Implement authentication/authorization
- Use environment variables for sensitive data
- Validate all user inputs on backend

## 🐛 Troubleshooting

### SSL Certificate Error
If you get SSL errors, ensure the backend is running with HTTPS at `https://localhost:7192`

### Module Not Found Errors
Clear node_modules and reinstall:
```bash
rm -r node_modules package-lock.json
npm install
```

### Port Already in Use
Change the Vite port in `vite.config.ts` or kill the process using port 5173

## 📦 Dependencies

- `vue`: ^3.5.30
- `vue-router`: ^4.x.x
- `axios`: ^1.x.x
- `element-plus`: ^2.13.6
- `@element-plus/icons-vue`: ^2.3.2
- `typescript`: ~5.9.3
- `vite`: ^8.0.1

## 📖 Additional Resources

- [Vue 3 Documentation](https://vuejs.org/)
- [Vite Documentation](https://vitejs.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/)
- [Element Plus Documentation](https://element-plus.org/)
- [Axios Documentation](https://axios-http.com/)
- [Vue Router Documentation](https://router.vuejs.org/)

## 🎯 Next Steps

1. Connect to the running .NET Core backend at `https://localhost:7192`
2. Test API endpoints in Swagger: `https://localhost:7192/swagger/index.html`
3. Create test data in the database
4. Test all CRUD operations in the UI
5. Add authentication/authorization
6. Implement additional features (reports, exports, etc.)

## 📄 License

This HR Management System is provided as-is for educational and business purposes.

---

**Built with ❤️ using Vue 3 + TypeScript + Element Plus**
