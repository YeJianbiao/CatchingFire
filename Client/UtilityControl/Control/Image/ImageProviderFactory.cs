using System;
using Utility.Sys;

namespace UtilityControl.Control.Image
{
    internal class ImageProviderFactory
    {

        public virtual IImageProvider GetInstance(string url)
        {
            try
            {
                Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);
                switch (uri.Scheme.ToUpper())
                {
                    case "HTTP":
                        return Singleton<WebImageProvider>.GetInstance();
                    case "FILE":
                        return Singleton<LocalImageProvider>.GetInstance();
                    default:
                        return Singleton<UnknownImageProvider>.GetInstance();
                }
            }
            catch (Exception)
            {
                return Singleton<UnknownImageProvider>.GetInstance();
            }
            
        }

    }
}
