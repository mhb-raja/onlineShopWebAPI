using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Common
{
    public static class PathTools
    {
        #region domain



        #endregion

        #region product

        public static string ProductImagePath = "/images/products/";
        public static string ProductImageServerPath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products/");

        public static string ProductThumbPath = "/images/products/thumbs/";        
        public static string ProductThumbServerPath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products/thumbs/");
        #endregion

        #region product gallery

        public static string ProductGalleryImagePath = "/images/product-gallery/";
        public static string ProductGalleryImageServerPath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/product-gallery/");

        public static string ProductGalleryThumbPath = "/images/product-gallery/thumbs/";
        public static string ProductGalleryThumbServerPath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/product-gallery/thumbs/");
        #endregion

        #region slider

        public static string SliderImagePath = "/images/sliders/";
        public static string SliderImageServerPath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/sliders/");

        public static string SliderThumbPath = "/images/sliders/thumbs/";
        public static string SliderThumbServerPath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/sliders/thumbs/");
        #endregion
    }
}
