using EronNew.Data;
using EronNew.Helpers;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private IDomainModel _model { get; set; }
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        public NotesController(
            ExtendedUserManager<ExtendedIdentityUser> userManager,
            IDomainModel model)
        {
            _userManager = userManager;
            _model = model;
        }
        // GET api/<SearchController>/5
        [HttpGet("api/GetNotes/{id}")]
        [Authorize]
        public async Task<string> GetNotes(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return "";
            }
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;
            _model.SetCulture(culture.Name);
            var noteDetails = await _model.GetUserNotesAsync(user.Id);
            return noteDetails.FirstOrDefault(x => x.Id == id).ToJson();
        }

        [Authorize]
        [Route("api/SaveNote/{id}")]
        [HttpPost]
        public async Task<string> SaveNote(long id, Input notes)
        {
            var user = await _userManager.GetUserAsync(User);
            if (notes != null)
                await _model.InsertNotes(user.Id, id, notes.notes);
            return "{ \"result\": \"Success\" }";
        }
        public class Input
        {
            public string notes { get; set; }
        }

        [Authorize]
        [Route("api/DeleteNotes/{id}")]
        [HttpPost]
        public async Task<string> DeleteNote(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            await _model.DeleteNoteGroup(id);
            return "Success";
        }

        [Authorize]
        [Route("api/DeleteNote/{id}")]
        [HttpPost]
        public async Task<string> DeleteNoteGroup(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            await _model.DeleteNote(id);
            return "Success";
        }
    }
}
