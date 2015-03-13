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

        protected internal IList<IndentFilter> filters = new LinkedList<IndentFilter>();

        public IndentCropper(int indent)
        {
            this.indent = indent;
        }

        public IndentCropper()
        {
            this.indent = DEFAULT_INDENT;
        }

        protected internal override Screenshot cropScreenshot(Bitmap image, ICollection<Coords
            > coordsToCompare)
        {
            Coords cropArea = this.createCropArea(coordsToCompare
                );
            Coords indentMask = this.createIndentMask(cropArea
                , image);
            Coords coordsWithIndent = this.applyIndentMask(cropArea
                , indentMask);
            Screenshot croppedShot = base.cropScreenshot(image, new HashSet
                <Coords>(java.util.Arrays.asList(coordsWithIndent
                    )));
            croppedShot.setOriginShift(coordsWithIndent);
            croppedShot.setCoordsToCompare(Coords.setReferenceCoords
                (coordsWithIndent, coordsToCompare));
            IList<NoFilteringArea
                > noFilteringAreas = this.createNotFilteringAreas(croppedShot);
            croppedShot.setImage(this.applyFilters(croppedShot.getImage()));
            this.pasteAreasToCompare(croppedShot.getImage(), noFilteringAreas);
            return croppedShot;
        }

        protected internal virtual Coords applyIndentMask
            (Coords origin, Coords
                mask)
        {
            var spreadCoords = new Coords
                (0, 0);
            spreadCoords.x = origin.x - mask.x;
            spreadCoords.y = origin.y - mask.y;
            spreadCoords.height = mask.y + origin.height + mask.height;
            spreadCoords.width = mask.x + origin.width + mask.width;
            return spreadCoords;
        }

        protected internal virtual Coords createIndentMask
            (Coords originCoords, Bitmap
                image)
        {
            var indentMask = new Coords
                (originCoords);
            indentMask.x = Math.min(this.indent, originCoords.x);
            indentMask.y = Math.min(this.indent, originCoords.y);
            indentMask.width = Math.min(this.indent, image.getWidth() - originCoords.x - originCoords
                .width);
            indentMask.height = Math.min(this.indent, image.getHeight() - originCoords.y -
                                                      originCoords.height);
            return indentMask;
        }

        protected internal virtual IList<NoFilteringArea
            > createNotFilteringAreas(Screenshot screenshot)
        {
            IList<NoFilteringArea
                > noFilteringAreas = new List<NoFilteringArea
                    >();
            foreach (Coords noFilteringCoords in screenshot
                .getCoordsToCompare())
            {
                if (noFilteringCoords.intersects(Coords.ofImage
                    (screenshot.getImage())))
                {
                    noFilteringAreas.Add(new NoFilteringArea
                        (screenshot.getImage(), noFilteringCoords));
                }
            }
            return noFilteringAreas;
        }

        protected internal virtual void pasteAreasToCompare(Bitmap
            filtered, IList<NoFilteringArea
                > noFilteringAreas)
        {
            java.awt.Graphics graphics = filtered.getGraphics();
            foreach (NoFilteringArea noFilteringArea
                in noFilteringAreas)
            {
                graphics.drawImage(noFilteringArea.getSubimage(), noFilteringArea.getCoords().x,
                    noFilteringArea.getCoords().y, null);
            }
            graphics.dispose();
        }

        public virtual IndentCropper addIndentFilter
            (IndentFilter filter)
        {
            this.filters.Add(filter);
            return this;
        }

        protected internal virtual Bitmap applyFilters(Bitmap
            image)
        {
            foreach (IndentFilter filter in this.filters)
            {
                image = filter.apply(image);
            }
            return image;
        }

        private class NoFilteringArea
        {
            private readonly Coords coords;
            private readonly Bitmap subimage;

            private NoFilteringArea(Bitmap origin, Coords
                noFilterCoords)
            {
                this.subimage = ImageTool.subImage(origin, noFilterCoords
                    );
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
