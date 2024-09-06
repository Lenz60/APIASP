using API.Models;
using API.ViewModel;

namespace API.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<EmployeeDto> GetAllEmployee();
        IEnumerable<EmployeeVM> EmployeeData();
        //IEnumerable<Employee> GetAllEmployee();
        EmployeeDto GetEmployeeById(string employeeId);
        int AddEmployee(string firstName, string lastName, string deptId);
        int UpdateEmployee(Employee employee);
        int DeleteEmployee(string employeeId);
        Employee GetEmployeeEntityById(string employeeId);
        int CountEmployee();

        EmployeeDto GetLastInserted();

        public class EmployeeDto
        {
            public string Employee_Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Dept_Id { get; set; }
        }

    }
}
