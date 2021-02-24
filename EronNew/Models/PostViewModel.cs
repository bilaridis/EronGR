using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EronNew.Models
{
    public class PostViewModel : IPostViewModel
    {
        public PostViewModel()
        {
            RelatedPosts = new List<IPage>();
            //PostImages = new List<PostsImages>();
        }
        public PostsModel Post { get; set; }
        public ExtraInformation ExtraInformation { get; set; }
        public AspNetUserProfile CardProfile { get; set; }
        public int CounterOfView { get; set; }
        public List<IFormFile> UploadImages { set; get; }
        public IFormFile UploadImage { set; get; }
        public WishList WishList { get; set; }
        //public List<PostsImages> PostImages { set; get; }
        public List<IPage> RelatedPosts { get; set; }
        public int EditTab { get; set; }
        public int NewTab { get; set; }
        public string ExtractInformation()
        {
            var returnResult = "";

            returnResult += Post.Areas.AreaName + " " + Post.SubAreas.AreaName + " ";
            returnResult += Post.SaleCategory + " " + Post.SubTypeInformation.Desc + " ";
            returnResult += Post.SubTypeInformation.SubDesc + " " + Post.Description + " ";
            returnResult += ((Post.Bathroom.HasValue && Post.Bathroom.Value > 0 )? "μπάνιο μπανιο μπάνια μπανια bathroom τουαλέτα τουαλετα " : "")   + " " + ((Post.Bedroom.HasValue && Post.Bedroom.Value > 0) ? "υπνοδωμάτιο υπνοδωματιο υπνοδωμάτια υπνοδωματια bedroom " : "");
            returnResult += ((Post.PetAllowed) ? "κατοικίδια κατοικιδια ζωα ζωο σκύλους σκύλος γάτα γατα pet  " : "");
            returnResult += ((Post.ParkingArea) ? "parking παρκινκ πάρκινγκ παρκινγκ θέση θεση στάθμευσης στάθμευσησ στάθμευση σταθμευση " : "");
            returnResult += ((Post.Furniture) ? "επιπλωμένο επίπλωση έπιπλα επιπλα επιπλωμενο επιπλωση " : "");
            returnResult += ((Post.View) ? "θεα θέα view " : "");
            returnResult += ((ExtraInformation.AirCondition) ? "κλιματισμος κλιματισμό κλιματισμό κλίματισμος aircondition " : "");
            returnResult += ((ExtraInformation.Bbq) ? "bbq ψησταριά ψησταρια " : "");
            returnResult += ((ExtraInformation.Elevator) ? "ασανσερ ασανσέρ ανελκυστήρασ ανελκυστηρα " : "");
            returnResult += ((ExtraInformation.Fireplace) ? "τζάκι fireplace " : "");
            returnResult += ((ExtraInformation.Garden) ? "κήπο κηπο " : "");
            returnResult += ((ExtraInformation.Hall) ? "χωλ χόλ hall " : "");
            returnResult += ((ExtraInformation.Heating) ? "θέρμανση θερμανση " : "");
            returnResult += ((ExtraInformation.Kitchen) ? "κουζίνα κουζινα  " : "");
            returnResult += ((ExtraInformation.Livingroom) ? "σαλόνι σαλονι " : "");
            returnResult += ((ExtraInformation.Maidroom) ? "βεστιάριο ντουλάπα ντουλαπα " : "");
            returnResult += ((ExtraInformation.Master) ? "master bedroom " : "");
            returnResult += ((ExtraInformation.RoofFloor) ? "ρετιρέ ρετιρε " : "");
            returnResult += ((ExtraInformation.SemiOutdoor) ? "υμιυπαίθρο υμιυπαιθρο " : "");
            returnResult += ((ExtraInformation.Storageroom) ? "αποθηκη αποθήκη αποθήκες αποθηκες " : "");
            returnResult += ((ExtraInformation.Swimmingpool) ? "πισίνα εσωτερική εσωτερικη πισινα " : "");
            returnResult += ((ExtraInformation.Wc) ? "wc τουαλέτα τουαλετα " : "");
            returnResult += Post.Owner.UserName + " ";
            returnResult += Post.TypeId + " ";


            return returnResult;

        }
    }
}
