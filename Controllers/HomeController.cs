using ImportExcel.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.XlsIO;
using System.Diagnostics;
using System.Text;

namespace ImportExcel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult<List<string>> GetColumnTable()
        {
            var listColumn = new List<string>();

            

            return listColumn;
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile uploadedFile)
        {
            StringBuilder sb = new StringBuilder();
            if (uploadedFile != null &&
                uploadedFile.Length > 0)
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", uploadedFile.FileName);

                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;

                    FileStream source = new FileStream(filepath, FileMode.Open, FileAccess.Read);

                    IWorkbook workbook = application.Workbooks.Open(source);
                    IWorksheet sheet = workbook.Worksheets[0];
                    int rows = sheet.Rows.Count();
                    
                    int c = 1;

                    sb.Append("<table class=\"table table-bordered table-responsive\">"); // Thêm class CSS cho bảng và đường viền
                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    while (sheet.Range[1, c].Value != null && sheet.Range[1, c].Value.Length > 0)
                    {
                        sb.Append("<th style=\"width: auto;\">" + sheet.Range[1, c].Value.ToString() + "</th>"); // Đặt độ rộng của các thẻ th về auto
                        c++;
                    }
                    sb.Append("</tr>");
                    sb.Append("</thead>");
                    sb.Append("<tbody>");

                    for (int row = 2; row <= rows; row++)
                    {
                        sb.Append("<tr>");
                        for (int col = 1; col <= c; col++)
                        {
                            sb.Append("<td style=\"width: auto;\">" + sheet.Range[row, col].Value.ToString() + "</td>"); // Đặt độ rộng của các ô td về auto
                        }
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody>");
                    sb.Append("</table>");
                }

                ViewBag.Message = "File uploaded successfully.";
            }
            else
            {
                ViewBag.Message = "No file selected.";
            }

            return View(new MyModel { HtmlContent = sb.ToString() });

        }

    }
    public class MyModel
    {
        public string HtmlContent { get; set; }

    }

}
