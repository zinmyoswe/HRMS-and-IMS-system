export interface Staff {
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

export interface Department {
  departmentId: number;
  departmentName: string;
}

export interface Position {
  positionId: number;
  positionName: string;
}

export interface Site {
  siteId: number;
  siteName: string;
  siteType: string;
  latitude?: number;
  longitude?: number;
}

export interface AttendanceLog {
  logId: number;
  attendanceId: number;
  logType: string;
  logTime: Date | string;
  distance?: number;
}

export interface Attendance {
  attendanceId: number;
  staffId: number;
  workDate: Date | string;
  siteId?: number;
  logs?: AttendanceLog[];
}

export interface Payroll {
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

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
}

