
namespace AShotNet.Util
{
    using System.Drawing;
    using AShotNet;
    using Coordinates;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	public class ImageTool
	{
      public static Bitmap subImage(Bitmap origin, Coords crop)
		{
			Coords intersection = ((Coords)Coords.ofImage(origin).intersection(crop));
			return origin.getSubimage(intersection.x, intersection.y, intersection.width, intersection.height);
		}

		public static Coords spreadCoordsInsideImage(
      Coords coordinates, int indent, Bitmap
			 image)
		{
			return new Coords(System.Math.max(0, coordinates
				.x - indent), System.Math.max(0, coordinates.y - indent), System.Math.min(image.
				getWidth(), coordinates.width + indent), System.Math.min(image.getHeight(), coordinates
				.height + indent));
		}

		public static bool rgbCompare(int rgb1, int rgb2, int inaccuracy)
		{
			if (inaccuracy == 0)
			{
				return rgb1 == rgb2;
			}
			int red1 = (rgb1 & unchecked((int)(0x00FF0000))) >> 16;
			int green1 = (rgb1 & unchecked((int)(0x0000FF00))) >> 8;
			int blue1 = (rgb1 & unchecked((int)(0x000000FF)));
			int red2 = (rgb2 & unchecked((int)(0x00FF0000))) >> 16;
			int green2 = (rgb2 & unchecked((int)(0x0000FF00))) >> 8;
			int blue2 = (rgb2 & unchecked((int)(0x000000FF)));
			return System.Math.Abs(red1 - red2) <= inaccuracy && System.Math.Abs(green1 - green2
				) <= inaccuracy && System.Math.Abs(blue1 - blue2) <= inaccuracy;
		}

    public static org.hamcrest.Matcher<Bitmap> equalImage(Bitmap second)
		{
			return new _TypeSafeMatcher_47(second);
		}

		private sealed class _TypeSafeMatcher_47 : TypeSafeMatcher<Bitmap
			>
		{
			public _TypeSafeMatcher_47(Bitmap second)
			{
				this.second = second;
			}

			protected override bool matchesSafely(Bitmap first)
			{
				if (!Coords.ofImage(first).Equals(Coords
					.ofImage(this.second)))
				{
					return false;
				}
				for (int x = 0; x < first.getWidth(); x++)
				{
					for (int y = 0; y < first.getHeight(); y++)
					{
						if (!ImageTool.rgbCompare(first.getRGB(x, y), this.second
							.getRGB(x, y), 10))
						{
							return false;
						}
					}
				}
				return true;
			}

			public override void describeTo(org.hamcrest.Description description)
			{
			}

			private readonly Bitmap second;
		}

		/// <exception cref="System.IO.IOException"/>
		public static byte[] toByteArray(Screenshot screenshot)
		{
			return toByteArray(screenshot.getImage());
		}

		/// <exception cref="System.IO.IOException"/>
		public static byte[] toByteArray(Bitmap image)
		{
			using (java.io.ByteArrayOutputStream baos = new java.io.ByteArrayOutputStream())
			{
				javax.imageio.ImageIO.write(image, "png", baos);
				return baos.toByteArray();
			}
		}
	}
}
