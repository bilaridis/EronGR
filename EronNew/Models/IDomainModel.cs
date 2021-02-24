using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EronNew.Models
{
    public interface IDomainModel
    {
        Task<Areas> GetArea(long Id);
        Task<PostsModel> GetPost(long? id);
        void SetCulture(string cultureName);
        void DeleteImageOfPost(string path, string imageName);
        Task<IEnumerable<TypesModel>> GetTypes(int id);
        Task<bool> SaveMyCard(AspNetUserProfile model);
        AspNetUserProfile GetMyCard(string aspNetUserId);
        List<IPage> GetRelatedPosts(long? postId = null, string aspNetUserId = null);
        Task<IEnumerable<Areas>> GetAreas(long parentId);
        Task<IEnumerable<TypesModel>> GetTypes(string desc);
        Task<List<AspNetUserNote>> GetUserNotesAsync(string id);
        Task<List<AspNetUserNotesDetail>> GetUserNotesAsync(long id, string ownerId);
        Task<List<IPage>> QuerySearch(SearchData searchData, string aspNetUserId = null);
        Task<bool> SaveQuerySearch(string title, SearchData searchData, string referer, string aspNetUserId = null);
        Task<bool> DeleteQuerySearch(Guid queryId, string aspNetUserId = null);
        Task<List<AspNetUserSavedSearch>> GetSavedSearchedByOwnerId(string aspNetUserId, int page = 1);
        void UpdateExtraInformation(long id, PostViewModel model);
        void UpdateBasicInformation(long id, PostViewModel model);
        Task<AdministrationViewModel> AdminGetPosts(string userId);
        Task InsertNotes(string ownerId, long postId, string notes);
        Task DeleteNote(long id);
        Task DeleteNoteGroup(long id);
        string AddDocumentForSearch(Document doc, List<string> tags);
        Task<PostViewModel> GetPostInformation(long? id, string userId);
        Task<List<IPage>> QueryMyPosts(SearchData searchData, string aspNetUserId);
        Task<List<IPage>> QueryMyWishList(SearchData searchData, string aspNetUserId);
        Task<List<IPage>> QueryRecentsPostsList(SearchData searchData, string aspNetUserId);
        Task<PostViewModel> CreateDraftPost(PostViewModel model, string userId);
        Task PostInterest(long Id, string userId, string ipAddress, UserInterest userInterest, string fbclid = null, string location = null);
        void InsertImageOfPost(long id, string filePath, ImageUploader uploader, string imageEnding, string type);
        Task<bool> InsertOrder(int productId, string ownerId, long? fPostId);
        Task<bool> CancelOrder(int orderId, string aspNetUserId);
        Task<List<Order>> GetOrdersByOwnerId(string ownerId);
        Task<Wallet> GetWallet(string id);
        Task<WalletViewModel> GetFinace(string aspNetUserId);
    }
}