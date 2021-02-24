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
    public partial class EditMyCardModel : PageModel
    {
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly SignInManager<ExtendedIdentityUser> _signInManager;
        private IDomainModel _model { get; set; }

        public EditMyCardModel(
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
        public AspNetUserProfile Input { get; set; }

        //public class AspNetUserProfile
        //{
        //    public long Id { get; set; }
        //    public string AspNetUserId { get; set; }
        //    public string PhotoImage { get; set; }
        //    public string Title { get; set; }
        //    public string SubTitle { get; set; }
        //    public string Info { get; set; }
        //    public string InfoText { get; set; }
        //    public string Facebook { get; set; }
        //    public string Twitter { get; set; }
        //    public string LinkedIn { get; set; }
        //    public string EmailAccount { get; set; }
        //    public bool? Premium { get; set; }
        //    public bool? Active { get; set; }
        //    public int? Template { get; set; }
        //}

        private async Task LoadAsync(ExtendedIdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            var dbModel = _model.GetMyCard(user.Id);
            if(dbModel == null)
            {
                Input = new AspNetUserProfile();
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
            Input.AspNetUserId = user.Id;
            Input.Active = true;
            Input.Premium = false;
            Input.Template = 1;

            if (Input.Image != null)
            {
                string uploads = Path.Combine("\\\\Bilaridis2020\\", "uploads\\" + user.Id + "\\");
                string filePath = Path.Combine(uploads, Input.Image.FileName);
                bool exists = Directory.Exists(uploads);

                if (!exists)
                    Directory.CreateDirectory(uploads);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.Image.CopyToAsync(fileStream);
                }
                Input.PhotoImage = "Uploads/" + user.Id + "/" + Input.Image.FileName;
            }
            
            await _model.SaveMyCard(Input);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
