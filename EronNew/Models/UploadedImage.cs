namespace EronNew.Models
{
    public class UploadedImage
    {
        public string name { get; set; }
        public string uuid { get; set; }
        public long size { get; set; }
        public string deleteFileEndPoint { get; set; }
        public object deleteFileParams { get; set; }
        public string thumbnailUrl { get; set; }
    }



}
