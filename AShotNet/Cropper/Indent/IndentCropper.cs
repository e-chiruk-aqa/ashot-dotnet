using System.IO;

namespace AShotNet.Cropper.Indent
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Coordinates;
    using Util;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public class IndentCropper : DefaultCropper
    {
        public const int DEFAULT_INDENT = 50;

        private readonly int indent = DEFAULT_INDENT;

        protected internal IList<IndentFilter> filters = new List<IndentFilter>();

        public IndentCropper(int indent)
        {
            this.indent = indent;
        }

        public IndentCropper()
        {
            this.indent = DEFAULT_INDENT;
        }

        public override Screenshot cropScreenshot(Bitmap image, ICollection<Coords> coordsToCompare)
        {
            //Coords cropArea = this.createCropArea(coordsToCompare);
            //Coords indentMask = this.createIndentMask(cropArea, image);
            //Coords coordsWithIndent = this.applyIndentMask(cropArea, indentMask);
            var coordsWithIndent = new Coords(0, 0, image.Width, image.Height);
            //Screenshot croppedShot = base.cropScreenshot(image, new HashSet<Coords> {coordsWithIndent});
            var croppedShot = new Screenshot(image); 
            croppedShot.setOriginShift(coordsWithIndent);
            croppedShot.setCoordsToCompare(Coords.setReferenceCoords(coordsWithIndent, coordsToCompare));
            IList<NoFilteringArea> noFilteringAreas = this.createNotFilteringAreas(croppedShot);
            croppedShot.setImage(this.ApplyFilters(croppedShot.getImage()));
            this.pasteAreasToCompare(croppedShot.getImage(), noFilteringAreas);
            return croppedShot;
        }

        protected internal virtual Coords applyIndentMask(Coords origin, Coords mask)
        {
            var spreadCoords = new Coords(0, 0, 0, 0);
            spreadCoords.X = origin.X - mask.X;
            spreadCoords.Y = origin.Y - mask.Y;
            spreadCoords.Height = mask.Y + origin.Height + mask.Height;
            spreadCoords.Width = mask.X + origin.Width + mask.Width;
            return spreadCoords;
        }

        protected internal virtual Coords createIndentMask(Coords originCoords, Bitmap image)
        {
            var indentMask = new Coords(originCoords.Rectangle);
            indentMask.X = Math.Min(this.indent, originCoords.X);
            indentMask.Y = Math.Min(this.indent, originCoords.Y);
            indentMask.Width = Math.Min(this.indent, image.Width - originCoords.X - originCoords.Width);
            indentMask.Height = Math.Min(this.indent, image.Height - originCoords.Y - originCoords.Height);
            return indentMask;
        }

        protected internal virtual IList<NoFilteringArea> createNotFilteringAreas(Screenshot screenshot)
        {
            IList<NoFilteringArea> noFilteringAreas = new List<NoFilteringArea>();
            foreach (Coords noFilteringCoords in screenshot.getCoordsToCompare())
            {
                if (noFilteringCoords.IntersectsWith(Coords.ofImage(screenshot.getImage())))
                {
                    noFilteringAreas.Add(new NoFilteringArea(screenshot.getImage(), noFilteringCoords));
                }
            }
            return noFilteringAreas;
        }

        protected internal virtual void pasteAreasToCompare(Bitmap filtered, IList<NoFilteringArea> noFilteringAreas)
        {
            Graphics graphics = Graphics.FromImage(filtered);
            foreach (NoFilteringArea noFilteringArea
                in noFilteringAreas)
            {
                graphics.DrawImage(noFilteringArea.getSubimage(), noFilteringArea.getCoords().X, noFilteringArea.getCoords().Y);
            }
            graphics.Dispose();
        }

        public virtual IndentCropper AddIndentFilter(IndentFilter filter)
        {
            this.filters.Add(filter);
            return this;
        }

        protected internal virtual Bitmap ApplyFilters(Bitmap
            image)
        {
            foreach (IndentFilter filter in this.filters)
            {
                image = filter.apply(image);
            }
            return image;
        }

        public class NoFilteringArea
        {
            private readonly Coords coords;
            private readonly Bitmap subimage;

            public NoFilteringArea(Bitmap origin, Coords noFilterCoords)
            {
                this.subimage = ImageTool.subImage(origin, noFilterCoords);
                this.coords = noFilterCoords;
            }

            public virtual Bitmap getSubimage()
            {
                return this.subimage;
            }

            public virtual Coords getCoords()
            {
                return this.coords;
            }
        }
    }
}
