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
            string sql = "  SELECT U.emp_id, U.dep, U.[rank], E.ENG_NAME, E.CHI_NAME FROM cr_user U LEFT JOIN EMPLOYEE E ON U.emp_id = E.EMP_ID where U.account = @K_Account ";
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
            string sql = "  SELECT U.emp_id, U.dep, U.[rank], E.ENG_NAME, E.CHI_NAME FROM cr_user U LEFT JOIN EMPLOYEE E ON U.emp_id = E.EMP_ID where U.emp_id = @K_emp_id ";
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
            string sql = $"SELECT TOP (1)  U.emp_id, U.dep, U.[rank], E.ENG_NAME, E.CHI_NAME FROM cr_user U LEFT JOIN EMPLOYEE E ON U.emp_id = E.EMP_ID where U.[rank] = '{CDictionary.LAB_MANAGER}' AND U.dep = @K_dep ";
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
                            user.Rank = !dr.IsDBNull(dr.GetOrdinal("rank")) ? dr["rank"].ToString().Trim() : "";
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
    }
}

