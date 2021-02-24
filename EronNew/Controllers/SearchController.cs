using EronNew.Data;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private IDomainModel _model;
        public SearchController(
            ILogger<HomeController> logger,
            IDomainModel model,
            IEmailSender emailSender,
            ExtendedUserManager<ExtendedIdentityUser> userManager
            )
        {
            _emailSender = emailSender;
            _logger = logger;
            _model = model;
            _userManager = userManager;
        }

        // GET: SearchesController
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _model.GetSavedSearchedByOwnerId(user.Id,1));
        }


        [HttpGet]
        [Route("Search/Delete/{id}")]
        [Authorize]
        public async Task<JsonResult> DeleteQuery(Guid id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.DeleteQuerySearch(id, user.Id);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }



        // GET: SearchesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SearchesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SearchesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SearchesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
