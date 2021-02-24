using EronNew.Data;
using EronNew.Helpers;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private IDomainModel _model;
        public HomeController(
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
            _logger.LogInformation("EronSite - Home Controller starts");
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError("EronSite - " + ex.Message);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string fbclid)
        {
            try
            {
                _logger.LogInformation("EronSite - Home Controller - Action Index starts");
                var viewModel = new PageViewModel();
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                if (fbclid != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                    if (user != null)
                    {
                        var orders = await _model.GetOrdersByOwnerId(user.Id);
                        if (orders.Any(x => x.Product.Id == 4))
                        {
                            viewModel.WelcomeOfferShow = false;
                        }
                        else
                        {
                            await _model.InsertOrder(4, user.Id, null);
                            viewModel.WelcomeOfferShow = true;
                        }
                        await _model.PostInterest(0, user.Id, remoteIpAddress.ToString(), UserInterest.fbclid, fbclid);
                        viewModel.Tiles = _model.GetRelatedPosts(null, user.Id);
                    }
                    else
                    {
                        viewModel.WelcomeOfferShow = false;
                        await _model.PostInterest(0, null, remoteIpAddress.ToString(), UserInterest.fbclid, fbclid, StaticHelpers.GetLocation(remoteIpAddress.ToString()));
                        viewModel.Tiles = _model.GetRelatedPosts();
                    }
                }
                else
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var orders = await _model.GetOrdersByOwnerId(user.Id);
                        if (orders.Any(x => x.Product.Id == 4))
                        {
                            viewModel.WelcomeOfferShow = false;
                        }
                        else
                        {
                            await _model.InsertOrder(4, user.Id, null);
                            viewModel.WelcomeOfferShow = true;
                        }
                        viewModel.Tiles = _model.GetRelatedPosts(null, user.Id);
                    }
                    else
                    {
                        viewModel.WelcomeOfferShow = false;
                        viewModel.Tiles = _model.GetRelatedPosts();
                    }
                }

                _logger.LogInformation("EronSite - Home Controller - Action Index ends");
                return View(viewModel);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                throw ex;
            }

        }

        [AllowAnonymous]
        public IActionResult OurServices()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult AboutUs()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult FAQ()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ContactUs()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ContactUsAsync(ContactEmail contactEmail)
        {
            await _emailSender.SendEmailAsync("contact@eron.gr", "Eron.gr - Contact Us Page ", StaticHelpers.GetContactEmail(contactEmail.name, contactEmail.message, contactEmail.phone, contactEmail.email, contactEmail.subject));
            return View();
        }


        [HttpGet]
        [Route("Home/Search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(SearchData request)
        {
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;
            _model.SetCulture(culture.Name);
            var areas = _model.GetAreas(0);
            var subAreas = _model.GetAreas(request.Area);


            if (culture.Name == "el-GR")
            {
                ViewBag.Areas = new SelectList(await areas, "AreaId", "AreaName");
                ViewBag.SubAreas = new SelectList(await subAreas, "AreaId", "AreaName");
                ViewBag.SubTypes = new SelectList(await _model.GetTypes(request.TypeDesc), "Id", "SubDesc");
            }
            else
            {
                ViewBag.Areas = new SelectList(await areas, "AreaId", "AreaEnglishName");
                ViewBag.SubAreas = new SelectList(await subAreas, "AreaId", "AreaEnglishName");
                ViewBag.SubTypes = new SelectList(await _model.GetTypes(request.TypeDesc), "Id", "SubDescEnglish");
            }

            return View(request);
        }

        [HttpPost]
        [Route("Home/Query")]
        [AllowAnonymous]
        public async Task<string> SearchQueryAsync(SearchData searchData)
        {
            try
            {
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    //await _model.SaveQuerySearch(searchData, user.Id);
                    var results = await _model.QuerySearch(searchData, user.Id);
                    var jsonResult = JsonConvert.SerializeObject(results, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return jsonResult;
                }
                else
                {
                    var results = await _model.QuerySearch(searchData);
                    var jsonResult = JsonConvert.SerializeObject(results, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return jsonResult;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }

        }

        [HttpPost]
        [Route("Home/SaveQuery/{id}")]
        [AllowAnonymous]
        public async Task<string> SaveQueryAsync(string id, SearchData searchData)
        {
            try
            {
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    await _model.SaveQuerySearch(id, searchData, Request.Headers["Referer"].ToString().Split('?')[1], user.Id);
                    return "'Message': 'OK'";
                }
                else
                {
                    return "'Message': 'Not Logged In'";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var ehf = HttpContext.Features.Get<IExceptionHandlerFeature>();
            //ViewData["ErrorMessage"] = ehf.Error.Message;
            var RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError("EronSite - Error Controller starts :" + RequestId + "  | " + ehf.Error.Message + " | " + HttpContext.Request.Path);
            return View(new ErrorViewModel { RequestId = RequestId });
        }
    }
}
