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
    
            cboTenPhong.ItemsSource = DataProvider.getTable(sql).DefaultView;
            cboTenPhong.DisplayMemberPath = "TenPhong";
            cboTenPhong.SelectedValuePath = "MaPhong";
            
        }
        //Phương thức load dữ liệu lên ô Combobox Chức vụ
        public void loadChucVu()
        {
            string sql = "SELECT * FROM CHUCVU ";
           
            cboChucVu.ItemsSource = DataProvider.getTable(sql).DefaultView;
            cboChucVu.DisplayMemberPath = "TenChucVu";
            cboChucVu.SelectedValuePath = "MaChucVu";
        }

        #endregion
        //Phương thức load dữ liệu lên DataGridView danh sách nv
        public void loadDSNV()
        {
            string sql = "SELECT * FROM NhanVien";
            DataGrid.ItemsSource = DataProvider.getTable(sql).DefaultView;

        }

        private void Window_Load(object sender, RoutedEventArgs e)
        {
            DataProvider.moKetNoi();
           loadChucVu();
          loadPB();
            loadDSNV();
            DataProvider.dongKetNoi();
            
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


        #endregion

        private void TimKiem(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT * FROM DSNV WHERE HoTen Like N'%{0}'", txtTimKiem.Text.Trim());
            
            DataGrid.ItemsSource= DataProvider.getTable(sql).DefaultView;
        }

       

        private void ThongKePhongBan(object sender, EventArgs e)
        {
            string sql = @"SELECT B.TenPhong, count(A.MaPhong) as SoLuong  " +
                    "FROM DSNV as A, DMPHONG as B " +
                    "WHERE A.MaPhong = B.MaPhong " +
                    "GROUP BY B.TenPhong";
            

            DataGrid.ItemsSource = DataProvider.getTable(sql).DefaultView;
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

            loadDSNV();
        }

       // Xóa 1 nhân viên được chọn trên DataGridView
        private void Xoa(object sender, EventArgs e)
        {
           
        }

        private void Sua(object sender, EventArgs e)
        {
            
           

        }

        private void Them(object sender, EventArgs e)
        {
            

                
        }

       
    }
}
