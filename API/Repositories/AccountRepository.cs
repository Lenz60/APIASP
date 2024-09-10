using API.Context;
using API.Helper;
using API.Models;
using API.Repositories.Interfaces;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext _myContext;
        private readonly JWTHelper _jwtContext;
        private readonly BcryptHelper _bcryptContext;

        public AccountRepository(MyContext myContext, JWTHelper jwtContext, BcryptHelper bcryptHelper)
        {
            _myContext = myContext;
            _jwtContext = jwtContext;
            _bcryptContext = bcryptHelper;
        }

        public bool Login(Credentials credentials)
        {
            if (credentials.Username.Contains("@"))
            {
                var checkEmailExists = _myContext.Employees.Any(e => e.Email == credentials.Username);
                if (!checkEmailExists)
                {
                    throw new Exception($"Email {credentials.Username} is not found, Please register first");
                }

                var checkPass = CheckPass(credentials.Username,credentials.Password);
                return checkPass;

            }
            else
            {
                var checkUsernameExists = _myContext.Accounts.Any(a => a.Username == credentials.Username);
                if (!checkUsernameExists)
                {
                    throw new Exception($"Username {credentials.Username} is not found, Please register first");
                }
                else
                {
                    var checkPass = CheckPass(credentials.Username,credentials.Password);
                    return checkPass;

                }

            }
            throw new Exception("Password is incorrect");

        }

        private bool CheckPass(string username,string password)
        {
            //Verify the bcrypt hash
            var DBPass = _myContext.Accounts.Where(a => a.Username == username).Select(a => a.Password).FirstOrDefault();
            var verify = _bcryptContext.VerifyPassword(password, DBPass);
            return verify;
            //var checkPass = _myContext.Accounts.Where(a => a.Password == password).Count();
            //if (checkPass > 0)
            //{
            //    return true;
            //}
            //return false;
        }


        public int AddAccount(string firstName, string lastName, string email, string password, string dept_Id)
        {
            var baseUsername = firstName + "." + lastName;
            var username = GenerateUsername(baseUsername);
            var year = DateTime.Now.Year.ToString();
            var date = DateTime.Now.Date.ToString("dd");

            var newEmpId = GenerateNewEmpId();


            var employee = new EmployeeVM2
            {
                Employee_Id = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Dept_Id = dept_Id
            };



            var emailExists = _myContext.Employees.Any(e => e.Email == email);
            if (emailExists)
            {
                throw new Exception($"Email {email} already exists");
            }
            var DeptExists = _myContext.Departments.Any(d => d.Dept_Id == dept_Id);
            if (!DeptExists)
            {
                throw new Exception($"Department with ID : {dept_Id} does not exist");
            }

            //_myContext.SaveChanges();
            var newEmployee = new Employee(newEmpId, firstName, lastName, email, dept_Id);
            //_myContext.Employees.Add(employee);
            _myContext.Employees.Add(newEmployee);

            var hashPassword = _bcryptContext.HashPassword(password);
            var hashedPassword = "";
            if (hashPassword != null)
            {
                hashedPassword = hashPassword;
            }
            else
            {
                throw new Exception("Password failed to hash");
            }

            var newAccount = new Account(newEmpId, username, hashedPassword);
            _myContext.Accounts.Add(newAccount);
            return _myContext.SaveChanges();
        }

        private string GenerateUsername(string userName)
        {
            var baseUsername = userName.ToLower();
            var username = baseUsername;
            int count = 1;
            while (_myContext.Accounts.Any(a => a.Username == username))
            {
                if (username.Length > baseUsername.Length)
                {
                    username = username.Substring(0, username.Length - 3);
                }

                username = $"{username}{count.ToString("D3")}";
                count++;
            }
            return username;
        }

        private string GenerateNewEmpId()
        {
            var year = DateTime.Now.Year.ToString();
            var date = DateTime.Now.Date.ToString("dd");
            var countData = CountEmployee();
            string newEmpId = $"{year}{date}{(countData + 1).ToString("D4")}";

            // Ensure the newEmpId is unique
            var check = GetEmployeeEntityById(newEmpId);
            if (check != null)
            {
                string prefix = newEmpId.Substring(0, newEmpId.Length - 4);
                int numericPart = int.Parse(newEmpId.Substring(newEmpId.Length - 4));
                numericPart++;

                newEmpId = $"{year}{date}{prefix}{numericPart:D4}";
            }

            return newEmpId;
        }

        public IEnumerable<AccountDataVm> GetAccountData()
        {
            var result = _myContext.Employees.Include(a => a.Accounts).Include(d => d.Departments).Select(e => new AccountDataVm
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

        public Employee GetEmployeeEntityById(string employeeId)
        {
            return _myContext.Employees.Find(employeeId);
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

        public int DeleteAccount(string accountId)
        {
            var employee = GetEmployeeEntityById(accountId);
            if (employee == null)
            {
                throw new Exception($"Employee with ID : {accountId} does not exist");
            }
            _myContext.Employees.Remove(employee);
            return _myContext.SaveChanges();
        }

        public AccountDataVm GetAccountDataByCreds(string username)
        {
            if (username.Contains("@"))
            {
                var result = _myContext.Employees.Include(a => a.Accounts).Include(d => d.Departments).Select(e => new AccountDataVm
                {
                    Nik = e.Employee_Id,
                    Email = e.Email,
                    Username = e.Accounts.Username,
                    FullName = e.FirstName + " " + e.LastName,
                    DepartmentName = e.Departments.Dept_Name

                }
           ).Where(e => e.Email == username).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                return null;
            }
            else
            {
                var result = _myContext.Employees.Include(a => a.Accounts).Include(d => d.Departments).Select(e => new AccountDataVm
                {
                    Nik = e.Employee_Id,
                    Email = e.Email,
                    Username = e.Accounts.Username,
                    FullName = e.FirstName + " " + e.LastName,
                    DepartmentName = e.Departments.Dept_Name

                }
          ).Where(a => a.Username == username).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                return null;
            }

        }

        public string GenerateToken(CredsPayload payload)
        {
            return _jwtContext.GenerateToken(payload);

        }
    }
}
