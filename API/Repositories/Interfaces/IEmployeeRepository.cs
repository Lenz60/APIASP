using API.Models;
using API.ViewModel;

namespace API.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<EmployeeDto> GetAllEmployee();
        IEnumerable<EmployeeVM> EmployeeData();
        IEnumerable<EmployeeDataVM> EmployeeVMData2();
        IEnumerable<Employee> GetAllOfEmployeeData();
        //IEnumerable<Employee> GetAllEmployee();
        EmployeeDto GetEmployeeById(string employeeId);
        int AddEmployee(string firstName, string lastName, string email, string dept_Id);
        int UpdateEmployee(Employee employee);
        int DeleteEmployee(string employeeId);
        Employee GetEmployeeEntityById(string employeeId);
        int CountEmployee();

        string CheckSameUsername(string userName);
        string GenerateNewEmpId();

        string checkExists(string value, string context);

        AccountVM GetLastInsertedAccount();

        EmployeeDto GetLastInserted();

        public class EmployeeDto
        {
            public string Employee_Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Dept_Id { get; set; }
        }

    }
}
