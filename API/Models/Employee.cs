using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Employee
    {
        [Key]
        public string Employee_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Department? Departments { get; set; }
        [ForeignKey("Departments")]
        public string Dept_Id { get; set; }

        public Employee(string employee_Id, string firstName, string lastName, string dept_Id)
        {
            Employee_Id = employee_Id;
            FirstName = firstName;
            LastName = lastName;
            Dept_Id = dept_Id;
        }
    }
}
