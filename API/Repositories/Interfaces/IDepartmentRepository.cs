using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAllDepartments();
        Department GetDepartmentById(string deptId);
        int AddDepartment(string dept_Initial, string dept_Name);
        int UpdateDepartment(Department department);
        int DeleteDepartment(string deptId);
        int CountDepartment();

        Department GetLastInserted();
    }
}
