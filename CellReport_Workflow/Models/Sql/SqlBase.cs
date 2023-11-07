using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Dynamic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CellReport_Workflow.Models.Sql
{
    public class SqlBase
    {
        private string DB { get; } = "DB_TEST";
        private string DB_Log { get; } = "Log";
        private string CONNSTR { get; set; }
        public string connstr
        {
            get { return CONNSTR; }
            private set { CONNSTR = value; }
        }
        private string LOG_CONNSTR { get; set; }
        public string log_connstr
        {
            get { return LOG_CONNSTR; }
            private set { LOG_CONNSTR = value; }
        }
        public SqlBase()
        {
            this.connstr = GetConnstr(DB);
            this.log_connstr = GetConnstr(DB_Log);
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
        public SqlConnection LOG_CONN_Builder() { return new(log_connstr); }

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



    }
}
