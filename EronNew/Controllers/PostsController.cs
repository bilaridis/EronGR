using EronNew.Data;
using EronNew.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
namespace EronNew.Models
{
    public class PostsController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IDomainModel _model;
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly SignInManager<ExtendedIdentityUser> _signInManager;
        private readonly ILogger<PostsController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _host;

        public PostsController(
            ILogger<PostsController> logger,
            IDomainModel model,
            IWebHostEnvironment environment,
            IEmailSender emailSender,
            ExtendedUserManager<ExtendedIdentityUser> userManager,
            SignInManager<ExtendedIdentityUser> signInManager,
            IWebHostEnvironment host)
        {
            _host = host;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = environment;
            _model = model;
            _logger = logger;

        }

        // GET: Posts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(long? id, string fbclid)
        {
            try
            {
                ViewData["domain"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var user = await _userManager.GetUserAsync(User);
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var post = await _model.GetPostInformation(id, user?.Id);
                if (post.Post == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (post.Post?.Images.Count() == 0)
                {
                    post.Post.Images.Add(new PostsImages() { UrlImage = "", ImageName = "no_image_available_v1.jpg" });
                }
                ViewBag.Area = "To Show the Area Name";
                if (user != null)
                {

                    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

                    await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.ViewPost, fbclid, StaticHelpers.GetLocation(remoteIpAddress.ToString()));
                }
                else
                {
                    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                    await _model.PostInterest(id.Value, null, remoteIpAddress.ToString(), UserInterest.ViewPost, fbclid, StaticHelpers.GetLocation(remoteIpAddress.ToString()));
                }

                return View(post);
            }
            catch (Exception ex)
            {

                throw new Exception(HttpContext.Request.Path + " - " + ex.ToString());
            }

        }

        // GET: Posts/Create/
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;
            _model.SetCulture(culture.Name);

            if (culture.Name == "el-GR")
            {
                ViewBag.Areas = new SelectList(await _model.GetAreas(0), "AreaId", "AreaName");
            }
            else
            {
                ViewBag.Areas = new SelectList(await _model.GetAreas(0), "AreaId", "AreaEnglishName");
            }


            return View();
            //String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            // return Redirect("Login", "Identity/Account", new 
            // {
            //     redirect_uri = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Post/Create"
            // });
        }

        [Authorize]
        public IActionResult Analytics()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    postViewModel.Post.OwnerId = user.Id;
                    postViewModel.Post.CreatedDate = DateTime.Now;
                    var draftmodel = await _model.CreateDraftPost(postViewModel, user.Id);

                    return Json(postViewModel.Post.id);
                    //var routeValues = new RouteValueDictionary() {
                    //  { "id", draftmodel.Post.id.ToString() }
                    //};

                    //return RedirectToAction("Submit", "Posts", routeValues);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Submit(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var post = await _model.GetPostInformation(id, user.Id);
                if (culture.Name == "el-GR")
                {
                    ViewBag.Areas = new SelectList(await _model.GetAreas(0), "AreaId", "AreaName");
                    ViewBag.SubAreas = new SelectList(await _model.GetAreas(post.Post.Area), "AreaId", "AreaName");
                    ViewBag.SubTypes = new SelectList(await _model.GetTypes(post.Post.TypeId), "Id", "SubDesc");
                }
                else
                {
                    ViewBag.Areas = new SelectList(await _model.GetAreas(0), "AreaId", "AreaEnglishName");
                    ViewBag.SubAreas = new SelectList(await _model.GetAreas(post.Post.Area), "AreaId", "AreaEnglishName");
                    ViewBag.SubTypes = new SelectList(await _model.GetTypes(post.Post.TypeId), "Id", "SubDesc");
                }
                if (post.Post.StateOfPost == 1) { post.Post.StateOfPost = 2; }
                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Submit(long? id, PostViewModel postViewModel)
        {
            try
            {
                postViewModel.Post.StateOfPost = 2;
                if (postViewModel.Post != null && string.IsNullOrEmpty(postViewModel.Post.SearchRawKey))
                    postViewModel.Post.SearchRawKey = Guid.NewGuid().ToString();



                _model.UpdateBasicInformation(id.Value, postViewModel);
                if (postViewModel.ExtraInformation != null)
                    _model.UpdateExtraInformation(id.Value, postViewModel);
                var user = await _userManager.GetUserAsync(User);
                var orders = await _model.GetOrdersByOwnerId(user.Id);
                if (!orders.Any(x => x.Product.Id == 5))
                {
                    await _model.InsertOrder(5, user.Id, id);
                }
                return RedirectToAction("Index", "Administration");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Upload(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture;
                _model.SetCulture(culture.Name);
                var post = await _model.GetPostInformation(id, user.Id);
                var areas = _model.GetAreas(0);
                var subAreas = _model.GetAreas(post.Post.Area);
                if (culture.Name == "el-GR")
                {
                    ViewBag.Areas = new SelectList(await areas, "AreaId", "AreaName");
                    ViewBag.SubAreas = new SelectList(await subAreas, "AreaId", "AreaName");
                    ViewBag.SubTypes = new SelectList(await _model.GetTypes(post.Post.TypeId), "Id", "SubDesc");
                }
                else
                {
                    ViewBag.Areas = new SelectList(await areas, "AreaId", "AreaEnglishName");
                    ViewBag.SubAreas = new SelectList(await subAreas, "AreaId", "AreaEnglishName");
                    ViewBag.SubTypes = new SelectList(await _model.GetTypes(post.Post.TypeId), "Id", "SubDescEnglish");
                }
                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        [HttpGet]
        [Route("Posts/Details/ShowPhones/{id}")]
        [Authorize]
        public async Task<JsonResult> ShowPhones(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                if (user != null)
                {
                    await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.ShowPhones);
                }
                else
                {
                    await _model.PostInterest(id.Value, null, remoteIpAddress.ToString(), UserInterest.ShowPhones);
                }
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }
        [HttpGet]
        [Route("Posts/Details/WishList/{id}")]
        [Authorize]
        public async Task<JsonResult> WishList(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.WishList);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }

        [HttpGet]
        [Route("Posts/Details/UnWishlist/{id}")]
        [Authorize]
        public async Task<JsonResult> UnWishlist(long? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                await _model.PostInterest(id.Value, user.Id, remoteIpAddress.ToString(), UserInterest.UnWishList);
                return Json(new ReturnImageObject() { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new ReturnImageObject() { success = false });
            }
        }

        [Authorize]
        [Route("Posts/Details/ContactUs/{id}")]
        [HttpPost]
        public async Task<IActionResult> DetailsContactUs(Guid id, string postId, ContactEmail contactEmail)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var email = await _userManager.GetEmailAsync(user);
            await _emailSender.SendEmailAsync(email, "Eron.gr - Contact for Ad with Code: " + postId,
                StaticHelpers.GetContactEmail(contactEmail.name, contactEmail.message, contactEmail.phone, contactEmail.email, contactEmail.subject));
            return Json("Success");
        }

