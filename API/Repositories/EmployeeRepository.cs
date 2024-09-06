using API.Context;
using API.Models;
using API.Repositories.Interfaces;
using API.ViewModel;
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

        public int AddEmployee(string firstName, string lastName, string email, string dept_Id)
        {
            
            var baseUsername = firstName + lastName;
            var username = CheckSameUsername(baseUsername);
            var year = DateTime.Now.Year.ToString();
            var date = DateTime.Now.Date.ToString("dd");

            var newEmpId = GenerateNewEmpId();


            var employee = new EmployeeCreateVM
            {
                Employee_Id = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Dept_Id = dept_Id
            };
            var newEmployee = new Employee(newEmpId, firstName, lastName, email, dept_Id);
            //_myContext.Employees.Add(employee);
            _myContext.Employees.Add(newEmployee);
            //_myContext.SaveChanges();

            var newAccount = new Account(newEmpId, username, "12345");
            _myContext.Accounts.Add(newAccount);
            return _myContext.SaveChanges();
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

        public int DeleteEmployee(string employeeId)
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

        
        public IEnumerable<EmployeeVM> EmployeeData()
        {

            //return _myContext.Employees.ToList();
            return _myContext.Employees
                .Include(s => s.Departments) // Ensure 'Department' is the correct navigation property
                .Select(s => new EmployeeVM
                {
                    Employee_Id = s.Employee_Id,
                    FullName = s.FirstName + "" + s.LastName,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Dept_Name = s.Departments.Dept_Name // Ensure 'Dept_Name' is the correct property
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

        public string CheckSameUsername(string baseUsername)
        {

            string newUsername = baseUsername;
            int maxSuffix = -1;

            // Get all usernames that start with the baseUsername
            var similarUsernames = _myContext.Accounts
                .Where(a => a.Username.StartsWith(baseUsername + "."))
                .Select(a => a.Username)
                .ToList();

            foreach (var username in similarUsernames)
            {
                // Ensure that the suffix is numeric and directly follows the baseUsername
                if (username.Length > baseUsername.Length)
                {
                    var suffixStr = username.Substring(baseUsername.Length + 1);
                    if (int.TryParse(suffixStr, out int suffix))
                    {
                        if (suffix > maxSuffix)
                        {
                            maxSuffix = suffix;
                        }
                    }
                }
            }

            // Increment the max suffix found
            maxSuffix++;
            newUsername = $"{baseUsername}.{maxSuffix.ToString("D3")}";

            return newUsername;
        }

        public AccountVM GetLastInsertedAccount()
        {
            var lastInserted = _myContext.Employees
                .Include(d => d.Departments)
                .OrderByDescending(x => x.Employee_Id).FirstOrDefault();
            if (lastInserted != null)
            {
                //return lastInserted;

                return new AccountVM
                {
                    Nik = lastInserted.Employee_Id,
                    FullName = $"{lastInserted.FirstName} {lastInserted.LastName}",
                    DepartmentName = lastInserted.Departments.Dept_Name, // Assuming Department has a Name property
                    Email = lastInserted.Email,

                };
            }
            return null;
        }

        public IEnumerable<Employee> GetAllOfEmployeeData()
        {
            return _myContext.Employees.Include(a => a.Accounts).ToList();
        }

        public string GenerateNewEmpId()
        {
            var year = DateTime.Now.Year.ToString();
            var date = DateTime.Now.Date.ToString("dd");
            var countData = CountEmployee();
            string newEmpId = $"{year}{date}{(countData + 1).ToString("D4")}";

            // Ensure the newEmpId is unique
            var check = GetEmployeeEntityById(newEmpId);
            if (check != null)
            {
                newEmpId = $"{year}{date}{(countData + 2).ToString("D4")}";
            }

            return newEmpId;
        }

        public IEnumerable<IEmployeeRepository.EmployeeDto> GetAllEmployee()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmployeeDataVM> EmployeeVMData2()
        {
            var result = _myContext.Employees.Include(a => a.Accounts).Include(d => d.Departments).Select(e => new EmployeeDataVM
            {
                Nik = e.Employee_Id,
                Email = e.Email,
                Username = e.Accounts.Username,
                FullName = e.FirstName + " " + e.LastName,
                DepartmentName = e.Departments.Dept_Name

            }
            ).ToList();
            if (result != null)
            {
                return result;
            }
            return null;
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
