using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface ISystemAccountRepo
    {
        SystemAccount? GetAccountByEmail(string email);
        List<SystemAccount> GetAccounts();
        SystemAccount? GetAccountById(int id);
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(int id);
    }
}