namespace API.ViewModel
{
    public class AccountVM
    {
        public string Nik { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string Email { get; set; }
    }

    public class AccountDataVm
    {
        public string Nik { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
    }

    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
