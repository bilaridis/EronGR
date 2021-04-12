using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EronNew.Data;
using EronNew.Models;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _host;

        public EditMyCardModel(
            ExtendedUserManager<ExtendedIdentityUser> userManager,
            SignInManager<ExtendedIdentityUser> signInManager,
            IDomainModel model,
            IWebHostEnvironment host)
        {
            _host = host;
            _model = model;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public AspNetUserProfile Input { get; set; }

        public string Domain { get; set; }


        private async Task LoadAsync(ExtendedIdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            var dbModel = _model.GetMyCard(user.Id);
            if (dbModel == null)
            {
                Input = new AspNetUserProfile();
            }
            else
            {
                Input = dbModel;
                //Input.SetDescription();
            }
            Domain = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}//";

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
                var uploadPath = "\\\\Bilaridis2020\\BetaSite\\";
                if (_host.EnvironmentName == "Production")
                {
                    uploadPath = "\\\\Bilaridis2020\\ProdSite\\";
                }
                else if (_host.ContentRootPath == "E:\\Repos\\EronNew\\EronNew")
                {
                    uploadPath = "\\\\Bilaridis2020\\BetaSite\\";
                }
                string uploads = Path.Combine(uploadPath, "" + user.Id + "\\");
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
            else
            {
                Input.PhotoImage = null;
            }
            Domain = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}//";
            await _model.SaveMyCard(Input);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
