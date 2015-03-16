namespace AShotNet.Test
{
	/// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
 [DeploymentItem("img", "./img/")]
 [TestClass]
	public class DifferTest
	{
		public static readonly Bitmap IMAGE_A_SMALL = loadImage("img/A_s.png"			);

    public static readonly Bitmap IMAGE_B_SMALL = loadImage("img/B_s.png");

		public static readonly ImageDiffer IMAGE_DIFFER 			 = new ImageDiffer().withColorDistortion(10);

    public static Bitmap loadImage(string path)
		{
			
        try{

             Assembly currentAssembly = Assembly.GetAssembly(this.GetType());
            string asseblyLocation = currentAssembly.Location;

            logOutputDir = Path.GetDirectoryName(asseblyLocation);



              return new Bitmap(Image.FromFile(logOutputDir + path);
              
       

			
			}
			catch (System.Exception e)
			{
				throw new System.Exception(e);
			}
		}

		/// <exception cref="System.Exception"/>
     [TestMethod]
		public virtual void testSameSizeDiff()
		{
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL
				, IMAGE_B_SMALL);
			NUnit.Framework.Assert.assertThat(diff.getMarkedImage(), ru.yandex.qatools.ashot.util.ImageTool
				.equalImage(loadImage("img/expected/same_size_diff.png")));
		}

		/// <exception cref="System.Exception"/>
 [TestMethod]
		public virtual void testSetDiffColor()
		{
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL
				, IMAGE_B_SMALL);
			NUnit.Framework.Assert.assertThat(diff.withDiffColor(java.awt.Color.GREEN).getMarkedImage
				(), ru.yandex.qatools.ashot.util.ImageTool.equalImage(loadImage("img/expected/green_diff.png"
				)));
		}

		/// <exception cref="System.Exception"/>
 [TestMethod]
		public virtual void testSetDiffSizeTrigger()
		{
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL
				, IMAGE_B_SMALL);
			NUnit.Framework.Assert.assertThat(diff.withDiffSizeTrigger(624).hasDiff(), org.hamcrest.Matchers.@is
				(false));
			NUnit.Framework.Assert.assertThat(diff.withDiffSizeTrigger(623).hasDiff(), org.hamcrest.Matchers.@is
				(true));
		}

		/// <exception cref="System.Exception"/>
 [TestMethod]
		public virtual void testEqualImagesDiff()
		{
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL
				, IMAGE_A_SMALL);
			NUnit.Framework.Assert.IsFalse(diff.hasDiff());
		}

		/// <exception cref="System.Exception"/>
 [TestMethod]
		public virtual void testIgnoredCoordsSame()
		{
			ru.yandex.qatools.ashot.Screenshot a = createScreenshotWithSameIgnoredAreas(IMAGE_A_SMALL
				);
			ru.yandex.qatools.ashot.Screenshot b = createScreenshotWithSameIgnoredAreas(IMAGE_B_SMALL
				);
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = IMAGE_DIFFER.makeDiff(a, b);
			NUnit.Framework.Assert.assertThat(diff.getMarkedImage(), ru.yandex.qatools.ashot.util.ImageTool
				.equalImage(loadImage("img/expected/ignore_coords_same.png")));
		}

		/// <exception cref="System.Exception"/>
   [TestMethod]
		public virtual void testIgnoredCoordsNotSame()
		{
			ru.yandex.qatools.ashot.Screenshot a = createScreenshotWithIgnoredAreas(IMAGE_A_SMALL
				, new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords>(java.util.Arrays.asList
				(new ru.yandex.qatools.ashot.coordinates.Coords(50, 50))));
			ru.yandex.qatools.ashot.Screenshot b = createScreenshotWithIgnoredAreas(IMAGE_B_SMALL
				, new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords>(java.util.Arrays.asList
				(new ru.yandex.qatools.ashot.coordinates.Coords(80, 80))));
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = IMAGE_DIFFER.makeDiff(a, b);
			NUnit.Framework.Assert.assertThat(diff.getMarkedImage(), ru.yandex.qatools.ashot.util.ImageTool
				.equalImage(loadImage("img/expected/ignore_coords_not_same.png")));
		}

		/// <exception cref="System.Exception"/>
   [TestMethod]
		public virtual void testCoordsToCompareAndIgnoredCombine()
		{
			ru.yandex.qatools.ashot.Screenshot a = createScreenshotWithIgnoredAreas(IMAGE_A_SMALL
				, new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords>(java.util.Arrays.asList
				(new ru.yandex.qatools.ashot.coordinates.Coords(60, 60))));
			a.setCoordsToCompare(new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords
				>(java.util.Arrays.asList(new ru.yandex.qatools.ashot.coordinates.Coords(50, 50, 
				100, 100))));
			ru.yandex.qatools.ashot.Screenshot b = createScreenshotWithIgnoredAreas(IMAGE_B_SMALL
				, new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords>(java.util.Arrays.asList
				(new ru.yandex.qatools.ashot.coordinates.Coords(80, 80))));
			b.setCoordsToCompare(new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords
				>(java.util.Arrays.asList(new ru.yandex.qatools.ashot.coordinates.Coords(50, 50, 
				100, 100))));
			ru.yandex.qatools.ashot.comparison.ImageDiff diff = IMAGE_DIFFER.makeDiff(a, b);
			NUnit.Framework.Assert.assertThat(diff.getMarkedImage(), ru.yandex.qatools.ashot.util.ImageTool
				.equalImage(loadImage("img/expected/combined_diff.png")));
		}

		private ru.yandex.qatools.ashot.Screenshot createScreenshotWithSameIgnoredAreas(java.awt.image.BufferedImage
			 image)
		{
			return createScreenshotWithIgnoredAreas(image, new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords
				>(java.util.Arrays.asList(new ru.yandex.qatools.ashot.coordinates.Coords(50, 50)
				)));
		}

		private ru.yandex.qatools.ashot.Screenshot createScreenshotWithIgnoredAreas(java.awt.image.BufferedImage
			 image, System.Collections.Generic.ICollection<ru.yandex.qatools.ashot.coordinates.Coords
			> ignored)
		{
			ru.yandex.qatools.ashot.Screenshot screenshot = new ru.yandex.qatools.ashot.Screenshot
				(image);
			screenshot.setIgnoredAreas(ignored);
			return screenshot;
		}
	}
}
