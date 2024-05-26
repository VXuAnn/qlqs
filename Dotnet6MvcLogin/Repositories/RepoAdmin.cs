using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcLogin.Models;
using System.Data;
using System.Drawing;

namespace MvcLogin.Repositories
{
    public class RepoAdmin
    {
        

        #region Liên kết csdl
        private SqlConnection _connection;
        public RepoAdmin()
        {
            string connStr = "Data Source=VUAN;Initial Catalog=QLQS2;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True";

            _connection = new SqlConnection(connStr);
        }
        #endregion


        #region Xem quân số
        public List<QuanSo> SearchQuanSo(DateTime? ngay)
        {
            List<QuanSo> listQuanSo = new List<QuanSo>();

            SqlCommand cmd = new SqlCommand("getBaoCaoQuanSoByDate", _connection);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@ngay", ngay.Value);




            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                listQuanSo.Add(new QuanSo
                {
                    IdBcqs = Convert.ToInt32(dr["id_bcqs"]),
                    IdDv = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty,
                    Ngay = dr["ngay"] != DBNull.Value ? Convert.ToDateTime(dr["ngay"]) : (DateTime?)null,
                    TongQs = dr["tong_qs"] != DBNull.Value ? Convert.ToInt32(dr["tong_qs"]) : (int?)null,
                    QsVang = dr["qs_vang"] != DBNull.Value ? Convert.ToInt32(dr["qs_vang"]) : (int?)null,
                    DaoNgu = dr["dao_ngu"] != DBNull.Value ? Convert.ToInt32(dr["dao_ngu"]) : (int?)null,
                    DiVien = dr["di_vien"] != DBNull.Value ? Convert.ToInt32(dr["di_vien"]) : (int?)null,
                    BenhXa = dr["benh_xa"] != DBNull.Value ? Convert.ToInt32(dr["benh_xa"]) : (int?)null,
                    DiHoc = dr["di_hoc"] != DBNull.Value ? Convert.ToInt32(dr["di_hoc"]) : (int?)null,
                    DiThucTe = dr["di_thuc_te"] != DBNull.Value ? Convert.ToInt32(dr["di_thuc_te"]) : (int?)null,
                    DiThucTap = dr["di_thuc_tap"] != DBNull.Value ? Convert.ToInt32(dr["di_thuc_tap"]) : (int?)null,
                    DiTt = dr["di_tt"] != DBNull.Value ? Convert.ToInt32(dr["di_tt"]) : (int?)null,
                    DiCtac = dr["di_ctac"] != DBNull.Value ? Convert.ToInt32(dr["di_ctac"]) : (int?)null,
                    ThaiSan = dr["thai_san"] != DBNull.Value ? Convert.ToInt32(dr["thai_san"]) : (int?)null,
                    LyDoKhac = dr["ly_do_khac"] != DBNull.Value ? Convert.ToInt32(dr["ly_do_khac"]) : (int?)null,
                    ChuThich = dr["chu_thich"] != DBNull.Value ? Convert.ToString(dr["chu_thich"]) : string.Empty
                });
            }

            return listQuanSo;
        }
        #endregion

        #region Xem Người dùng
        public List<Users> GetUsers()
        {
            List<Users> listUser = new List<Users>();
            SqlCommand cmd = new SqlCommand("GetUser", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                string uname = dr["UserName"] != DBNull.Value ? Convert.ToString(dr["UserName"]) : string.Empty;
                if (!IsAdmin(uname))
                {
                    Users user = new Users
                    {
                        FName = dr["FirstName"] != DBNull.Value ? Convert.ToString(dr["FirstName"]) : string.Empty,
                        LName = dr["LastName"] != DBNull.Value ? Convert.ToString(dr["LastName"]) : string.Empty,
                        UserName = uname,
                        Email = dr["Email"] != DBNull.Value ? Convert.ToString(dr["Email"]) : string.Empty,
                        IdDv = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty
                    };

                    // Kiểm tra và gán giá trị cho thuộc tính IsDataEntryAllowed
                    user.IsDataEntryAllowed = CheckDataEntryStatusForToday(user);

                    listUser.Add(user);
                }
            }
            return listUser;
        }
        public List<Users> SearchUser(string id_dv)
        {
            List<Users> listUser = new List<Users>();

            SqlCommand cmd = new SqlCommand("GetUserByDV", _connection);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@id_dv", id_dv);




            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                string uname = dr["UserName"] != DBNull.Value ? Convert.ToString(dr["UserName"]) : string.Empty;
                if (!IsAdmin(uname))
                {
                    Users user = new Users
                    {
                        FName = dr["FirstName"] != DBNull.Value ? Convert.ToString(dr["FirstName"]) : string.Empty,
                        LName = dr["LastName"] != DBNull.Value ? Convert.ToString(dr["LastName"]) : string.Empty,
                        UserName = uname,
                        Email = dr["Email"] != DBNull.Value ? Convert.ToString(dr["Email"]) : string.Empty,
                        IdDv = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty
                    };

                    // Kiểm tra và gán giá trị cho thuộc tính IsDataEntryAllowed
                    user.IsDataEntryAllowed = CheckDataEntryStatusForToday(user);

                    listUser.Add(user);
                }
            }

