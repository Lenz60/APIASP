using API.Context;
using API.Migrations;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext _myContext;

        public DepartmentRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        //public int AddDepartment(string deptId, string deptInitial, string deptName)
        //{
        //    //var countData = CountDepartment();
        //    //string newDeptId = $"D{(countData + 1).ToString("D3")}";
        //    //deptId = newDeptId;
        //    var newDepartment = new Department(deptId, deptInitial, deptName);

        //    _myContext.Departments.Add(newDepartment);
        //    return _myContext.SaveChanges();

        //    //return 0;
        //} 

        public int AddDepartment(string deptInitial, string deptName)
        {
            var countData = CountDepartment();
            string newDeptId = $"D{(countData + 1).ToString("D3")}";
            var check = GetDepartmentById(newDeptId);
            if(check != null)
            {
                countData = CountDepartment();
                newDeptId = $"D{(countData + 2).ToString("D3")}";
            }
            //deptId = newDeptId;

            var newDepartment = new Department(newDeptId, deptInitial, deptName);

            _myContext.Departments.Add(newDepartment);
            return _myContext.SaveChanges();

            //return 0;
        }

        public int DeleteDepartment(string deptId)
        {


            var selectedDepartment = GetDepartmentById(deptId);

            if (selectedDepartment != null)
            {
                _myContext.Departments.Remove(selectedDepartment);
                return _myContext.SaveChanges();
            }
            return 0;



        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _myContext.Departments.ToList();
        }

        public Department GetDepartmentById(string deptId)
        {
            var selectedDepartment = _myContext.Departments.Find(deptId);
            if (selectedDepartment != null)
            {
                return selectedDepartment;
            }
            return null;
        }

        public int UpdateDepartment(Department department)
        {
            var check = GetDepartmentById(department.Dept_Id);
            if (check != null)
            {
                _myContext.Entry(check).State = EntityState.Detached;
                _myContext.Entry(department).State = EntityState.Modified;
                return _myContext.SaveChanges();

            }
            return 0;
        }

        public int CountDepartment()
        {
            var count = _myContext.Departments.Count();
            if (count > 0)
            {
                return count;
            }
            return 0;
        }

        public Department GetLastInserted()
        {
            var lastInserted = _myContext.Departments.OrderByDescending(x => x.Dept_Id).FirstOrDefault();
            if (lastInserted != null)
            {
                return lastInserted;
            }
            return null;
        }


    }
}
