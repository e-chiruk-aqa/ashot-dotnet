using Sharpen;

namespace ru.yandex.qatools.ashot.comparison
{
    using AShotNet;
    using AShotNet.Coordinates;
    using AShotNet.Util;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	public class ImageDiffer
	{
		private const int DEFAULT_COLOR_DISTORTION = 15;

		private int colorDistortion = DEFAULT_COLOR_DISTORTION;

		public virtual ru.yandex.qatools.ashot.comparison.ImageDiffer withColorDistortion
			(int distortion)
		{
			this.colorDistortion = distortion;
			return this;
		}

		public virtual ru.yandex.qatools.ashot.comparison.ImageDiff makeDiff(Screenshot
			 expected, Screenshot actual)
		{
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = new ru.yandex.qatools.ashot.comparison.ImageDiff
				(expected.getImage(), actual.getImage());
			for (int i = 0; i < diff.getDiffImage().getWidth(); i++)
			{
				for (int j = 0; j < diff.getDiffImage().getHeight(); j++)
				{
					if (insideBothImages(i, j, expected.getImage(), actual.getImage()))
					{
						if (shouldCompare(i, j, expected, actual) && !ImageTool.rgbCompare
							(expected.getImage().getRGB(i, j), actual.getImage().getRGB(i, j), colorDistortion
							))
						{
							diff.addDiffPoint(i, j);
						}
						else
						{
							diff.getDiffImage().setRGB(i, j, expected.getImage().getRGB(i, j));
						}
					}
					else
					{
						setSharedPoint(i, j, expected, actual, diff);
					}
				}
			}
			return diff;
		}

		public virtual ru.yandex.qatools.ashot.comparison.ImageDiff makeDiff(java.awt.image.BufferedImage
			 expected, java.awt.image.BufferedImage actual)
		{
			return makeDiff(new Screenshot(expected), new Screenshot
				(actual));
		}

		private bool shouldCompare(int i, int j, Screenshot expected
			, Screenshot actual)
		{
			return notIgnoredInBoth(i, j, expected, actual) && isToCompareInBoth(i, j, expected
				, actual);
		}

		private bool notIgnoredInBoth(int i, int j, Screenshot expected
			, Screenshot actual)
		{
			return !isIgnored(i, j, expected.getIgnoredAreas()) || !isIgnored(i, j, actual.getIgnoredAreas
				());
		}

		private bool isToCompareInBoth(int i, int j, Screenshot expected
			, Screenshot actual)
		{
			return isToCompare(i, j, expected.getCoordsToCompare()) || isToCompare(i, j, actual
				.getCoordsToCompare());
		}

		private bool isIgnored(int i, int j, System.Collections.Generic.ICollection<Coords
			> ignoredCoords)
		{
			bool isIgnored = false;
			foreach (Coords coords in ignoredCoords)
			{
				if (coords.contains(i, j))
				{
					isIgnored = true;
					break;
				}
			}
			return isIgnored;
		}

		private void setSharedPoint(int i, int j, Screenshot expected
			, Screenshot actual, ru.yandex.qatools.ashot.comparison.ImageDiff
			 diff)
		{
			if (Coords.ofImage(expected.getImage()).contains
				(i, j))
			{
				diff.getDiffImage().setRGB(i, j, expected.getImage().getRGB(i, j));
			}
			else
			{
				if (Coords.ofImage(actual.getImage()).contains
					(i, j))
				{
					diff.getDiffImage().setRGB(i, j, actual.getImage().getRGB(i, j));
				}
			}
		}

		private bool isToCompare(int i, int j, System.Collections.Generic.ICollection<Coords
			> coordsToCompare)
		{
			bool isToCompare = false;
			foreach (Coords coords in coordsToCompare)
			{
				if (coords.contains(i, j))
				{
					isToCompare = true;
					break;
				}
			}
			return isToCompare;
		}

		private bool insideBothImages(int i, int j, java.awt.image.BufferedImage expected
			, java.awt.image.BufferedImage actual)
		{
			return Coords.ofImage(expected).contains(i, j
				) && Coords.ofImage(actual).contains(i, j);
		}
	}
}