            return listUser;
        }
        #endregion

        #region Xem lịch sử đăng nhập
        public List<LoginHistory> SearchHisoryDate(DateTime? ngay)
        {
            List<LoginHistory> listUser = new List<LoginHistory>();

            SqlCommand cmd = new SqlCommand("getHistoryByDate", _connection);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@ngay", ngay.Value.Date);




            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                listUser.Add(
                     new LoginHistory
                     {
                         FirstName = dr["FirstName"] != DBNull.Value ? Convert.ToString(dr["FirstName"]) : string.Empty,
                         LoginTime = dr["LoginTime"] != DBNull.Value ? Convert.ToDateTime(dr["LoginTime"]) : (DateTime?)null,

                     }
                 );
            }

            return listUser;
        }
        public List<LoginHistory> SearchHisoryName(string? name)
        {
            List<LoginHistory> listUser = new List<LoginHistory>();

            SqlCommand cmd = new SqlCommand("getHistoryByName", _connection);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@name", name);




            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                listUser.Add(
                     new LoginHistory
                     {
                         FirstName = dr["FirstName"] != DBNull.Value ? Convert.ToString(dr["FirstName"]) : string.Empty,
                         LoginTime = dr["LoginTime"] != DBNull.Value ? Convert.ToDateTime(dr["LoginTime"]) : (DateTime?)null,

                     }
                 );
            }

            return listUser;
        }
        #endregion

        #region kiểm tra admin
        private bool IsAdmin(string uname)
        {
            // Thực hiện kiểm tra nếu idDv là của admin
            // Trong trường hợp này, bạn cần biết cách xác định idDv của admin trong cơ sở dữ liệu
            // Đây chỉ là một ví dụ giả định
            return uname == "admin"; // Thay "adminId" bằng idDv của admin thực tế
        }
        #endregion

        #region lưu lại lịch sử đăng nhập
        public bool SaveLoginHistoryToDatabase(LoginHistory lg)
        {
            SqlCommand cmd = new SqlCommand("addHistory", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            cmd.Parameters.AddWithValue("@fname", lg.FirstName != null ? lg.FirstName : DBNull.Value);
            cmd.Parameters.AddWithValue("@lname", lg.LastName != null ? lg.LastName : DBNull.Value);
            cmd.Parameters.AddWithValue("@ltime", lg.LoginTime != null ? lg.LoginTime : DBNull.Value);
            cmd.Parameters.AddWithValue("@uname", lg.UserName != null ? lg.UserName : DBNull.Value);

            _connection.Open();

            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1)
            {
                return true;
            }
            else
            { return false; }

        }
        #endregion

        #region Xem THDV
        public List<THDV> GetTHDV(DateTime? ngay)
        {
            List<THDV> listQuanSo = new List<THDV>();

            SqlCommand cmd = new SqlCommand("getTHDV", _connection);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@ngay", ngay.Value);




            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                listQuanSo.Add(new THDV
                {

                    IdDv = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty,
                    Ngay = dr["ngay"] != DBNull.Value ? Convert.ToDateTime(dr["ngay"]) : (DateTime?)null,
                    Nvvs = dr["nvvs"] != DBNull.Value ? Convert.ToString(dr["nvvs"]) : string.Empty,
                    TenTB = dr["ten_tb"] != DBNull.Value ? Convert.ToString(dr["ten_tb"]) : string.Empty,
                    CanhGac = dr["canh_gac"] != DBNull.Value ? Convert.ToString(dr["canh_gac"]) : string.Empty,
                    GhiChu = dr["ghi_chu"] != DBNull.Value ? Convert.ToString(dr["ghi_chu"]) : string.Empty,

                });
            }

            return listQuanSo;
        }
        #endregion

        #region kiểm tra đã nhập chưa
        public bool CheckDataEntryStatusForToday(Users u)
        {
            SqlCommand cmd = new SqlCommand("getUserByIdAndDate", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            cmd.Parameters.AddWithValue("@id_dv", u.IdDv);
            cmd.Parameters.AddWithValue("@now", DateTime.Now);


            _connection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            bool hasEntry = reader.HasRows; // Kiểm tra xem có bản ghi nào trả về không

            reader.Close();
            _connection.Close();

            return hasEntry;
        }
        #endregion

        #region Xem lịch sử hành động người dùng
        public List<Combinecs> SearchHisoryActDate(DateTime? ngay)
        {
            List<Combinecs> listUser = new List<Combinecs>();

            SqlCommand cmd = new SqlCommand("getHisActByDate", _connection);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@ngay", ngay);




            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                listUser.Add(
                     new Combinecs
                     {
                         Uname = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty,
                         LastAction = dr["LastAction"] != DBNull.Value ? Convert.ToString(dr["LastAction"]) : string.Empty,
                         LastActionTime = dr["LastActionTime"] != DBNull.Value ? Convert.ToDateTime(dr["LastActionTime"]) : (DateTime?)null,


                     }
                 );
            }

            return listUser;
        }

        public List<Combinecs> SearchHisoryActID(string? id_dv)
        {
            List<Combinecs> listUser = new List<Combinecs>();

            SqlCommand cmd = new SqlCommand("getHisActById", _connection);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@id_dv", id_dv);




            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                listUser.Add(
                     new Combinecs
                     {
                         Uname = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty,
                         LastAction = dr["LastAction"] != DBNull.Value ? Convert.ToString(dr["LastAction"]) : string.Empty,
                         LastActionTime = dr["LastActionTime"] != DBNull.Value ? Convert.ToDateTime(dr["LastActionTime"]) : (DateTime?)null,


                     }
                 );
            }

            return listUser;
        }
        #endregion

        #region Sửa thông tin quân số

        public QuanSo GetBaoCaoQuanSoById(string IdDv)
        {
            QuanSo qs = new QuanSo();

            SqlCommand cmd = new SqlCommand("getBaoCaoQuanSoById", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_dv", IdDv);





            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                qs = new QuanSo
                {
                    IdDv = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty,
                    Ngay = dr["ngay"] != DBNull.Value ? Convert.ToDateTime(dr["ngay"]) : (DateTime?)null,
                    TongQs = dr["tong_qs"] != DBNull.Value ? Convert.ToInt32(dr["tong_qs"]) : (int?)null,
                    QsVang = dr["qs_vang"] != DBNull.Value ? Convert.ToInt32(dr["qs_vang"]) : (int?)null,
                    DaoNgu = dr["dao_ngu"] != DBNull.Value ? Convert.ToInt32(dr["dao_ngu"]) : (int?)null,
                    DiVien = dr["di_vien"] != DBNull.Value ? Convert.ToInt32(dr["di_vien"]) : (int?)null,
                    BenhXa = dr["benh_xa"] != DBNull.Value ? Convert.ToInt32(dr["benh_xa"]) : (int?)null,
                    DiHoc = dr["di_hoc"] != DBNull.Value ? Convert.ToInt32(dr["di_hoc"]) : (int?)null,
                    DiThucTe = dr["di_thuc_te"] != DBNull.Value ? Convert.ToInt32(dr["di_thuc_te"]) : (int?)null,
                    DiThucTap = dr["di_thuc_tap"] != DBNull.Value ? Convert.ToInt32(dr["di_thuc_tap"]) : (int?)null,
                    DiTt = dr["di_tt"] != DBNull.Value ? Convert.ToInt32(dr["di_tt"]) : (int?)null,
                    DiCtac = dr["di_ctac"] != DBNull.Value ? Convert.ToInt32(dr["di_ctac"]) : (int?)null,
                    ThaiSan = dr["thai_san"] != DBNull.Value ? Convert.ToInt32(dr["thai_san"]) : (int?)null,
                    LyDoKhac = dr["ly_do_khac"] != DBNull.Value ? Convert.ToInt32(dr["ly_do_khac"]) : (int?)null,
                    ChuThich = dr["chu_thich"] != DBNull.Value ? Convert.ToString(dr["chu_thich"]) : string.Empty

                };
            }

            return qs;

        }

        public bool EditQuanSo(QuanSo qs)
        {
            SqlCommand cmd = new SqlCommand("updateBaoCaoQuanSo", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_dv", qs.IdDv != null ? qs.IdDv : DBNull.Value);
            cmd.Parameters.AddWithValue("@ngay", qs.Ngay != null ? qs.Ngay : DBNull.Value);
            cmd.Parameters.AddWithValue("@tong_qs", qs.TongQs != null ? qs.TongQs : DBNull.Value);
            cmd.Parameters.AddWithValue("@qs_vang", qs.QsVang != null ? qs.QsVang : DBNull.Value);
            cmd.Parameters.AddWithValue("@dao_ngu", qs.DaoNgu != null ? qs.DaoNgu : DBNull.Value);
            cmd.Parameters.AddWithValue("@di_vien", qs.DiVien != null ? qs.DiVien : DBNull.Value);
            cmd.Parameters.AddWithValue("@benh_xa", qs.BenhXa != null ? qs.BenhXa : DBNull.Value);
            cmd.Parameters.AddWithValue("@di_hoc", qs.DiHoc != null ? qs.DiHoc : DBNull.Value);
            cmd.Parameters.AddWithValue("@di_thuc_te", qs.DiThucTe != null ? qs.DiThucTe : DBNull.Value);
            cmd.Parameters.AddWithValue("@di_thuc_tap", qs.DiThucTap != null ? qs.DiThucTap : DBNull.Value);
            cmd.Parameters.AddWithValue("@di_tt", qs.DiTt != null ? qs.DiTt : DBNull.Value);
            cmd.Parameters.AddWithValue("@di_ctac", qs.DiCtac != null ? qs.DiCtac : DBNull.Value);
            cmd.Parameters.AddWithValue("@thai_san", qs.ThaiSan != null ? qs.ThaiSan : DBNull.Value);
            cmd.Parameters.AddWithValue("@ly_do_khac", qs.LyDoKhac != null ? qs.LyDoKhac : DBNull.Value);
            cmd.Parameters.AddWithValue("@chu_thich", qs.ChuThich != null ? qs.ChuThich : DBNull.Value);

            _connection.Open();

            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1)
            {
                return true;
            }
            else
            { return false; }
        }
        #endregion

        #region Sửa thông tin THDV

        public THDV GetTHDVById(string IdDv)
        {
            THDV qs = new THDV();

            SqlCommand cmd = new SqlCommand("getTHDVById", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@id_dv", IdDv));
            cmd.Parameters.Add(new SqlParameter("@ngay", DateTime.Now)); // Thêm tham số ngày






            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                qs = new THDV
                {




                    IdDv = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty,
                    Ngay = dr["ngay"] != DBNull.Value ? Convert.ToDateTime(dr["ngay"]) : (DateTime?)null,
                    Nvvs = dr["nvvs"] != DBNull.Value ? Convert.ToString(dr["nvvs"]) : string.Empty,
                    TenTB = dr["ten_tb"] != DBNull.Value ? Convert.ToString(dr["ten_tb"]) : string.Empty,
                    CanhGac = dr["canh_gac"] != DBNull.Value ? Convert.ToString(dr["canh_gac"]) : string.Empty,
                    GhiChu = dr["ghi_chu"] != DBNull.Value ? Convert.ToString(dr["ghi_chu"]) : string.Empty,

                };
            }

            return qs;

        }
        public bool EditTHDV(THDV qs)
        {
            SqlCommand cmd = new SqlCommand("updateTHDV", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_dv", qs.IdDv != null ? qs.IdDv : DBNull.Value);
            cmd.Parameters.AddWithValue("@ngay", qs.Ngay != null ? qs.Ngay : DBNull.Value);
            cmd.Parameters.AddWithValue("@ten_tb", qs.TenTB != null ? qs.TenTB : DBNull.Value);
            cmd.Parameters.AddWithValue("@nvvs", qs.Nvvs != null ? qs.Nvvs : DBNull.Value);
            cmd.Parameters.AddWithValue("@canh_gac", qs.CanhGac != null ? qs.CanhGac : DBNull.Value);
            cmd.Parameters.AddWithValue("@ghi_chu", qs.GhiChu != null ? qs.GhiChu : DBNull.Value);


            _connection.Open();

            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1)
            {
                return true;
            }
            else
            { return false; }
        }

        #endregion

        #region Thêm đơn vị
        public bool addDonVi(DonVi dv)
        {
            SqlCommand cmd = new SqlCommand("addDonVi", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            cmd.Parameters.AddWithValue("@id_dv", dv.IdDv != null ? dv.IdDv : DBNull.Value);
            cmd.Parameters.AddWithValue("@ten_dv", dv.TenDv != null ? dv.TenDv : DBNull.Value);



            _connection.Open();

            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1)
            {
                return true;
            }
            else
            { return false; }
        }
        #endregion

        #region Xem đơn vị
        public List<DonVi> GetDonVi()
        {
            List<DonVi> listUser = new List<DonVi>();
            SqlCommand cmd = new SqlCommand("getDonVi", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {

                DonVi user = new DonVi
                {
                    TenDv = dr["ten_dv"] != DBNull.Value ? Convert.ToString(dr["ten_dv"]) : string.Empty,

                    IdDv = dr["id_dv"] != DBNull.Value ? Convert.ToString(dr["id_dv"]) : string.Empty
                };




                listUser.Add(user);

            }
            return listUser;
        }
        #endregion

        #region xuất pdf
        public DataTable GetBaoCaoQuanSoData(DateTime ngay)
        {
            DataTable dataTable = new DataTable();
            try
            {
                _connection.Open();
                string query = "SELECT * FROM BaoCaoQuanSo WHERE Ngay = @Ngay";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@Ngay", ngay);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
            }
            finally
            {
                _connection.Close();
            }
            return dataTable;
        }
        #endregion

    }
}
