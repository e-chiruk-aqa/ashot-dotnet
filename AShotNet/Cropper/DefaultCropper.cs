namespace AShotNet.Cropper
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Coordinates;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public class DefaultCropper : ImageCropper
    {
        public override Screenshot cropScreenshot(Bitmap image, ICollection<Coords> coordsToCompare)
        {
            Coords cropArea = Coords.unity(coordsToCompare);
            Coords imageIntersection = Coords.ofImage(image).Intersect(cropArea);
            if (imageIntersection.IsEmpty)
            {
                return new Screenshot(image);
            }
            var cropped = new Bitmap(imageIntersection.Width, imageIntersection.Height, image.PixelFormat);
            Graphics g = Graphics.FromImage(cropped);
            
            g.DrawImage(image, new Rectangle(0, 0, imageIntersection.Width, imageIntersection.Height), cropArea
                .X, cropArea.Y, cropArea.X + imageIntersection.Width, cropArea.Y + imageIntersection
                    .Height, GraphicsUnit.Point);


            g.Dispose();
            var screenshot = new Screenshot
                (cropped);
            screenshot.setOriginShift(cropArea);
            screenshot.setCoordsToCompare(Coords.setReferenceCoords
                (screenshot.getOriginShift(), coordsToCompare));
            return screenshot;
        }

        protected internal virtual Coords createCropArea(ICollection<Coords> coordsToCompare)
        {
            return Coords.unity(coordsToCompare);
        }
    }
}
