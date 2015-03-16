namespace AShotNet.Extentions
{
    using System.Drawing;

    public static class BitmapExtentions
    {
        public static Bitmap GetSubImage(this Bitmap origin, int x, int y, int w, int h)
        {
            var theRect = new Rectangle(x, y, w, h);
            Bitmap croppedImage = origin.Clone(theRect, origin.PixelFormat);

            return croppedImage;
        }
    }
}
