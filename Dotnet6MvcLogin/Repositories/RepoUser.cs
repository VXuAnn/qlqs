using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcLogin.Models;
using System.Data;
using System.Drawing;

namespace MvcLogin.Repositories
{
    public class RepoUser
    {
        private SqlConnection _connection;

        public RepoUser()
        {
            string connStr = "Data Source=VUAN;Initial Catalog=QLQS2;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True";

            _connection = new SqlConnection(connStr);
        }

        public Combinecs GetBaoCaoQuanSoById(string IdDv)
        {
            Combinecs combinedData = new Combinecs(); // Tạo một đối tượng mới để chứa cả QuanSo và THDV
            DateTime today = DateTime.Today; // Lấy ngày hôm nay

            using (SqlCommand cmd = new SqlCommand("getBaoCaoQuanSoByDateAndId", _connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@id_dv", IdDv));
                cmd.Parameters.Add(new SqlParameter("@ngay", today)); // Thêm tham số ngày

                _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read(); // Đọc dòng đầu tiên

                    // Kiểm tra xem ngày của QuanSo có phù hợp với ngày hôm nay không
                    if (reader["ngay"] != DBNull.Value && Convert.ToDateTime(reader["ngay"]).Date == today)
                    {
                        // Gán dữ liệu cho đối tượng QuanSo
                        combinedData.QuanSoModel = new QuanSo
                        {
                            IdBcqs = Convert.ToInt32(reader["id_bcqs"]),
                            IdDv = reader["id_dv"] != DBNull.Value ? Convert.ToString(reader["id_dv"]) : string.Empty,
                            Ngay = reader["ngay"] != DBNull.Value ? Convert.ToDateTime(reader["ngay"]) : (DateTime?)null,
                            TongQs = reader["tong_qs"] != DBNull.Value ? Convert.ToInt32(reader["tong_qs"]) : (int?)null,
                            QsVang = reader["qs_vang"] != DBNull.Value ? Convert.ToInt32(reader["qs_vang"]) : (int?)null,
                            DaoNgu = reader["dao_ngu"] != DBNull.Value ? Convert.ToInt32(reader["dao_ngu"]) : (int?)null,
                            DiVien = reader["di_vien"] != DBNull.Value ? Convert.ToInt32(reader["di_vien"]) : (int?)null,
                            BenhXa = reader["benh_xa"] != DBNull.Value ? Convert.ToInt32(reader["benh_xa"]) : (int?)null,
                            DiHoc = reader["di_hoc"] != DBNull.Value ? Convert.ToInt32(reader["di_hoc"]) : (int?)null,
                            DiThucTe = reader["di_thuc_te"] != DBNull.Value ? Convert.ToInt32(reader["di_thuc_te"]) : (int?)null,
                            DiThucTap = reader["di_thuc_tap"] != DBNull.Value ? Convert.ToInt32(reader["di_thuc_tap"]) : (int?)null,
                            DiTt = reader["di_tt"] != DBNull.Value ? Convert.ToInt32(reader["di_tt"]) : (int?)null,
                            DiCtac = reader["di_ctac"] != DBNull.Value ? Convert.ToInt32(reader["di_ctac"]) : (int?)null,
                            ThaiSan = reader["thai_san"] != DBNull.Value ? Convert.ToInt32(reader["thai_san"]) : (int?)null,
                            LyDoKhac = reader["ly_do_khac"] != DBNull.Value ? Convert.ToInt32(reader["ly_do_khac"]) : (int?)null,
                            ChuThich = reader["chu_thich"] != DBNull.Value ? Convert.ToString(reader["chu_thich"]) : string.Empty
                        };
                    }
                }

                reader.Close();
            }

            // Kiểm tra xem QuanSo đã được lấy hay chưa
            if (combinedData.QuanSoModel != null)
            {
                using (SqlCommand cmd2 = new SqlCommand("getTHDVById", _connection))
                {
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd2.Parameters.Add(new SqlParameter("@id_dv", IdDv));
                    cmd2.Parameters.Add(new SqlParameter("@ngay", today)); // Thêm tham số ngày

                    SqlDataReader reader2 = cmd2.ExecuteReader();

                    if (reader2.HasRows)
                    {
                        reader2.Read(); // Đọc dòng đầu tiên

                        // Kiểm tra xem ngày của THDV có phù hợp với ngày hôm nay không
                        if (reader2["ngay"] != DBNull.Value && Convert.ToDateTime(reader2["ngay"]).Date == today)
                        {
                            // Gán dữ liệu cho đối tượng THDV
                            combinedData.THDVModel = new THDV
                            {
                                IdDv = reader2["id_dv"] != DBNull.Value ? Convert.ToString(reader2["id_dv"]) : string.Empty,
                                Ngay = reader2["ngay"] != DBNull.Value ? Convert.ToDateTime(reader2["ngay"]) : (DateTime?)null,
                                TenTB = reader2["ten_tb"] != DBNull.Value ? Convert.ToString(reader2["ten_tb"]) : string.Empty,
                                Nvvs = reader2["nvvs"] != DBNull.Value ? Convert.ToString(reader2["nvvs"]) : string.Empty,
                                CanhGac = reader2["canh_gac"] != DBNull.Value ? Convert.ToString(reader2["canh_gac"]) : string.Empty,
                                GhiChu = reader2["ghi_chu"] != DBNull.Value ? Convert.ToString(reader2["ghi_chu"]) : string.Empty
                            };
                        }
                    }

                    reader2.Close();
                }
            }

            _connection.Close();

            return combinedData;
        }




