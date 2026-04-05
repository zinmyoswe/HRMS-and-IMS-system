/*
SQL Server DDL script for HRSystem (based on models in HRSystembackend/Models).
Run on: LAPTOP-12T4N1BP\SQLEXPRESS
Authentication: Windows Authentication (Integrated Security)
Database: HRSystem
- Creates tables, constraints, indexes and seed data.
- Money columns use decimal(12,2).
- Date/time uses datetime2 and UTC defaults.
*/

SET NOCOUNT ON;
GO

-- Create database if not exists and use it
IF DB_ID(N'HRSystem') IS NULL
BEGIN
    CREATE DATABASE [HRSystem];
END
GO

USE [HRSystem];
GO

-- Departments
IF OBJECT_ID('dbo.Departments','U') IS NULL
BEGIN
CREATE TABLE dbo.Departments
(
    DepartmentId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DepartmentName NVARCHAR(200) NOT NULL
);
END
GO

-- Positions
IF OBJECT_ID('dbo.Positions','U') IS NULL
BEGIN
CREATE TABLE dbo.Positions
(
    PositionId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PositionName NVARCHAR(200) NOT NULL
);
END
GO

-- Roles
IF OBJECT_ID('dbo.Roles','U') IS NULL
BEGIN
CREATE TABLE dbo.Roles
(
    RoleId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL
);
END
GO

-- Sites
IF OBJECT_ID('dbo.Sites','U') IS NULL
BEGIN
CREATE TABLE dbo.Sites
(
    SiteId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    SiteName NVARCHAR(200) NOT NULL,
    SiteType NVARCHAR(50) NOT NULL,
    Latitude DECIMAL(9,6) NULL,
    Longitude DECIMAL(9,6) NULL
);
END
GO

-- Staff
IF OBJECT_ID('dbo.Staff','U') IS NULL
BEGIN
CREATE TABLE dbo.Staff
(
    StaffId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StaffCode NVARCHAR(32) NOT NULL,
    StaffName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    Phone NVARCHAR(50) NULL,
    Address NVARCHAR(500) NULL,
    DateOfBirth DATE NULL,
    JoinedDate DATE NOT NULL,
    DepartmentId INT NULL,
    PositionId INT NULL,
    ManagerId INT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Staff_IsActive DEFAULT (1),
    CreatedDate DATETIME2(7) NOT NULL CONSTRAINT DF_Staff_CreatedDate DEFAULT (SYSUTCDATETIME()),
    UpdatedDate DATETIME2(7) NOT NULL CONSTRAINT DF_Staff_UpdatedDate DEFAULT (SYSUTCDATETIME())
);
-- Unique indexes
CREATE UNIQUE INDEX UX_Staff_StaffCode ON dbo.Staff(StaffCode);
CREATE UNIQUE INDEX UX_Staff_Email ON dbo.Staff(Email);

-- Foreign keys
ALTER TABLE dbo.Staff
    ADD CONSTRAINT FK_Staff_Department FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments(DepartmentId) ON DELETE NO ACTION;

ALTER TABLE dbo.Staff
    ADD CONSTRAINT FK_Staff_Position FOREIGN KEY (PositionId) REFERENCES dbo.Positions(PositionId) ON DELETE NO ACTION;

ALTER TABLE dbo.Staff
    ADD CONSTRAINT FK_Staff_Manager FOREIGN KEY (ManagerId) REFERENCES dbo.Staff(StaffId) ON DELETE NO ACTION;
END
GO

-- StaffRoles junction table (many-to-many)
IF OBJECT_ID('dbo.StaffRoles','U') IS NULL
BEGIN
CREATE TABLE dbo.StaffRoles
(
    StaffId INT NOT NULL,
    RoleId INT NOT NULL,
    CONSTRAINT PK_StaffRoles PRIMARY KEY (StaffId, RoleId)
);

ALTER TABLE dbo.StaffRoles
    ADD CONSTRAINT FK_StaffRoles_Staff FOREIGN KEY (StaffId) REFERENCES dbo.Staff(StaffId) ON DELETE CASCADE;

ALTER TABLE dbo.StaffRoles
    ADD CONSTRAINT FK_StaffRoles_Role FOREIGN KEY (RoleId) REFERENCES dbo.Roles(RoleId) ON DELETE CASCADE;
END
GO

-- Attendance (per staff per day)
IF OBJECT_ID('dbo.Attendances','U') IS NULL
BEGIN
CREATE TABLE dbo.Attendances
(
    AttendanceId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StaffId INT NOT NULL,
    WorkDate DATE NOT NULL,
    SiteId INT NULL
);
CREATE INDEX IX_Attendance_Staff_WorkDate ON dbo.Attendances(StaffId, WorkDate);

