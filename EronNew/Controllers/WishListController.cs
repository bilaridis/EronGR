using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class WishListController : Controller
    {
        private readonly ILogger<WishListController> _logger;

        private IDomainModel _model;
        public WishListController(ILogger<WishListController> logger, IDomainModel model)
        {

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
        [Route("WishList/Query")]
        [AllowAnonymous]
        public string SearchQuery(SearchData searchData)
        {
            try
            {
                var userInformation = User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var results = _model.QueryMyWishList(searchData, userInformation).Result;
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
