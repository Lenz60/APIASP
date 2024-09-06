using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace API.Models
{
    public class Account
    {
        private string newEmpId;
        private string v;

        public Account() { }
        public Account(string accountId, string username, string password)
        {
            Account_Id = accountId;
            Username = username;
            Password = password;
        }


        //public Account(string newEmpId, string username, string password)
        //{
        //    newEmpId = newEmpId;
        //    Username = username;
        //    password = Password;
        //}

        [Key, ForeignKey("Employees")]
        public string Account_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual Employee? Employees { get; set; }
        

        //public Account(string account_Id, string username, string password, Employee? employees, string employee_Id)
        //{
        //    Account_Id = account_Id;
        //    Username = username;
        //    Password = password;
        //    Employees = employees;
        //    Employee_Id = employee_Id;
        //}

    }
}
