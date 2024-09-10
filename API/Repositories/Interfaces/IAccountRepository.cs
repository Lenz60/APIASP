using API.Models;
using API.ViewModel;

namespace API.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        bool Login(Credentials credentials);
        IEnumerable<AccountDataVm> GetAccountData();
        int AddAccount(string firstName, string lastName, string email,string password, string dept_Id);

        int DeleteAccount(string accountId);
        //string GenerateUsername(string userName);
        //string GenerateNewEmpId();
        Employee GetEmployeeEntityById(string employeeId);
        int CountEmployee();
        AccountVM GetLastInsertedAccount();

       string GenerateToken(CredsPayload payload);

        AccountDataVm GetAccountDataByCreds(string username);

    }
}
