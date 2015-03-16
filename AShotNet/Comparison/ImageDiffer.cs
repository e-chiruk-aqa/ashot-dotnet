namespace AShotNet.Comparison
{
    using System.Collections.Generic;
    using System.Drawing;
    using Coordinates;
    using Util;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    public class ImageDiffer
    {
        private const int DEFAULT_COLOR_DISTORTION = 15;

        private int colorDistortion = DEFAULT_COLOR_DISTORTION;

        public virtual ImageDiffer withColorDistortion(int distortion)
        {
            this.colorDistortion = distortion;
            return this;
        }

        public virtual ImageDiff makeDiff(Screenshot expected, Screenshot actual)
        {
            var diff = new ImageDiff(expected.getImage(), actual.getImage());
            for (int i = 0; i < diff.getDiffImage().Width; i++)
            {
                for (int j = 0; j < diff.getDiffImage().Height; j++)
                {
                    if (this.insideBothImages(i, j, expected.getImage(), actual.getImage()))
                    {
                        if (this.shouldCompare(i, j, expected, actual) && !ImageTool.rgbCompare(expected.getImage().GetPixel(i, j), actual.getImage().GetPixel(i, j), this.colorDistortion))
                        {
                            diff.addDiffPoint(i, j);
                        }
                        else
                        {
                            diff.getDiffImage().SetPixel(i, j, expected.getImage().GetPixel(i, j));
                        }
                    }
                    else
                    {
                        this.setSharedPoint(i, j, expected, actual, diff);
                    }
                }
            }
            return diff;
        }

        public virtual ImageDiff makeDiff(Bitmap expected, Bitmap actual)
        {
            return this.makeDiff(new Screenshot(expected), new Screenshot(actual));
        }

        private bool shouldCompare(int i, int j, Screenshot expected, Screenshot actual)
        {
            return this.notIgnoredInBoth(i, j, expected, actual) && this.isToCompareInBoth(i, j, expected, actual);
        }

        private bool notIgnoredInBoth(int i, int j, Screenshot expected, Screenshot actual)
        {
            return !this.isIgnored(i, j, expected.getIgnoredAreas()) || !this.isIgnored(i, j, actual.getIgnoredAreas());
        }

        private bool isToCompareInBoth(int i, int j, Screenshot expected, Screenshot actual)
        {
            return this.isToCompare(i, j, expected.getCoordsToCompare()) || this.isToCompare(i, j, actual.getCoordsToCompare());
        }

        private bool isIgnored(int i, int j, ICollection<Coords> ignoredCoords)
        {
            bool isIgnored = false;
            foreach (Coords coords in ignoredCoords)
            {
                if (coords.Contains(new Point(i, j)))
                {
                    isIgnored = true;
                    break;
                }
            }
            return isIgnored;
        }

        private void setSharedPoint(int i, int j, Screenshot expected, Screenshot actual, ImageDiff diff)
        {
            if (Coords.ofImage(expected.getImage()).Contains(new Point(i, j)))
            {
                diff.getDiffImage().SetPixel(i, j, expected.getImage().GetPixel(i, j));
            }
            else
            {
                if (Coords.ofImage(actual.getImage()).Contains(new Point(i, j)))
                {
                    diff.getDiffImage().SetPixel(i, j, actual.getImage().GetPixel(i, j));
                }
            }
        }

        private bool isToCompare(int i, int j, ICollection<Coords
            > coordsToCompare)
        {
            bool isToCompare = false;
            foreach (Coords coords in coordsToCompare)
            {
                if (coords.Contains(new Point(i, j)))
                {
                    isToCompare = true;
                    break;
                }
            }
            return isToCompare;
        }

        private bool insideBothImages(int i, int j, Bitmap expected, Bitmap actual)
        {
            return Coords.ofImage(expected).Contains(new Point(i, j)) && Coords.ofImage(actual).Contains(new Point(i, j));
        }
    }
}
