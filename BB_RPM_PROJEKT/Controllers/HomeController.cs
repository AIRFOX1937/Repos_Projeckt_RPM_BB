using BB_RPM_PROJEKT.AdditionalClasses;
using BB_RPM_PROJEKT.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace BB_RPM_PROJEKT.Controllers
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

        [HttpPost]
        public IActionResult Index(string log, string pass)
        {
            MySqlConnection conn = MySqlDb.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "select * from users where login_u = @login";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@login", log);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (log == reader.GetString(1) && pass == reader.GetString(2))
                        {
                            if(Convert.ToInt32(reader.GetString(3)) == 2)
                            {
                                return Redirect("/Home/NewsT");
                            }
                            if (Convert.ToInt32(reader.GetString(3)) == 2)
                            {
                                return Redirect("/Home/News");
                            }
                        }
                        else
                        {
                            ViewBag.H = "Неправильный логин или пароль";
                            return View();
                        }
                    }
                }
                //if (log != "" && log != null && pass != "" && pass != null)
                //{
                //    if (log == "student")
                //    {
                //        return Redirect("/Home/News");
                //    }
                //    else if (log == "teacher")
                //    {
                //        return Redirect("/Home/NewsT");
                //    }
                //}
                //else
                //{
                //    ViewBag.H = "Ошибка. Введите данные";
                //}
            }
            catch
            {
                ViewBag.H = "Ошибка";
            }
            finally
            {
                conn.Close();
            }
            return View();
        }

        public IActionResult Disciplins()
        {
            return View();
        }

        public IActionResult DisciplinsT()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult NewsT()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Raspisanie()
        {
            return View();
        }

        public IActionResult RaspisanieT()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Raspisanie(string gruppa, string chetnost1, string fio, string chetnost2)
        {
            try
            {
                if (gruppa == "4335" && chetnost1 == "нечётная")
                {
                    return Redirect("/Home/nechet4335");
                }
                if (fio == "Петров Петр Петрович" && chetnost2 == "нечётная")
                {
                    return Redirect("/Home/nechet4335");
                }
            }
            catch
            {

            }
            return View();
        }

        [HttpPost]
        public IActionResult RaspisanieT(string gruppa, string chetnost1, string fio, string chetnost2)
        {
            try
            {
                if (gruppa == "4335" && chetnost1 == "нечётная")
                {
                    return Redirect("/Home/nechet4335");
                }
                if (fio == "Петров Петр Петрович" && chetnost2 == "нечётная")
                {
                    return Redirect("/Home/nechet4335");
                }
            }
            catch
            {

            }
            return View();
        }

        public IActionResult nechet4335()
        {
            return View();
        }

        public IActionResult ikg()
        {
            return View();
        }

        public IActionResult ikg_l()
        {
            return View();
        }

        public IActionResult ikg_prep_ikg()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}