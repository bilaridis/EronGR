using EronNew.Data;
using EronNew.Helpers;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    [AllowAnonymous]
    public class CardController : Controller
    {
        private readonly ILogger<CardController> _logger;
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private IDomainModel _model;
        public CardController(ILogger<CardController> logger, IDomainModel model, IEmailSender emailSender, ExtendedUserManager<ExtendedIdentityUser> userManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _model = model;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index(Guid id)
        {
            var dbCard = _model.GetMyCard(id.ToString()) ?? new AspNetUserProfile();

            var dbmodel = new SearchData() { CardProfile = dbCard };
            //ViewBag.app_id = "407388710507530";
            //ViewBag.ogtype = "website";
            //ViewBag.ogtitle = @$"Eron.gr - User Profile ";
            //ViewBag.ogurl = @$"https://www.eron.gr/Card/Index/{dbmodel.CardProfile.AspNetUserId}";
            //ViewBag.ogimage = @$"https://www.eron.gr/{dbmodel.CardProfile.PhotoImage}";
            //ViewBag.ogdescription = @$"{dbmodel.CardProfile.InfoText}";

            return View(dbmodel);
        }

        [AllowAnonymous]
        [Route("Card/Index/ContactUs/{id}")]
        [HttpPost]
        public async Task<IActionResult> IndexContactUs(Guid id, ContactEmail contactEmail)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var email = await _userManager.GetEmailAsync(user);
            await _emailSender.SendEmailAsync(email, "Eron.gr - Contact Form ",
                StaticHelpers.GetContactEmail(contactEmail.name, contactEmail.message, contactEmail.phone, contactEmail.email, contactEmail.subject));
            return RedirectToAction("Index", "Card", new RouteValueDictionary(new { controller = "Card", action = "Index", Id = id }));
        }

        [HttpPost]
        [Route("Card/Query/{id}")]
        [AllowAnonymous]
        public string SearchQuery(string id, SearchData searchData)
        {
            try
            {
                //var user = await _userManager.GetUserAsync(User);
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var results = _model.QueryMyPosts(searchData, id).Result;
                return JsonConvert.SerializeObject(results, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }

        }
    }
}
