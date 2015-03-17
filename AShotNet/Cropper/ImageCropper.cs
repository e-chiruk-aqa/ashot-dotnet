namespace AShotNet.Cropper
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Coordinates;
    using Extentions;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public abstract class ImageCropper
    {
        public virtual Screenshot crop(Bitmap image, ICollection<Coords> cropArea)
        {
            return cropArea.IsEmpty() ? new Screenshot(image) : this.cropScreenshot(image, cropArea);
        }

        public abstract Screenshot cropScreenshot(Bitmap image, ICollection<Coords> coordsToCompare);
    }
}
