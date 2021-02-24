using System;
using System.Collections.Generic;

namespace EronNew
{
    using EronNew.Models;
    using Microsoft.Extensions.Logging;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;



    public static class ImageHelper
    {
        public static string AssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static List<UploadedImage> GetFineUploader(this PostViewModel viewModel, ILogger<PostsController> _logger)
        {
            try
            {
                var uploadedImages = new List<UploadedImage>();

                foreach (PostsImages item in viewModel.Post.Images)
                {
                    uploadedImages.Add(new UploadedImage()
                    {
                        name = item.ImageName,
                        uuid = item.ImageName.Replace(".Jpeg", ""),
                        thumbnailUrl = "/" + item.UrlImage + "/Thumbnails/" + item.ImageName
                    });
                }

                return uploadedImages;// JsonConvert.SerializeObject(uploadedImages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        public static Image ResizeImage(Image imgToResize, Size destinationSize)
        {

            var originalWidth = imgToResize.Width;
            var originalHeight = imgToResize.Height;

            //how many units are there to make the original length
            var hRatio = (float)originalHeight / destinationSize.Height;
            var wRatio = (float)originalWidth / destinationSize.Width;

            //get the shorter side
            var ratio = Math.Min(hRatio, wRatio);

            var hScale = Convert.ToInt32(destinationSize.Height * ratio);
            var wScale = Convert.ToInt32(destinationSize.Width * ratio);

            //start cropping from the center
            var startX = (originalWidth - wScale) / 2;
            var startY = (originalHeight - hScale) / 2;

            //crop the image from the specified location and size
            var sourceRectangle = new Rectangle(startX, startY, wScale, hScale);

            //the future size of the image
            var bitmap = new Bitmap(destinationSize.Width, destinationSize.Height);

            //fill-in the whole bitmap
            var destinationRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            //generate the new image
            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }

            return bitmap;
        }
    }



}
