using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Employee
    {
        [Key]
        public string Employee_Id { get; set; }

        //public Employee(string username, string firstName, string lastName, string email, string dept_Id)
        //{
        //    this.username = username;
        //    FirstName = firstName;
        //    LastName = lastName;
        //    Email = email;
        //    Dept_Id = dept_Id;
        //}

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public virtual Department? Departments { get; set; }
        [ForeignKey("Departments")]
        public string Dept_Id { get; set; }

       public virtual Account Accounts { get; set; }
        public Employee(string employee_Id, string firstName, string lastName, string email, string dept_Id)
        {
            Employee_Id = employee_Id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Dept_Id = dept_Id;
            
        }
        
    }
}
