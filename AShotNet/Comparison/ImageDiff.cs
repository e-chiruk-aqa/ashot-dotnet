using Sharpen;

namespace ru.yandex.qatools.ashot.comparison
{
	/// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	public class ImageDiff
	{
		public static readonly ru.yandex.qatools.ashot.comparison.ImageDiff EMPTY_DIFF = 
			new ru.yandex.qatools.ashot.comparison.ImageDiff();

		private System.Collections.Generic.ICollection<java.awt.Point> diffPoints = new java.util.LinkedHashSet
			<java.awt.Point>();

		/// <summary>Deposed diff points relatively the least diff area.</summary>
		private System.Collections.Generic.ICollection<java.awt.Point> deposedPoints = new 
			java.util.LinkedHashSet<java.awt.Point>();

		/// <summary>The color which marks the differences between the images.</summary>
		/// <seealso cref="java.awt.Color"/>
		private java.awt.Color diffColor = java.awt.Color.RED;

		private java.awt.image.BufferedImage diffImage;

		private bool marked = false;

		/// <summary>Images are considered the same if the number of distinguished pixels does not exceed this value.
		/// 	</summary>
		private int diffSizeTrigger;

		public ImageDiff(java.awt.image.BufferedImage expected, java.awt.image.BufferedImage
			 actual)
		{
			int width = System.Math.max(expected.getWidth(), actual.getWidth());
			int height = System.Math.max(expected.getHeight(), actual.getHeight());
			this.diffImage = new java.awt.image.BufferedImage(width, height, actual.getType()
				);
		}

		private ImageDiff()
		{
		}

		/// <summary>Sets the color which marks the differences between the images.</summary>
		/// <remarks>
		/// Sets the color which marks the differences between the images.
		/// This color will be used with <code>Color.WHITE</code> in the checkerboard pattern.
		/// The default value is <code>Color.RED</code>.
		/// </remarks>
		/// <param name="diffColor">the color which marks the differences</param>
		/// <returns>self for fluent style</returns>
		/// <seealso cref="java.awt.Color"/>
		public virtual ru.yandex.qatools.ashot.comparison.ImageDiff withDiffColor(java.awt.Color
			 diffColor)
		{
			this.diffColor = diffColor;
			this.marked = false;
			return this;
		}

		/// <summary>Sets the maximum number of distinguished pixels when images are still considered the same.
		/// 	</summary>
		/// <param name="diffSizeTrigger">the number of different pixels</param>
		/// <returns>self for fluent style</returns>
		public virtual ru.yandex.qatools.ashot.comparison.ImageDiff withDiffSizeTrigger(int
			 diffSizeTrigger)
		{
			this.diffSizeTrigger = diffSizeTrigger;
			return this;
		}

		/// <returns>Diff image with empty spaces in diff areas.</returns>
		public virtual java.awt.image.BufferedImage getDiffImage()
		{
			return diffImage;
		}

		public virtual void addDiffPoint(int x, int y)
		{
			diffPoints.add(new java.awt.Point(x, y));
		}

		/// <summary>Marks diff on inner image and returns it.</summary>
		/// <remarks>
		/// Marks diff on inner image and returns it.
		/// Idempotent.
		/// </remarks>
		/// <returns>marked diff image</returns>
		public virtual java.awt.image.BufferedImage getMarkedImage()
		{
			if (!marked)
			{
				foreach (java.awt.Point dot in diffPoints)
				{
					diffImage.setRGB((int)dot.getX(), (int)dot.getY(), pickDiffColor(dot).getRGB());
				}
				marked = true;
			}
			return diffImage;
		}

		private java.awt.Color pickDiffColor(java.awt.Point dot)
		{
			return ((dot.getX() + dot.getY()) % 2 == 0) ? diffColor : java.awt.Color.WHITE;
		}

		/// <summary>Returns <tt>true</tt> if there are differences between images.</summary>
		/// <returns><tt>true</tt> if there are differences between images.</returns>
		public virtual bool hasDiff()
		{
			return diffPoints.Count > diffSizeTrigger;
		}

		public override bool Equals(object obj)
		{
			if (obj is ru.yandex.qatools.ashot.comparison.ImageDiff)
			{
				ru.yandex.qatools.ashot.comparison.ImageDiff item = (ru.yandex.qatools.ashot.comparison.ImageDiff
					)obj;
				if (diffPoints.Count != item.diffPoints.Count)
				{
					return false;
				}
				System.Collections.Generic.ICollection<java.awt.Point> referencedPoints = getDeposedPoints
					();
				System.Collections.Generic.ICollection<java.awt.Point> itemReferencedPoints = item
					.getDeposedPoints();
				foreach (java.awt.Point point in referencedPoints)
				{
					if (!itemReferencedPoints.contains(point))
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
			return getDeposedPoints().GetHashCode();
		}

		private System.Collections.Generic.ICollection<java.awt.Point> getDeposedPoints()
		{
			if (deposedPoints.isEmpty())
			{
				deposedPoints = deposeReference(this);
			}
			return deposedPoints;
		}

		private java.awt.Point getReferenceCorner(ru.yandex.qatools.ashot.comparison.ImageDiff
			 diff)
		{
			double x = ch.lambdaj.Lambda.min(diff.diffPoints, ch.lambdaj.Lambda.on<java.awt.Point
				>().getX());
			double y = ch.lambdaj.Lambda.min(diff.diffPoints, ch.lambdaj.Lambda.on<java.awt.Point
				>().getY());
			return new java.awt.Point((int)x, (int)y);
		}

		private System.Collections.Generic.ICollection<java.awt.Point> deposeReference(ru.yandex.qatools.ashot.comparison.ImageDiff
			 diff)
		{
			java.awt.Point reference = getReferenceCorner(diff);
			System.Collections.Generic.ICollection<java.awt.Point> referenced = new java.util.HashSet
				<java.awt.Point>();
			foreach (java.awt.Point point in diff.diffPoints)
			{
				referenced.add(new java.awt.Point(point.x - reference.x, point.y - reference.y));
			}
			return referenced;
		}
	}
}
