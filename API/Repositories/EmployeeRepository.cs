using API.Context;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext _myContext;

        public EmployeeRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public int AddEmployee(string firstName, string lastName, string deptId)
        {
            var countData = CountEmployee();
            var year = DateTime.Now.Year.ToString();
            var date = DateTime.Now.Date.ToString("dd");
            string newEmpId = $"{year}{date}{(countData + 1).ToString("D4")}";
            var check = GetEmployeeById(newEmpId);
            if (check != null)
            {
                countData = CountEmployee();
                newEmpId = $"{year}{date}{(countData + 2).ToString("D4")}";
            }
            //deptId = newDeptId;

            var newEmployee = new Employee(newEmpId, firstName, lastName, deptId);

            _myContext.Employees.Add(newEmployee);
            return _myContext.SaveChanges();
            
            //return countData;
        }

        public int CountEmployee()
        {
            var count = _myContext.Employees.Count();
            if (count > 0)
            {
                return count;
            }
            return 0;
        }
        public Employee GetEmployeeEntityById(string employeeId)
        {
            return _myContext.Employees.Find(employeeId);
        }

        public int  DeleteEmployee(string employeeId)
        {
            var selectedEmployee = GetEmployeeEntityById(employeeId);

            if (selectedEmployee != null)
            {
                //var Id = selectedEmployee.Employee_Id;
                _myContext.Employees.Remove(selectedEmployee);
                return _myContext.SaveChanges();
            }
            return 0;
        }

        public IEnumerable<IEmployeeRepository.EmployeeDto> GetAllEmployee()
        {
            return _myContext.Employees
       .Select(e => new IEmployeeRepository.EmployeeDto
       {
           Employee_Id = e.Employee_Id,
           FirstName = e.FirstName,
           LastName = e.LastName,
           Dept_Id = e.Dept_Id
       })
       .ToList();

        }

        public IEmployeeRepository.EmployeeDto GetEmployeeById(string employeeId)
        {
            var selectedEmployee = _myContext.Employees.Find(employeeId);
            if (selectedEmployee != null)
            {
                return new IEmployeeRepository.EmployeeDto
                {
                    Employee_Id = selectedEmployee.Employee_Id,
                    FirstName = selectedEmployee.FirstName,
                    LastName = selectedEmployee.LastName,
                    Dept_Id = selectedEmployee.Dept_Id
                };
            }
            return null;
        }

        public IEmployeeRepository.EmployeeDto GetLastInserted()
        {
            var lastInserted = _myContext.Employees.OrderByDescending(x => x.Employee_Id).FirstOrDefault();
            if (lastInserted != null)
            {
                //return lastInserted;

                return new IEmployeeRepository.EmployeeDto
                {
                    Employee_Id = lastInserted.Employee_Id,
                    FirstName = lastInserted.FirstName,
                    LastName = lastInserted.LastName,
                    Dept_Id = lastInserted.Dept_Id
                };
            }
            return null;
        }

        public int UpdateEmployee(Employee employee)
        {
            var check = GetEmployeeEntityById(employee.Employee_Id);
            if (check != null)
            {
                _myContext.Entry(check).State = EntityState.Detached;
                _myContext.Entry(employee).State = EntityState.Modified;
                return _myContext.SaveChanges();

            }
            return 0;
        }

        public class EmployeeDto
        {
            public string Employee_Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Dept_Id { get; set; }
        }

    }
}
