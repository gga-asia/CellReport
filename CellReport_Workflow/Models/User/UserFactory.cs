using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Models.User
{
    public class UserFactory
    {
        SqlBase sqlBase = new();



        public User GetUserByAccount(string Account)
        {
            User user = new();
            user.Account = Account;
            string sql = "  SELECT U.emp_id, U.dep, U.[rank], U.dep_rank, E.ENG_NAME, E.CHI_NAME FROM cr_user U LEFT JOIN EMPLOYEE E ON U.emp_id = E.EMP_ID where U.account = @K_Account ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_Account", Account) };
            try
            {
                user = UserReader(user, Paras, sql);
                #region
                //using (SqlConnection con = new(sqlBase.connstr))
                //{
                //    con.Open();
                //    using (SqlCommand cmd = new(sql, con))
                //    {
                //        if (Paras != null)
                //            cmd.Parameters.AddRange(Paras.ToArray());
                //        SqlDataReader dr = cmd.ExecuteReader();
                //        user = UserReader(dr, user);
                //    }
                //    con.Close();
                //}
                #endregion
                return user;
            }
            catch (Exception ex)
            {
                return user;
            }
        }
        public User GetUserByID(string EMP_ID)
        {
            User user = new();
            user.Emp_Id = EMP_ID;
            string sql = "  SELECT U.emp_id, U.dep, U.[rank], U.dep_rank, E.ENG_NAME, E.CHI_NAME FROM cr_user U LEFT JOIN EMPLOYEE E ON U.emp_id = E.EMP_ID where U.emp_id = @K_emp_id ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_emp_id", EMP_ID) };
            try
            {
                user = UserReader(user, Paras, sql);
                #region
                //using (SqlConnection con = new(sqlBase.connstr))
                //{
                //    con.Open();
                //    using (SqlCommand cmd = new(sql, con))
                //    {
                //        if (Paras != null)
                //            cmd.Parameters.AddRange(Paras.ToArray());
                //        SqlDataReader dr = cmd.ExecuteReader();
                //        user = UserReader(dr, user);
                //    }
                //    con.Close();
                //}
                #endregion
                return user;
            }
            catch (Exception ex)
            {
                return user;
            }
        }

        public List<User> GetUserByDepartment(string Department, Boolean MANAGER = false)
        {
            List<User> user = new();
            string sql = "  SELECT U.emp_id, U.dep, U.[rank], U.dep_rank, E.ENG_NAME, E.CHI_NAME FROM cr_user U LEFT JOIN EMPLOYEE E ON U.emp_id = E.EMP_ID where U.dep = @K_dep ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_dep", Department) };
            try
            {
                user = Users_Reader(user, Paras, sql);
                return user;
            }
            catch (Exception ex)
            {
                return user;
            }
        }








        #region
        //public User UserReader(SqlDataReader dr, User user)
        //{
        //    try
        //    {
        //        if (dr.Read())
        //        {
        //            user.Account = !dr.IsDBNull(dr.GetOrdinal("ENG_NAME")) ? dr["ENG_NAME"].ToString() : "";
        //            user.Name = !dr.IsDBNull(dr.GetOrdinal("CHI_NAME")) ? dr["CHI_NAME"].ToString() : "";
        //            user.Emp_Id = !dr.IsDBNull(dr.GetOrdinal("emp_id")) ? dr["emp_id"].ToString() : "";
        //            user.Department = !dr.IsDBNull(dr.GetOrdinal("dep")) ? dr["dep"].ToString() : "";
        //            user.Rank = !dr.IsDBNull(dr.GetOrdinal("rank")) ? dr["rank"].ToString() : "";
        //        }
        //        dr.Close();
        //    }
        //    catch 
        //    {
        //        return user;
        //    }
        //    return user;
        //}
        #endregion
        public User GetLabManager(string LabName)
        {
            User user = new();
            string sql = $"SELECT TOP (1)  U.emp_id, U.dep, U.dep_rank, U.[rank], E.ENG_NAME, E.CHI_NAME FROM cr_user U LEFT JOIN EMPLOYEE E ON U.emp_id = E.EMP_ID where U.[dep_rank] = '{CDictionary.MANAGER}' AND U.dep = @K_dep ";
            //" [emp_id],[account],[dep],[rank] FROM cr_user WHERE [rank] = '{CDictionary.LAB_MANAGER}' AND dep = @K_dep ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_dep", LabName) };
            user = UserReader(user, Paras, sql);
            return user;
        }


        private User UserReader(User user, List<SqlParameter> Paras,string sql)
        {
            try
            {
                using (SqlConnection con = new(sqlBase.connstr))
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        if (Paras != null)
                            cmd.Parameters.AddRange(Paras.ToArray());
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            user.Account = !dr.IsDBNull(dr.GetOrdinal("ENG_NAME")) ? dr["ENG_NAME"].ToString().Trim() : "";
                            user.Name = !dr.IsDBNull(dr.GetOrdinal("CHI_NAME")) ? dr["CHI_NAME"].ToString().Trim() : "";
                            user.Emp_Id = !dr.IsDBNull(dr.GetOrdinal("emp_id")) ? dr["emp_id"].ToString().Trim() : "";
                            user.Department = !dr.IsDBNull(dr.GetOrdinal("dep")) ? dr["dep"].ToString().Trim() : "";
                            user.Department_Rank = !dr.IsDBNull(dr.GetOrdinal("dep_rank")) ? dr["dep_rank"].ToString().Trim() : "";
                            user.Rank = !dr.IsDBNull(dr.GetOrdinal("rank")) ? dr["rank"].ToString().Trim() : "";
                            if(!string.IsNullOrEmpty(user.Account) && !string.IsNullOrEmpty(user.Emp_Id))
                            {
                                if (user.Emp_Id.StartsWith("B"))
                                    user.Mail = user.Account + "@BionetCorp.com";
                                else if (user.Emp_Id.StartsWith("G"))
                                    user.Mail = user.Account + "@GGA.ASIA";
                            }
                        }
                        dr.Close();
                    }
                    con.Close();
                }
                return user;
            }
            catch (Exception ex)
            {
                return user;
            }
        }
        private List<User> Users_Reader(List<User> users, List<SqlParameter> Paras, string sql)
        {
            try
            {
                using (SqlConnection con = new(sqlBase.connstr))
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        if (Paras != null)
                            cmd.Parameters.AddRange(Paras.ToArray());
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            User x = new();
                            x.Account = !dr.IsDBNull(dr.GetOrdinal("ENG_NAME")) ? dr["ENG_NAME"].ToString().Trim() : "";
                            x.Name = !dr.IsDBNull(dr.GetOrdinal("CHI_NAME")) ? dr["CHI_NAME"].ToString().Trim() : "";
                            x.Emp_Id = !dr.IsDBNull(dr.GetOrdinal("emp_id")) ? dr["emp_id"].ToString().Trim() : "";
                            x.Department = !dr.IsDBNull(dr.GetOrdinal("dep")) ? dr["dep"].ToString().Trim() : "";
                            x.Department_Rank = !dr.IsDBNull(dr.GetOrdinal("dep_rank")) ? dr["dep_rank"].ToString().Trim() : "";
                            x.Rank = !dr.IsDBNull(dr.GetOrdinal("rank")) ? dr["rank"].ToString().Trim() : "";
                            if (!string.IsNullOrEmpty(x.Account) && !string.IsNullOrEmpty(x.Emp_Id))
                            {
                                if (x.Emp_Id.StartsWith("B"))
                                    x.Mail = x.Account + "@BionetCorp.com";
                                else if (x.Emp_Id.StartsWith("G"))
                                    x.Mail = x.Account + "@GGA.ASIA";
                            }
                            users.Add(x);
                        }
                        dr.Close();
                    }
                    con.Close();
                }
                return users;
            }
            catch (Exception ex)
            {
                return users;
            }
        }
    }
}

