using BusinessObjects;
using DataAccessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public class SystemAccountRepo : ISystemAccountRepo
    {
        public SystemAccount? GetAccountByEmail(string email) => SystemAccountDAO.Instance.GetAccountByEmail(email);
        public List<SystemAccount> GetAccounts() => SystemAccountDAO.Instance.GetAccounts();
        public SystemAccount? GetAccountById(int id) => SystemAccountDAO.Instance.GetAccountById(id);
        public void AddAccount(SystemAccount account) => SystemAccountDAO.Instance.AddAccount(account);
        public void UpdateAccount(SystemAccount account) => SystemAccountDAO.Instance.UpdateAccount(account);
        public void DeleteAccount(int id) => SystemAccountDAO.Instance.DeleteAccount(id);
    }
}