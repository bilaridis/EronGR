using EronNew.Data;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    [Authorize]
    public class RecentPostsController : Controller
    {
        private readonly ILogger<RecentPostsController> _logger;
        private IDomainModel _model;
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;

        public RecentPostsController(ILogger<RecentPostsController> logger, 
            IDomainModel model,
             ExtendedUserManager<ExtendedIdentityUser> userManager)
        {
            _userManager = userManager;
            _model = model;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var userInformation = User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var dbCard = _model.GetMyCard(userInformation) ?? new AspNetUserProfile();

            var dbmodel = new SearchData() { CardProfile = dbCard };
            return View(dbmodel);
        }


        [HttpPost]
        [Route("RecentPosts/Query")]
        [Authorize]
        public async Task<string> SearchQuery(SearchData searchData)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var results = _model.QueryRecentsPostsList(searchData, user.Id).Result;
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
