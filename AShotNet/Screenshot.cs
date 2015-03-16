namespace AShotNet
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Coordinates;

    /// <summary>Result of screen capture.</summary>
    /// <remarks>
    ///     Result of screen capture.
    ///     Contains final processed image and all required information for image comparison.
    /// </remarks>
    [Serializable]
    public class Screenshot
    {
        private const long serialVersionUID = 1241241256734156872L;

        private ICollection<Coords> coordsToCompare;

        private ICollection<Coords> ignoredAreas = new HashSet<Coords>();

        [NonSerialized] private Bitmap image;

        /// <summary>
        ///     Coords, containing x and y shift from origin image coordinates system
        ///     Actually it is coordinates of cropped area on origin image.
        /// </summary>
        /// <remarks>
        ///     Coords, containing x and y shift from origin image coordinates system
        ///     Actually it is coordinates of cropped area on origin image.
        ///     Should be set if image is cropped.
        /// </remarks>
        private Coords originShift = new Coords(0, 0, 0, 0);

        public Screenshot(Bitmap image)
        {
            this.image = image;
            this.coordsToCompare = new HashSet<Coords> {Coords.ofImage(image)};
        }

        public virtual Bitmap getImage()
        {
            return this.image;
        }

        public virtual void setImage(Bitmap image)
        {
            this.image = image;
        }

        public virtual ICollection<Coords> getCoordsToCompare()
        {
            return this.coordsToCompare;
        }

        public virtual void setCoordsToCompare(ICollection<Coords> coordsToCompare)
        {
            this.coordsToCompare = coordsToCompare;
        }

        public virtual ICollection<Coords> getIgnoredAreas()
        {
            return this.ignoredAreas;
        }

        public virtual void setIgnoredAreas(ICollection<Coords> ignoredAreas)
        {
            this.ignoredAreas = ignoredAreas;
        }

        public virtual Coords getOriginShift()
        {
            return this.originShift;
        }

        public virtual void setOriginShift(Coords originShift
            )
        {
            this.originShift = originShift;
        }

        /// <exception cref="System.IO.IOException" />
        private void WriteObject(Stream @out)
        {
            this.image.Save(@out, ImageFormat.Png);
        }

        /// <exception cref="System.IO.IOException" />
        /// <exception cref="java.lang.ClassNotFoundException" />
        private void ReadObject(Stream @in)
        {
            this.image = new Bitmap(Image.FromStream(@in));
        }
    }
}
