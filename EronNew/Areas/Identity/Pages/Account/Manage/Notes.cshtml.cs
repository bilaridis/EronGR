using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EronNew.Data;
using EronNew.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EronNew.Areas.Identity.Pages.Account.Manage
{
    public partial class NotesModel : PageModel
    {
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly SignInManager<ExtendedIdentityUser> _signInManager;
        private IDomainModel _model { get; set; }

        public NotesModel(
            ExtendedUserManager<ExtendedIdentityUser> userManager,
            SignInManager<ExtendedIdentityUser> signInManager,
            IDomainModel model)
        {
            _model = model;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public List<AspNetUserNote> Input { get; set; }

        private async Task LoadAsync(ExtendedIdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            var dbModel = await _model.GetUserNotesAsync(user.Id);
            if(dbModel == null)
            {
                Input = new List<AspNetUserNote>();
            }
            else
            {
                Input = dbModel;
            }
            
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            //Input.AspNetUserId = user.Id;
            //Input.Active = true;
            //Input.Premium = false;
            //Input.Template = 1;

            //if (Input.Image != null)
            //{
            //    string uploads = Path.Combine("\\\\Bilaridis2020\\", "uploads\\" + user.Id + "\\");
            //    string filePath = Path.Combine(uploads, Input.Image.FileName);
            //    bool exists = Directory.Exists(uploads);

            //    if (!exists)
            //        Directory.CreateDirectory(uploads);

            //    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await Input.Image.CopyToAsync(fileStream);
            //    }
            //    Input.PhotoImage = "Uploads/" + user.Id + "/" + Input.Image.FileName;
            //}
            
            //await _model.SaveMyCard(Input);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
