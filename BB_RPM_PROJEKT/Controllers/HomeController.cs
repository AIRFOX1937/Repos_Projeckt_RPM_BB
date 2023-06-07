using BB_RPM_PROJEKT.AdditionalClasses;
using BB_RPM_PROJEKT.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using BBLib.Models;
using System.Data.Entity;
using Org.BouncyCastle.Asn1.X509;
using BBLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Text.RegularExpressions;

namespace BB_RPM_PROJEKT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public MyAppDbContext db = new MyAppDbContext();
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
            var user = db.Users.Where(u => u.LoginU == log && u.PassU == pass).FirstOrDefault();
            if (user == null)
            {
                ViewBag.H = "Неправильный логин или пароль";
            }
            else
            {
                Class1.user = user;
                switch (user.RoleU)
                {
                    case 1:
                        return Redirect("/Home/NewsA");
                    case 3:
                        return Redirect("/Home/News");
                    case 2:
                        return Redirect("/Home/NewsT");
                }
            }
            return View();
        }

        public IActionResult Disciplins()
        {
            ViewBag.FioU = Class1.user.FioU;
            var disc = from Group in db.Groups
                        join GD in db.GDs on Group.IdG equals GD.IdG
                        join Disc in db.Discs on GD.IdD equals Disc.IdD
                        where GD.IdG == Class1.user.GroupS
                        select new GrDi
                        {
                            IdGD = GD.IdGD,
                            NumG = Group.NumG,
                            NameD = Disc.NameD,
                        };
            return View(disc.OrderBy(x => x.NameD).ToList());
        }

        public IActionResult Papki(string id)
        {
            ViewBag.FioU = Class1.user.FioU;
            ViewBag.D = db.Discs.FirstOrDefault(r => r.NameD == id).NameD.ToString();
            var dis = db.Discs.FirstOrDefault(r => r.NameD == id);
            Class1.disc = dis;
            return View();
        }

        public IActionResult DisciplinsT()
        {
            ViewBag.FioU = Class1.user.FioU;
            var users = from User in db.Users
                        join UD in db.UDs on User.IdU equals UD.IdU
                        join Disc in db.Discs on UD.IdD equals Disc.IdD
                        where UD.IdU == Class1.user.IdU
                        select new UsDi
                        {
                            IdUD = UD.IdUD,
                            FioU = User.FioU,
                            NameD = Disc.NameD,
                        };
            return View(users.OrderBy(x => x.NameD).ToList());
        }

        public IActionResult PapkiT(string id)
        {
            ViewBag.FioU = Class1.user.FioU;
            ViewBag.D = db.Discs.FirstOrDefault(r => r.NameD == id).NameD.ToString();
            var dis = db.Discs.FirstOrDefault(r => r.NameD == id);
            Class1.disc = dis;
            return View();
        }

        public IActionResult DokiT()
        {
            int number = int.Parse(Request.Query["number"]);
            Class1.vidi = db.Vidis.Where(x => x.IdV == number).FirstOrDefault();
            ViewBag.D = Class1.disc.NameD;
            ViewBag.V = db.Vidis.Where(x => x.IdV == number).FirstOrDefault().NameV;
            var doki1 = from User in db.Users
                        join Doki in db.Dokis on User.IdU equals Doki.IdU
                        join Vidi in db.Vidis on Doki.IdV equals Vidi.IdV
                        join Disc in db.Discs on Doki.IdDi equals Disc.IdD
                        where Disc.NameD == Class1.disc.NameD && Doki.IdV == number
                        select new Doki1
                        {
                            IdDo = Doki.IdD,
                            FioU = User.FioU,
                            NameV = Vidi.NameV,
                            NameDo = Doki.NameD,
                            SsilkaD = Doki.SsilkaD,
                            FlagD = Doki.FlagD,
                            NameD = Disc.NameD
                        };
            return View(new List<Doki1>(doki1.OrderBy(x => x.NameDo).ToList()));
        }

        public IActionResult AddDok()
        {
            ViewBag.FioU = Class1.user.FioU;
            ViewBag.VidList = new SelectList(db.Vidis.ToList(), "IdV", "NameV");
            var users = from User in db.Users
                        join UD in db.UDs on User.IdU equals UD.IdU
                        join Disc in db.Discs on UD.IdD equals Disc.IdD
                        where UD.IdU == Class1.user.IdU
                        select new UsDi1
                        {
                            IdUD = UD.IdUD,
                            IdD = UD.IdD,
                            NameD = Disc.NameD,
                        };
            ViewBag.DiscList = new SelectList(users, "IdD", "NameD");
            return View(new Doki1());
        }

        [HttpPost]
        public IActionResult AddDo(string vi, string di, string na, string ss, string fl)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (na != null && ss != null)
            {
                using (var context = new MyAppDbContext())
                {
                    Doki news = new Doki
                    {
                        IdV = Convert.ToInt32(vi),
                        IdDi = Convert.ToInt32(di),
                        NameD = na,
                        SsilkaD = ss,
                        FlagD = Convert.ToInt32(fl),
                        IdU = Class1.user.IdU
                    };
                    context.Dokis.Add(news);
                    context.SaveChanges();
                }
                return Redirect(Request.Headers["Referer"]);
            }
            return RedirectToAction("AddDok");
        }

        public IActionResult EditDok(int id)
        {
            ViewBag.FioU = Class1.user.FioU;
            ViewBag.VidList = new SelectList(db.Vidis.ToList(), "IdV", "NameV");
            var users = from User in db.Users
                        join UD in db.UDs on User.IdU equals UD.IdU
                        join Disc in db.Discs on UD.IdD equals Disc.IdD
                        where UD.IdU == Class1.user.IdU
                        select new UsDi1
                        {
                            IdUD = UD.IdUD,
                            IdD = UD.IdD,
                            NameD = Disc.NameD,
                        };
            var data = from User in db.Users
                        join Doki in db.Dokis on User.IdU equals Doki.IdU
                        join Vidi in db.Vidis on Doki.IdV equals Vidi.IdV
                        join Disc in db.Discs on Doki.IdDi equals Disc.IdD
                        where Doki.IdD == id
                        select new Doki1
                        {
                            IdDo = Doki.IdD,
                            FioU = User.FioU,
                            NameV = Vidi.NameV,
                            NameDo = Doki.NameD,
                            SsilkaD = Doki.SsilkaD,
                            FlagD = Doki.FlagD,
                            NameD = Disc.NameD
                        };
            ViewBag.DiscList = new SelectList(users, "IdD", "NameD");
            return View(data.FirstOrDefault(t => t.IdDo == id));
        }

        [HttpPost]
        public IActionResult SaveDo(string vi, string di, string na, string ss, string fl, int id)
        {
            var news = db.Dokis.Where(x => x.IdD == id).FirstOrDefault();
            news.IdV = Convert.ToInt32(vi);
            news.IdDi = Convert.ToInt32(di);
            news.NameD = na;
            news.SsilkaD = ss;
            news.FlagD = Convert.ToInt32(fl);
            db.SaveChanges();
            return Redirect(Request.Headers["Referer"]);
        }

        public IActionResult DeleteDo(int id)
        {
            var data = db.Dokis.FirstOrDefault(t => t.IdD == id);
            db.Dokis.Remove(data);
            db.SaveChanges();
            return Redirect(Request.Headers["Referer"]);
        }

        public IActionResult OtchetiT(int id)
        {
            ViewBag.D = Class1.disc.NameD;
            ViewBag.V = Class1.vidi.NameV;
            ViewBag.Z = db.Dokis.Where(x => x.IdD == id).FirstOrDefault().NameD;
            var data = from Otcheti in db.Otchetis
                       join User in db.Users on Otcheti.IdU equals User.IdU
                        join Doki in db.Dokis on Otcheti.IdD equals Doki.IdD
                        join Disc in db.Discs on Doki.IdDi equals Disc.IdD
                        join Group in db.Groups on User.GroupS equals Group.IdG
                        where Disc.NameD == Class1.disc.NameD && Doki.IdV == Class1.vidi.IdV && Doki.NameD == db.Dokis.Where(x => x.IdD == id).FirstOrDefault().NameD
            select new Otcheti1
                        {
                            IdO = Otcheti.IdO,
                            NameD = Doki.NameD,
                            FioU = User.FioU,
                            Ssilka = Otcheti.Ssilka,
                            DateO = Otcheti.DateO
                        };
            return View(data.OrderBy(x => x.NameD).ToList());
        }

        public IActionResult News()
        {
            ViewBag.FioU = Class1.user.FioU;
            var news = MyAppDbContext.GetContext().News;
            return View(news.OrderByDescending(x => x.DateN).ToList());
        }

        public IActionResult Doki()
        {
            int number = int.Parse(Request.Query["number"]);
            ViewBag.D = Class1.disc.NameD;
            ViewBag.V = db.Vidis.Where(x => x.IdV == number).FirstOrDefault().NameV;
            var doki1 = from User in db.Users
                        join Doki in db.Dokis on User.IdU equals Doki.IdU
                        join Vidi in db.Vidis on Doki.IdV equals Vidi.IdV
                        join Disc in db.Discs on Doki.IdDi equals Disc.IdD
                        where Disc.NameD == Class1.disc.NameD && Doki.IdV == number
                        select new Doki1
                        {
                            IdDo = Doki.IdD,
                            FioU = User.FioU,
                            NameV = Vidi.NameV,
                            NameDo = Doki.NameD,
                            SsilkaD = Doki.SsilkaD,
                            FlagD = Doki.FlagD,
                            NameD = Disc.NameD
                        };
            var doki2 = from User in db.Users
                       join Doki in db.Dokis on User.IdU equals Doki.IdU
                       join Vidi in db.Vidis on Doki.IdV equals Vidi.IdV
                       join Disc in db.Discs on Doki.IdDi equals Disc.IdD
                       where Disc.NameD == Class1.disc.NameD && Doki.IdV == number && Doki.FlagD == 1
                       select new Doki1
                       {
                           IdDo = Doki.IdD,
                           FioU = User.FioU,
                           NameV = Vidi.NameV,
                           NameDo = Doki.NameD,
                           SsilkaD = Doki.SsilkaD,
                           FlagD = Doki.FlagD,
                           NameD = Disc.NameD
                       };
            return View(new Tuple<List<Doki1>, List<Doki1>>(doki1.OrderBy(x => x.NameD).ToList(), doki2.OrderBy(x => x.NameD).ToList()));
        }

        public IActionResult News1(int id)
        {
            ViewBag.FioU = Class1.user.FioU;

            var news = db.News.FirstOrDefault(r => r.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        public IActionResult News2(int id)
        {
            ViewBag.FioU = Class1.user.FioU;

            var news = db.News.FirstOrDefault(r => r.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        public IActionResult NewsT()
        {
            ViewBag.FioU = Class1.user.FioU;
            var news = MyAppDbContext.GetContext().News;
            return View(news.OrderByDescending(x => x.DateN).ToList());
        }

        public IActionResult NewsA()
        {
            ViewBag.FioU = Class1.user.FioU;
            var news = from News in db.News
                        select new News
                        {
                            Id = News.Id,
                            Img = News.Img,
                            DateN = News.DateN,
                            Zag = News.Zag,
                            Txt = News.Txt,
                            Txt1 = News.Txt1
                        };
            return View(news.OrderByDescending(x => x.DateN).ToList());
        }

        public IActionResult AddNews()
        {
            ViewBag.FioU = Class1.user.FioU;
            return View(new News());
        }

        public IActionResult EditNews(int id)
        {
            var data = db.News.FirstOrDefault(t => t.Id == id);
            return View(data);
        }

        [HttpPost]
        public IActionResult SaveN(string im, string za, string tx, string tx1, int id)
        {
            var news = db.News.Where(x => x.Id == id).FirstOrDefault();
            news.Img = im;
            news.Zag = za;
            news.Txt = tx;
            news.Txt1 = tx1;
            db.SaveChanges();
            return RedirectToAction("NewsA");
        }

        public IActionResult DeleteN(int id)
        {
            var data = db.News.FirstOrDefault(t => t.Id == id);
            db.News.Remove(data);
            db.SaveChanges();
            return RedirectToAction("NewsA");
        }

        [HttpPost]
        public IActionResult AddN(string im, string za, string tx, string tx1)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (ModelState.IsValid)
            {
                using (var context = new MyAppDbContext())
                {
                    News news = new News
                    {
                        Img = im,
                        Zag = za,
                        Txt = tx,
                        DateN = DateTime.Now,
                        Txt1 = tx1
                    };
                    context.News.Add(news);
                    context.SaveChanges();
                }
                return RedirectToAction("NewsA");
            }
            return RedirectToAction("AddNews");
        }

        public IActionResult AddOtchet(int id)
        {
            ViewBag.FioU = Class1.user.FioU;
            return View(new Otcheti() { IdD = id});
        }

        [HttpPost]
        public IActionResult AddO(string im, int a)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (ModelState.IsValid)
            {
                using (var context = new MyAppDbContext())
                {
                    Otcheti news = new Otcheti
                    {
                        IdD = a,
                        IdU = Class1.user.IdU,
                        Ssilka = im,
                        DateO = DateTime.Now
                    };
                    context.Otchetis.Add(news);
                    context.SaveChanges();
                }
                return Redirect(Request.Headers["Referer"]);
            }
            return Redirect(Request.Headers["Referer"]);
        }

        public IActionResult AddDiscs()
        {
            ViewBag.FioU = Class1.user.FioU;
            return View(new Disc());
        }

        [HttpPost]
        public IActionResult AddD(string na)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (ModelState.IsValid)
            {
                using (var context = new MyAppDbContext())
                {
                    Disc news = new Disc
                    {
                        NameD = na
                    };
                    context.Discs.Add(news);
                    context.SaveChanges();
                }
                return RedirectToAction("DiscsA");
            }
            return RedirectToAction("AddDiscs");
        }

        public IActionResult DeleteD(int id)
        {
            var data = db.Discs.FirstOrDefault(t => t.IdD == id);
            db.Discs.Remove(data);
            db.SaveChanges();
            return RedirectToAction("DiscsA");
        }

        public IActionResult EditDiscs(int id)
        {
            var data = db.Discs.FirstOrDefault(t => t.IdD == id);
            return View(data);
        }

        [HttpPost]
        public IActionResult SaveD(string na, int id)
        {
            var news = db.Discs.Where(x => x.IdD == id).FirstOrDefault();
            news.NameD = na;
            db.SaveChanges();
            return RedirectToAction("DiscsA");
        }

        public IActionResult UseriA()
        {
            ViewBag.FioU = Class1.user.FioU;
            var users = from User in db.Users join Role in db.Roles on User.RoleU equals Role.IdR
                        join Group in db.Groups on User.GroupS equals (int?)Group.IdG into gj
                        from Group in gj.DefaultIfEmpty()
                        select new UserRoleGroupcs
                        {
                            IdU = User.IdU,
                            LoginU = User.LoginU,
                            PassU = User.PassU,
                            RoleU = Role.NameR,
                            FioU = User.FioU,
                            GroupU = Group != null ? Group.NumG : null
                        };
            return View(users.OrderBy(x => x.FioU).ToList());
        }

        public IActionResult AddUser()
        {
            ViewBag.FioU = Class1.user.FioU;
            ViewBag.RoleList = new SelectList(db.Roles.ToList(), "IdR", "NameR");
            ViewBag.GroupList = new SelectList(db.Groups.ToList(), "IdG", "NumG");
            return View(new User());
        }

        [HttpPost]
        public IActionResult AddU(string lo, string pa, string ro, string fi, string gr)
        {
            ViewBag.FioU = Class1.user.FioU;
            if(lo != null && pa!= null && fi != null)
            {
                if(MyAppDbContext.GetContext().Users.Where(x => x.LoginU == lo).FirstOrDefault() == null)
                {
                    using (var context = new MyAppDbContext())
                    {
                        if (ro == "3")
                        {
                            User news = new User
                            {
                                LoginU = lo,
                                PassU = pa,
                                RoleU = 3,
                                FioU = fi,
                                GroupS = Convert.ToInt32(gr)
                            };
                            context.Users.Add(news);
                            context.SaveChanges();
                        }
                        else
                        {
                            User news = new User
                            {
                                LoginU = lo,
                                PassU = pa,
                                RoleU = Convert.ToInt32(ro),
                                FioU = fi,
                                GroupS = null
                            };
                            context.Users.Add(news);
                            context.SaveChanges();
                        }
                    }
                    return RedirectToAction("UseriA");
                }
            }
            return RedirectToAction("AddUser");
        }

        public IActionResult DeleteU(int id)
        {
            var data = db.Users.FirstOrDefault(t => t.IdU == id);
            db.Users.Remove(data);
            db.SaveChanges();
            return RedirectToAction("UseriA");
        }

        public IActionResult EditUser(int id)
        {
            var data = db.Users.FirstOrDefault(t => t.IdU == id);
            ViewBag.RoleList = new SelectList(db.Roles.ToList(), "IdR", "NameR");
            ViewBag.GroupList = new SelectList(db.Groups.ToList(), "IdG", "NumG");
            return View(data);
        }

        [HttpPost]
        public IActionResult SaveU(string lo, string pa, string ro, string fi, string gr, int id)
        {
            var news = db.Users.Where(x => x.IdU == id).FirstOrDefault();
            if (ro == "3")
            {
                news.LoginU = lo;
                news.PassU = pa;
                news.RoleU = 3;
                news.FioU = fi;
                news.GroupS = Convert.ToInt32(gr);
            }
            else
            {
                news.LoginU = lo;
                news.PassU = pa;
                news.RoleU = Convert.ToInt32(ro);
                news.FioU = fi;
                news.GroupS = null;
            }
            db.SaveChanges();
            return RedirectToAction("UseriA");
        }

        public IActionResult AddGroup()
        {
            ViewBag.FioU = Class1.user.FioU;
            return View(new BBLib.Models.Group());
        }

        [HttpPost]
        public IActionResult AddG(string nu)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (ModelState.IsValid)
            {
                if (MyAppDbContext.GetContext().Groups.Where(x => x.NumG == nu).FirstOrDefault() == null)
                {
                    using (var context = new MyAppDbContext())
                    {
                        BBLib.Models.Group news = new BBLib.Models.Group
                        {
                            NumG = nu
                        };
                        context.Groups.Add(news);
                        context.SaveChanges();
                    }
                    return RedirectToAction("GroupsA");
                }
            }
            return RedirectToAction("AddGroup");
        }

        public IActionResult DeleteG(int id)
        {
            var data = db.Groups.FirstOrDefault(t => t.IdG == id);
            db.Groups.Remove(data);
            db.SaveChanges();
            return RedirectToAction("GroupsA");
        }

        public IActionResult EditGroup(int id)
        {
            var data = db.Groups.FirstOrDefault(t => t.IdG == id);
            return View(data);
        }

        [HttpPost]
        public IActionResult SaveG(string nu, int id)
        {
            var news = db.Groups.Where(x => x.IdG == id).FirstOrDefault();
            news.NumG = nu;
            db.SaveChanges();
            return RedirectToAction("GroupsA");
        }

        public IActionResult GroupDisc()
        {
            ViewBag.FioU = Class1.user.FioU;
            var users = from Group in db.Groups
                        join GD in db.GDs on Group.IdG equals GD.IdG
                        join Disc in db.Discs on GD.IdD equals Disc.IdD
                        select new GrDi
                        {
                            IdGD = GD.IdGD,
                            NumG = Group.NumG,
                            NameD = Disc.NameD,
                        };
            return View(users.OrderBy(x => x.NumG).ToList());
        }

        public IActionResult AddGD()
        {
            ViewBag.FioU = Class1.user.FioU;
            ViewBag.GroupList = new SelectList(db.Groups.ToList(), "IdG", "NumG");
            ViewBag.DiscList = new SelectList(db.Discs.ToList(), "IdD", "NameD");
            return View(new GD());
        }

        [HttpPost]
        public IActionResult AddGrDi(string gr, string di)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (gr != null && di != null)
            {
                if (MyAppDbContext.GetContext().GDs.Where(x => x.IdG == Convert.ToInt32(gr) && x.IdD == Convert.ToInt32(di)).FirstOrDefault() == null)
                {
                    using (var context = new MyAppDbContext())
                    {
                        GD news = new GD
                        {
                            IdG = Convert.ToInt32(gr),
                            IdD = Convert.ToInt32(di)
                        };
                        context.GDs.Add(news);
                        context.SaveChanges();
                    }
                    return RedirectToAction("GroupDisc");
                }
            }
            return RedirectToAction("AddGD");
        }

        public IActionResult DeleteGD(int id)
        {
            var data = db.GDs.FirstOrDefault(t => t.IdGD == id);
            db.GDs.Remove(data);
            db.SaveChanges();
            return RedirectToAction("GroupDisc");
        }

        public IActionResult EditGD(int id)
        {
            var data = db.GDs.FirstOrDefault(t => t.IdGD == id);
            ViewBag.GroupList = new SelectList(db.Groups.ToList(), "IdG", "NumG");
            ViewBag.DiscList = new SelectList(db.Discs.ToList(), "IdD", "NameD");
            return View(data);
        }

        [HttpPost]
        public IActionResult SaveGD(string di, string gr, int id)
        {
            var news = db.GDs.Where(x => x.IdGD == id).FirstOrDefault();
            news.IdG = Convert.ToInt32(gr);
            news.IdD = Convert.ToInt32(di);
            db.SaveChanges();
            return RedirectToAction("GroupDisc");
        }

        public IActionResult UserDisc()
        {
            ViewBag.FioU = Class1.user.FioU;
            var users = from User in db.Users
                        join UD in db.UDs on User.IdU equals UD.IdU
                        join Disc in db.Discs on UD.IdD equals Disc.IdD
                        select new UsDi
                        {
                            IdUD = UD.IdUD,
                            FioU = User.FioU,
                            NameD = Disc.NameD,
                        };
            return View(users.OrderBy(x => x.FioU).ToList());
        }

        public IActionResult AddUD()
        {
            ViewBag.FioU = Class1.user.FioU;
            ViewBag.GroupList = new SelectList(db.Users.Where(x => x.RoleU == 2).ToList(), "IdU", "FioU");
            ViewBag.DiscList = new SelectList(db.Discs.ToList(), "IdD", "NameD");
            return View(new UD());
        }

        [HttpPost]
        public IActionResult AddUsDi(string gr, string di)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (gr != null && di != null)
            {
                if (MyAppDbContext.GetContext().UDs.Where(x => x.IdU == Convert.ToInt32(gr) && x.IdD == Convert.ToInt32(di)).FirstOrDefault() == null)
                {
                    using (var context = new MyAppDbContext())
                    {
                        UD news = new UD
                        {
                            IdU = Convert.ToInt32(gr),
                            IdD = Convert.ToInt32(di)
                        };
                        context.UDs.Add(news);
                        context.SaveChanges();
                    }
                    return RedirectToAction("UserDisc");
                }
            }
            return RedirectToAction("AddUD");
        }

        public IActionResult DeleteUD(int id)
        {
            var data = db.UDs.FirstOrDefault(t => t.IdUD == id);
            db.UDs.Remove(data);
            db.SaveChanges();
            return RedirectToAction("UserDisc");
        }

        public IActionResult EditUD(int id)
        {
            var data = db.UDs.FirstOrDefault(t => t.IdUD == id);
            ViewBag.GroupList = new SelectList(db.Users.Where(x => x.RoleU == 2).ToList(), "IdU", "FioU");
            ViewBag.DiscList = new SelectList(db.Discs.ToList(), "IdD", "NameD");
            return View(data);
        }

        [HttpPost]
        public IActionResult SaveUD(string di, string gr, int id)
        {
            var news = db.UDs.Where(x => x.IdUD == id).FirstOrDefault();
            news.IdU = Convert.ToInt32(gr);
            news.IdD = Convert.ToInt32(di);
            db.SaveChanges();
            return RedirectToAction("UserDisc");
        }

        public IActionResult DiscsA()
        {
            ViewBag.FioU = Class1.user.FioU;
            var discs = from Disc in db.Discs
                        select new Disc
                        {
                            IdD = Disc.IdD,
                            NameD = Disc.NameD,
                        };
            return View(discs.OrderBy(x => x.NameD).ToList());
        }

        public IActionResult GroupsA()
        {
            ViewBag.FioU = Class1.user.FioU;
            var groups = from Group in db.Groups
                        select new BBLib.Models.Group
                        {
                            IdG = Group.IdG,
                            NumG = Group.NumG,
                        };
            return View(groups.OrderBy(x => x.NumG).ToList());
        }


        public IActionResult Raspisanie()
        {
            ViewBag.FioU = Class1.user.FioU;
            var products1 = MyAppDbContext.GetContext().Groups.ToList();
            var products2 = MyAppDbContext.GetContext().Chets.ToList();
            var products3 = MyAppDbContext.GetContext().Users.Where(x => x.RoleU == 2).ToList();
            return View(new Tuple<List<BBLib.Models.Group>, List<Chet>, List<User>>(products1, products2, products3));
        }

        public IActionResult RaspisSelect(int id, int id1)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (id1 == 1 || id1 == 2)
            {
                var users1 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 1 && (Raspi.IdC == id1 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users2 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 2 && (Raspi.IdC == id1 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users3 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 3 && (Raspi.IdC == id1 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users4 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 4 && (Raspi.IdC == id1 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users5 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 5 && (Raspi.IdC == id1 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users6 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 6 && (Raspi.IdC == id1 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                if (MyAppDbContext.GetContext().Groups.Where(x => x.IdG == id).FirstOrDefault() != null)
                {
                    ViewBag.Gr = MyAppDbContext.GetContext().Groups.Where(x => x.IdG == id).FirstOrDefault().NumG;
                }
                if (id1 == 1)
                    ViewBag.Ch = "четная неделя";
                if (id1 == 2)
                    ViewBag.Ch = "нечетная неделя";
                if (id1 == 3)
                    ViewBag.Ch = "четная/нечетная неделя";
                return View(new Tuple<List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>>(users1.ToList(), users2.ToList(), users3.ToList(), users4.ToList(), users5.ToList(), users6.ToList()));
            }
            else
            {
                var users1 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 1 && (Raspi.IdC == id1 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users2 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 2 && (Raspi.IdC == id1 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users3 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 3 && (Raspi.IdC == id1 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users4 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 4 && (Raspi.IdC == id1 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users5 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 5 && (Raspi.IdC == id1 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users6 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdG == id && Raspi.IdN == 6 && (Raspi.IdC == id1 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                if (MyAppDbContext.GetContext().Groups.Where(x => x.IdG == id).FirstOrDefault() != null)
                {
                    ViewBag.Gr = MyAppDbContext.GetContext().Groups.Where(x => x.IdG == id).FirstOrDefault().NumG;
                }
                if (id1 == 1)
                    ViewBag.Ch = "четная неделя";
                if (id1 == 2)
                    ViewBag.Ch = "нечетная неделя";
                if (id1 == 3)
                    ViewBag.Ch = "четная/нечетная неделя";
                return View(new Tuple<List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>>(users1.ToList(), users2.ToList(), users3.ToList(), users4.ToList(), users5.ToList(), users6.ToList()));
            }
        }

        public IActionResult RaspisSelect1(int id3, int id4)
        {
            ViewBag.FioU = Class1.user.FioU;
            if (id3 == 1 || id4 == 2)
            {
                var users1 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 1 && (Raspi.IdC == id4 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users2 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 2 && (Raspi.IdC == id4 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users3 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 3 && (Raspi.IdC == id4 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users4 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 4 && (Raspi.IdC == id4 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users5 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 5 && (Raspi.IdC == id4 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users6 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 6 && (Raspi.IdC == id4 || Raspi.IdC == 3)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                ViewBag.Gr = MyAppDbContext.GetContext().Users.Where(x => x.IdU == id3).FirstOrDefault().FioU;
                if (id4 == 1)
                    ViewBag.Ch = "четная неделя";
                if (id4 == 2)
                    ViewBag.Ch = "нечетная неделя";
                if (id4 == 3)
                    ViewBag.Ch = "четная/нечетная неделя";
                return View(new Tuple<List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>>(users1.ToList(), users2.ToList(), users3.ToList(), users4.ToList(), users5.ToList(), users6.ToList()));
            }
            else
            {
                var users1 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 1 && (Raspi.IdC == id4 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users2 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 2 && (Raspi.IdC == id4 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users3 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 3 && (Raspi.IdC == id4 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users4 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 4 && (Raspi.IdC == id4 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users5 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 5 && (Raspi.IdC == id4 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                var users6 = from Raspi in db.Raspis
                             join Group in db.Groups on Raspi.IdG equals Group.IdG
                             join Chet in db.Chets on Raspi.IdC equals Chet.IdC
                             join Nedeli in db.Nedelis on Raspi.IdN equals Nedeli.IdN
                             join Time in db.Times on Raspi.IdT equals Time.IdT
                             join Disc in db.Discs on Raspi.IdD equals Disc.IdD
                             join Vidi in db.Vidis on Raspi.IdV equals Vidi.IdV
                             join User in db.Users on Raspi.IdU equals User.IdU
                             where Raspi.IdU == id3 && Raspi.IdN == 6 && (Raspi.IdC == id4 || Raspi.IdC == 1 || Raspi.IdC == 2)
                             orderby Raspi.IdT
                             select new Raspi1
                             {
                                 IdR = Raspi.IdR,
                                 NumG = Group.NumG,
                                 NameC = Chet.NameC,
                                 NameN = Nedeli.NameN,
                                 NameT = Time.NameT,
                                 DateR = Raspi.DateR,
                                 NameD = Disc.NameD,
                                 NameV = Vidi.NameV,
                                 AudR = Raspi.AudR,
                                 ZdR = Raspi.ZdR,
                                 FioU = User.FioU,
                             };
                ViewBag.Gr = MyAppDbContext.GetContext().Users.Where(x => x.IdU == id3).FirstOrDefault().FioU;
                if (id4 == 1)
                    ViewBag.Ch = "четная неделя";
                if (id4 == 2)
                    ViewBag.Ch = "нечетная неделя";
                if (id4 == 3)
                    ViewBag.Ch = "четная/нечетная неделя";
                return View(new Tuple<List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>, List<Raspi1>>(users1.ToList(), users2.ToList(), users3.ToList(), users4.ToList(), users5.ToList(), users6.ToList()));
            }
        }

        public IActionResult RaspisanieT()
        {
            ViewBag.FioU = Class1.user.FioU;
            var products1 = MyAppDbContext.GetContext().Groups.ToList();
            var products2 = MyAppDbContext.GetContext().Chets.ToList();
            var products3 = MyAppDbContext.GetContext().Users.Where(x => x.RoleU == 2).ToList();
            return View(new Tuple<List<BBLib.Models.Group>, List<Chet>, List<User>>(products1, products2, products3));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}