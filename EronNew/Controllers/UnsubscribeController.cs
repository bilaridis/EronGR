using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    public class UnsubscribeController : Controller
    {
        // GET: UnsubscribeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UnsubscribeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UnsubscribeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnsubscribeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Confirmed));
            }
            catch
            {
                return View();
            }
        }

        // GET: UnsubscribeController/Edit/5
        public ActionResult Confirmed(int id)
        {
            return View();
        }
    }
}
