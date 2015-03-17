namespace AShotNet.Test
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using Comparison;
    using Coordinates;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Util;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [TestClass]
    [DeploymentItem("img", "./img/")]
    public class DifferTest
    {
        public static readonly Bitmap IMAGE_A_SMALL = loadImage("img/A_s.png");

        public static readonly Bitmap IMAGE_B_SMALL = loadImage("img/B_s.png");

        public static readonly ImageDiffer IMAGE_DIFFER = new ImageDiffer().withColorDistortion(10);

        public static Bitmap loadImage(string path)
        {
            Assembly currentAssembly = Assembly.GetAssembly(typeof (DifferTest));
            string asseblyLocation = currentAssembly.Location;

            string logOutputDir = Path.GetDirectoryName(asseblyLocation);


            return new Bitmap(Image.FromFile(logOutputDir + path));
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testSameSizeDiff()
        {
            ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL
                , IMAGE_B_SMALL);

            Assert.AreSame(diff.getMarkedImage(), ImageTool.equalImage(loadImage("img/expected/same_size_diff.png")));
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testSetDiffColor()
        {
            ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL, IMAGE_B_SMALL);
            Assert.AreSame(diff.withDiffColor(Color.Green).getMarkedImage(), ImageTool.equalImage(loadImage("img/expected/green_diff.png")));
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testSetDiffSizeTrigger()
        {
            ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL
                , IMAGE_B_SMALL);
            Assert.AreSame(diff.withDiffSizeTrigger(624).hasDiff(), false);
            Assert.AreSame(diff.withDiffSizeTrigger(623).hasDiff(), true);
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testEqualImagesDiff()
        {
            ImageDiff diff = IMAGE_DIFFER.makeDiff(IMAGE_A_SMALL
                , IMAGE_A_SMALL);
            Assert.IsFalse(diff.hasDiff());
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testIgnoredCoordsSame()
        {
            Screenshot a = this.createScreenshotWithSameIgnoredAreas(IMAGE_A_SMALL);
            Screenshot b = this.createScreenshotWithSameIgnoredAreas(IMAGE_B_SMALL);
            ImageDiff diff = IMAGE_DIFFER.makeDiff(a, b);
            Assert.AreSame(diff.getMarkedImage(), ImageTool.equalImage(loadImage("img/expected/ignore_coords_same.png")));
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testIgnoredCoordsNotSame()
        {
            Screenshot a = this.createScreenshotWithIgnoredAreas(IMAGE_A_SMALL, new HashSet<Coords> {new Coords(0, 0, 50, 50)});
            Screenshot b = this.createScreenshotWithIgnoredAreas(IMAGE_B_SMALL, new HashSet<Coords> {new Coords(0, 0, 80, 80)});
            ImageDiff diff = IMAGE_DIFFER.makeDiff(a, b);
            Assert.AreSame(diff.getMarkedImage(), ImageTool.equalImage(loadImage("img/expected/ignore_coords_not_same.png")));
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testCoordsToCompareAndIgnoredCombine()
        {
            Screenshot a = this.createScreenshotWithIgnoredAreas(IMAGE_A_SMALL, new HashSet<Coords> {new Coords(0, 0, 60, 60)});
            a.setCoordsToCompare(new HashSet<Coords> {new Coords(50, 50, 100, 100)});
            Screenshot b = this.createScreenshotWithIgnoredAreas(IMAGE_B_SMALL, new HashSet<Coords> {new Coords(0, 0, 80, 80)});
            b.setCoordsToCompare(new HashSet<Coords> {new Coords(50, 50, 100, 100)});
            ImageDiff diff = IMAGE_DIFFER.makeDiff(a, b);
            Assert.AreSame(diff.getMarkedImage(), ImageTool.equalImage(loadImage("img/expected/combined_diff.png")));
        }

        private Screenshot createScreenshotWithSameIgnoredAreas(Bitmap image)
        {
            return this.createScreenshotWithIgnoredAreas(image, new HashSet<Coords> {new Coords(0, 0, 50, 50)});
        }

        private Screenshot createScreenshotWithIgnoredAreas(Bitmap image, ICollection<Coords> ignored)
        {
            var screenshot = new Screenshot(image);
            screenshot.setIgnoredAreas(ignored);
            return screenshot;
        }
    }
}