ALTER TABLE dbo.Attendances
    ADD CONSTRAINT FK_Attendance_Staff FOREIGN KEY (StaffId) REFERENCES dbo.Staff(StaffId) ON DELETE NO ACTION;

ALTER TABLE dbo.Attendances
    ADD CONSTRAINT FK_Attendance_Site FOREIGN KEY (SiteId) REFERENCES dbo.Sites(SiteId) ON DELETE NO ACTION;
END
GO

-- AttendanceLogs (multiple IN/OUT per attendance)
IF OBJECT_ID('dbo.AttendanceLogs','U') IS NULL
BEGIN
CREATE TABLE dbo.AttendanceLogs
(
    LogId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    AttendanceId INT NOT NULL,
    LogType NVARCHAR(10) NOT NULL, -- 'IN' / 'OUT'
    LogTime DATETIME2(7) NOT NULL,
    Distance DECIMAL(9,2) NULL
);

CREATE INDEX IX_AttendanceLogs_AttendanceId ON dbo.AttendanceLogs(AttendanceId);

ALTER TABLE dbo.AttendanceLogs
    ADD CONSTRAINT FK_AttendanceLogs_Attendance FOREIGN KEY (AttendanceId) REFERENCES dbo.Attendances(AttendanceId) ON DELETE CASCADE;
END
GO

-- SalaryStructure (configuration)
IF OBJECT_ID('dbo.SalaryStructures','U') IS NULL
BEGIN
CREATE TABLE dbo.SalaryStructures
(
    SalaryStructureId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StaffId INT NOT NULL,
    BasicSalary DECIMAL(12,2) NOT NULL,
    PhoneAllowance DECIMAL(12,2) NOT NULL,
    MealAllowance DECIMAL(12,2) NOT NULL,
    HouseAllowance DECIMAL(12,2) NOT NULL,
    TaxRate DECIMAL(5,2) NOT NULL, -- percentage like 3.00 or 5.00
    SocialSecurity DECIMAL(12,2) NOT NULL,
    EffectiveDate DATE NOT NULL
);

CREATE INDEX IX_SalaryStructures_StaffId ON dbo.SalaryStructures(StaffId);

ALTER TABLE dbo.SalaryStructures
    ADD CONSTRAINT FK_SalaryStructure_Staff FOREIGN KEY (StaffId) REFERENCES dbo.Staff(StaffId) ON DELETE CASCADE;
END
GO

-- Payroll (monthly result)
IF OBJECT_ID('dbo.Payrolls','U') IS NULL
BEGIN
CREATE TABLE dbo.Payrolls
(
    PayrollId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StaffId INT NOT NULL,
    [Year] INT NOT NULL,
    [Month] INT NOT NULL,
    WorkingDays INT NOT NULL,
    PresentDays INT NOT NULL,
    AbsentDays INT NOT NULL,
    LeaveDays INT NOT NULL,
    TotalAllowance DECIMAL(12,2) NOT NULL,
    TotalDeduction DECIMAL(12,2) NOT NULL,
    GrossSalary DECIMAL(12,2) NOT NULL,
    NetSalary DECIMAL(12,2) NOT NULL
);

CREATE INDEX IX_Payroll_Staff_Year_Month ON dbo.Payrolls(StaffId, [Year], [Month]);

ALTER TABLE dbo.Payrolls
    ADD CONSTRAINT FK_Payrolls_Staff FOREIGN KEY (StaffId) REFERENCES dbo.Staff(StaffId) ON DELETE CASCADE;
END
GO

-- AllowanceType
IF OBJECT_ID('dbo.AllowanceTypes','U') IS NULL
BEGIN
CREATE TABLE dbo.AllowanceTypes
(
    AllowanceTypeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL
);
END
GO

-- StaffAllowance (dynamic allowances per staff)
IF OBJECT_ID('dbo.StaffAllowances','U') IS NULL
BEGIN
CREATE TABLE dbo.StaffAllowances
(
    StaffAllowanceId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StaffId INT NOT NULL,
    AllowanceTypeId INT NOT NULL,
    Amount DECIMAL(12,2) NOT NULL,
    EffectiveDate DATE NOT NULL
);

CREATE INDEX IX_StaffAllowances_StaffId ON dbo.StaffAllowances(StaffId);

ALTER TABLE dbo.StaffAllowances
    ADD CONSTRAINT FK_StaffAllowances_Staff FOREIGN KEY (StaffId) REFERENCES dbo.Staff(StaffId) ON DELETE CASCADE;

