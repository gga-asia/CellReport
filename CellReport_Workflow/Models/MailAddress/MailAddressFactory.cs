using CellReport_Workflow.Models.Sql;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace CellReport_Workflow.Models.MailAddress
{
    public class MailAddressFactory
    {
        //private readonly ISql _sqlBase;
        //public MailAddressFactory(ISql sqlBase)
        //{
        //    _sqlBase = sqlBase;

        //}
        SqlBase sqlBase = new();
        public List<MailAddress> GETGroupAddress(string Group)
        {
            List<MailAddress> datas = new();
            string sql = " SELECT G.EMP_ID, E.ENG_NAME ";
            sql += " FROM GROUPEMP G ";
            sql += " LEFT JOIN EMPLOYEE E ON G.EMP_ID = E.EMP_ID ";
            sql += " WHERE G.P_ID = @K_Group ";
            List<SqlParameter> Paras = new(){ new("K_Group", Group)};
            datas = Users_Reader(datas, Paras, sql);
            return datas;
        }
        public string GetEmailByGroup(string Group)
        {
            List<MailAddress> datas = GETGroupAddress(Group);
            string AddressString = "";
            for (int i = 0; i < datas.Count; i++)
                AddressString += $"{datas[i].Mail};";   
            return AddressString;
        }
        public List<MailAddress> SearchUserFromEMPLOYEE(string QueryString)
        {
            List<MailAddress> datas = new();
            string sql = " SELECT EMP_ID, ENG_NAME ";
            sql += " FROM EMPLOYEE ";
            sql += " WHERE EMP_ID like @K_QueryString OR ENG_NAME like @K_QueryString ";
            List<SqlParameter> Paras = new() { new("K_QueryString", $"%{QueryString}%") };
            datas = Users_Reader(datas, Paras, sql);
            return datas;
        }



        private List<MailAddress> Users_Reader(List<MailAddress> users, List<SqlParameter> Paras, string sql)
        {
            try
            {
                using (SqlConnection con = sqlBase.CONN_Builder())
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        if (Paras != null)
                            cmd.Parameters.AddRange(Paras.ToArray());
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            MailAddress x = new();
                            x.Account = !dr.IsDBNull(dr.GetOrdinal("ENG_NAME")) ? dr["ENG_NAME"].ToString().Trim() : "";
                            x.Emp_ID = !dr.IsDBNull(dr.GetOrdinal("EMP_ID")) ? dr["EMP_ID"].ToString().Trim() : "";
                            if (!string.IsNullOrEmpty(x.Account) && !string.IsNullOrEmpty(x.Emp_ID))
                            {
                                if (x.Emp_ID.StartsWith("B"))
                                    x.Mail = x.Account + "@BionetCorp.com";
                                else if (x.Emp_ID.StartsWith("G"))
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
