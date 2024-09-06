using API.Context;
using API.Models;
using API.Repositories.Interfaces;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AccountRepository : IRegisterRepository
    {
        private readonly MyContext _myContext;

        public AccountRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public int Register(string firstName, string lastName, string email, string dept_Id)
        {
            var username = firstName + lastName;
            var tempUsername = $"{firstName + lastName}001";
            var check = GetEmployeeFromId(username);
            var countData = CountEmployee();
            var year = DateTime.Now.Year.ToString();
            var date = DateTime.Now.Date.ToString("dd");
            // check if employee exists
            //if (check != null)
            //{
            //    //if Employee exists
            //    username = $"{username}.{(countData + 2).ToString("D3")}";
            //    var checkDuplicate = CheckSameUsername(username);
            //    if (checkDuplicate > 0)
            //    {
            //        //username = $"{username.ToString("D3")}";
            //        username = $"{username}.{(countData + 1).ToString("D3")}";
            //    }
            //}

            string newEmpId = $"{year}{date}{(countData + 1).ToString("D4")}";
            if (check != null)
            {
                newEmpId = $"{year}{date}{(countData + 2).ToString("D4")}";
            }
            var checkDuplicate = CheckSameUsername(username);
            if (checkDuplicate > 0)
            {
                username = $"{username}.{(countData + 2).ToString("D3")}";
            }
            username = $"{username}.{(countData + 1).ToString("D3")}";

            var employee = new EmployeeCreateVM
            {
                Employee_Id = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Dept_Id = dept_Id
            };
            var newEmployee = new Employee(username, firstName, lastName, email, dept_Id);
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

      

        public int CheckSameUsername(string userName)
        {
            var count = _myContext.Employees.Where(e => _myContext.Accounts.Any(a => a.Username == userName)).Count();
            if (count > 0)
            {
                return count;
            }
            return 0;
        }

        public AccountVM GetLastInserted()
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

        public Account GetEmployeeFromId(string userName)
        {
            var account = _myContext.Accounts.Find(userName);
            if (account != null) { return account; }
            return null;
        }
    }
}
