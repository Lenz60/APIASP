using API.Models;
using API.ViewModel;

namespace API.Repositories.Interfaces
{
    public interface IRegisterRepository
    {
        int Register(string firstName, string lastName, string email, string dept_Id);
        int CountEmployee();
        int CheckSameUsername(string userName);

        Account GetEmployeeFromId(string userName);
        AccountVM GetLastInserted();

    }
}