ALTER TABLE dbo.StaffAllowances
    ADD CONSTRAINT FK_StaffAllowances_AllowanceType FOREIGN KEY (AllowanceTypeId) REFERENCES dbo.AllowanceTypes(AllowanceTypeId) ON DELETE NO ACTION;
END
GO

-- DeductionType
IF OBJECT_ID('dbo.DeductionTypes','U') IS NULL
BEGIN
CREATE TABLE dbo.DeductionTypes
(
    DeductionTypeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL
);
END
GO

-- Helpful FK indexes
IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.Staff')) 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Staff') AND name = 'IX_Staff_DepartmentId')
        CREATE INDEX IX_Staff_DepartmentId ON dbo.Staff(DepartmentId);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Staff') AND name = 'IX_Staff_PositionId')
        CREATE INDEX IX_Staff_PositionId ON dbo.Staff(PositionId);
END
GO

-- Seed sample data (IDs chosen to match EF seed)
-- Departments
IF NOT EXISTS (SELECT 1 FROM dbo.Departments WHERE DepartmentId = 1)
    INSERT INTO dbo.Departments(DepartmentId, DepartmentName) VALUES (1, N'Human Resources');
IF NOT EXISTS (SELECT 1 FROM dbo.Departments WHERE DepartmentId = 2)
    INSERT INTO dbo.Departments(DepartmentId, DepartmentName) VALUES (2, N'Engineering');

-- Positions
IF NOT EXISTS (SELECT 1 FROM dbo.Positions WHERE PositionId = 1)
    INSERT INTO dbo.Positions(PositionId, PositionName) VALUES (1, N'Software Engineer');
IF NOT EXISTS (SELECT 1 FROM dbo.Positions WHERE PositionId = 2)
    INSERT INTO dbo.Positions(PositionId, PositionName) VALUES (2, N'HR Specialist');

-- Roles
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 1) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (1, N'Senior');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 2) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (2, N'Middle');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 3) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (3, N'Junior');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 4) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (4, N'Intern');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 5) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (5, N'Manager');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 6) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (6, N'Senior Manager');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 7) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (7, N'Director');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleId = 8) INSERT INTO dbo.Roles(RoleId, RoleName) VALUES (8, N'Admin');

-- Sites
IF NOT EXISTS (SELECT 1 FROM dbo.Sites WHERE SiteId = 1) INSERT INTO dbo.Sites(SiteId, SiteName, SiteType) VALUES (1, N'Head Office', N'Office');
IF NOT EXISTS (SELECT 1 FROM dbo.Sites WHERE SiteId = 2) INSERT INTO dbo.Sites(SiteId, SiteName, SiteType) VALUES (2, N'Factory A', N'Factory');

-- Staff (seed one staff)
IF NOT EXISTS (SELECT 1 FROM dbo.Staff WHERE StaffId = 1)
BEGIN
    INSERT INTO dbo.Staff(StaffId, StaffCode, StaffName, Email, Phone, Address, DateOfBirth, JoinedDate, DepartmentId, PositionId, ManagerId, IsActive, CreatedDate, UpdatedDate)
    VALUES (1, N'STF001', N'Alice Johnson', N'alice.johnson@example.com', N'+1234567890', N'123 Main St', '1990-01-01', '2020-06-01', 2, 1, NULL, 1, SYSUTCDATETIME(), SYSUTCDATETIME());
END

-- StaffRoles for the seeded staff (StaffId 1 assigned RoleId 1 and 8)
IF NOT EXISTS (SELECT 1 FROM dbo.StaffRoles WHERE StaffId = 1 AND RoleId = 1)
    INSERT INTO dbo.StaffRoles(StaffId, RoleId) VALUES (1, 1);
IF NOT EXISTS (SELECT 1 FROM dbo.StaffRoles WHERE StaffId = 1 AND RoleId = 8)
    INSERT INTO dbo.StaffRoles(StaffId, RoleId) VALUES (1, 8);

-- SalaryStructure seed
IF NOT EXISTS (SELECT 1 FROM dbo.SalaryStructures WHERE SalaryStructureId = 1)
BEGIN
    INSERT INTO dbo.SalaryStructures(SalaryStructureId, StaffId, BasicSalary, PhoneAllowance, MealAllowance, HouseAllowance, TaxRate, SocialSecurity, EffectiveDate)
    VALUES (1, 1, 8000.00, 50.00, 150.00, 500.00, 3.00, 200.00, CONVERT(date, SYSUTCDATETIME()));
END

GO