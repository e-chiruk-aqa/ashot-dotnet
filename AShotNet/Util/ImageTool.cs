namespace AShotNet.Util
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Coordinates;
    using Extentions;
    using NHamcrest;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    public class ImageTool
    {
        public static Bitmap subImage(Bitmap origin, Coords crop)
        {
            Coords intersection = Coords.ofImage(origin).Intersect(crop);
            return origin.GetSubImage(intersection.X, intersection.Y, intersection.Width, intersection.Height);
        }

        public static Coords spreadCoordsInsideImage(
            Coords coordinates, int indent, Bitmap
                image)
        {
            return new Coords(Math.Max(0, coordinates.X - indent), Math.Max(0, coordinates.Y - indent), Math.Min(image.
                Width, coordinates.Width + indent), Math.Min(image.Height, coordinates.Height + indent));
        }

        public static bool rgbCompare(Color rgb1, Color rgb2, int inaccuracy)
        {
            if (inaccuracy == 0)
            {
                return rgb1 == rgb2;
            }
            int red1 = (rgb1.ToArgb() & unchecked(0x00FF0000)) >> 16;
            int green1 = (rgb1.ToArgb() & unchecked(0x0000FF00)) >> 8;
            int blue1 = (rgb1.ToArgb() & unchecked(0x000000FF));
            int red2 = (rgb2.ToArgb() & unchecked(0x00FF0000)) >> 16;
            int green2 = (rgb2.ToArgb() & unchecked(0x0000FF00)) >> 8;
            int blue2 = (rgb2.ToArgb() & unchecked(0x000000FF));
            return Math.Abs(red1 - red2) <= inaccuracy && Math.Abs(green1 - green2) <= inaccuracy && Math.Abs(blue1 - blue2) <= inaccuracy;
        }

        public static Matcher<Bitmap> equalImage(Bitmap second)
        {
            return new _TypeSafeMatcher_47(second);
        }

        /// <exception cref="System.IO.IOException" />
        public static byte[] toByteArray(Screenshot screenshot)
        {
            return toByteArray(screenshot.getImage());
        }

        /// <exception cref="System.IO.IOException" />
        public static byte[] toByteArray(Bitmap image)
        {
            var byteArray = new byte[0];
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        private sealed class _TypeSafeMatcher_47 : Matcher<Bitmap>
        {
            private readonly Bitmap second;

            public _TypeSafeMatcher_47(Bitmap second)
            {
                this.second = second;
            }

            public static byte[] ShaHash(Image image)
            {
                var bytes = new byte[1];
                bytes = (byte[])(new ImageConverter()).ConvertTo(image, bytes.GetType());

                return (new SHA256Managed()).ComputeHash(bytes);
            }

            public static bool AreEqual(Image imageA, Image imageB)
            {
                if (imageA.Width != imageB.Width) return false;
                if (imageA.Height != imageB.Height) return false;

                var hashA = ShaHash(imageA);
                var hashB = ShaHash(imageB);

                return !hashA
                    .Where((nextByte, index) => nextByte != hashB[index])
                    .Any();
            }

            public override bool Matches(Bitmap first)
            {
                if (!Coords.ofImage(first).Equals(Coords.ofImage(this.second)))
                {
                    return false;
                }
                for (int x = 0; x < first.Width; x++)
                {
                    for (int y = 0; y < first.Height; y++)
                    {
                        if (!rgbCompare(first.GetPixel(x, y), this.second.GetPixel(x, y), 10))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public override void DescribeTo(IDescription description) {}
        }
    }
}