        public bool AddQuanSo(QuanSo qs, THDV thdv)
        {
            //SqlCommand cmd = new SqlCommand("AddQuanSo", _connection);
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (SqlCommand cmd = new SqlCommand("AddQuanSo", _connection))
            {
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
                    // Nếu thêm thành công vào bảng QuanSo, thêm dữ liệu vào bảng TinhHinhDonVi
                    using (SqlCommand cmd2 = new SqlCommand("insertTinhHinhDonVi", _connection))
                    {
                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@id_dv", thdv.IdDv != null ? thdv.IdDv : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@ngay", thdv.Ngay != null ? thdv.Ngay : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@ten_tb", thdv.TenTB != null ? thdv.TenTB : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@nvvs", thdv.Nvvs != null ? thdv.Nvvs : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@canh_gac", thdv.CanhGac != null ? thdv.CanhGac : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@ghi_chu", thdv.GhiChu != null ? thdv.GhiChu : DBNull.Value);

                        _connection.Open();
                        int j = cmd2.ExecuteNonQuery();
                        _connection.Close();

                        // Trả về true nếu cả hai lệnh INSERT đều thành công
                        return j >= 1;
                    }

                }
                else
                {
                    return false;
                }
            }
        }
        // Phương thức isExistingRecord sẽ nhận một tham số là ngày cần kiểm tra
        public bool isExistingRecord(DateTime ngay, string id)
        {
            SqlCommand cmd = new SqlCommand("getBaoCaoQuanSoByDateAndId", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // Sử dụng tham số để chỉ định ngày cần kiểm tra
            cmd.Parameters.AddWithValue("@ngay", ngay);
            cmd.Parameters.AddWithValue("@id_dv", id);


            _connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            bool recordExists = reader.HasRows;

            reader.Close();
            _connection.Close();

            // Return true if there is at least one record for the specified date
            return recordExists;
        }


        public QuanSo ReturnQuanSo(DateTime ngay, string id)
        {
            QuanSo listQuanSo = new QuanSo();

            SqlCommand cmd = new SqlCommand("getBaoCaoQuanSoByDateAndId", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ngay", ngay);
            cmd.Parameters.AddWithValue("@iddv", id);



            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                listQuanSo = new QuanSo
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
                };
            }

            return listQuanSo;
        }

        public bool EditQuanSo( QuanSo qs, THDV thdv)
        {
            using (SqlCommand cmd = new SqlCommand("updateBaoCaoQuanSo", _connection))
            {
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
                    // Nếu thêm thành công vào bảng QuanSo, thêm dữ liệu vào bảng TinhHinhDonVi
                    using (SqlCommand cmd2 = new SqlCommand("updateTHDV", _connection))
                    {
                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@id_dv", thdv.IdDv != null ? thdv.IdDv : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@ngay", thdv.Ngay != null ? thdv.Ngay : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@ten_tb", thdv.TenTB != null ? thdv.TenTB : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@nvvs", thdv.Nvvs != null ? thdv.Nvvs : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@canh_gac", thdv.CanhGac != null ? thdv.CanhGac : DBNull.Value);
                        cmd2.Parameters.AddWithValue("@ghi_chu", thdv.GhiChu != null ? thdv.GhiChu : DBNull.Value);

                        _connection.Open();
                        int j = cmd2.ExecuteNonQuery();
                        _connection.Close();

                        // Trả về true nếu cả hai lệnh INSERT đều thành công
                        return j >= 1;
                    }

                }
                else
                {
                    return false;
                }
            }

           
        }
        public bool addCombinces(Combinecs c)
        {
            SqlCommand cmd = new SqlCommand("insertHisAct", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            cmd.Parameters.AddWithValue("@id_dv", c.Uname != null ? c.Uname : DBNull.Value);
            cmd.Parameters.AddWithValue("@Ltime", c.LastActionTime != null ? c.LastActionTime : DBNull.Value);
            cmd.Parameters.AddWithValue("@Laction", c.LastAction != null ? c.LastAction : DBNull.Value);
            

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
    }
}