        [Authorize]
        [Route("Posts/Details/SaveNotes/{id}")]
        [HttpPost]
        public async Task<IActionResult> SaveNotes(long id, string notes)
        {
            var user = await _userManager.GetUserAsync(User);
            await _model.InsertNotes(user.Id, id, notes);
            return Json("Success");
        }


        private string AspectRatio(int x, int y)
        {
            double value = (double)x / y;
            if (value > 1.7)
                return "16:9";
            else if (value > 1.3333)
                return "4:3";
            else
                return "1:1";
        }

        [HttpDelete]
        [Route("Posts/Submit/DeletePhoto/{id}")]
        [Authorize]
        public JsonResult DeletePhoto(string id, ImageUploader uploader)
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
            _model.DeleteImageOfPost(uploadPath, id);
            return Json(new ReturnImageObject() { success = true });
        }
        [HttpGet]
        [Route("Posts/Submit/GetUploads/{id}")]
        [Authorize]
        public async Task<JsonResult> GetUploads(long? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var dbmodel = await _model.GetPostInformation(id, user.Id);
            var returnJson = dbmodel.GetFineUploader(_logger);
            return Json(returnJson);
        }

        private bool ThumbnailCallback()
        {
            return false;
        }
        [HttpPost]
        [Route("Posts/Submit/Upload/{id}")]
        [Authorize]
        public async Task<JsonResult> Upload(long? id, ImageUploader uploader)
        {
            try
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
                var userInformation = User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                string uploads = Path.Combine(uploadPath, userInformation + "/" + id.Value.ToString() + "/");

                if (uploader.qqfile.Length > 0)
                {
                    var imageEnding = "";
                    using (var image = Image.FromStream(uploader.qqfile.OpenReadStream()))
                    {
                        imageEnding = "." + image.RawFormat.ToString();
                        uploader.width = image.Width;
                        uploader.height = image.Height;
                        //if (AspectRatio(image.Width, image.Height) != "16:9")
                        //{
                        //    return Json(new ReturnImageObject() { success = false, message = "Η Αναλογία της φωτογραφίας πρέπει να είναι 16:9 ή 4:3" });
                        //}

                        string imageFilePath = Path.Combine(uploads, "Images\\" + uploader.qquuid.ToString() + imageEnding);
                        string thubmnailFilePath = Path.Combine(uploads, "Thumbnails\\" + uploader.qquuid.ToString() + imageEnding);
                        bool exists = Directory.Exists(uploads + "Images\\");
                        if (!exists)
                            Directory.CreateDirectory(uploads + "Images\\");

                        bool exists2 = Directory.Exists(uploads + "Thumbnails\\");
                        if (!exists2)
                            Directory.CreateDirectory(uploads + "Thumbnails\\");

                        using (Stream fileStream = new FileStream(imageFilePath, FileMode.Create))
                        {
                            await uploader.qqfile.CopyToAsync(fileStream);
                        }
                        using (Stream fileStream = new FileStream(thubmnailFilePath, FileMode.Create))
                        {
                            var newSize = Size.Empty;
                            newSize.Height = 224;
                            newSize.Width = 398;
                            ImageHelper.ResizeImage(image, newSize).Save(fileStream, image.RawFormat);
                        }
                        _model.InsertImageOfPost(id.Value, "Uploads/" + userInformation + "/" + id.Value.ToString() + "/", uploader, imageEnding, "image");
                    }


                }
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
