using System.Windows.Media;

namespace UtilityControl.Control.Image
{
    interface IImageProvider
    {

        ImageSource GenereateThumbnail(object fileSource, double width, double height);

    }
}
