using System;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Core.Utilities.Extensions
{
    public static class ImageUploaderExtension
    {
        /// <summary>
        /// Saves the specified string as jpeg image in the originalPath. And removes the old image physically
        /// </summary>
        /// <param name="Base64Image">Image string from client</param>
        /// <param name="path">The path to save the image in</param>
        /// <param name="makeThumbnail"></param>
        /// <param name="oldVersion">Last saved image that is going to be replaced.</param>
        /// <returns></returns>
        public static string SaveBase64ImageToServer(string Base64Image, string path, string oldVersion = null, bool makeThumbnail = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(Base64Image))
                {
                    var imageFile = Base64ToImage(Base64Image);
                    var thumbnail = doresize(imageFile);
                    var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
                    imageFile.AddImageToServer(imageName, path, oldVersion);
                    if (makeThumbnail)
                        thumbnail.AddImageToServer(imageName, path + "thumbs/", oldVersion);
                    return imageName;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        ///// <summary>
        ///// Saves the specified string as jpeg image in the originalPath. And removes the old image physically
        ///// </summary>
        ///// <param name="Base64Image">Image string from client</param>
        ///// <param name="path">The path to save the image in</param>
        ///// <param name="oldVersion">Last saved image that is going to be replaced.</param>
        ///// <returns></returns>
        //public static string SaveBase64ImageToServer(string Base64Image, string path, string oldVersion = null)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(Base64Image))
        //        {
        //            var imageFile = Base64ToImage(Base64Image);
        //            var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
        //            imageFile.AddImageToServer(imageName, path, oldVersion);
        //            return imageName;
        //        }
        //        return null;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}



        #region save base64string image to server        

        /// <summary>
        /// Saves the new specified image in the originalPath. And removes the old image physically
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fileName"></param>
        /// <param name="originalPath">The path to save the image in</param>
        /// <param name="deleteFileName">Last saved image that is going to be replaced.</param>
        public static void AddImageToServer(this Image image, string fileName, string originalPath,
            string deleteFileName = null)
        {
            if (image != null)
            {
                if (!Directory.Exists(originalPath))
                    Directory.CreateDirectory(originalPath);
                if (!string.IsNullOrEmpty(deleteFileName))
                    File.Delete(originalPath + deleteFileName);
                string imageName = originalPath + fileName;
                using (var stream = new FileStream(imageName, FileMode.Create))
                {
                    if (!Directory.Exists(imageName))
                        image.Save(stream, ImageFormat.Jpeg);
                }

            }
        }

        public static byte[] decodeUrlBase64(string s)
        {
            return Convert.FromBase64String(s.Substring(s.LastIndexOf(',') + 1));
        }

        public static Image Base64ToImage(string base64string)
        {
            var res = decodeUrlBase64(base64string);
            MemoryStream ms = new MemoryStream(res, 0, res.Length);
            ms.Write(res, 0, res.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        #endregion

        #region resize
        public static Image doresize(this Image img)//, string path)
        {
            try
            {
                //string path = Server.MapPath("~/Images");
                //Image img = Image.FromFile(string.Concat(path, filename));//Image img = Image.FromFile(string.Concat(path,"/3904.jpg"));
                Bitmap b = new Bitmap(img);
                Image i = resizeImage(b, new Size(100, 100));
                //i.AddImageToServer(filename, path + "thumbs/");
                return i;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public static void doresize(string filename, string path)
        {
            try
            {
                //string path = Server.MapPath("~/Images");
                Image img = Image.FromFile(string.Concat(path, filename));//Image img = Image.FromFile(string.Concat(path,"/3904.jpg"));
                Bitmap b = new Bitmap(img);
                Image i = resizeImage(b, new Size(100, 100));
                i.AddImageToServer(filename, path + "thumbs/");
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public static Image resizeImage(this Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        #endregion
    }
}
