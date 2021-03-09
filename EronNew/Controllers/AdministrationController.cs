using EronNew.Data;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IDomainModel _model;
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly SignInManager<ExtendedIdentityUser> _signInManager;
        private readonly ILogger<AdministrationController> _logger;

        public AdministrationController(
            ExtendedUserManager<ExtendedIdentityUser> userManager,
            SignInManager<ExtendedIdentityUser> signInManager, 
            IDomainModel model, 
            IWebHostEnvironment environment,
            ILogger<AdministrationController> logger)
        {
            _logger = logger;
            _hostingEnvironment = environment;
            _model = model;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: AdministrationController
        public ActionResult HomeAsync()
        {
            return View();
        }

        // GET: AdministrationController
        public async Task<ActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            //await _userManager.CreateWallet(user);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;
            _model.SetCulture(culture.Name);
            var entities = await _model.AdminGetPosts(user.Id);
            return View(entities);
        }

        // GET: AdministrationController
        public ActionResult TrafficAsync()
        {
            return View();
        }

        [HttpGet]
        [Route("Administration/Publish/{id}")]
        [Authorize]
        public async Task<JsonResult> Publish(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.PublishPost);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }
        [HttpGet]
        [Route("Administration/Delete/{id}")]
        [Authorize]
        public async Task<JsonResult> Delete(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.DeletePost);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }
        [HttpGet]
        [Route("Administration/Sold/{id}")]
        [Authorize]
        public async Task<JsonResult> Sold(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.SoldPost);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }
        [HttpGet]
        [Route("Administration/Reserve/{id}")]
        [Authorize]
        public async Task<JsonResult> Reserve(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.ReservePost);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }

        [HttpGet]
        [Route("Administration/Hide/{id}")]
        [Authorize]
        public async Task<JsonResult> Hide(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.HidePost);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }
    }
}
