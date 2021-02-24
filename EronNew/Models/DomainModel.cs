using EronNew.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Watson.ORM.Core;

namespace EronNew.Models
{
    public enum UserInterest { WishList = 0, ShowPhones = 1, RequestInformation = 2, ViewPost = 3, DeletePost = 4, PublishPost = 5, SoldPost = 6, UnWishList = 7, ReservePost = 8, HidePost = 9, fbclid = 10 }
    public class DomainModel : IDomainModel, IDisposable
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<DomainModel> _logger;
        private string _cultureName;
        private bool disposedValue;
        public DomainModel(ILogger<DomainModel> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

        }
        public void SetCulture(string cultureName)
        {
            _cultureName = cultureName;
        }
        public async Task<Areas> GetArea(long Id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var currentArea = await context.Areas.FirstOrDefaultAsync(x => x.Id == Id);
            return currentArea;
        }
        public async Task<IEnumerable<TypesModel>> GetTypes(string desc)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            return await context.Types.Where(x => x.Desc == desc).ToListAsync();
        }
        public async Task<IEnumerable<TypesModel>> GetTypes(int id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            return await context.Types.Where(x => x.Id == id).ToListAsync();
        }
        public async Task<PostsModel> GetPost(long? id)
        {
            try
            {
                if (id == null) return null;

                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                var modelPost = await context.Posts
                    .Include(p => p.Owner)
                    .Include(p => p.SubTypeInformation)
                    .Include(p => p.Areas)
                    .Include(p => p.SubAreas)
                    .Include(p => p.Images).FirstOrDefaultAsync(x => x.id == id);

                return modelPost;

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return null;
            }
        }
        public async void DeleteImageOfPost(string path, string imageName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                var entity = await context.PostsImages.FirstOrDefaultAsync(x => x.ImageName.Contains(imageName));
                if (entity != null)
                {
                    var imagePath = path + entity.UrlImage.Replace("/", "\\") + "\\Images\\" + entity.ImageName;
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    var thumbPath = path + entity.UrlImage.Replace("/", "\\") + "\\Thumbnails\\" + entity.ImageName;
                    if (File.Exists(thumbPath))
                    {
                        File.Delete(thumbPath);
                    }
                    context.PostsImages.Remove(entity);
                    await context.SaveChangesAsync();
                }

            }
        }
        public AspNetUserProfile GetMyCard(string aspNetUserId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                var returnModel = context.AspNetUserProfiles.FirstOrDefault(x => x.AspNetUserId == aspNetUserId);
                return returnModel;
            }
        }
        public List<IPage> GetRelatedPosts(long? postId = null, string aspNetUserId = null)
        {
            var tiles = new List<IPage>();
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();

            var res = context.Posts.Include(p => p.ExtraInformation)
                .Include(p => p.Owner)
                .Include(p => p.SubTypeInformation)
                .Include(p => p.Areas)
                .Include(p => p.SubAreas)
                .IncludeLocaliseOptions(_cultureName)
                .Include(p => p.Images)
                .Include(w => w.WishLists)
               .Where(x => x.Active && x.StateOfPost == 2 && !x.Deleted && !x.Hide);

            if (postId != null) res = res.Where(x => postId.Value != x.id);

            return res.OrderByDescending(x => x.CreatedDate.Value).Take(10).ToList().GetBase(10, aspNetUserId);
        }
        public async Task<bool> SaveMyCard(AspNetUserProfile model)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                var result = false;
                var modelToUpdate = await context.AspNetUserProfiles.FindAsync(model.Id);
                if (modelToUpdate != null)
                {
                    context.Entry(modelToUpdate).CurrentValues.SetValues(model);
                    context.SaveChanges();
                    result = true;
                }
                else
                {
                    context.AspNetUserProfiles.Add(model);
                    context.SaveChanges();
                    result = true;
                }
                return result;
            }
        }
        public async Task<IEnumerable<Areas>> GetAreas(long parentId)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<IronKeyContext>();
                    if (parentId == -1)
                    {
                        var areas = context.Areas.ToList();
                        var parentAreas = areas.Where(x => x.ParentAreaId == 0).ToList();
                        foreach (Areas children in areas.Where(x => parentId > 0))
                        {
                            children.AreaName = parentAreas.FirstOrDefault(x => x.ParentAreaId == children.ParentAreaId).AreaName + " - " + children.AreaName;
                        }
                        return areas;
                    }
                    else
                        return await context.Areas.Where(x => x.ParentAreaId == parentId).ToListAsync();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return null;
            }

        }
        public async Task<List<IPage>> QuerySearch(SearchData searchData, string aspNetUserId = null)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var results = context.Posts
                .Include(p => p.ExtraInformation)
                .Include(p => p.Owner)
                .Include(p => p.SubTypeInformation)
                .Include(p => p.Areas)
                .Include(p => p.SubAreas)
                .Include(w => w.WishLists)
                .IncludeLocaliseOptions(_cultureName)
                .Include(p => p.Images).AsQueryable().Where(x => x.Active && x.StateOfPost == 2 && !x.Deleted && !x.Hide);
            if (searchData.Bathroom.HasValue) results = results.Where(x => x.Bathroom >= searchData.Bathroom);
            if (searchData.Bedroom.HasValue) results = results.Where(x => x.Bedroom >= searchData.Bedroom);
            if (searchData.SaleCategory != null) results = results.Where(x => x.SaleCategory == searchData.SaleCategory);
            if (searchData.EnergyEfficiency != null) results = results.Where(x => x.EnergyEfficiency == searchData.EnergyEfficiency);
            if (searchData.TypeDesc != null) results = results.Where(x => searchData.TypeDesc != "" && x.TypeId == searchData.TypeDesc);
            if (searchData.SubType.Count > 0) results = results.Where(x => searchData.SubType.Contains(x.SubTypeId.Value));
            if (searchData.Area > 0) results = results.Where(x => x.Area == searchData.Area);
            if (searchData.SubArea.Count > 0) results = results.Where(x => searchData.SubArea.Contains(x.SubAreaId));
            if (searchData.conYearFrom > 0) results = results.Where(x => x.ConstructionYear.HasValue && x.ConstructionYear >= searchData.conYearFrom);
            if (searchData.conYearTo > 0) results = results.Where(x => x.ConstructionYear.HasValue && x.ConstructionYear <= searchData.conYearTo);
            if (searchData.renYearFrom > 0) results = results.Where(x => x.RenovationYear.HasValue && x.RenovationYear >= searchData.renYearFrom);
            if (searchData.renYearTo > 0) results = results.Where(x => x.RenovationYear.HasValue && x.RenovationYear <= searchData.renYearTo);
            if (searchData.squaresFrom > 0) results = results.Where(x => x.Square.HasValue && x.Square >= searchData.squaresFrom);
            if (searchData.squaresTo > 0) results = results.Where(x => x.Square.HasValue && x.Square <= searchData.squaresTo);
            if (searchData.priceFrom > 0) results = results.Where(x => x.PriceTotal.HasValue && x.PriceTotal >= searchData.priceFrom);
            if (searchData.priceTo > 0) results = results.Where(x => x.PriceTotal.HasValue && x.PriceTotal <= searchData.priceTo);
            if (searchData.PetAllowed == "on") results = results.Where(x => x.PetAllowed == true);
            if (searchData.ParkingArea == "on") results = results.Where(x => x.ParkingArea == true);
            if (searchData.AirCondition == "on") results = results.Where(x => x.ExtraInformation.First().AirCondition == true);
            if (searchData.Bbq == "on") results = results.Where(x => x.ExtraInformation.First().Bbq == true);
            if (searchData.Elevator == "on") results = results.Where(x => x.ExtraInformation.First().Elevator == true);
            if (searchData.Fireplace == "on") results = results.Where(x => x.ExtraInformation.First().Fireplace == true);
            if (searchData.Garden == "on") results = results.Where(x => x.ExtraInformation.First().Garden == true);
            //public int? GardenSpace) results = results.Where(x => x.ExtraInformation.First().);
            if (searchData.Gym == "on") results = results.Where(x => x.ExtraInformation.First().Gym == true);
            if (searchData.Hall == "on") results = results.Where(x => x.ExtraInformation.First().Hall == true);
            if (searchData.Heating == "on") results = results.Where(x => x.ExtraInformation.First().Heating == true);
            //public string HeatingSystem) results = results.Where(x => x.ExtraInformation.First().);
            if (searchData.Kitchen == "on") results = results.Where(x => x.ExtraInformation.First().Kitchen == true);
            if (searchData.Livingroom == "on") results = results.Where(x => x.ExtraInformation.First().Livingroom == true);
            if (searchData.Maidroom == "on") results = results.Where(x => x.ExtraInformation.First().Maidroom == true);
            if (searchData.Master == "on") results = results.Where(x => x.ExtraInformation.First().Master == true);
            if (searchData.RoofFloor == "on") results = results.Where(x => x.ExtraInformation.First().RoofFloor == true);
            if (searchData.SemiOutdoor == "on") results = results.Where(x => x.ExtraInformation.First().SemiOutdoor == true);
            //public int? SemiOutdoorSquare) results = results.Where(x => x.ExtraInformation.First().);
            if (searchData.Storageroom == "on") results = results.Where(x => x.ExtraInformation.First().Storageroom == true);
            //public int? StorageroomSquare) results = results.Where(x => x.ExtraInformation.First().);
            if (searchData.Swimmingpool == "on") results = results.Where(x => x.ExtraInformation.First().Swimmingpool == true);
            if (searchData.Wc == "on") results = results.Where(x => x.ExtraInformation.First().Wc == true);
            if (searchData.HouseKeepingMoney == "on") results = results.Where(x => x.ExtraInformation.First().HouseKeepingMoney == true);
            if (searchData.HomeAlarm == "on") results = results.Where(x => x.ExtraInformation.First().HomeAlarm == true);
            if (searchData.SecureDoor == "on") results = results.Where(x => x.ExtraInformation.First().SecureDoor == true);
            if (searchData.AluminumFrames == "on") results = results.Where(x => x.ExtraInformation.First().AluminumFrames == true);
            if (searchData.FTTH == "on") results = results.Where(x => x.ExtraInformation.First().FTTH == true);
            results = results.OrderBy(searchData.SortList);
            var res = await results.ToListAsync();

            return res.Skip((searchData.numberOfPage - 1) * 16).Take(16).ToList().GetBase(res.Count, aspNetUserId);
        }
        public async Task<bool> SaveQuerySearch(string title, SearchData searchData, string referer, string aspNetUserId = null)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var jsonConverters = new List<JsonConverter>();
            jsonConverters.Add(new SearchDataConverter());
            var saved = new AspNetUserSavedSearch()
            {
                AspNetUserId = aspNetUserId,
                QueryString = referer,
                SearchData = JsonConvert.SerializeObject(searchData, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        //Converters = jsonConverters
                    }),
                CreatedAt = DateTime.Now,
                Title = title,
                Body = JsonConvert.SerializeObject(searchData, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        //Converters = jsonConverters,
                        ContractResolver = new SearchDataResolver()
                    })
                .Replace("[]", "All")
                .Replace("\r\n", "")
                .Replace("\"", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace(":", "").Trim()
            };
            context.AspNetUserSavedSearches.Add(saved);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteQuerySearch(Guid queryId, string aspNetUserId = null)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();

            var item = await context.AspNetUserSavedSearches.FirstOrDefaultAsync(x => x.Id == queryId);
            if (item != null)
            {
                context.AspNetUserSavedSearches.Remove(item);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<List<AspNetUserSavedSearch>> GetSavedSearchedByOwnerId(string aspNetUserId, int page = 1)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            return await context.AspNetUserSavedSearches.Where(x => x.AspNetUserId == aspNetUserId).Skip((page - 1) * 16).Take(16).ToListAsync();
        }
        public string AddDocumentForSearch(Document doc, List<string> tags)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _indexEngine = new MyIndexEngine();
                _indexEngine.Load(new DatabaseSettings("localhost", 0, "sa", "IIS", "SQLEXPRESS", "SearchEngine"));

                _indexEngine.Add(doc, tags);
                return doc.GUID;
            }
        }
        public async void UpdateBasicInformation(long id, PostViewModel model)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            await UpdateBasicInformation(model.Post);
        }
        private async Task<bool> UpdateBasicInformation(PostsModel model)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var result = false;
            var modelToUpdate = await context.Posts.FindAsync(model.id);
            if (modelToUpdate != null)
            {
                if (modelToUpdate.StateOfPost == 2)
                {
                    context.PostsHistories.Add(new PostsHistory()
                    {
                        PostId = modelToUpdate.id,
                        CreatedAt = DateTime.Now,
                        Price = modelToUpdate.PriceTotal
                    });
                }
                context.Entry(modelToUpdate).CurrentValues.SetValues(model);
                await context.SaveChangesAsync();
                result = true;
            }
            return result;
        }
        public async void UpdateExtraInformation(long id, PostViewModel model)
        {
            //await UpdateBasicInformation(model.Post);
            await UpdateExtraInformation(model.ExtraInformation);
            //var post = await context.ExtraInformations.FirstOrDefaultAsync(x => x.FPostId == id);

            //return post;
        }
        private async Task<bool> UpdateExtraInformation(ExtraInformation model)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                var result = false;
                var modelToUpdate = await context.ExtraInformations.FindAsync(model.PostId);
                if (modelToUpdate != null)
                {
                    context.Entry(modelToUpdate).CurrentValues.SetValues(model);
                    context.SaveChanges();
                    result = true;
                }
                return result;
            }
        }
        public async Task<AdministrationViewModel> AdminGetPosts(string userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var viewModel = new AdministrationViewModel
            {
                basicInformation = await context.Posts
                .Include(p => p.Owner)
                .Include(p => p.SubTypeInformation)
                .Include(p => p.Areas)
                .Include(p => p.SubAreas)
                .IncludeLocaliseOptions(_cultureName)
                .Where(x => x.OwnerId == userId && !x.Deleted).ToListAsync(),

                //ExtraInformation = await context.ExtraInformations.FirstOrDefaultAsync(x => x.FPostId == id)
            };

            //viewModel.RelatedPosts.AddRange(GetRelatedPosts(viewModel.Post.SearchRawKey));

            return viewModel;
        }
        public async Task<PostViewModel> GetPostInformation(long? id, string aspNetUserId)
        {
            try
            {
                if (aspNetUserId == null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetService<IronKeyContext>();
                    var viewModel = new PostViewModel
                    {
                        Post = await context.Posts
                        .Include(p => p.Owner)
                        .Include(p => p.SubTypeInformation)
                        .Include(p => p.Areas)
                        .Include(p => p.SubAreas)
                        .Include(p => p.Images)
                        .Include(p => p.WishLists)
                        .IncludeLocaliseOptions(_cultureName)
                        .FirstOrDefaultAsync(x => x.id == id && x.Deleted == false && x.Hide == false),

                        ExtraInformation = await context.ExtraInformations.FirstOrDefaultAsync(x => x.FPostId == id),


                    };
                    if (viewModel.Post != null)
                    {
                        viewModel.CardProfile = await context.AspNetUserProfiles.FirstOrDefaultAsync(x => x.AspNetUserId == viewModel.Post.OwnerId);
                        var posts = GetRelatedPosts(viewModel.Post?.id);
                        if (posts?.Count > 0)
                            viewModel.RelatedPosts.AddRange(posts);
                    }
                    viewModel.CounterOfView = context.AspNetUserInterests.Where(x => x.PostId == id.Value).GroupBy(x => new { x.AspNetUserId, x.PostId, x.ClickDate.Value.DayOfYear }).Count();

                    return viewModel;
                }
                else
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetService<IronKeyContext>();
                    var viewModel = new PostViewModel
                    {
                        Post = await context.Posts
                        .Include(p => p.Owner)
                        .Include(p => p.SubTypeInformation)
                        .Include(p => p.Areas)
                        .Include(p => p.SubAreas)
                        .IncludeLocaliseOptions(_cultureName)
                        .Include(p => p.Images)
                        .Include(p => p.WishLists)
                        .FirstOrDefaultAsync(x => x.id == id && x.Deleted == false && (x.Hide == false || x.OwnerId == aspNetUserId)),

                        ExtraInformation = await context.ExtraInformations.FirstOrDefaultAsync(x => x.FPostId == id)
                    };
                    if (viewModel.Post != null)
                    {
                        viewModel.CardProfile = await context.AspNetUserProfiles.FirstOrDefaultAsync(x => x.AspNetUserId == viewModel.Post.OwnerId);
                        var posts = GetRelatedPosts(viewModel.Post?.id, aspNetUserId);
                        if (posts?.Count > 0)
                            viewModel.RelatedPosts.AddRange(posts);
                    }
                    viewModel.CounterOfView = context.AspNetUserInterests.Where(x => x.PostId == id.Value).GroupBy(x => new { x.AspNetUserId, x.PostId, x.ClickDate.Value.DayOfYear }).Count();
                    return viewModel;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task PostInterest(long Id, string userId, string ipAddress, UserInterest userInterest, string fbclid = null, string location = null)
        {
            //if (userId == "::1" || userId == "dfd87bad-d0b4-4564-a368-4b7447f477a9") { return; }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                switch (userInterest)
                {
                    case UserInterest.WishList:
                        {
                            await context.AspNetUserInterests.AddAsync(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                WishList = true,
                                Location = location
                            });
                            await context.WishLists.AddAsync(new WishList()
                            {
                                Active = true,
                                FpostId = Id,
                                AspNetUserId = userId,
                                Added = DateTime.Now,
                                WishListName = "Default"
                            });
                            break;
                        }
                    case UserInterest.ShowPhones:
                        {
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                ShowPhones = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.RequestInformation:
                        {
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                RequestInformation = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.ViewPost:
                        {
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                Fbclid = fbclid,
                                IpAddress = ipAddress,
                                ViewPost = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.DeletePost:
                        {
                            var modelPost = await context.Posts.FindAsync(Id);
                            modelPost.Deleted = true;
                            modelPost.Hide = true;
                            modelPost.Active = false;
                            await UpdateBasicInformation(modelPost);
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                DeletePost = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.PublishPost:
                        {
                            var modelPost = await context.Posts.FindAsync(Id);
                            modelPost.Active = true;
                            modelPost.Deleted = false;
                            //modelPost.Sold = false;
                            await UpdateBasicInformation(modelPost);
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                PublishPost = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.SoldPost:
                        {
                            var modelPost = await context.Posts.FindAsync(Id);
                            modelPost.Active = false;
                            modelPost.Sold = true;
                            await UpdateBasicInformation(modelPost);
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                SoldPost = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.UnWishList:
                        {
                            //var modelPost = await context.Posts.FindAsync(Id);
                            //modelPost.Sold = true;
                            //await UpdateBasicInformation(modelPost);
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                UnWishList = true,
                                Location = location
                            });

                            var wishPost = context.WishLists.FirstOrDefault(x => x.FpostId == Id && x.Removed == null && x.Active == true);
                            if (wishPost != null)
                            {
                                var updateModel = new WishList()
                                {
                                    Id = wishPost.Id,
                                    FpostId = wishPost.FpostId,
                                    Added = wishPost.Added,
                                    AspNetUserId = wishPost.AspNetUserId,
                                    WishListName = wishPost.WishListName,
                                    Removed = DateTime.Now,
                                    Active = false
                                };

                                context.Entry(wishPost).CurrentValues.SetValues(updateModel);
                            }

                            break;
                        }

                    case UserInterest.ReservePost:
                        {
                            var modelPost = await context.Posts.FindAsync(Id);
                            modelPost.Reserved = !modelPost.Reserved;
                            await UpdateBasicInformation(modelPost);
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                DeletePost = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.HidePost:
                        {
                            var modelPost = await context.Posts.FindAsync(Id);
                            modelPost.Hide = !modelPost.Hide;
                            //modelPost.Sold = false;
                            await UpdateBasicInformation(modelPost);
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                PostId = Id,
                                PublishPost = true,
                                Location = location
                            });
                            break;
                        }
                    case UserInterest.fbclid:
                        {
                            context.AspNetUserInterests.Add(new AspNetUserInterest()
                            {
                                ClickDate = DateTime.Now,
                                AspNetUserId = userId,
                                IpAddress = ipAddress,
                                PostId = Id,
                                Fbclid = fbclid,
                                Location = location
                            });
                            break;
                        }
                    default:
                        return;
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<IPage>> QueryMyPosts(SearchData searchData, string aspNetUserId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            if (aspNetUserId == null)
            {
                var results = context.Posts
                   .Include(p => p.ExtraInformation)
                   .Include(p => p.Owner)
                   .Include(p => p.SubTypeInformation)
                   .Include(p => p.Areas)
                   .Include(p => p.SubAreas)
                   .Include(p => p.Images)
                                   .Include(w => w.WishLists)
                   .IncludeLocaliseOptions(_cultureName)
                   .Where(x => x.Active && x.StateOfPost == 2 && !x.Deleted && !x.Hide && x.OwnerId == aspNetUserId);
                var res = await results.ToListAsync();

                return res.Skip((searchData.numberOfPage - 1) * 12).Take(12).ToList().GetBase(res.Count, aspNetUserId);
            }
            else
            {
                var results = context.Posts
                                .Include(p => p.ExtraInformation)
                                .Include(p => p.Owner)
                                .Include(p => p.SubTypeInformation)
                                .Include(p => p.Areas)
                                .Include(p => p.SubAreas)
                                .IncludeLocaliseOptions(_cultureName)
                                .Include(p => p.Images).Where(x => x.Active && x.StateOfPost == 2 && !x.Deleted && !x.Hide && x.OwnerId == aspNetUserId);
                var res = await results.ToListAsync();

                return res.Skip((searchData.numberOfPage - 1) * 12).Take(12).ToList().GetBase(res.Count, aspNetUserId);
            }

        }
        public async Task<List<IPage>> QueryMyWishList(SearchData searchData, string aspNetUserId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();

            var results = context.Posts
                .Include(p => p.ExtraInformation)
                .Include(p => p.Owner)
                .Include(p => p.SubTypeInformation)
                .Include(p => p.Areas)
                .Include(p => p.SubAreas)
                .Include(p => p.Images)
                .Include(p => p.WishLists)
                .IncludeLocaliseOptions(_cultureName)
                .Where(x => x.Active == true && !x.Deleted && !x.Hide && x.WishLists.Any(x => x.AspNetUserId == aspNetUserId && x.Active == true));
            var res = await results.ToListAsync();

            return res.Skip((searchData.numberOfPage - 1) * 16).Take(16).ToList().GetBase(res.Count, aspNetUserId);
        }
        public async Task<List<IPage>> QueryRecentsPostsList(SearchData searchData, string aspNetUserId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetService<IronKeyContext>();

                var myViewPosts = await context.AspNetUserInterests
                    .Where(x => x.AspNetUserId == aspNetUserId && x.ViewPost.HasValue && x.ViewPost.Value)
                    .GroupBy(x => x.PostId).Select(group => new { PostId = group.Key, ClickDate = group.Max(date => date.ClickDate) }).OrderByDescending(x => x.ClickDate).ToListAsync();

                var interests = context.AspNetUserInterests
                    .Include(p => p.Post)
                    //.Include(p => p.Post.ExtraInformation)
                    //.Include(p => p.Post.Owner)
                    //.Include(p => p.Post.SubTypeInformation)
                    .Include(p => p.Post.Areas)//
                    .Include(p => p.Post.SubAreas)//
                    .Include(p => p.Post.Images)//
                                                //.Include(p => p.Post.AspNetUserInterests)
                    .Include(w => w.Post.WishLists)//
                    .Where(x => x.Post.Active == true && !x.Post.Deleted && !x.Post.Hide && x.AspNetUserId == aspNetUserId).Take(50).AsEnumerable();

                var results = interests.GroupBy(x => new { x.Post })
                .Select(group => new { Post = group.Key, ClickDate = group.Max(x => x.ClickDate) });
                foreach (var item in results)
                {
                    item.Post.Post.SetLocaliseOptions(_cultureName);
                }
                return results.OrderByDescending(x => x.ClickDate).Skip((searchData.numberOfPage - 1) * 16).Take(16).Select(x => x.Post.Post).ToList().GetBase(results.ToList().Count, aspNetUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<PostViewModel> CreateDraftPost(PostViewModel model, string userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                await context.Posts.AddAsync(model.Post);
                await context.SaveChangesAsync();
                await context.ExtraInformations.AddAsync(new ExtraInformation() { FPostId = model.Post.id });
                await context.SaveChangesAsync();
                var robject = await GetPostInformation(model.Post.id, userId);
                return robject;
            }
        }
        public async void InsertImageOfPost(long id, string filePath, ImageUploader uploader, string imageEnding, string type)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IronKeyContext>();

                var entity = new PostsImages()
                {
                    Active = true,
                    Deleted = false,
                    FpostId = id,
                    Description = "",
                    ImageName = uploader.qquuid.ToString() + imageEnding,
                    UploadedDate = DateTime.Now,
                    UrlImage = filePath,
                    Sort = 0,
                    Width = uploader.width,
                    Height = uploader.height,
                    Type = type
                };
                context.PostsImages.Add(entity);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<AspNetUserNote>> GetUserNotesAsync(string id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            return await context.AspNetUserNotes
                .Include(x => x.AspNetUserNotesDetails)
                .Include(x => x.Post)
                .Include(x => x.Post.Images)
                .Include(x => x.Post.Areas)
                .Include(x => x.Post.SubAreas)
                .Where(x => x.AspNetUserId == id).ToListAsync();
        }
        public async Task<List<AspNetUserNotesDetail>> GetUserNotesAsync(long id, string ownerId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            return await context.AspNetUserNotesDetails.Include(x => x.Fnote).Include(x => x.Fnote.AspNetUser).Where(x => x.FnoteId == id && x.Fnote.AspNetUserId == ownerId).ToListAsync();
        }
        public async Task InsertNotes(string ownerId, long postId, string notes)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var conversationModel = await context.AspNetUserNotes.FirstOrDefaultAsync(x => x.PostId == postId && x.AspNetUserId == ownerId);
            if (conversationModel == null)
            {
                var newModel = new AspNetUserNote()
                {
                    PostId = postId,
                    AspNetUserId = ownerId,
                    CreatedAt = DateTime.Now
                };
                await context.AspNetUserNotes.AddAsync(newModel);
                await context.SaveChangesAsync();
                await context.AspNetUserNotesDetails.AddAsync(new AspNetUserNotesDetail()
                {
                    FnoteId = newModel.Id,
                    Note = notes
                });
                await context.SaveChangesAsync();
            }
            else
            {
                await context.AspNetUserNotesDetails.AddAsync(new AspNetUserNotesDetail()
                {
                    FnoteId = conversationModel.Id,
                    Note = notes
                });
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteNote(long id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();

            var item = await context.AspNetUserNotesDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                context.AspNetUserNotesDetails.Remove(item);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteNoteGroup(long id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var item = await context.AspNetUserNotesDetails.Where(x => x.FnoteId == id).ToListAsync();
            var groupitem = await context.AspNetUserNotes.FirstOrDefaultAsync(x => x.Id == id);
            if (groupitem != null)
            {
                if (item != null)
                {
                    context.AspNetUserNotesDetails.RemoveRange(item);
                }
                context.AspNetUserNotes.Remove(groupitem);
                await context.SaveChangesAsync();
            }
        }
        public async Task<bool> InsertOrder(int productId, string aspNetUserId, long? fPostId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetService<IronKeyContext>();
                var wallet = await context.Wallets.FirstOrDefaultAsync(x => x.AspNetUserId == aspNetUserId);
                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if (product.TypeOfPayment == "Credit")
                {
                    context.Orders.Add(new Order()
                    {
                        FpostId = fPostId,
                        OwnerId = aspNetUserId,
                        ProductId = productId,
                        Summary = product.Amount,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        StartDate = DateTime.Now
                    });
                    wallet.Tokens += (product.Amount);

                    await context.SaveChangesAsync();
                    return true;

                }
                else if (product.TypeOfPayment == "Debit")
                {
                    if (wallet.Tokens >= (product.Amount * product.Months))
                    {
                        context.Orders.Add(new Order()
                        {
                            FpostId = fPostId,
                            OwnerId = aspNetUserId,
                            ProductId = productId,
                            Summary = (product.Amount * product.Months),
                            CreatedAt = DateTime.Now,
                            IsActive = true,
                            StartDate = DateTime.Now
                        });
                        wallet.Tokens -= product.Amount * product.Months;

                        if (fPostId != null)
                            (await context.Posts.FirstOrDefaultAsync(x => x.id == fPostId)).Premium = true;

                        await context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return false;
            }


        }
        public async Task<bool> CancelOrder(int orderId, string aspNetUserId) {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var wallet = await context.Wallets.FirstOrDefaultAsync(x => x.AspNetUserId == aspNetUserId);
            var order = context.Orders.Include(p => p.Product).FirstOrDefault(x => x.Id == orderId);

            order.IsCanceled = true;
            order.EndDate = DateTime.Now;

            var days = (int)((DateTime.Now - order.StartDate.Value).TotalDays);
            int months = (int)days / 30;

            wallet.Tokens += (order.Product.Months - (months + 1)) * order.Product.Amount;

            order.IsRefunded = true;
            order.IsActive = false;

            (await context.Posts.FirstOrDefaultAsync(x => x.id == order.FpostId)).Premium = false;


            await context.SaveChangesAsync();


            return true;
        }
        public async Task<List<Order>> GetOrdersByOwnerId(string ownerId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var orders = context.Orders
                .Include(p => p.Product)
                .Include(p => p.Owner)
                .Include(p => p.Fpost)
                .Where(x => x.OwnerId == ownerId).ToListAsync();
            return await orders;
        }
        public async Task<Wallet> GetWallet(string id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            return await context.Wallets.FirstOrDefaultAsync(x => x.AspNetUserId == id);
        }
        public async Task<WalletViewModel> GetFinace(string aspNetUserId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<IronKeyContext>();
            var finance = new WalletViewModel();
            finance.Wallet = await context.Wallets.FirstOrDefaultAsync(x => x.AspNetUserId == aspNetUserId);
            finance.Orders = await context.Orders
                .Include(p => p.Product)
                .Include(p => p.Owner)
                .Include(p => p.Fpost)
                .Where(x => x.OwnerId == aspNetUserId).ToListAsync();
            return finance;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DomainModel()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
