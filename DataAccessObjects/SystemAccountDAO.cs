using BusinessObjects;
using DataAccessObjects.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? instance = null;
        private static readonly object instanceLock = new object();

        private SystemAccountDAO() { }

        public static SystemAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountDAO();
                    }
                    return instance;
                }
            }
        }

        // Hàm dùng cho chức năng Login
        public SystemAccount? GetAccountByEmail(string email)
        {
            using var context = new FunewsManagementContext();
            return context.SystemAccounts.SingleOrDefault(a => a.AccountEmail == email);
        }

        // --- Bổ sung các hàm CRUD để Admin quản lý tài khoản ---

        public List<SystemAccount> GetAccounts()
        {
            using var context = new FunewsManagementContext();
            return context.SystemAccounts.ToList();
        }

        public SystemAccount? GetAccountById(int id)
        {
            using var context = new FunewsManagementContext();
            return context.SystemAccounts.SingleOrDefault(a => a.AccountId == id);
        }

        public void AddAccount(SystemAccount account)
        {
            using var context = new FunewsManagementContext();
            context.SystemAccounts.Add(account);
            context.SaveChanges();
        }

        public void UpdateAccount(SystemAccount account)
        {
            using var context = new FunewsManagementContext();
            context.SystemAccounts.Update(account);
            context.SaveChanges();
        }

        public void DeleteAccount(int id)
        {
            using var context = new FunewsManagementContext();
            var account = context.SystemAccounts.SingleOrDefault(a => a.AccountId == id);
            if (account != null)
            {
                context.SystemAccounts.Remove(account);
                context.SaveChanges();
            }
        }
    }
}