using CellReport_Workflow.Models.Sql;
using Microsoft.IdentityModel.Tokens;
using System.DirectoryServices;
using System.Runtime.ConstrainedExecution;

namespace CellReport_Workflow.Models.User
{
    public class User //: IUser
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Emp_Id { get; set; }
        public string? Department { get; set; }
        public string? Rank { get; set; }
        public string? PageEnAble { get; set; }

        public void BuildUserByAccount()
        {
            if (!string.IsNullOrEmpty(Account))
            {
                User user = new UserFactory().GetUserByAccount(Account);
                if (!string.IsNullOrEmpty(user.Emp_Id))
                {
                    Emp_Id = user.Emp_Id;
                    Account = user.Account;
                    Name = user.Name;
                    Department = user.Department;
                    Rank = user.Rank;
                }
            }
        }
        public void BuildUserByID()
        {
            if (!string.IsNullOrEmpty(Emp_Id))
            {
                User user = new UserFactory().GetUserByID(Emp_Id);
                if (!string.IsNullOrEmpty(user.Emp_Id))
                {
                    Emp_Id = user.Emp_Id;
                    Account = user.Account;
                    Name = user.Name;
                    Department = user.Department;
                    Rank = user.Rank;
                }
            }
        }
        public bool CheckPassword()
        {
            try
            {
                string domainAndUsername = "TPE0M001" + @"\" + Account;
                System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry("LDAP://babybanks.com/DC=babybanks,DC=com", domainAndUsername, Password);
                DirectorySearcher search = new DirectorySearcher(entry);
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (result != null)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
