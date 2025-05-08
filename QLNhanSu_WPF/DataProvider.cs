using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace QLNhanSu_WPF
{
    public static class DataProvider
    {
       
            public static SqlConnection cn;
            public static SqlDataAdapter da;
            public static SqlCommand cmd;

            //Mở kết nối
            public static void moKetNoi()
            {
                cn = new SqlConnection();
                cn.ConnectionString = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString.ToString();
                cn.Open();
            }

            //Đóng kết nối
            public static void dongKetNoi()
            {
                cn.Close();
            }

            //Lấy dữ liệu
            public static DataTable getTable(string sql)
            {
                da = new SqlDataAdapter(sql, cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            //Cập nhật dữ liệu
            public static void updateData(string sql, object[] value = null, string[] name = null)
            {
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Clear();
                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                        cmd.Parameters.AddWithValue(name[i], value[i]);
                }
                cmd.ExecuteNonQuery(); //Insert, Update, Delete
                cmd.Dispose();
            }
            //Kiểm tra dữ liệu (trùng mã NV)
            public static int checkData(string sql)
            {
                cmd = new SqlCommand(sql, cn);
                int i = (int)cmd.ExecuteScalar();// Select count(*)...
                cmd.Dispose();
                return i;
            }


        }
}
