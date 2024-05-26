
using Microsoft.AspNetCore.Mvc;
using ThongKeDataChart.Data;
namespace MvcLogin.Controllers
{
    public class ThongKeRecordController : Controller
    {
        private DbContextThongKe _context;
        public ThongKeRecordController(DbContextThongKe context)
        {
            _context = context;
        }

    
        [HttpGet]
        public IActionResult UpdateChart(DateTime? startDate, DateTime? endDate, string idDv)
        {
            if (startDate == null || endDate == null || idDv == null)
            {
                return BadRequest("Thiếu thông tin ngày hoặc đơn vị.");
            }

            List<DateTime> dateRange = Enumerable.Range(0, 1 + endDate.Value.Subtract(startDate.Value).Days)
                                                  .Select(offset => startDate.Value.AddDays(offset))
                                                  .ToList();

            List<int> dataForChart = new List<int>();
            List<int> dataForQsVang = new List<int>(); 

            foreach (var date in dateRange)
            {
                var dataForDay = _context.BaoCaoQuanSo
                                         .Where(q => q.ngay.Date == date.Date && q.id_dv == idDv)
                                         .Select(q => q.tong_qs)
                                         .Sum();

                if (dataForDay != null)
                {
                    dataForChart.Add(dataForDay);
                }
                else
                {
                    dataForChart.Add(0);
                }

                // Lấy dữ liệu cho cột qs_vắng
                var dataQsVangForDay = _context.BaoCaoQuanSo
                                              .Where(q => q.ngay.Date == date.Date && q.id_dv == idDv)
                                              .Select(q => q.qs_vang)
                                              .Sum();

                if (dataQsVangForDay != null)
                {
                    dataForQsVang.Add(dataQsVangForDay);
                }
                else
                {
                    dataForQsVang.Add(0);
                }
            }

            var chartData = new
            {
                labels = dateRange.Select(date => date.ToString("dd/MM/yyyy")),
                dataTongQS = dataForChart, // Dữ liệu cho cột tổng quân số
                dataQsVang = dataForQsVang // Dữ liệu cho cột qs_vắng
            };

            return Json(chartData);
        }

  
        [HttpGet]
        public IActionResult UpdateChartForHocVien(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                return BadRequest("Thiếu thông tin ngày.");
            }

            List<DateTime> dateRange = Enumerable.Range(0, 1 + endDate.Value.Subtract(startDate.Value).Days)
                                                  .Select(offset => startDate.Value.AddDays(offset))
                                                  .ToList();

            List<int> dataForChart = new List<int>();
            List<int> dataForQsVang = new List<int>();

            foreach (var date in dateRange)
            {
                var totalTongQS = _context.BaoCaoQuanSo
                                          .Where(q => q.ngay.Date == date.Date)
                                          .Select(q => q.tong_qs)
                                          .Sum();

                var totalQsVang = _context.BaoCaoQuanSo
                                           .Where(q => q.ngay.Date == date.Date)
                                           .Select(q => q.qs_vang)
                                           .Sum();

                dataForChart.Add(totalTongQS);
                dataForQsVang.Add(totalQsVang);
            }

            var chartData = new
            {
                labels = dateRange.Select(date => date.ToString("dd/MM/yyyy")),
                dataTongQS = dataForChart,
                dataQsVang = dataForQsVang
            };

            return Json(chartData);
        }


        public IActionResult Index()
        {
            var quanso = _context.BaoCaoQuanSo.ToList();

            return View(quanso);
        }
    }
}
