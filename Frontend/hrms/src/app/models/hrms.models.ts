export interface Employee {
  id?: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  position: string;
  departmentId: number;
  deptName?: string;
  accountNumber: string;
  isActive: boolean;
  address: string;
}

export interface Salary {
  id?: number;
  employeeId: number;
  baseSalary: number;
  medicalAllowance: number;
  conveyanceAllowance: number;
  otherBonuses: number;
  deductions: number;
}

export interface Payroll {
  id?: number;
  employeeName?: string;
  month: number;
  year: number;
  grossSalary: number;
  taxDeduction: number;
  netSalary: number;
  status: string;
}