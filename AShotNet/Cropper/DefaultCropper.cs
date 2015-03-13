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
        protected internal override Screenshot cropScreenshot(Bitmap
            image, ICollection<Coords
                > coordsToCompare)
        {
            Coords cropArea = Coords
                .unity(coordsToCompare);
            var imageIntersection = ((Coords
                ) Coords.ofImage(image).intersection(cropArea
                    ));
            if (imageIntersection.isEmpty())
            {
                return new Screenshot(image);
            }
            var cropped = new Bitmap(imageIntersection
                .width, imageIntersection.height, image.getType());
            java.awt.Graphics g = cropped.getGraphics();
            g.drawImage(image, 0, 0, imageIntersection.width, imageIntersection.height, cropArea
                .x, cropArea.y, cropArea.x + imageIntersection.width, cropArea.y + imageIntersection
                    .height, null);
            g.dispose();
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
