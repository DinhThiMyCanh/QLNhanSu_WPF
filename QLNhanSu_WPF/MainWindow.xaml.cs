using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace QLNhanSu_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string st = @"Data Source=CANH-DHQN\SQLEXPRESS;Initial Catalog=QLNhanSu;Integrated Security=True";
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        SqlCommandBuilder builder;

        public RoutedEvent Ten_NV { get; }

        public MainWindow()
        {
            InitializeComponent();


            Loaded += new RoutedEventHandler(Window_Load);//Sự kiện Load của Form

            btnThem.Click += new RoutedEventHandler(Them);
            btnSua.Click += new RoutedEventHandler(Sua);
            btnXoa.Click += new RoutedEventHandler(Xoa);

            btnLamMoi.Click += new RoutedEventHandler(LamMoi);
            btnThongKe.Click += new RoutedEventHandler(ThongKePhongBan);
            btnTimKiem.Click += new RoutedEventHandler(TimKiem);

          
            DataGrid.SelectionChanged += new SelectionChangedEventHandler(Data_Click);


        }
        #region Load dữ liệu
        //Phương thức load dữ liệu lên ô Combobox Phòng ban
        public void loadPB()
        {
            string sql = "SELECT * FROM DMPHONG";
            //Khởi tạo DataAdapter
            da = new SqlDataAdapter(sql, cn);
            //Đổ dữ liệu lên Dataset
            da.Fill(ds, "PhongBan");

            //Lấy dữ liệu từ Dataset đổ lên Combobox
            cboTenPhong.ItemsSource = ds.Tables["PhongBan"].DefaultView;
            cboTenPhong.DisplayMemberPath = "TenPhong";
            cboTenPhong.SelectedValuePath = "MaPhong";
            da.Dispose();
        }
        //Phương thức load dữ liệu lên ô Combobox Chức vụ
        public void loadChucVu()
        {
            string sql = "SELECT * FROM CHUCVU ";
            //Khởi tạo DataAdapter
            da = new SqlDataAdapter(sql, cn);
            //Đổ dữ liệu lên Dataset
            da.Fill(ds, "ChucVu");

            //Lấy dữ liệu từ Dataset đổ lên Combobox
            cboChucVu.ItemsSource = ds.Tables["ChucVu"].DefaultView;
            cboChucVu.DisplayMemberPath = "TenChucVu";
            cboChucVu.SelectedValuePath = "MaChucVu";
            da.Dispose();
        }

        #endregion
        //Phương thức load dữ liệu lên DataGridView danh sách nv
        public void loadDSNV()
        {
            string sql = "SELECT * FROM DSNV";
            //Khởi tạo DataAdapter
            da = new SqlDataAdapter(sql, cn);
            //Đổ dữ liệu lên Dataset
            da.Fill(ds, "NhanVien");

            //Lấy dữ liệu từ Dataset đổ lên DataGridView
            DataGrid.ItemsSource = ds.Tables["NhanVien"].DefaultView;

        }
        private void Window_Load(object sender, RoutedEventArgs e)
        {
            cn = new SqlConnection(st);
            ds = new DataSet();
           loadChucVu();
          loadPB();
            loadDSNV();
            builder = new SqlCommandBuilder(da);
            
        }

        //Buộc dữ liệu lên các control
        private void Data_Click(object sender, EventArgs e)
        {
            if (DataGrid.SelectedIndex.ToString()!=null) //Có dòng được chọn
            {
                DataRowView drv = (DataRowView)DataGrid.SelectedItem;//Dòng đang chọn
                if (drv!=null)
                {
                    txtMaNV.Text = drv[0].ToString();
                    txtHoTen.Text = drv[1].ToString();
                    dtpNgaySinh.Text = drv[2].ToString();
                    string gt = drv[3].ToString();
                    if (gt.Equals("True"))
                        rdNam.IsChecked= true;
                    else
                        rdNu.IsChecked= true;

                    txtSoDT.Text = drv[4].ToString();
                    txtHSL.Text = drv[5].ToString();
                    cboTenPhong.SelectedValue = drv[6].ToString();
                    cboChucVu.SelectedValue = drv[7].ToString();
                }    
            }    
        }


       

        #region xác định tính hợp lệ của dữ liệu
        public bool isNumber(string value)
        {
            bool ktra;
            float result;
            ktra = float.TryParse(value, out result);
            return ktra;
        }

        //Kiểm tra sự trùng lặp của khóa chính
        public bool KtraMaNV(string ma)
        {
            bool kt = false;
            DataTable dt = ds.Tables["NhanVien"];
            foreach (DataRow r in dt.Rows)
                if (r[0].Equals(ma))
                {
                    kt = true;
                    break;
                }
            return kt;
        }

        #endregion

        private void TimKiem(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT * FROM DSNV WHERE HoTen Like N'%{0}'", txtTimKiem.Text.Trim());
            da = new SqlDataAdapter(sql, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataGrid.ItemsSource= dt.DefaultView;
        }

       

        private void ThongKePhongBan(object sender, EventArgs e)
        {
            string sql = @"SELECT B.TenPhong, count(A.MaPhong) as SoLuong  " +
                    "FROM DSNV as A, DMPHONG as B " +
                    "WHERE A.MaPhong = B.MaPhong " +
                    "GROUP BY B.TenPhong";
            da = new SqlDataAdapter(sql, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataGrid.ItemsSource = dt.DefaultView;
        }

        private void LamMoi(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtHoTen.Focus();
            dtpNgaySinh.Text = DateTime.Now.ToString();
            if (rdNam.IsChecked == false)
                rdNam.IsChecked = true;
            txtHSL.Clear();
            txtSoDT.Clear();
            cboTenPhong.SelectedIndex = 0;
            cboChucVu.SelectedIndex = 0;

            DataGrid.ItemsSource = ds.Tables["NhanVien"].DefaultView;
        }

       // Xóa 1 nhân viên được chọn trên DataGridView
        private void Xoa(object sender, EventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (dr == MessageBoxResult.Yes)
            {
                int i = DataGrid.SelectedIndex;//Chỉ số dòng được chọn
                try
                {
                    DataTable dt = ds.Tables["NhanVien"];
                    //Xóa trên DataSet
                    dt.Rows[i].Delete();
                    //Cập nhật từ Dataset xuống Database
                    da.Update(ds, "NhanVien");
                    MessageBox.Show("Xóa thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xóa không thành công!");
                }

            }
        }

        private void Sua(object sender, EventArgs e)
        {
            DataTable dt = ds.Tables["NhanVien"];
            int ma = int.Parse(txtMaNV.Text);
            string dk = string.Format("MaNV='{0}'", ma);
            DataRow[] rows = dt.Select(dk);
            foreach (DataRow r in rows)
            {
                r[1] = txtHoTen.Text;
                r[2] = dtpNgaySinh.SelectedDate.ToString();
                r[3] = rdNam.IsChecked == true ? true : false;
                r[4] = txtSoDT.Text;
                r[5] = Math.Round(float.Parse(txtHSL.Text), 2);
                r[6] = cboTenPhong.SelectedValue.ToString();
                r[7] = cboChucVu.SelectedValue.ToString();
                //Cập nhật dữ liệu từ Dataset xuống Database
                da.Update(ds, "NhanVien");
            }
            MessageBox.Show("Sửa thành công!");

        }

        private void Them(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtHoTen.Text)) && (isNumber(txtHSL.Text)))
            {
                //Thêm trên Dataset
                DataTable dt = ds.Tables["NhanVien"];
                DataRow r = dt.NewRow(); //Thêm 1 dòng trống
                r[1] = txtHoTen.Text;
                r[2] = dtpNgaySinh.SelectedDate.ToString();
                r[3] = rdNam.IsChecked == true ? true : false;
                r[4] = txtSoDT.Text;
                r[5] = Math.Round(float.Parse(txtHSL.Text), 2);
                r[6] = cboTenPhong.SelectedValue.ToString();
                r[7] = cboChucVu.SelectedValue.ToString();
                dt.Rows.Add(r);

                //Cập nhật dữ liệu từ Dataset xuống Database
                da.Update(ds, "NhanVien");

                MessageBox.Show("Thêm thành công!");
            }
            else
                MessageBox.Show("Thêm chưa thành công!");
        }

       
    }
}
