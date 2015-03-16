namespace AShotNet.Comparison
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Extentions;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    public class ImageDiff
    {
        public static readonly ImageDiff EMPTY_DIFF = new ImageDiff();

        private readonly Bitmap diffImage;

        private readonly ICollection<Point> diffPoints = new HashSet<Point>();

        /// <summary>Deposed diff points relatively the least diff area.</summary>
        private ICollection<Point> deposedPoints = new HashSet<Point>();

        /// <summary>The color which marks the differences between the images.</summary>
        private Color diffColor = Color.Red;

        /// <summary>
        ///     Images are considered the same if the number of distinguished pixels does not exceed this value.
        /// </summary>
        private int diffSizeTrigger;

        private bool marked;

        public ImageDiff(Bitmap expected, Bitmap actual)
        {
            int width = Math.Max(expected.Width, actual.Width);
            int height = Math.Max(expected.Height, actual.Height);
            this.diffImage = new Bitmap(width, height, actual.PixelFormat);
        }

        private ImageDiff() {}

        /// <summary>Sets the color which marks the differences between the images.</summary>
        /// <remarks>
        ///     Sets the color which marks the differences between the images.
        ///     This color will be used with <code>Color.WHITE</code> in the checkerboard pattern.
        ///     The default value is <code>Color.RED</code>.
        /// </remarks>
        /// <param name="diffColor">the color which marks the differences</param>
        /// <returns>self for fluent style</returns>
        /// <seealso cref="java.awt.Color" />
        public virtual ImageDiff withDiffColor(Color diffColor)
        {
            this.diffColor = diffColor;
            this.marked = false;
            return this;
        }

        /// <summary>
        ///     Sets the maximum number of distinguished pixels when images are still considered the same.
        /// </summary>
        /// <param name="diffSizeTrigger">the number of different pixels</param>
        /// <returns>self for fluent style</returns>
        public virtual ImageDiff withDiffSizeTrigger(int diffSizeTrigger)
        {
            this.diffSizeTrigger = diffSizeTrigger;
            return this;
        }

        /// <returns>Diff image with empty spaces in diff areas.</returns>
        public virtual Bitmap getDiffImage()
        {
            return this.diffImage;
        }

        public virtual void addDiffPoint(int x, int y)
        {
            this.diffPoints.Add(new Point(x, y));
        }

        /// <summary>Marks diff on inner image and returns it.</summary>
        /// <remarks>
        ///     Marks diff on inner image and returns it.
        ///     Idempotent.
        /// </remarks>
        /// <returns>marked diff image</returns>
        public virtual Bitmap getMarkedImage()
        {
            if (!this.marked)
            {
                foreach (Point dot in this.diffPoints)
                {
                    this.marked = this.diffImage.GetPixel(dot.X, dot.Y) == this.pickDiffColor(dot);
                }
                this.marked = true;
            }
            return this.diffImage;
        }

        private Color pickDiffColor(Point dot)
        {
            return ((dot.X + dot.Y)%2 == 0) ? this.diffColor : Color.White;
        }

        /// <summary>Returns <tt>true</tt> if there are differences between images.</summary>
        /// <returns><tt>true</tt> if there are differences between images.</returns>
        public virtual bool hasDiff()
        {
            return this.diffPoints.Count > this.diffSizeTrigger;
        }

        public override bool Equals(object obj)
        {
            if (obj is ImageDiff)
            {
                var item = (ImageDiff
                    ) obj;
                if (this.diffPoints.Count != item.diffPoints.Count)
                {
                    return false;
                }
                ICollection<Point> referencedPoints = this.getDeposedPoints();
                ICollection<Point> itemReferencedPoints = item.getDeposedPoints();
                foreach (Point point in referencedPoints)
                {
                    if (!itemReferencedPoints.Contains(point))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.getDeposedPoints().GetHashCode();
        }

        private ICollection<Point> getDeposedPoints()
        {
            if (this.deposedPoints.IsEmpty())
            {
                this.deposedPoints = this.deposeReference(this);
            }
            return this.deposedPoints;
        }

        private Point getReferenceCorner(ImageDiff diff)
        {
            int x = diff.diffPoints.Min(pt => pt.X);
            int y = diff.diffPoints.Min(pt => pt.Y);

            return new Point(x, y);
        }

        private ICollection<Point> deposeReference(ImageDiff diff)
        {
            Point reference = this.getReferenceCorner(diff);
            ICollection<Point> referenced = new HashSet<Point>();
            foreach (Point point in diff.diffPoints)
            {
                referenced.Add(new Point(point.X - reference.X, point.Y - reference.Y));
            }
            return referenced;
        }
    }
}
