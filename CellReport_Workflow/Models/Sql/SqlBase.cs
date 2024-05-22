using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Dynamic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CellReport_Workflow.Models.Sql
{
    public class SqlBase: ISql
    {
        //private readonly IConfiguration _configuration;
        private string DB { get; } = "DB_TEST";
        private string DB_MAIL { get; } = "DB_MAIL";

        private string CONNSTR { get; set; }
        public string connstr
        {
            get { return CONNSTR; }
            private set { CONNSTR = value; }
        }

        private string MAIL_CONNSTR { get; set; }
        public string mail_connstr
        {
            get { return MAIL_CONNSTR; }
            private set { MAIL_CONNSTR = value; }
        }
        public SqlBase(/*IConfiguration configuration*/)
        {
            //_configuration = configuration;
            this.connstr = GetConnstr(DB);
            this.mail_connstr = GetConnstr(DB_MAIL);
            //this.connstr = _configuration["ConnectionStrings:DB"];
            //this.log_connstr = _configuration["ConnectionStrings:Log"];
        }




        public string GetConnstr(string DBjson)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connstr = config.GetConnectionString($"{DBjson}").ToString();
            return connstr;
        }
        public SqlConnection CONN_Builder() { return new(connstr); }
        public SqlConnection MAIL_CONN_Builder() { return new(mail_connstr); }

        public string SqlNonQuery(string sql, List<SqlParameter> Paras)
        {
            try
            {
                using (SqlConnection con = CONN_Builder())
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        cmd.Parameters.AddRange(Paras.ToArray());
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                return CDictionary.OK;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string MAIL_SqlNonQuery(string sql, List<SqlParameter> Paras)
        {
            try
            {
                using (SqlConnection con = MAIL_CONN_Builder())
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        cmd.Parameters.AddRange(Paras.ToArray());
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                return CDictionary.OK;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string SqlInsert_ReturnID(string sql, List<SqlParameter> Paras)
        {
            try
            {
                string id = "";
                using (SqlConnection con = CONN_Builder())
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        cmd.Parameters.AddRange(Paras.ToArray());
                        int a = (int)cmd.ExecuteScalar();
                        id = a.ToString();
                    }
                    con.Close();
                }
                return id;
            }
            catch (Exception ex)
            {
                return CDictionary.ERR;
                //return ex.ToString();
            }
        }


        public string StringReader(string sql, string ColumnName, List<SqlParameter> Paras)
        {
            string str = "";
            try
            {
                using (SqlConnection con = CONN_Builder())
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        if (Paras != null && Paras.Count > 0)
                            cmd.Parameters.AddRange(Paras.ToArray());
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                            str = !dr.IsDBNull(dr.GetOrdinal(ColumnName)) ? dr[ColumnName].ToString() : "";
                    }
                    con.Close();
                }
                return str;
            }
            catch (Exception ex)
            {
                return CDictionary.ERR;
                //return ex.ToString();
            }
        }
        public List<string> ListStringReader(string sql, string ColumnName, List<SqlParameter> Paras)
        {
            List<string> datas = new();
            try
            {
                using (SqlConnection con = CONN_Builder())
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        if (Paras != null && Paras.Count > 0)
                            cmd.Parameters.AddRange(Paras.ToArray());
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string z = !dr.IsDBNull(dr.GetOrdinal(ColumnName)) ? dr[ColumnName].ToString() : "";
                            datas.Add(z);
                        }
                    }
                    con.Close();
                }
                return datas;
            }
            catch (Exception ex)
            {
                return datas;
                //return ex.ToString();
            }
        }

        public List<string> List_StringReader(string sql, string ColumnName, List<SqlParameter> Paras)
        {
            List<string> str = new();
            try
            {
                using (SqlConnection con = CONN_Builder())
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        if (Paras != null && Paras.Count > 0)
                            cmd.Parameters.AddRange(Paras.ToArray());
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string x = "";
                            x = !dr.IsDBNull(dr.GetOrdinal(ColumnName)) ? dr[ColumnName].ToString() : "";
                            str.Add(x);
                        }
                    }
                    con.Close();
                }
                return str;
            }
            catch (Exception ex)
            {
                return null;
                //return ex.ToString();
            }
        }



        public List<T> DynamicDataBuidler<T>(string sql, List<SqlParameter> Paras)
        {
            SqlConnection con = CONN_Builder();
            con.Open();
            SqlCommand cmd = new(sql, con);
            if (Paras != null)
                cmd.Parameters.AddRange(Paras.ToArray());

            SqlDataReader dr = cmd.ExecuteReader();
            List<object> datas = new();
            try
            {
                while (dr.Read())
                {
                    Dictionary<string, object> data = new();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string val = !dr.IsDBNull(dr.GetOrdinal(dr.GetName(i))) ? dr[dr.GetName(i)].ToString() : "";
                        data.Add(dr.GetName(i), val);
                    }
                    datas.Add(data);
                }
                dr.Close();
                cmd.Dispose();
                con.Close();
                con.Dispose();

                string JsonData = JsonConvert.SerializeObject(datas);

                return JsonConvert.DeserializeObject<List<T>>(JsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new();
            }
        }
        //List<Product> product  = SqlBase.DynamicDataBuidler<Product>(sql, null);

        public string GetCV_PT_ForPBSC(string PBSC_FULL_ID)
        {
            string sql = "SELECT T.CV_PT ";
            sql += " FROM AP_PBSC_T P ";
            sql += " INNER JOIN CUSTOMER C ON P.SEQ_NUM = C.SEQ_NUM ";
            sql += " INNER JOIN  AP_ProjectType_T T ON C.ProjectType = T.SID ";
            sql += " WHERE P.PBSC_FULL_ID = @K_PBSC_FULL_ID ";
            List<SqlParameter> Paras = new() { new("K_PBSC_FULL_ID", PBSC_FULL_ID) };

            return StringReader(sql, "CV_PT", Paras);

        }

    }
}
