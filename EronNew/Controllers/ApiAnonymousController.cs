using EronNew.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Localization;
using EronNew.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EronNew.Controllers
{

    [ApiController]
    [AllowAnonymous]
    public class ApiAnonymousController : ControllerBase
    {
        private IDomainModel _model { get; set; }
        public ApiAnonymousController(IDomainModel model)
        {
            _model = model;
        }

        // GET api/<SearchController>/5
        [HttpGet("api/Types/{id}")]
        [AllowAnonymous]
        public Task<string> GetTypes(string id)
        {
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;
            _model.SetCulture(culture.Name);
            if (culture.Name == "el-GR")
            {
                var returnObject = _model.GetTypes(id).Result.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.SubDesc
                });
                return Task.FromResult(returnObject.ToJson());
            }
            else
            {
                var returnObject = _model.GetTypes(id).Result.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.SubDescEnglish
                });
                return Task.FromResult(returnObject.ToJson());
            }
            

            
        }
        // GET api/<SearchController>/5
        [HttpGet("api/GetSubType/{id}")]
        [AllowAnonymous]
        public async Task<string> GetSubType(int id)
        {
            var returnObject = await _model.GetTypes(id);

            return returnObject.ToJson();
        }
        // GET api/<SearchController>/5
        [HttpGet("api/GetArea/{id}")]
        [AllowAnonymous]
        public async Task<string> GetArea(long id)
        {
            var returnObject = await _model.GetArea(id);

            return returnObject.ToJson();
        }

        // GET api/<SearchController>/5
        [HttpGet("api/GetAreas/{id}")]
        [AllowAnonymous]
        public async Task<string> GetAreas(long id)
        {
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;
            _model.SetCulture(culture.Name);
            if (culture.Name == "el-GR")
            {
                var areas = await _model.GetAreas(id);
                var returnObject = areas.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.AreaName
                });
                return returnObject.ToJson();
            }
            else
            {
                var areas = await _model.GetAreas(id);
                var returnObject = areas.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.AreaEnglishName
                });
                return returnObject.ToJson();
            }
            //var returnObject = await _model.GetAreas(id);

            //return returnObject.ToJson();
        }
    }
}
