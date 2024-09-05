using API.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    
    public class Department
    {
        [Key]
        public string? Dept_Id { get; set; }

        public string? Dept_Initial { get; set; }

        public string? Dept_Name { get; set; }

       
        //public Department(string dept_Initial, string dept_Name)
        //{
        //    Dept_Id = Ulid.NewUlid().ToString();
        //    Dept_Initial = dept_Initial;
        //    Dept_Name = dept_Name;
        //}

        public Department(string dept_Id, string dept_Initial, string dept_Name)
        {
            Dept_Id = dept_Id;
            Dept_Initial = dept_Initial;
            Dept_Name = dept_Name;
        }
    }

}
